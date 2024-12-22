using Bogus;
using BookMe.Domain.Entities;
using BookMe.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookMe.Infrastructure.Seeders
{
    public class OpinionSeeder : ISeeder
    {
        private readonly BookMeDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<OpinionSeeder> _logger;
        private static readonly Random _random = new Random();

        public OpinionSeeder(BookMeDbContext dbContext, UserManager<ApplicationUser> userManager, ILogger<OpinionSeeder> logger)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task Seed()
        {
            if (await _dbContext.Database.CanConnectAsync())
            {
                if (!_dbContext.Opinions.Any())
                {
                    var userIds = _userManager.Users.Select(u => u.Id).ToList();
                    var bookings = _dbContext.Bookings.Include(b => b.Offer).Include(b => b.Employee).ToList();

                    var opinionGenerator = new Faker<Opinion>("pl")
                        .RuleFor(o => o.Rating, f => GenerateRandomRating())
                        .RuleFor(o => o.Content, f => f.Lorem.Sentence(f.Random.Number(1, 15)))
                        .RuleFor(o => o.CreatedAt, f => f.Date.Past(1));

                    var opinions = new List<Opinion>();

                    foreach (var booking in bookings)
                    {
                        var opinion = opinionGenerator
                            .RuleFor(o => o.ServiceId, f => booking.Offer.ServiceId)
                            .RuleFor(o => o.EmployeeId, f => booking.EmployeeId)
                            .RuleFor(o => o.OfferId, f => booking.OfferId)
                            .RuleFor(o => o.BookingId, f => booking.Id)
                            .RuleFor(o => o.UserId, f => booking.UserId)
                            .Generate();

                        opinions.Add(opinion);
                    }

                    _dbContext.Opinions.AddRange(opinions);

                    try
                    {
                        await _dbContext.SaveChangesAsync();
                    }
                    catch (DbUpdateException ex)
                    {
                        _logger.LogError(ex, "An error occurred while saving the opinions.");
                        throw;
                    }
                }
            }
        }

        private int GenerateRandomRating()
        {
            return _random.Next(1, 6);
        }
    }
}
