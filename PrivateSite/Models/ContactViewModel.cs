using System.ComponentModel.DataAnnotations; //Grants access to annotations for validation

namespace PrivateSite.Models
{
    public class ContactViewModel
    {
        [Required(ErrorMessage = "*Name is required")] //Makes the field required
        public string Name { get; set; }

        [Required(ErrorMessage = "*Email is required")]
        [DataType(DataType.EmailAddress)] //Certain formatting is expected (@ symbol, .com, ect...)
        public string Email { get; set; }

        [Required(ErrorMessage = "*Subject is required")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "*Message is required")]
        [DataType(DataType.MultilineText)] //Makes the textbox for this field bigger
        public string Message { get; set; }
    }
}

