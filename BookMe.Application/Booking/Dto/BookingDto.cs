namespace BookMe.Application.Booking.Dto
{
    public class BookingDto
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string UserId { get; set; }
        public string UserFullName { get; set; }
        public int EmployeeId { get; set; }
        public int OfferId { get; set; }
        public string OfferName { get; set; }
        public string ServiceName { get; set; }
        public string ServiceEncodedName { get; set; }
        
        public int ServiceId { get; set; }
        public decimal OfferPrice { get; set; }
        public int OfferDuration { get; set; }
        public string EmployeeFullName { get; set; }
        public int? OpinionId { get; set; }

        public int OpinionRating { get; set; }

        public string OpinionContent { get; set; }
    }
}
