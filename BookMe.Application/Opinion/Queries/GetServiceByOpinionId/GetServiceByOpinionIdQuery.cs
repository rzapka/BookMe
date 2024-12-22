using AutoMapper;
using BookMe.Application.Service.Dto;
using BookMe.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMe.Application.Opinion.Queries.GetServiceByOpinionId
{
    public class GetServiceByOpinionIdQuery : IRequest<ServiceDto>
    {

        public int Id { get; set; }
    }
}
