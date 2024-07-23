using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IllegalLibAPI.Models
{
    public record User
    {
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid UserId { get; init; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        [Required]
        public long Downloads { get; set; } = 0;

        [MaxLength(5000)]
        public string? Bio { get; set; } = string.Empty;

        public BookRequest[] BookRequests { get; set; } = Array.Empty<BookRequest>();

        [Required]
        public AuthUser AuthUser { get; set; }

        public User() { }

        public User(Guid userId, string firstName, string lastName, AuthUser authUser)
        {
            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
            AuthUser = authUser;
        }
    }
}