using IllegalLibAPI.Models;

namespace IllegalLibAPI.Interfaces
{
    public interface IPublisherRepository
    {
        Task<Publisher> GetPublisherByIdAsync(int publisherId);
        Task CreatePublisherAsync();
        Task UpdatePublisherAsync(int publisherId, Publisher publisher);
        Task DeletePublisherAsync(int publisherId);
    }
}