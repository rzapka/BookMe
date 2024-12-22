using BookMe.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookMe.Domain.Interfaces
{
    public interface IOpeningHoursRepository
    {
        Task<IEnumerable<OpeningHour>> GetByServiceIdAsync(int serviceId);
        Task<OpeningHour> GetByIdAsync(int id);
        Task AddAsync(OpeningHour openingHour);
        Task UpdateAsync(OpeningHour openingHour);
        Task DeleteAsync(int id);
        Task<IEnumerable<OpeningHour>> GetOpeningHoursByServiceEncodedNameAsync(string encodedName);

        Task<List<string>> GetTakenDaysOfWeekByServiceId(int serviceId);
    }
}
