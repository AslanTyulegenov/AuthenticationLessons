using System.ComponentModel.DataAnnotations;

namespace Authorization.IdentityServer.Models;

public class LoginViewModel
{
    [Required]
    public string UserName { get; set; }
    [Required]
    public string Password { get; set; }
    [Required]
    public string ReturnUrl { get; set; }
}
