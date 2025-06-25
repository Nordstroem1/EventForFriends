using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Application.Interfaces;

namespace Infrastructure.Databases
{
    public class mySqlDb : IdentityDbContext<User, IdentityRole, string>, IMySqlContext
    {
        public mySqlDb(DbContextOptions<mySqlDb> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Comment> Comment { get; set; }

        public Task LoadCollectionAsync<TEntity>(TEntity entity, string collectionName) where TEntity : class
        {
            var entry = this.Entry(entity);
            return entry.Collection(collectionName).LoadAsync();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>()
           .HasMany(e => e.LikeList)
           .WithMany(u => u.Events)
           .UsingEntity<Dictionary<string, object>>(
           "EventUser",
           j => j.HasOne<User>().WithMany().HasForeignKey("UserId"),
           j => j.HasOne<Event>().WithMany().HasForeignKey("EventId"));

            base.OnModelCreating(modelBuilder);
        }
    }
}
