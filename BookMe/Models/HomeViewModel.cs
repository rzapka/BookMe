using BookMe.Application.Service.Dto;
using BookMe.Application.ServiceCategory.Dto;
using System.Collections.Generic;

namespace BookMe.Models
{
    public class HomeViewModel
    {
        public IEnumerable<ServiceCategoryDto> ServiceCategories { get; set; }
        public List<ServiceDto> RecommendedServices { get; set; }
    }
}
