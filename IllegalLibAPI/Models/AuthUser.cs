using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace IllegalLibAPI.Models
{
    public record AuthUser
    {
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid UserId { get; init; }
        [Required]
        [MaxLength(100, ErrorMessage = "Please enter a username below 100 characters")]
        public string Username { get; init; }
        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{10,65}$")]
        public string Password { get; init; }
        [Required]
        [EmailAddress]
        public string Email { get; init; }
        [MinLength(128)]
        public string ResetToken { get; init; }
        public string JwtToken { get; init; }
        [JsonIgnore]
        public string RefreshToken { get; init; }

        public User? User
        { get; init; }
    }
}