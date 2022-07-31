using Microsoft.AspNetCore.Identity;

namespace Authorization.Database.Entity;

public class ApplicationUser : IdentityUser<Guid>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}
