using BookMe.Application.Service.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMe.Application.Service.Queries.GetServiceByEncodedName
{
    public class GetServiceByEncodedNameQuery : IRequest<ServiceDto>
    {
        public string EncodedName { get; set; }

        public GetServiceByEncodedNameQuery(string encodedName)
        {
            EncodedName = encodedName;
        }
    }
}
