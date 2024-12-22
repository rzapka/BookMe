using Bogus;
using BookMe.Infrastructure.Persistence;
using BookMe.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookMe.Infrastructure.Seeders
{
    public class OfferSeeder : ISeeder
    {
        private readonly BookMeDbContext dbContext;

        public OfferSeeder(BookMeDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task Seed()
        {
            if (await dbContext.Database.CanConnectAsync())
            {
                if (!dbContext.Offers.Any())
                {
                    var services = dbContext.Services.Include(s => s.ServiceCategory).ToList();

                    // Mapowanie kategorii na przykładowe nazwy ofert
                    var categoryToOfferNames = new Dictionary<string, List<string>>
                    {
                        { "Fryzjer", new List<string> { "Strzyżenie męskie", "Strzyżenie damskie", "Farbowanie włosów", "Modelowanie włosów" } },
                        { "Barber Shop", new List<string> { "Strzyżenie brody", "Strzyżenie włosów", "Pakiet: broda + włosy", "Odżywianie brody" } },
                        { "Trening i dieta", new List<string> { "Konsultacja dietetyczna", "Plan treningowy", "Trening personalny", "Analiza składu ciała" } },
                        { "Masaż", new List<string> { "Masaż relaksacyjny", "Masaż leczniczy", "Masaż sportowy", "Masaż gorącymi kamieniami" } },
                        { "Fizjoterapia", new List<string> { "Rehabilitacja kręgosłupa", "Terapia manualna", "Trening rehabilitacyjny", "Kinesiotaping" } },
                        { "Salon Kosmetyczny", new List<string> { "Peeling kawitacyjny", "Mezoterapia igłowa", "Mikrodermabrazja", "Zabieg nawilżający" } },
                        { "Tatuaż i Piercing", new List<string> { "Tatuaż mały", "Tatuaż średni", "Piercing nosa", "Piercing ucha" } },
                        { "Stomatolog", new List<string> { "Wypełnienie zęba", "Czyszczenie kamienia", "Implant zębowy", "Leczenie kanałowe" } },
                        { "Medycyna estetyczna", new List<string> { "Botox", "Wypełnienie kwasem hialuronowym", "Zabieg laserowy", "Lifting twarzy" } },
                        { "Paznokcie", new List<string> { "Manicure hybrydowy", "Pedicure SPA", "Przedłużanie paznokci", "Zdobienie paznokci" } },
                        { "Brwi i rzęsy", new List<string> { "Henna brwi", "Przedłużanie rzęs", "Laminacja rzęs", "Regulacja brwi" } },
                        { "Makijaż", new List<string> { "Makijaż ślubny", "Makijaż wieczorowy", "Makijaż dzienny", "Kurs makijażu" } },
                        { "Depilacja", new List<string> { "Depilacja woskiem", "Depilacja laserowa", "Depilacja cukrowa", "Depilacja twarzy" } },
                        { "Inne", new List<string> { "Konsultacja ogólna", "Usługa specjalna", "Dostosowana usługa", "Porada ekspertów" } }
                    };

                    var offers = new List<Offer>();
                    var locale = "pl";

                    foreach (var service in services)
                    {
                        var categoryName = service.ServiceCategory?.Name ?? "Inne";
                        if (!categoryToOfferNames.TryGetValue(categoryName, out var offerNames))
                        {
                            offerNames = categoryToOfferNames["Inne"]; // Domyślna lista nazw dla kategorii "Inne"
                        }

                        // Lista nazw już użytych w tym serwisie
                        var usedOfferNames = new HashSet<string>();

                        var offerGenerator = new Faker<Offer>(locale)
                            .RuleFor(o => o.Name, f =>
                            {
                                var availableNames = offerNames.Except(usedOfferNames).ToList();
                                if (!availableNames.Any())
                                {
                                    return null; // Brak dostępnych nazw
                                }
                                var selectedName = f.PickRandom(availableNames);
                                usedOfferNames.Add(selectedName); // Dodanie do użytych nazw
                                return selectedName;
                            })
                            .RuleFor(o => o.Duration, f => GetRandomNumberInRangeDivisibleBy10(20, 90))
                            .RuleFor(o => o.Price, f => GetRandomNumberInRangeDivisibleBy10(20, 300))
                            .RuleFor(o => o.ServiceId, f => service.Id);

                        // Generowanie od 3 do 6 ofert dla każdej usługi
                        var generatedOffers = offerGenerator.Generate(_random.Next(3, Math.Min(offerNames.Count, 7)));

                        foreach (var offer in generatedOffers.Where(o => o.Name != null)) // Ignoruj oferty bez nazwy
                        {
                            offer.EncodeName();
                            offers.Add(offer);
                        }
                    }

                    dbContext.Offers.AddRange(offers);
                    await dbContext.SaveChangesAsync();
                }
            }
        }

        private int GetRandomNumberInRangeDivisibleBy10(int min, int max)
        {
            if (min > max)
            {
                int temp = min;
                min = max;
                max = temp;
            }

            Random random = new Random();

            int randomNumber = random.Next(min, max + 1);

            int result = (randomNumber / 10) * 10;

            return result;
        }

        private static readonly Random _random = new Random();
    }
}
