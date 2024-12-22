using Bogus;
using BookMe.Domain.Entities;
using BookMe.Domain.Enums;
using BookMe.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace BookMe.Infrastructure.Seeders
{
    public class EmployeeSeeder : ISeeder
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly BookMeDbContext _dbContext;

        public EmployeeSeeder(UserManager<ApplicationUser> userManager, BookMeDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        public async Task Seed()
        {
            if (!_dbContext.Employees.Any())
            {
                var httpClient = new HttpClient();

                var userGenerator = new Faker<ApplicationUser>()
                    .RuleFor(u => u.Email, (f, u) => f.Internet.Email())
                    .RuleFor(u => u.UserName, (f, u) => u.Email)
                    .RuleFor(u => u.Gender, f => f.PickRandom<Gender>())
                    .RuleFor(u => u.FirstName, (f, u) =>
                    {
                        var bogusGender = u.Gender == Gender.Male ? Bogus.DataSets.Name.Gender.Male : Bogus.DataSets.Name.Gender.Female;
                        return f.Name.FirstName(bogusGender);
                    })
                    .RuleFor(u => u.LastName, (f, u) =>
                    {
                        var bogusGender = u.Gender == Gender.Male ? Bogus.DataSets.Name.Gender.Male : Bogus.DataSets.Name.Gender.Female;
                        return f.Name.LastName(bogusGender);
                    });

                var serviceIds = _dbContext.Services.Select(s => s.Id).ToList();

                foreach (var serviceId in serviceIds)
                {
                    var users = userGenerator.Generate(4);
                    foreach (var user in users)
                    {
                        user.EmailConfirmed = true;
                        var gender = user.Gender == Gender.Male ? "male" : "female";
                        var response = await httpClient.GetStringAsync($"https://randomuser.me/api/?gender={gender}");
                        var json = JObject.Parse(response);
                        user.AvatarUrl = json["results"][0]["picture"]["large"].ToString();

                        var newUser = await _userManager.CreateAsync(user, "zaq1@WSX");
                        if (newUser.Succeeded)
                        {
                            var employee = new Employee
                            {
                                ServiceId = serviceId,
                                UserId = user.Id
                            };
                            _dbContext.Employees.Add(employee);
                        }
                        else
                        {
                            foreach (var error in newUser.Errors)
                            {
                                Console.WriteLine($"Error creating user: {error.Description}");
                            }
                        }
                    }
                }
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
