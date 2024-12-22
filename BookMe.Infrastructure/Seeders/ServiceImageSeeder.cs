using Bogus;
using BookMe.Domain.Entities;
using BookMe.Infrastructure.Persistence;

namespace BookMe.Infrastructure.Seeders
{
    public class ServiceImageSeeder : ISeeder
    {
        private readonly BookMeDbContext dbContext;

        public ServiceImageSeeder(BookMeDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task Seed()
        {
            if (await dbContext.Database.CanConnectAsync())
            {
                if (!dbContext.ServiceImages.Any())
                {
                    // Pobierz wszystkie istniejące serwisy z bazy
                    var services = dbContext.Services.ToList();

                    var locale = "pl";
                    var imageGenerator = new Faker<ServiceImage>(locale)
                        .RuleFor(si => si.Url, f => $"https://picsum.photos/seed/{Guid.NewGuid()}/1200/800");

                    foreach (var service in services)
                    {
                        // Generuj losową liczbę zdjęć dla każdego serwisu (od 4 do 8)
                        var images = imageGenerator.GenerateBetween(4, 8);

                        // Przypisz zdjęcia do serwisu
                        foreach (var image in images)
                        {
                            image.ServiceId = service.Id;
                        }

                        // Dodaj wygenerowane zdjęcia do kontekstu
                        dbContext.ServiceImages.AddRange(images);
                    }

                    // Zapisz zmiany w bazie danych
                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
