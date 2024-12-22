using BookMe.Domain.Entities;
using BookMe.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookMe.Infrastructure.Seeders
{
    public class OpeningHoursSeeder : ISeeder
    {
        private readonly BookMeDbContext _dbContext;

        private readonly string[] _polishDaysOfWeek = { "Poniedziałek", "Wtorek", "Środa", "Czwartek", "Piątek", "Sobota", "Niedziela" };

        public OpeningHoursSeeder(BookMeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Seed()
        {
            if (await _dbContext.Database.CanConnectAsync())
            {
                if (!_dbContext.OpeningHours.Any())
                {
                    var serviceIds = _dbContext.Services.Select(s => s.Id).ToList();
                    foreach (var serviceId in serviceIds)
                    {
                        var openingHoursList = GenerateOpeningHours();
                        foreach (var item in openingHoursList)
                        {
                            item.ServiceId = serviceId;
                        }
                        _dbContext.OpeningHours.AddRange(openingHoursList);
                    }

                    await _dbContext.SaveChangesAsync();
                }
            }
        }

        private List<OpeningHour> GenerateOpeningHours()
        {
            List<OpeningHour> openingHoursList = new List<OpeningHour>();

            Random random = new Random();

            foreach (var day in _polishDaysOfWeek)
            {
                var isClosed = random.Next(0, 5) == 0;  // Randomly decide if the service is closed (1 in 5 chance)

                if (isClosed)
                {
                    openingHoursList.Add(new OpeningHour
                    {
                        DayOfWeek = day,
                        OpeningTime = TimeSpan.Zero,  // If closed, opening/closing times are irrelevant
                        ClosingTime = TimeSpan.Zero,
                        Closed = true
                    });
                }
                else
                {
                    int openingHour = random.Next(6, 10);  // Random opening time between 6 and 10 AM
                    int closingHour = random.Next(16, 20);  // Random closing time between 4 and 8 PM

                    openingHoursList.Add(new OpeningHour
                    {
                        DayOfWeek = day,
                        OpeningTime = new TimeSpan(openingHour, 0, 0),
                        ClosingTime = new TimeSpan(closingHour, 0, 0),
                        Closed = false
                    });
                }
            }

            return openingHoursList;
        }
    }
}
