using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using BookMe.Domain.Entities;
using BookMe.Infrastructure.Persistence;
using BookMe.Infrastructure.Seeders;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public class BookingSeeder : ISeeder
{
    private readonly BookMeDbContext _dbContext;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<BookingSeeder> _logger;
    private static readonly Random _random = new Random();

    public BookingSeeder(BookMeDbContext dbContext, UserManager<ApplicationUser> userManager, ILogger<BookingSeeder> logger)
    {
        _dbContext = dbContext;
        _userManager = userManager;
        _logger = logger;
    }

    public async Task Seed()
    {
        if (await _dbContext.Database.CanConnectAsync())
        {
            if (!_dbContext.Bookings.Any())
            {
                var users = _userManager.Users
                    .Select(u => new { u.Id, u.FirstName, u.LastName })
                    .ToList();
                var userIds = users.Select(u => u.Id).ToList(); // Lista samych Id użytkowników

                var services = _dbContext.Services
                    .Include(s => s.Offers)
                    .Include(s => s.Employees)
                    .ToList();

                var bookingGenerator = new Faker<Booking>("pl")
                    .RuleFor(b => b.StartTime, f => f.Date.Between(DateTime.UtcNow.AddYears(-1), DateTime.UtcNow.AddMonths(1)))
                    .RuleFor(b => b.UserId, f => GetRandomUserId(userIds))
                    .FinishWith((f, b) => b.SetEndTime());

                var bookings = new List<Booking>();
                var notifications = new List<Notification>();

                foreach (var service in services)
                {
                    var employees = service.Employees.ToList();
                    var offers = service.Offers.ToList();

                    if (!offers.Any())
                    {
                        continue;
                    }

                    var numberOfBookings = _random.Next(50, 101);

                    for (int i = 0; i < numberOfBookings; i++)
                    {
                        var booking = bookingGenerator.Generate();
                        booking.OfferId = offers[_random.Next(offers.Count)].Id;

                        if (employees.Any())
                        {
                            booking.EmployeeId = employees[_random.Next(employees.Count)].Id;
                        }
                        else
                        {
                            booking.EmployeeId = null;
                        }

                        booking.ServiceId = service.Id;
                        bookings.Add(booking);

                        // Add notification for future bookings
                        if (booking.StartTime > DateTime.UtcNow)
                        {
                            var user = users.First(u => u.Id == booking.UserId);
                            notifications.Add(new Notification
                            {
                                EmployeeId = booking.EmployeeId ?? 0,
                                Message = $"Nowa wizyta od {user.FirstName} {user.LastName} zaplanowana na {booking.StartTime:yyyy-MM-dd HH:mm}",
                                IsRead = false,
                                CreatedAt = DateTime.UtcNow
                            });
                        }
                    }
                }

                _dbContext.Bookings.AddRange(bookings);
                _dbContext.Notifications.AddRange(notifications);

                try
                {
                    await _dbContext.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {
                    _logger.LogError(ex, "An error occurred while saving the bookings and notifications.");
                    throw;
                }
            }
        }
    }

    private string GetRandomUserId(List<string> userIds)
    {
        return userIds[_random.Next(userIds.Count)];
    }
}
