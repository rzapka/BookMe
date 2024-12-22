using System.Collections.Generic;

namespace BookMe.Domain.Entities
{
    public class Employee
    {
        public int Id { get; set; }
        public int ServiceId { get; set; }
        public Service Service { get; set; } = default!;

        public string UserId { get; set; } = Guid.NewGuid().ToString();
        public ApplicationUser User { get; set; } = default!;

        public List<Opinion> Opinions { get; set; } = new();
        public List<Booking> Bookings { get; set; } = new();
    }
}
