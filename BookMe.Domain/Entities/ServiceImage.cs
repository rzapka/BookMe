using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMe.Domain.Entities
{
    public class ServiceImage
    {
        public int Id { get; set; }

        public string Url { get; set; } = string.Empty;

        public int ServiceId { get; set; }

        public Service Service { get; set; } = default!;
    }
}
