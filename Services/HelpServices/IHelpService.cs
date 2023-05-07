using Biddo.Models;
using Biddo.Services.Models;

namespace Biddo.Services.HelpServices
{
    public interface IHelpService
    {
        IEnumerable<QueryModel> GetAllTickets();

    }
}
