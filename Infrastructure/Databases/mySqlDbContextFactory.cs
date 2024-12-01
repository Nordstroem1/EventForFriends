using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Databases
{
    public class mySqlDbContextFactory : IDesignTimeDbContextFactory<mySqlDb>
    {
        public mySqlDb CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<mySqlDb>();

            // Adjust the path to the appsettings.json file
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Presentation"))
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(connectionString);

            return new mySqlDb(optionsBuilder.Options, configuration);
        }
    }
}