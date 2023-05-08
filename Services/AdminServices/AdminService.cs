
namespace Biddo.Services.AdminServices
{
    public class AdminService : IAdminService
    {
        private readonly BiddoContext _context;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IConfiguration _configuration;

        public AdminService(BiddoContext context, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _context = context;
            _httpContext = httpContextAccessor;
            _configuration = configuration;
        }

        public IEnumerable<object> GetAllDetails(string filter)
        {
            var choosenList = new List<object>();

            try
            {

                if(filter == "User")
                {
                    var userList = _context.UserModel.Select(user => new
                    {
                        user.UserId,
                        user.Name,
                        user.Email,
                        user.PhoneNumber
                    }).ToList();
                    choosenList.Add(userList);
                }
                else
                {
                    var vendorList = _context.VendorModel.Select(vendor => new
                    {
                        vendor.VendorId,
                        vendor.VendorName,
                        vendor.VendorEmail,
                        vendor.VendorPhoneNumber,
                        vendor.VendorLocation,
                        vendor.VendorCompanyName,
                        vendor.VendorGSTNumber
                    }).ToList();
                    //foreach(var vendor in vendorList)
                    //{
                    //    var newArray = new List<object>();
                    //    var providedService = _context.ProvidedServiceTable.Where(provide => provide.VendorId == vendor.VendorId).ToList();

                    //    newArray.Add(providedService);
                    //    newArray.Add(vendor);



                    //}
                        choosenList.Add(vendorList);
                }

                return choosenList;
            }
            catch (Exception ex)
            {
                return new List<object>().Append(ex.Message);
            }
        }
    }
}
