using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace BookMe.Domain.Attributes
{
    public class ValidImageUrlAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var url = value as string;

            if (string.IsNullOrEmpty(url))
            {
                return ValidationResult.Success;
            }

            if (Uri.TryCreate(url, UriKind.Absolute, out Uri? uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps))
            {
          
                var imageExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".svg", ".webp" };
                if (Regex.IsMatch(uriResult.AbsolutePath, @"(\.jpg|\.jpeg|\.png|\.gif|\.bmp|\.svg|\.webp)$", RegexOptions.IgnoreCase))
                {
                    return ValidationResult.Success;
                }
                return new ValidationResult("URL musi prowadzić do pliku graficznego.");
            }

            return new ValidationResult("Podaj poprawny URL.");
        }
    }
}
