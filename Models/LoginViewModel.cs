using System.ComponentModel.DataAnnotations;

namespace AdminPanel.Models;

public class LoginViewModel
{
    [Required(ErrorMessage = "Το username είναι υποχρεωτικό")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Ο κωδικός είναι υποχρεωτικός")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    public string? ReturnUrl { get; set; }
}
