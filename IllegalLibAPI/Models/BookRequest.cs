using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IllegalLibAPI.Models
{
    public class BookRequest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RequestId { get; set; }
        [Required]
        [MaxLength(80)]
        public string Title { get; set; }
        [Required]
        [MaxLength(50)]
        public string Author { get; set; }
        [ForeignKey("User")]
        public Guid UserId { get; set; }

        public User User { get; set;}
    }
}