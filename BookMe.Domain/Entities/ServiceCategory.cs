using BookMe.Domain.Utilities;
using System.Collections.Generic;

namespace BookMe.Domain.Entities
{
    public class ServiceCategory
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public string EncodedName { get; private set; } 

        public List<Service> Services { get; set; } = new();

        public void EncodeName()
        {
            EncodedName = NameEncoder.Encode(Name);
        }
    }
}
