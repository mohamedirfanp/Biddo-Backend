using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Biddo.Models
{
    public class SelectedServiceModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SelectedServiceId { get; set; }

        public int EventId { get; set; }

        public string SelectServiceName { get; set; }

        public int MinBudget { get; set; }

        public int MaxBudget { get; set; }

        public bool IsServiceCompleted { get; set; }

        [ForeignKey("EventId")]
        public EventModelTable? Event;

    }
}
