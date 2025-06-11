using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Application.Queries.EventQueries.GetAllEvents
{
    public class GetAllEventsWithinAreaQueryHandler(IGenericRepository<Event> eventRepository, 
                                                    UserManager<User> userManager, 
                                                    ILogger<GetAllEventsWithinAreaQueryHandler>logger) : IRequestHandler<GetAllEventsWithinAreaQuery, OperationResult<List<Event>>>
    {
        
        public async Task<OperationResult<List<Event>>> Handle(GetAllEventsWithinAreaQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var loggedInUser = await userManager.FindByIdAsync(request.UserId);

                if (loggedInUser is null) 
                    return OperationResult<List<Event>>.Fail("Could not find the logged in user.", "GetAllEventsWithinAreaQueryHandler");

                var allEvents = await eventRepository.GetAllAsync();

                var nearbyEvents = allEvents
                            .Where(e =>
                                CalculateDistanceInKilometers(loggedInUser.Latitude, loggedInUser.Longitude, e.Latitude, e.Longitude) 
                                <= request.AllowedDistance).ToList();

                return OperationResult<List<Event>>.Success(nearbyEvents);
            }
            catch
            {
                return OperationResult<List<Event>>.Fail("Something went wrong while fetching events.", "GetAllEventsWithinAreaQueryHandler");
            }
        }
        private double CalculateDistanceInKilometers(double usersLongitude, double usersLatitude, double eventsLongitude, double eventsLatitude)
        {
            try
            {
                const double EarthRadiusKm = 6371;

                double latitudeDifferenceInRadians = DegreesToRadians(eventsLatitude - usersLatitude);
                double longitudeDifferenceInRadians = DegreesToRadians(eventsLongitude - usersLongitude);

                double userLatitudeInRadians = DegreesToRadians(usersLatitude);
                double eventLatitudeInRadians = DegreesToRadians(eventsLatitude);

                double haversineOfCentralAngle = Math.Sin(latitudeDifferenceInRadians / 2) * Math.Sin(latitudeDifferenceInRadians / 2) +
                                     Math.Cos(userLatitudeInRadians) * Math.Cos(eventLatitudeInRadians) *
                                     Math.Sin(longitudeDifferenceInRadians / 2) * Math.Sin(longitudeDifferenceInRadians / 2);

                double centralAngleInRadians = 2 * Math.Atan2(
                    Math.Sqrt(haversineOfCentralAngle),
                    Math.Sqrt(1 - haversineOfCentralAngle));

                return EarthRadiusKm * centralAngleInRadians;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180;
        }
    }
}
