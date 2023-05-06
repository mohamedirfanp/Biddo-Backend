using Biddo.Services.Models;
using Microsoft.AspNetCore.Mvc;

namespace Biddo.Services.AuthServices
{
    public interface IAuthService
    {
        // Signup 
        IActionResult CreateUser(UserSignupDto user);
        IActionResult CreateVendor(VendorSignDto vendor);

        // User Login
        IActionResult UserLogin(LoginDto request);

        // Vendor Login 
        IActionResult VendorLogin(LoginDto request);

        // Change Password
        IActionResult ChangePassword(ChangePwdDto request);

        // To Get Profile
        object GetProfile();

        // Update Profile for User
        IActionResult UpdateProfileForUser(UserSignupDto user);

        // Update Profile for Vendor
        IActionResult UpdateProfileForVendor(VendorSignDto vendor);




    }
}
