using IllegalLibAPI.Models;

namespace IllegalLibAPI.Interfaces
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetBooksAsync();
        Task<Book> GetBookByIdAsync(int bookId);
        Task<Book> CreateBookAsync();
        Task<Book> UpdateBookAsync(int bookId, Book book);
        Task<Book> DeleteBookAsync(int bookId);
    }
}