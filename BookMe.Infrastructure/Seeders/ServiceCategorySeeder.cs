using BookMe.Domain.Entities;
using BookMe.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMe.Infrastructure.Seeders
{
    public class ServiceCategorySeeder : ISeeder
    {
        private readonly BookMeDbContext dbContext;

        public ServiceCategorySeeder(BookMeDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task Seed()
        {
            if (await dbContext.Database.CanConnectAsync())
            {
                if (!dbContext.ServiceCategories.Any())
                {
                    var categories = new List<ServiceCategory>
                    {
                       new ServiceCategory {Name = "Barber Shop", ImageUrl = "https://gyazo.com/069e951e943159bb845445c9251ba0a5.png"},
                       new ServiceCategory {Name = "Fryzjer", ImageUrl = "https://i.gyazo.com/9b2ac4fadd55a64ba2eb6ffef6543b4e.png"},
                       new ServiceCategory {Name = "Trening i dieta", ImageUrl = "https://i.gyazo.com/3f1f3d8822bdc87c58115c7717484d4d.png"},
                       new ServiceCategory {Name = "Masaż", ImageUrl = "https://i.gyazo.com/2b65f423ed8d24cf5318b9bd6a0f89ce.png"},
                       new ServiceCategory {Name = "Fizjoterapia", ImageUrl = "https://i.gyazo.com/b3cf764fcf72322f3b3029ea003ad5aa.png"},
                       new ServiceCategory {Name = "Salon Kosmetyczny", ImageUrl = "https://i.gyazo.com/47582492dc8f12eb8704ed54bea77856.png"},
                       new ServiceCategory {Name = "Tatuaż i Piercing", ImageUrl = "https://i.gyazo.com/839ffb518bed761d5bb12358ecb7c81d.png"},
                       new ServiceCategory {Name = "Stomatolog", ImageUrl = "https://i.gyazo.com/8cf51bc73202bbe0c645d98f5992d0e3.png"},
                       new ServiceCategory {Name = "Medycyna estetyczna", ImageUrl = "https://i.gyazo.com/8736a23d287dd7526367768162abf724.png"},
                       new ServiceCategory {Name = "Paznokcie", ImageUrl = "https://i.gyazo.com/933229eb1f2a200598f6d45b030ca7a5.png"},
                       new ServiceCategory {Name = "Brwi i rzęsy", ImageUrl = "https://i.gyazo.com/a4ff013ac68b243a01ef5bafb7665360.png"},
                       new ServiceCategory {Name = "Makijaż", ImageUrl = "https://i.gyazo.com/95e0e680712f2ade8d861dff73d420b3.png"},
                       new ServiceCategory {Name = "Depilacja", ImageUrl = "https://i.gyazo.com/4453a39178210293f1414ccf6ad702db.png"},
                       //new ServiceCategory {Name = "Inne", ImageUrl = "https://i.gyazo.com/a60c5545bc9c8f2b9599cd02949767b8.png"},

                    };
                    foreach (var category in categories)
                    {
                        category.EncodeName();
                        dbContext.ServiceCategories.Add(category);
                    }
                    
                    await dbContext.SaveChangesAsync();
                  
                }
            }
        }
    }
}
