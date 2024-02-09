using IllegalLibAPI.Models;

namespace IllegalLibAPI.Interfaces
{
    public interface IBookReqRepository
    {
        Task<IEnumerable<BookRequest>> GetBookRequestsAsync();
        Task<BookRequest> GetBookRequestByIdAsync(int requestId);
        Task CreateBookRequestAsync(Guid userId);
        Task DeleteBookRequestAsync(int requestId);
    }
}