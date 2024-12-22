using BookMe.Application.Service.Dto;
using MediatR;

public class SearchServicesByNameQuery : IRequest<IEnumerable<ServiceDto>>
{
    public string SearchTerm { get; set; }
}
