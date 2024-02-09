using IllegalLibAPI.Models;

namespace IllegalLibAPI.Interfaces
{
    public interface IBookFileRepository
    {
        Task<IEnumerable<BookFile>> GetFilesByBookIdAsync(int bookId);
        Task<BookFile> GetBookFileByIdAsync(int fileId);
        Task AddBookFileAsync(BookFile bookFile);
        Task UpdateBookFileAsync(BookFile bookFile);
        Task DeleteBookFileAsync(int fileId);
    }
}