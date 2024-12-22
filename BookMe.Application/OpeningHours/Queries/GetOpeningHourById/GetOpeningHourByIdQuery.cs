using BookMe.Application.OpeningHours.Commands.UpdateOpeningHour;
using BookMe.Application.OpeningHours.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMe.Application.OpeningHours.Queries.GetOpeningHourById
{
    public class GetOpeningHourByIdQuery : IRequest<UpdateOpeningHourCommand>
    {
        public int Id { get; set; }

    }
}
