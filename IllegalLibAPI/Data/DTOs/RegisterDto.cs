using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class RegisterDTO
{
    [Required]
    [MaxLength(100)]
    public string UserName { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$",
    ErrorMessage = "Password must contain at least 8 characters, one letter, one number, and one special character.")]
    public string Password { get; set; }

    [Required]
    [NotMapped]
    [Compare("Password", ErrorMessage = "Password does not match")]
    public string ConfirmPassword { get; set; }

    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string LastName { get; set; }
}