using BookMe.Application.Employee.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMe.Application.Employee.Queries.GetEmployeesAsDictionary
{
    public class GetEmployeesAsDictionaryQuery : IRequest<Dictionary<int, string>>
    {
        public string ServiceEncodedName { get; set; } = string.Empty;
    }
}
