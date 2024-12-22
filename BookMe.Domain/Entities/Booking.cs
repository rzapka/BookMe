using System;
using System.Collections.Generic;

namespace BookMe.Domain.Entities
{
    public class Booking
    {
        public int Id { get; set; }
        public string UserId { get; set; } = default!;

        public int? EmployeeId { get; set; }

        public Employee? Employee { get; set; }
        public int OfferId { get; set; }
        public Offer Offer { get; set; } = default!;

        public ApplicationUser User { get; set; } = default!;

        public Service Service { get; set; } = default!;

        public int ServiceId { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public void SetEndTime()
        {
            if (Offer != null)
            {
                EndTime = StartTime.AddMinutes(Offer.Duration);
            }
        }

        public virtual Opinion Opinion { get; set; } = default!;
    }
}
