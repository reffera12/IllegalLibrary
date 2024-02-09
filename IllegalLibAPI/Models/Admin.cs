using System.ComponentModel.DataAnnotations;

namespace IllegalLibAPI.Models
{
    public class Admin
    {
        [Required]
        public Guid AdminId { get; set; }
    }
}