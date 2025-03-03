using Hotel_Reservation.Models;
using Microsoft.EntityFrameworkCore;

namespace Hotel_Reservation.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<AppUser> AppUser { get; set; }
        public DbSet<AppUserPermission> AppUserPermission { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Room> Room { get; set; }
        public DbSet<Reservation> Reservation { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<Services> Services { get; set; }
        public DbSet<Billing> Billing { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Define AppUser-Permission Relationship
            modelBuilder.Entity<AppUser>()
                .HasMany(u => u.Permissions)
                .WithOne(p => p.AppUser)
                .HasForeignKey(p => p.AppUserID)
                .OnDelete(DeleteBehavior.Cascade);

            // Define Reservation - Customer Relationship
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Customer)
                .WithMany(c => c.Reservations)
                .HasForeignKey(r => r.CustomerID)
                .OnDelete(DeleteBehavior.Restrict);

            // Define Reservation - Room Relationship
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Room)
                .WithMany(room => room.Reservations)
                .HasForeignKey(r => r.RoomID)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
