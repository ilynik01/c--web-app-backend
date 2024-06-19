using Microsoft.AspNetCore.Identity;

namespace WebApp.Models;

public class AppUser : IdentityUser
{
    public ICollection<Caesar> Caesars { get; set; }
    public ICollection<Vigenere> Vigeneres { get; set; }
    
}