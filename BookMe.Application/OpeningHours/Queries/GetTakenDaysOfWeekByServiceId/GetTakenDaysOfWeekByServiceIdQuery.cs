using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMe.Application.OpeningHours.Queries.GetTakenDaysOfWeekByServiceId
{
    public class GetTakenDaysOfWeekByServiceIdQuery : IRequest<List<string>>
    {
        public int ServiceId { get; set; }
    }
}
