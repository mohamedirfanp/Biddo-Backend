using Biddo.Models;

namespace Biddo.Services.Models
{
    public class EventDTO
    {
        public EventModelTable Event { get; set; }
        public List<SelectedServiceModel> SelectedServices { get; set; }
    }
}
