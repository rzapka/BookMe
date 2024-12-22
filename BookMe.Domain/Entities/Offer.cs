using BookMe.Domain.Utilities;
using System.Collections.Generic;

namespace BookMe.Domain.Entities
{
    public class Offer
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Duration { get; set; }
        public decimal Price { get; set; }

        public int ServiceId { get; set; }
        public Service Service { get; set; } = default!;

        public List<Booking> Bookings { get; set; } = new();

        public string EncodedName { get; private set; } = string.Empty;
        public void EncodeName()
        {
            EncodedName = NameEncoder.Encode(Name);
        }
    }
}
