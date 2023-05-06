using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Biddo.Models
{
    public class ProvidedServiceModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProvidedSericeID { get; set; }

        [Required]
        [StringLength(50)]
        public string ServiceName { get; set; }

        public int UtilizedCount { get; set; }

        public int VendorId { get; set; }

        [ForeignKey(nameof(VendorId))]
        public virtual VendorModel Vendor { get; set; }

    }
}
