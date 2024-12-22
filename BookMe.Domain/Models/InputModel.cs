using BookMe.Domain.Enums;

namespace BookMe.Domain.Models
{
    public class InputModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public string Email { get; set; }
        public string? AvatarUrl { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
