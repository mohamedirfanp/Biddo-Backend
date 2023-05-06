
using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Biddo.Models
{
    public class RatingModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RatingId { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        public string Review { get; set; }


        public int EventId { get; set; }
        
        [ForeignKey(nameof(VendorId))]
        public int VendorId { get; set; }

        public int ServiceId { get; set; }


        [ForeignKey(nameof(ServiceId))]
        public virtual SelectedServiceModel SelectedService { get; set; }

        [ForeignKey(nameof(EventId))]
        public virtual EventModelTable Event { get; set; }

    }
}
