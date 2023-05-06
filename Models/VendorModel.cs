using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Biddo.Models
{
    public class VendorModel
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VendorId { get; set; }

        [Required]
        public string VendorName { get; set; }

        [Required]
        public string VendorEmail { get; set; }

        [Required]
        public string VendorPhoneNumber { get; set;}

        [Required]
        public string VendorCompanyName { get; set; }

        [Required]
        public string VendorGSTNumber { get; set;}

        [Required]
        public string VendorLocation { get; set;}

        public DateTime VendorCreatedAt { get; set;}

        [Required]
        public byte[] PasswordHash { get; set;}

        public byte[] PasswordSalt { get; set;}

    }
}
