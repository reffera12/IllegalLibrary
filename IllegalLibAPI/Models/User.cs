using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IllegalLibAPI.Models
{
    public record User
    {
        [Required]
        [Key]
        [ForeignKey("AuthUser")]
        public Guid UserId { get; init; }

        public string FirstName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;

        [Required]
        public long Downloads { get; init; } = 0;

        [MaxLength(5000)]
        public string? Bio { get; init; } = string.Empty;

        public BookRequest[] BookRequests { get; init; } = Array.Empty<BookRequest>();

        [Required]
        public AuthUser AuthUser { get; init; }
    }
}