using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using tsc.RestaurantBookingApp.Core;

namespace tsc.RestaurantBookingApp.Data
{
    public class RestaurantTableBookingDbContext: DbContext
    {
        public RestaurantTableBookingDbContext(DbContextOptions<RestaurantTableBookingDbContext> options) : base(options)
        { }
            public DbSet<Restaurant> Restaurants { get; set; }
            public DbSet<DiningTable> DiningTables { get; set; }
            public DbSet<Reservation> Reservations { get; set; }
            public DbSet<RestaurantBranch> RestaurantBranches { get; set; }
            public DbSet<TimeSlot> TimeSlots{ get; set; }
            public DbSet<User> Users { get; set; }


    }
}
