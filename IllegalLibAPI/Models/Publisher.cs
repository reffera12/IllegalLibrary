using System.ComponentModel.DataAnnotations;

namespace IllegalLibAPI.Models
{
    public class Publisher
    {
        [Required]
        [Key]
        public int PublisherId { get; set; }
        [Required]
        public string Name { get; set; }
        
    }
}