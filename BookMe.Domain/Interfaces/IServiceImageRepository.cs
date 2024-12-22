using BookMe.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMe.Domain.Interfaces
{
    public interface IServiceImageRepository
    {
        public Task<List<ServiceImage>> GetServiceImagesByServiceIdAsync(int serviceId);

        Task<List<ServiceImage>> GetServiceImagesByEncodedNameAsync(string encodedName);

        Task<ServiceImage> GetServiceImageByIdAsync(int id);
        Task AddServiceImageAsync(ServiceImage serviceImage);
        Task UpdateServiceImageAsync(ServiceImage serviceImage);

        Task DeleteServiceImageAsync(int id);
    }
}
