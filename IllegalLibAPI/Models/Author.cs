using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IllegalLibAPI.Models
{
    public record Author
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AuthorId { get; init; }

        [Required]
        public string? Name { get; init; }

        [MaxLength(5000)]
        public string? Bio { get; init; }

        public ICollection<Book>? Books { get; init; }
    }
}