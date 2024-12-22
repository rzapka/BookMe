using BookMe.Domain.Utilities;
using System;
using System.Collections.Generic;

namespace BookMe.Domain.Entities
{
    public class Service
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ServiceContactDetails ContactDetails { get; set; } = default!;
        public int? ServiceCategoryId { get; set; }
        public ServiceCategory? ServiceCategory { get; set; }

        public string EncodedName { get; private set; } = string.Empty!;
        public void EncodeName()
        {
            EncodedName = NameEncoder.Encode(Name);
        }

        public string? ImageUrl { get; set; }

        public List<Offer> Offers { get; set; } = new();
        public List<Opinion> Opinions { get; set; } = new();
        public List<OpeningHour> OpeningHours { get; set; } = new();
        public List<Employee> Employees { get; set; } = new();
        public List<ServiceImage> ServiceImages { get; set; } = new();
        public List<Booking> Bookings { get; set; } = new();

        public double OpinionsCount { get; set; } = 0;
        public double AverageRating { get; set; } = 0;
    }
}
