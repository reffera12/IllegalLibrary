using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IllegalLibAPI.Models
{
    public class User
    {
        [Required]
        [Key]
        [ForeignKey("AuthUser")]
        public Guid UserId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        [Required]
        public long Downloads { get; set; } = 0;
        [MaxLength(5000)]
        public string? Bio { get; set; } = string.Empty;
        public BookRequest[] bookRequests { get; set; } = Array.Empty<BookRequest>();
        
        public AuthUser AuthUser { get; set; }
    }
}