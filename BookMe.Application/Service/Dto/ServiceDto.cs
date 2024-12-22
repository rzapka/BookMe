using BookMe.Application.Employee.Dto;
using BookMe.Application.Offer.Dto;
using BookMe.Application.OpeningHours.Dto;

namespace BookMe.Application.Service.Dto
{
    public class ServiceDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string EncodedName { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public string? ServiceCategoryName { get; set; }
        public double AverageRating { get; set; }
        public int OpinionsCount { get; set; }

        public int? ServiceCategoryId { get; set; }
        public string City { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;

        // Collections
        public List<OfferDto> Offers { get; set; } = new();
        public List<OpeningHourDto> OpeningHours { get; set; } = new();
        public List<EmployeeDto> Employees { get; set; } = new();
    }
}
