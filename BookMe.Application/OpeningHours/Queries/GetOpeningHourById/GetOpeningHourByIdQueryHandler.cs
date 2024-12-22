using AutoMapper;
using BookMe.Application.OpeningHours.Commands.UpdateOpeningHour;
using BookMe.Application.OpeningHours.Dto;
using BookMe.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMe.Application.OpeningHours.Queries.GetOpeningHourById
{
    public class GetOpeningHourByIdQueryHandler : IRequestHandler<GetOpeningHourByIdQuery, UpdateOpeningHourCommand>
    {
        private readonly IMapper _mapper;
        private readonly IOpeningHoursRepository _repository;

        public GetOpeningHourByIdQueryHandler(IMapper mapper, IOpeningHoursRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }
        public async Task<UpdateOpeningHourCommand> Handle(GetOpeningHourByIdQuery request, CancellationToken cancellationToken)
        {
            var openingHour = await _repository.GetByIdAsync(request.Id); 
            return _mapper.Map<UpdateOpeningHourCommand>(openingHour);
        }
    }
}
