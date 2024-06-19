using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace WebApp.Models;

public class Vigenere
{
    public Guid Id { get; set; }
    
    [Required(ErrorMessage = "Vigenere key is required.")]
    [MinLength(3, ErrorMessage = "Min length is 3.")]
    [MaxLength(128, ErrorMessage = "Max length is 128.")]
    [RegularExpression("^[a-z]+$", ErrorMessage = "Only lowercase letters allowed")]
    public string Key { get; set; }
    
    [MinLength(1, ErrorMessage = "Min length is 1.")]
    [MaxLength(4096, ErrorMessage = "Max length is 4096.")]
    [RegularExpression(@"^[ -~]+$", ErrorMessage = "Only letters, numbers, special characters")]
    public string EncryptedText { get; set; }
    
    
    [MaxLength(64)]
    public string AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
    
}