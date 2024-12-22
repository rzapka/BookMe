namespace BookMe.Application.ServiceCategory.Dto
{
    public class ServiceCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string EncodedName { get; set; }
        public string? ImageUrl { get; set; }
        public int ServicesCount { get; set; }

        public List<Service.Dto.ServiceDto> Services { get; set; } = new();
    }
}
