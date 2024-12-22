using BookMe.Application.Employee.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMe.Application.Offer.Dto
{
    public class OfferDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Duration { get; set; }
        public string ServiceName { get; set; }
        public List<EmployeeDto> Employees { get; set; }
    }
}
