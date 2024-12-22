using Bogus;
using BookMe.Domain.Entities;
using BookMe.Domain.Enums;
using BookMe.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace BookMe.Infrastructure.Seeders
{
    public class UserSeeder : ISeeder
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly BookMeDbContext _dbContext;
        private readonly ILogger<UserSeeder> _logger;

        public UserSeeder(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, BookMeDbContext dbContext, ILogger<UserSeeder> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task Seed()
        {
            _logger.LogInformation("Starting user seeding...");

            if (!_roleManager.Roles.Any())
            {
                // Add ADMIN role if it doesn't exist
                if (!await _roleManager.RoleExistsAsync("ADMIN"))
                {
                    _logger.LogInformation("Creating ADMIN role...");
                    await _roleManager.CreateAsync(new IdentityRole("ADMIN"));
                }
            }

            if (!_userManager.Users.Any())
            {
                var httpClient = new HttpClient();

                var adminUser = new ApplicationUser
                {
                    Email = "admin@admin.com",
                    UserName = "admin@admin.com",
                    EmailConfirmed = true,
                    Gender = Gender.Male,
                    FirstName = "Admin",
                    LastName = "User",
                    IsAdmin = true
                };
                adminUser.AvatarUrl = await GetAvatarUrl(httpClient, adminUser.Gender);
                var result = await _userManager.CreateAsync(adminUser, "zaq1@WSX");

                if (result.Succeeded)
                {
                    _logger.LogInformation("Admin user created successfully.");
                    await _userManager.AddToRoleAsync(adminUser, "ADMIN");
                }
                else
                {
                    _logger.LogError("Failed to create admin user: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
                }

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

                var users = userGenerator.Generate(100);

                foreach (var user in users)
                {
                    user.EmailConfirmed = true;
                    user.AvatarUrl = await GetAvatarUrl(httpClient, user.Gender);
                    var userResult = await _userManager.CreateAsync(user, "zaq1@WSX");

                    if (!userResult.Succeeded)
                    {
                        _logger.LogError("Failed to create user {Email}: {Errors}", user.Email, string.Join(", ", userResult.Errors.Select(e => e.Description)));
                    }
                }
            }

            _logger.LogInformation("User seeding completed.");
        }

        private async Task<string> GetAvatarUrl(HttpClient httpClient, Gender gender)
        {
            var genderString = gender == Gender.Male ? "male" : "female";
            var response = await httpClient.GetStringAsync($"https://randomuser.me/api/?gender={genderString}");
            var json = JObject.Parse(response);
            return json["results"][0]["picture"]["large"].ToString();
        }
    }
}
