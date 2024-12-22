using BookMe.Domain.Entities;
using System.Threading.Tasks;


namespace BookMe.Domain.Interfaces
{
    public interface IOfferRepository
    {
        public Task<Offer> GetByEncodedNames(string serviceEncodedName, string offerEncodedName);

        public Task<Offer> GetByIdAsync(int id);

        public Task<IEnumerable<Offer>> GetByServiceIdAsync(int serviceId);
        public Task<IEnumerable<Offer>> GetByServiceEncodedNameAsync(string encodedName);

        public Task AddAsync(Offer offer);

        public Task UpdateAsync(Offer offer);

        public Task DeleteAsync(int id);

    }
}