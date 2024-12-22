using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMe.Domain.Entities
{
    [Owned]
    public class ServiceContactDetails
    {
        public string City { get; set; } = string.Empty;

        public string Street { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string PostalCode { get; set; } = string.Empty;
    }
}
