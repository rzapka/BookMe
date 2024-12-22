namespace BookMe.Domain.Entities
{
    public class OpeningHour
    {
        public int Id { get; set; }
        public string DayOfWeek { get; set; } = string.Empty;
        public TimeSpan OpeningTime { get; set; }
        public TimeSpan ClosingTime { get; set; }
        public bool Closed { get; set; }  

        public int ServiceId { get; set; }
        public Service Service { get; set; } = default!;
    }
}
