namespace BookMe.Application.Offer.Dto
{
    public class OfferDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Duration { get; set; }
        public decimal Price { get; set; }
        public int ServiceId { get; set; }

        public string ServiceName { get; set; }
        public string EncodedName { get; set; }

        public string ServiceEncodedName { get; set; }

        public string EmployeeFullName { get; set; } 
    }
}
