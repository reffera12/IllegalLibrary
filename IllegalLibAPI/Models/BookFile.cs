using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IllegalLibAPI.Models
{
    public class BookFile
    {
        [Required]
        [Key]
        public int FileId { get; set; }
        [Required]
        [ForeignKey("Book")]
        public int BookId { get; set; }
        [Required]
        [RegularExpression("^(pdf|epub|fb2)$", ErrorMessage = "Invalid file type.")]
        public string FileType { get; set; }
        public byte[] FileContent { get; set; }

        public virtual Book Book { get; set; }

    }
}