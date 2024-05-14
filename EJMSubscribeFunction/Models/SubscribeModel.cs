using System.ComponentModel.DataAnnotations;

namespace EJMSiliconBackoffice.Models
{
    public class SubscribeModel
    {
        [StringLength(100, ErrorMessage ="Email is to long, make it shorter!")]
        [Display(Name = "Email", Prompt = "Your Email", Order = 0)]
        [RegularExpression("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid Email")]
        [EmailAddress]
        public string Email { get; set; } = null!;
    }
}
