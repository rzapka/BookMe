namespace BookMe.Application.OpeningHours.Dto
{
    public class OpeningHourDto
    {
        public int Id { get; set; }
        public string DayOfWeek { get; set; }
        public TimeSpan OpeningTime { get; set; }
        public TimeSpan ClosingTime { get; set; }
        public bool Closed { get; set; }  
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string ServiceEncodedName { get; set; }
    }
}
