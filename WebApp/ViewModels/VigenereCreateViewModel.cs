using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels;

public class VigenereCreateViewModel
{
    [Required(ErrorMessage = "Vigenere key is required.")]
    [MinLength(3, ErrorMessage = "Min length is 3.")]
    [MaxLength(128, ErrorMessage = "Max length is 128.")]
    [RegularExpression("^[a-z]+$", ErrorMessage = "Only lowercase letters allowed")]
    public string Key { get; set; }
    
     
    [MinLength(1, ErrorMessage = "Min length is 1.")]
    [MaxLength(4096, ErrorMessage = "Max length is 4096.")]
    [RegularExpression(@"^[ -~]+$", ErrorMessage = "Only letters, numbers, special characters")]
    public string EncryptedText { get; set; }
}