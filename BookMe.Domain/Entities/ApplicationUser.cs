using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BookMe.Domain.Attributes;
using Microsoft.AspNetCore.Identity;
using BookMe.Domain.Enums;

namespace BookMe.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public override string Email { get; set; } = string.Empty;

        public Gender Gender { get; set; }

        public bool IsAdmin { get; set; } = false;

        public List<Opinion> Opinions { get; set; } = default!;

        public string? AvatarUrl { get; set; }

        public List<Booking> Bookings { get; set; } = new();

        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";
    }
}
