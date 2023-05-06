using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Biddo.Models
{
    public class UserModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string? Name { get; set; }

        [Required]
        [StringLength(50)]
        public string Email { get; set; } = null!;

        [Required]
        [StringLength(10)]
        public string PhoneNumber { get; set; }

        [Required]
        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }
   
    }
}
