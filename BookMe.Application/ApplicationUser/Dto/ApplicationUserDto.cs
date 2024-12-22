using BookMe.Domain.Enums;

namespace BookMe.Application.ApplicationUser.Dto
{
    public class ApplicationUserDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? AvatarUrl { get; set; }
        public bool IsAdmin { get; set; } = false;
        public Gender Gender { get; set; }
        public string Password { get; set; } 
        public string ConfirmPassword { get; set; } 
        public string? NewPassword {  get; set; }
    }
}
