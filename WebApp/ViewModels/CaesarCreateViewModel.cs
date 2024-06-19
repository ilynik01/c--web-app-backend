using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels;


// Put here data only necessary for the view
public class CaesarCreateViewModel
{
    
    [Required(ErrorMessage = "Caesar key is required.")]
    [MinLength(1, ErrorMessage = "Min length is 1.")]
    [MaxLength(1, ErrorMessage = "Max length is 1.")]
    [RegularExpression("^[a-z]$", ErrorMessage = "Should be a lowercase letter")]
    public string Key { get; set; }
    
    
    [MinLength(1, ErrorMessage = "Min length is 1.")]
    [MaxLength(4096, ErrorMessage = "Max length is 4096.")]
    [RegularExpression(@"^[ -~]+$", ErrorMessage = "Only letters, numbers, special characters")]
    public string EncryptedText { get; set; }
}