using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IllegalLibAPI.Models
{
    public record Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookId { get; init; }

        [Required]
        [MaxLength(50)]
        public string Title { get; init; } = string.Empty;

        [Required]
        [MaxLength(4000)]
        public string Description { get; init; } = string.Empty;

        [Required]
        public string Genre { get; init; } = string.Empty;

        [Required]
        [MaxLength(1000)]
        public string CoverUrl { get; init; } = string.Empty;

        [Required]
        public int PublicationYear { get; init; }

        [Required]
        public Publisher? Publisher { get; init; }

        public Guid? BlobId { get; init; }

        [Required]
        [MinLength(1)]
        public ICollection<Author>? Authors { get; init; }

        public virtual ICollection<BookFile> BookFiles { get; init; } = new HashSet<BookFile>();
    }

}