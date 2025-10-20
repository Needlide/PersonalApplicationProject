using System.ComponentModel.DataAnnotations;

namespace PersonalApplicationProject.DTOs;

public class RegisterRequestDto
{
    [Required, EmailAddress]
    public string Email { get; set; }

    [Required, MinLength(8)]
    public string Password { get; set; }

    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }
}