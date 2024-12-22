using System;

namespace BookMe.Domain.Entities
{
    public class Opinion
    {
        public int Id { get; set; }
        public int ServiceId { get; set; }
        public Service Service { get; set; } = default!;

        public int Rating { get; set; }
        public string Content { get; set; } = string.Empty;

        public string UserId { get; set; }
        public ApplicationUser User { get; set; } = default!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int OfferId { get; set; }
        public Offer Offer { get; set; } = default!;

        public int? EmployeeId { get; set; }
        public Employee? Employee { get; set; } 

        public int BookingId { get; set; }
        public Booking Booking { get; set; } = default!;
    }
}
