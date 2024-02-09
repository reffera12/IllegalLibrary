using IllegalLibAPI.Models;

namespace IllegalLibAPI.Interfaces
{
    public interface IAuthorRepository
    {
        Task<Author> GetAuthorByIdAsync(int authorId);
        Task CreateAuthorAsync();
        Task UpdateAuthorAsync(int authorId, Author author);
        Task DeleteAuthorAsync(int authorId);
    }
}