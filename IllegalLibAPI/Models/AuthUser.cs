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
        public string Password { get; set; }
        [Required]
        [EmailAddress]  
        public string Email { get; set; }
        [MinLength(128)]
        public string ResetToken { get; init; }
        public string JwtToken { get; set; }
        [JsonIgnore]
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }

        public User? User
        { get; set; }

        public AuthUser() { }

        public AuthUser(string username, string password, string email){
            Username = username;
            Password =  password;  
            Email = email;
        }
    }
}