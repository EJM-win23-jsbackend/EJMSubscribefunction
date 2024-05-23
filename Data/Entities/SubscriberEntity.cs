using System.ComponentModel.DataAnnotations;

namespace Data.Entities
{
    public class SubscriberEntity
    {
        [Key]
        public string Id {  get; set; } = Guid.NewGuid().ToString();    

        [Required]
        public string Email { get; set; } = null!;

        public string? UserId { get; set; }
    }
}
