using Biddo.Services.AdminServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Biddo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService) 
        {
            _adminService = adminService;
        }


        [HttpGet("{filter}"), Authorize(Roles = "Admin")]
        public async Task<IEnumerable<object>> GetDetails(string filter)
        {
            var response = _adminService.GetAllDetails(filter);

            return response;
        }
    }
}
