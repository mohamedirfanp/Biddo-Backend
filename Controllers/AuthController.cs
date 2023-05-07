
using Microsoft.AspNetCore.Mvc;
using Biddo.Models;
using Microsoft.AspNetCore.Authorization;
using Biddo.Services.Models;

namespace Biddo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController( IAuthService authService)
        {
            _authService = authService;
        }


        // To create a User
        [HttpPost("userSignup")]
        public async Task<IActionResult> CreateUser([FromBody] UserSignupDto user)
        {
            var response =  _authService.CreateUser(user);

            if(response is BadRequestObjectResult badRequest)
            {
                return BadRequest(badRequest.Value);
            }

            return Ok(response);
        }

        // To create a Vendor
        [HttpPost("vendorSignup")]
        public async Task<IActionResult> CreateVendor([FromBody] VendorSignDto vendor)
        {
            var response = _authService.CreateVendor(vendor);
            if (response is BadRequestObjectResult badRequest)
            {
                return BadRequest(badRequest.Value);
            }

            return Ok(response);

        }

        // User Login
        [HttpPost("userlogin")]
        public async Task<IActionResult> UserLogin([FromBody] LoginDto request)
        {
            var response = _authService.UserLogin(request);
            if (response is BadRequestObjectResult badRequest)
            {
                return BadRequest(badRequest.Value);
            }

            return Ok(response);
        }

        // Vendor Login
        [HttpPost("vendorlogin")]
        public async Task<IActionResult> VendorLogin([FromBody] LoginDto request)
        {
            var response = _authService.VendorLogin(request);
            if (response is BadRequestObjectResult badRequest)
            {
                return BadRequest(badRequest.Value);
            }

            return Ok(response);
        }

        // Change Password Method
        [HttpPost("change-password"), Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePwdDto request)
        {
            var response = _authService.ChangePassword(request);
            if (response is BadRequestObjectResult badRequest)
            {
                return BadRequest(badRequest.Value);
            }

            return Ok(response);
        }

        // To Get the Profile
        [HttpGet("profile"), Authorize]
        public async Task<object> GetProfile()
        {
            var response = _authService.GetProfile();
            if (response is BadRequestObjectResult badRequest)
            {
                return BadRequest(badRequest.Value);
            }

            return Ok(response);
        }


        // Update Profile for User
        [HttpPost("user/profile"), Authorize(Roles = "User")]
        public async Task<IActionResult> UpdateProfileForUser([FromBody] UserSignupDto request)
        {
            var response = _authService.UpdateProfileForUser(request);
            if (response is BadRequestObjectResult badRequest)
            {
                return BadRequest(badRequest.Value);
            }

            return Ok(response);
        }

        // Update Profile for Vendor
        [HttpPost("vendor/profile"), Authorize(Roles = "Vendor")]
        public async Task<IActionResult> UpdateProfileForVendor([FromBody] VendorSignDto request)
        {
            Console.WriteLine("HERE : " + request.PhoneNumber);
            var response = _authService.UpdateProfileForVendor(request);
            if (response is BadRequestObjectResult badRequest)
            {
                return BadRequest(badRequest.Value);
            }

            return Ok(response);
        }


        
    }
}
