namespace Biddo.Services.AdminServices
{
    public interface IAdminService
    {
        IEnumerable<object> GetAllDetails(string filter);

    }
}
