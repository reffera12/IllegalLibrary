using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IllegalLibAPI.Models
{
    public class Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookId { get; set; }
        [Required]
        [MaxLength(50)]
        public string Title { get; set; } = string.Empty;
        [Required]
        [MaxLength(4000)]
        public string Description { get; set; } = string.Empty;
        [Required]
        public string Genre { get; set; } = string.Empty;
        [Required]
        [MaxLength(1000)]
        public string CoverUrl { get; set; } = string.Empty;
        [Required]
        public int PublicationYear { get; set; }
        [Required]
        public Publisher? Publisher { get; set; }
        public Guid? BlobId { get; set; }

        [Required]
        [MinLength(1)]
        public ICollection<Author>? Authors { get; set; }

        public virtual ICollection<BookFile> BookFiles { get; set; } = new HashSet<BookFile>();
    }
}