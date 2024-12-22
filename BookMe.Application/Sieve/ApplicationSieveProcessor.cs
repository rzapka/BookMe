using BookMe.Domain.Entities;
using Microsoft.Extensions.Options;
using Sieve.Models;
using Sieve.Services;

namespace BookMe.Application.Sieve
{
    public class ApplicationSieveProcessor : SieveProcessor
    {
        public ApplicationSieveProcessor(IOptions<SieveOptions> options) : base(options)
        {

        }

        protected override SievePropertyMapper MapProperties(SievePropertyMapper mapper)
        {
            mapper.Property<Domain.Entities.Opinion>(o => o.Content)
                .CanFilter();

            mapper.Property<Domain.Entities.Opinion>(o => o.User.FirstName)
                .CanFilter();

            mapper.Property<Domain.Entities.Opinion>(o => o.User.LastName)
                .CanFilter();

            mapper.Property<Domain.Entities.Opinion>(o => o.Rating) 
                .CanSort();

            return mapper;
        }
    }
}
