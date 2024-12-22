using BookMe.Application.Service.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMe.Application.Service.Queries.GetServiceDetailsByEncodedName
{
    public class GetServiceDetailsByEncodedNameQuery : IRequest<ServiceDto>
    {
        public string EncodedName { get; set; }

        public GetServiceDetailsByEncodedNameQuery(string encodedName)
        {
            EncodedName = encodedName;
        }
    }
}
