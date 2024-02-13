using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IllegalLibAPI.Models
{
    public record BookFile
    {
        [Required]
        [Key]
        public int FileId { get; init; }
        [Required]
        [ForeignKey("Book")]
        public int BookId { get; init; }
        [Required]
        [RegularExpression("^(pdf|epub|fb2)$", ErrorMessage = "Invalid file type.")]
        public string FileType { get; init; }
        public byte[] FileContent { get; init; }

        public virtual Book Book { get; init; }
    }
}