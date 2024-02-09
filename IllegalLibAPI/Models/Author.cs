using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IllegalLibAPI.Models
{
    public class Author
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AuthorId { get; set; }
        [Required]
        public string? Name { get; set; }
        [MaxLength(5000)]
        public string? Bio { get; set; }
        public ICollection<Book>? Books { get; set; }
    }
}