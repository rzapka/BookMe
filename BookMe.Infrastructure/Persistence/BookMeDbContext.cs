using BookMe.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookMe.Infrastructure.Persistence
{
    public class BookMeDbContext : IdentityDbContext<ApplicationUser>
    {
        public BookMeDbContext(DbContextOptions<BookMeDbContext> options) : base(options) { }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<ServiceCategory> ServiceCategories { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<Opinion> Opinions { get; set; }
        public DbSet<OpeningHour> OpeningHours { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        public DbSet<ServiceImage> ServiceImages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // -------------------------
            // ApplicationUser Configuration
            // -------------------------
            builder.Entity<ApplicationUser>(entity =>
            {
                entity.HasMany(u => u.Bookings)
                    .WithOne(b => b.User)
                    .HasForeignKey(b => b.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(u => u.Opinions)
                    .WithOne(o => o.User)
                    .HasForeignKey(o => o.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // -------------------------
            // Employee Configuration
            // -------------------------
            builder.Entity<Employee>(entity =>
            {
                entity.HasOne(e => e.User)
                    .WithOne()
                    .HasForeignKey<Employee>(e => e.UserId)
                    .OnDelete(DeleteBehavior.Restrict); // Brak kaskadowego usuwania

                entity.HasMany(e => e.Bookings)
                    .WithOne(b => b.Employee)
                    .HasForeignKey(b => b.EmployeeId)
                    .OnDelete(DeleteBehavior.SetNull); // Ustaw EmployeeId na NULL przy usunięciu pracownika

                entity.HasMany(e => e.Opinions)
                    .WithOne(o => o.Employee)
                    .HasForeignKey(o => o.EmployeeId)
                    .OnDelete(DeleteBehavior.Restrict); // Blokuj usunięcie pracownika, jeśli są powiązane opinie
            });


            // -------------------------
            // Booking Configuration
            // -------------------------
            builder.Entity<Booking>(entity =>
            {
                entity.HasOne(b => b.User)
                    .WithMany(u => u.Bookings)
                    .HasForeignKey(b => b.UserId)
                    .OnDelete(DeleteBehavior.Restrict); // Nie usuwaj użytkownika przy usunięciu bookingów

                entity.HasOne(b => b.Employee)
                    .WithMany(e => e.Bookings)
                    .HasForeignKey(b => b.EmployeeId)
                    .OnDelete(DeleteBehavior.SetNull); // Ustaw EmployeeId na NULL

                entity.HasOne(b => b.Offer)
                    .WithMany(o => o.Bookings)
                    .HasForeignKey(b => b.OfferId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(b => b.Service)
                    .WithMany(s => s.Bookings)
                    .HasForeignKey(b => b.ServiceId)
                    .OnDelete(DeleteBehavior.Cascade); // Kaskadowo usuń bookingi przy usunięciu Service

                entity.HasOne(b => b.Opinion)
                    .WithOne(o => o.Booking)
                    .HasForeignKey<Opinion>(o => o.BookingId)
                    .OnDelete(DeleteBehavior.Restrict); // Usuń booking, ale nie opinię
            });


            // -------------------------
            // Opinion Configuration
            // -------------------------
            builder.Entity<Opinion>(entity =>
            {
                entity.HasOne(o => o.Service)
                    .WithMany(s => s.Opinions)
                    .HasForeignKey(o => o.ServiceId)
                    .OnDelete(DeleteBehavior.Cascade); // Usuń opinie przy usunięciu Service

                entity.HasOne(o => o.User)
                    .WithMany(u => u.Opinions)
                    .HasForeignKey(o => o.UserId)
                    .OnDelete(DeleteBehavior.Restrict); // Nie usuwaj użytkownika przy usunięciu opinii

                entity.HasOne(o => o.Employee)
                    .WithMany(e => e.Opinions)
                    .HasForeignKey(o => o.EmployeeId)
                    .OnDelete(DeleteBehavior.SetNull); // Ustaw EmployeeId na NULL

                entity.HasOne(o => o.Offer)
                    .WithMany()
                    .HasForeignKey(o => o.OfferId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(o => o.Booking)
                    .WithOne(b => b.Opinion)
                    .HasForeignKey<Opinion>(o => o.BookingId)
                    .OnDelete(DeleteBehavior.Restrict); // Blokuj usunięcie opinii przy usunięciu bookingów
            });

            // -------------------------
            // ServiceCategory Configuration
            // -------------------------
            builder.Entity<ServiceCategory>(entity =>
            {
                entity.HasMany(c => c.Services)
                    .WithOne(s => s.ServiceCategory)
                    .HasForeignKey(s => s.ServiceCategoryId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // -------------------------
            // Service Configuration
            // -------------------------
            builder.Entity<Service>(entity =>
            {
                entity.OwnsOne(s => s.ContactDetails);

                entity.HasMany(s => s.Offers)
                    .WithOne(o => o.Service)
                    .HasForeignKey(o => o.ServiceId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(s => s.OpeningHours)
                    .WithOne(o => o.Service)
                    .HasForeignKey(o => o.ServiceId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(s => s.Employees)
                    .WithOne(e => e.Service)
                    .HasForeignKey(e => e.ServiceId)
                    .OnDelete(DeleteBehavior.Restrict); // Blokuj usunięcie pracowników przy usuwaniu Service

                entity.HasMany(s => s.Opinions)
                    .WithOne(o => o.Service)
                    .HasForeignKey(o => o.ServiceId)
                    .OnDelete(DeleteBehavior.Cascade); // Usuwaj opinie przy usunięciu Service

                entity.HasMany(s => s.Bookings)
                    .WithOne(b => b.Service)
                    .HasForeignKey(b => b.ServiceId)
                    .OnDelete(DeleteBehavior.Cascade); // Usuwaj bookingi przy usunięciu Service

                entity.HasIndex(s => s.Name).IsUnique();
            });

            // -------------------------
            // Offer Configuration
            // -------------------------
            builder.Entity<Offer>(entity =>
            {
                entity.HasMany(o => o.Bookings)
                    .WithOne(b => b.Offer)
                    .HasForeignKey(b => b.OfferId)
                    .OnDelete(DeleteBehavior.Restrict); // Nie usuwaj powiązanych rezerwacji przy usunięciu oferty
            });

            // -------------------------
            // OpeningHour Configuration
            // -------------------------
            builder.Entity<OpeningHour>(entity =>
            {
                entity.HasOne(o => o.Service)
                    .WithMany(s => s.OpeningHours)
                    .HasForeignKey(o => o.ServiceId)
                    .OnDelete(DeleteBehavior.Cascade); // Usuwanie powiązanych godzin otwarcia
            });

            // -------------------------
            // ServiceImage Configuration
            // -------------------------
            builder.Entity<ServiceImage>(entity =>
            {
                entity.HasOne(o => o.Service)
                    .WithMany(s => s.ServiceImages)
                    .HasForeignKey(o => o.ServiceId)
                    .OnDelete(DeleteBehavior.Cascade); // Usuwanie powiązanych godzin otwarcia
            });

        }
    }
}
