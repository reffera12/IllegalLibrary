using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IllegalLibAPI.Models
{
    public record BookRequest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RequestId { get; init; }

        [Required]
        [MaxLength(80)]
        public string Title { get; init; }

        [Required]
        [MaxLength(50)]
        public string Author { get; init; }

        [ForeignKey("User")]
        public Guid UserId { get; init; }

        public virtual User User { get; init; }
    }
}