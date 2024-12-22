using Bogus;
using BookMe.Domain.Entities;
using BookMe.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BookMe.Infrastructure.Seeders
{
    public class ServiceSeeder : ISeeder
    {
        private readonly BookMeDbContext dbContext;

        public ServiceSeeder(BookMeDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task Seed()
        {
            if (await dbContext.Database.CanConnectAsync())
            {
                if (!dbContext.Services.Any())
                {
                    var locale = "pl";
                    var contactDetailsGenerator = new Faker<ServiceContactDetails>(locale)
                        .RuleFor(s => s.City, f => f.Address.City())
                        .RuleFor(s => s.Street, f => f.Address.StreetAddress())
                        .RuleFor(s => s.PostalCode, f => f.Address.ZipCode())
                        .RuleFor(s => s.PhoneNumber, f => f.Phone.PhoneNumber());

                    var categoryIds = dbContext.ServiceCategories.Select(s => s.Id).ToList();

                    // Generate services with categories
                    var serviceGeneratorWithCategory = new Faker<Service>(locale)
                        .RuleFor(s => s.Name, f => f.Lorem.Sentence(f.Random.Number(1, 4)).TrimEnd('.'))
                        .RuleFor(s => s.Description, f => f.Lorem.Sentence(30))
                        .RuleFor(s => s.ServiceCategoryId, f => f.PickRandom(categoryIds))
                        .RuleFor(s => s.ContactDetails, f => contactDetailsGenerator.Generate())
                        .RuleFor(s => s.ImageUrl, f => $"https://picsum.photos/seed/{Guid.NewGuid()}/1200/800");

                    var servicesWithCategory = serviceGeneratorWithCategory.Generate(200);

                    // Generate services without categories (ServiceCategoryId = null)
                    var serviceGeneratorWithoutCategory = new Faker<Service>(locale)
                        .RuleFor(s => s.Name, f => f.Lorem.Sentence(f.Random.Number(1, 4)).TrimEnd('.'))
                        .RuleFor(s => s.Description, f => f.Lorem.Sentence(30))
                        .RuleFor(s => s.ServiceCategoryId, f => (int?)null) // ServiceCategoryId set to null
                        .RuleFor(s => s.ContactDetails, f => contactDetailsGenerator.Generate())
                        .RuleFor(s => s.ImageUrl, f => $"https://picsum.photos/seed/{Guid.NewGuid()}/1200/800");

                    var servicesWithoutCategory = serviceGeneratorWithoutCategory.Generate(100);

                    var allServices = servicesWithCategory.Concat(servicesWithoutCategory).ToList();

                    foreach (var service in allServices)
                    {
                       

                        // Generate a unique name in the database
                        var nameIsUnique = false;

                        while (!nameIsUnique)
                        {
                            if (dbContext.Services.Any(s => s.Name == service.Name))
                            {
                                // Append a new GUID fragment to make the name unique
                                service.Name = $"{service.Name}-{Guid.NewGuid().ToString().Substring(0, 8)}";
                            }
                            else
                            {
                                nameIsUnique = true;
                            }
                        }
                        service.EncodeName();
                        // Attempt to save the service and retry on failure
                        var saved = false;
                        while (!saved)
                        {
                            try
                            {
                                dbContext.Services.Add(service);
                                await dbContext.SaveChangesAsync();
                                saved = true;
                            }
                            catch (DbUpdateException ex) when ((ex.InnerException as Microsoft.Data.SqlClient.SqlException)?.Number == 2627) // Duplicate key error
                            {
                                // Handle duplicate name by generating a new one
                                service.Name = $"{service.Name}-{Guid.NewGuid().ToString().Substring(0, 8)}";
                            }
                        }
                    }
                }
            }
        }
    }
}
