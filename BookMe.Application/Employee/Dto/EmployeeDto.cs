using BookMe.Domain.Enums;

namespace BookMe.Application.Employee.Dto
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }

        public Gender Gender { get; set; }

        public string Password { get; set; } 

        public string? AvatarUrl { get; set; }

        public string FullName { get; set; }
    }
}
