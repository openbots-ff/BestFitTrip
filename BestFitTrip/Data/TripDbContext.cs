using System;
using BestFitTrip.Models;
using Microsoft.EntityFrameworkCore;

namespace BestFitTrip.Data
{
    public class TripDbContext : DbContext
    {
        public DbSet<Trip> Trips { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<DestinationValue> DestinationValues { get; set; }

        public TripDbContext(DbContextOptions<TripDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost,1433; Database=BestFitDb; User=SA; Password=<YourStrong@Passw0rd>");
        }
    }
}
