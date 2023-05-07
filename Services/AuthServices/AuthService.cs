using Biddo.Models;
using Biddo.Services.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Biddo.Services.AuthServices
{
    public class AuthService : IAuthService
	{
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly BiddoContext _context;
		private readonly IConfiguration _configuration;

		//private Dictionary<string, string> responseObject = new Dictionary<string, string>() { { "response" , string.Empty}, { "error", string.Empty} };

		public AuthService(BiddoContext context, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
		{
			_httpContextAccessor = httpContextAccessor;
			_context = context;
			_configuration = configuration;
		}

		// To Get the Current User ID
		public int GetCurrentUserId()
		{
			var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
			return int.Parse(userId);
		}

		// Check User Already Exists
		private bool UserAlreadyExist(string userEmail)
		{
			bool userAlreadyExistResponse = (bool)_context.UserModel?.Any(user => user.Email == userEmail);
			return userAlreadyExistResponse;
		}
		// Check Vendor Already Exists
		private bool VendorAlreadyExist(string vendorEmail)
		{
			return (_context.VendorModel?.Any(vendor => vendor.VendorEmail == vendorEmail)).HasValue;
		}

		// JWT Token Creation
		private string CreateToken(string userId, string role)
		{
			List<Claim> claims = new List<Claim>
			{
				new Claim(ClaimTypes.NameIdentifier, userId),
				new Claim(ClaimTypes.Role, role)
			};

			var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
				_configuration.GetSection("AppSettings:Token").Value));

			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

			var token = new JwtSecurityToken(
				claims: claims,
				expires: DateTime.Now.AddDays(1),
				signingCredentials: creds);

			var jwt = new JwtSecurityTokenHandler().WriteToken(token);

			return jwt;
		}

		// Password Hash Generate
		private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
		{
			using (var hmac = new HMACSHA512())
			{
				passwordSalt = hmac.Key;
				passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
			}
		}

		// Password Verification
		private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
		{
			using (var hmac = new HMACSHA512(passwordSalt))
			{
				var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
				return computedHash.SequenceEqual(passwordHash);
			}
		}


		// Creating a User
		public IActionResult CreateUser(UserSignupDto user)
		{
			try
			{

				if(UserAlreadyExist(user.Email))
				{
					return new BadRequestObjectResult("User Already Exist");
				}
				CreatePasswordHash(user.Password, out byte[] passwordHash, out byte[] passwordSalt);

				UserModel newUser = new UserModel();

				newUser.Email = user.Email;
				newUser.PhoneNumber = user.PhoneNumber;
				newUser.Name = user.Name;
				newUser.PasswordHash = passwordHash;
				newUser.PasswordSalt = passwordSalt;

				_context.UserModel.Add(newUser);
				_context.SaveChanges();

				return new OkObjectResult("Successfully Registered");
			}
			catch (Exception ex)
			{
				return new BadRequestObjectResult(ex.Message);
			}

		}
		
		// Creating a Vendor
		public IActionResult CreateVendor(VendorSignDto vendor)
		{
			try
			{

				if (UserAlreadyExist(vendor.Email))
				{
					return new BadRequestObjectResult("Vendor Already Exist");
				}

				CreatePasswordHash(vendor.Password, out byte[] passwordHash, out byte[] passwordSalt);
				VendorModel newVender = new VendorModel();
				newVender.VendorName = vendor.Name;
				newVender.VendorEmail = vendor.Email;
				newVender.VendorGSTNumber = vendor.CompanyGSTNumber;
				newVender.VendorCompanyName = vendor.CompanyName;
				newVender.VendorPhoneNumber = vendor.PhoneNumber;
				newVender.VendorLocation = vendor.CompanyAddress;
				newVender.VendorCreatedAt = DateTime.Now;
				newVender.PasswordHash = passwordHash;
				newVender.PasswordSalt = passwordSalt;

				_context.VendorModel.Add(newVender);
				_context.SaveChanges();

				foreach (var service in vendor.ServiceProvide)
				{
					ProvidedServiceModel providedService = new ProvidedServiceModel();
				
					providedService.ServiceName = service;
					providedService.VendorId = newVender.VendorId;
					_context.ProvidedServiceTable.Add(providedService);
					_context.SaveChanges();
				}
				return new OkObjectResult("Successfully Registered");
			}
			catch(Exception ex)
			{
				return new BadRequestObjectResult(ex.Message);
			}
		}


		// User Login
		public IActionResult UserLogin(LoginDto request)
		{
			try
			{

				if(!UserAlreadyExist(request.Email))
				{
					return new BadRequestObjectResult($"{request.Email} is invalid.");
				}

				var user = _context.UserModel.FirstOrDefault(user => user.Email == request.Email);

				if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
				{
					return new BadRequestObjectResult($"Invalid Password");
				}

				string jwtToken = "";

				if(!user.isAdmin)
				{
					jwtToken = CreateToken(user.UserId.ToString(), "User");

				}
				else
				{
					jwtToken = CreateToken(user.UserId.ToString(), "Admin");
				}

				return new OkObjectResult(jwtToken);
			}
			catch(Exception ex)
			{
				return new BadRequestObjectResult(ex.Message);
			}
		}

		// Vendor Login
		public IActionResult VendorLogin(LoginDto request)
		{
			try
			{

				if(!VendorAlreadyExist(request.Email))
				{
					return new BadRequestObjectResult($"{request.Email} is Invalid");
				}

				var vendor = _context.VendorModel.FirstOrDefault(vendor => vendor.VendorEmail == request.Email);

				if(!VerifyPasswordHash(request.Password, vendor.PasswordHash, vendor.PasswordSalt))
				{
					return new BadRequestObjectResult($"Invalid Password");
				}

				string jwtToken = CreateToken(vendor.VendorId.ToString(), "Vendor");

				return new OkObjectResult(jwtToken);
			}
			catch(Exception ex)
			{
				return new BadRequestObjectResult($"{ex.Message}");
			}

		}

		// Change Password
		public IActionResult ChangePassword(ChangePwdDto request)
		{
			try
			{
				var currentUserId = GetCurrentUserId();

				string currentRole = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);

				if(currentRole == "User")
				{
					var currentUser = _context.UserModel.Where(user => user.UserId ==  currentUserId).FirstOrDefault();
					if (!VerifyPasswordHash(request.oldPassword, currentUser.PasswordHash, currentUser.PasswordSalt))
					{
						return new BadRequestObjectResult("Invalid Old Password");
					}

					CreatePasswordHash(request.newPassword, out byte[] passwordHash, out byte[] passwordSalt);

					currentUser.PasswordHash = passwordHash;
					currentUser.PasswordSalt = passwordSalt;

					_context.SaveChanges();


				}
				else if(currentRole == "Vendor")
				{
					var currentUser = _context.VendorModel.Where(vendor => vendor.VendorId == currentUserId).FirstOrDefault();
					if (!VerifyPasswordHash(request.oldPassword, currentUser.PasswordHash, currentUser.PasswordSalt))
					{
						return new BadRequestObjectResult("Invalid Old Password");
					}

					CreatePasswordHash(request.newPassword, out byte[] passwordHash, out byte[] passwordSalt);

					currentUser.PasswordHash = passwordHash;
					currentUser.PasswordSalt = passwordSalt;

					_context.SaveChanges();
				}
				return new OkObjectResult("Password Updated Successfully");

			}
			catch(Exception ex)
			{
				return new BadRequestObjectResult(ex.Message);
			}

		}

		// Get Profile
		public object GetProfile()
		{
			try
			{
				var currentUserId = GetCurrentUserId();
				string currentRole = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);

                dynamic currentProfile = new System.Dynamic.ExpandoObject();

				currentProfile.ProvidedService = new List<ProvidedServiceModel>();


                if (currentRole == "User")
				{
					var currentUser = _context.UserModel.Where(user => user.UserId ==  currentUserId).Select(user => new
					{
						user.Name,
						user.Email,
						user.PhoneNumber
					}).FirstOrDefault();

					currentProfile.Profile = currentUser;

                    
                }
				else if(currentRole == "Vendor")
				{
					var currentVendor = _context.VendorModel.Where(vendor => vendor.VendorId == currentUserId).Select(vendor => new
					{
						vendor.VendorName,
						vendor.VendorEmail,
						vendor.VendorPhoneNumber,
						vendor.VendorLocation,
						vendor.VendorCompanyName,
						vendor.VendorGSTNumber
					}).FirstOrDefault();

					currentProfile.Profile = currentVendor;

					var currentProvidedService = _context.ProvidedServiceTable.Where(provided => provided.VendorId == currentUserId).ToList();

					currentProfile.ProvidedService = currentProvidedService;

				}

                return new OkObjectResult(currentProfile);

            }
			catch(Exception ex)
			{
				return new BadRequestObjectResult(ex.Message);
			}
        }

		public IActionResult UpdateProfileForUser(UserSignupDto user)
		{
			try
			{
				var currentUserId = GetCurrentUserId();
				var currentUser = _context.UserModel.Where(user => user.UserId == currentUserId).FirstOrDefault();

				currentUser.PhoneNumber = user.PhoneNumber;
				currentUser.Name = user.Name;

				_context.SaveChanges();

				return new OkObjectResult("Successfully Updated Profile");

			}
			catch (Exception ex) 
			{
				return new BadRequestObjectResult(ex.Message);
			}
		}

		public IActionResult UpdateProfileForVendor(VendorSignDto vendor)
		{
			try
			{
				var currentVendorId = GetCurrentUserId();
				var currentVendor = _context.VendorModel.Where(vendor => vendor.VendorId == currentVendorId).FirstOrDefault();

				currentVendor.VendorPhoneNumber = vendor.PhoneNumber;
				currentVendor.VendorName = vendor.Name;

				_context.SaveChanges();

                // TO Delete the Old Service and insert new Service
                _context.ProvidedServiceTable.RemoveRange(_context.ProvidedServiceTable.Where(p => p.VendorId == currentVendorId));
                _context.SaveChanges();

                foreach (var service in vendor.ServiceProvide)
                {
					ProvidedServiceModel newProvidedService = new ProvidedServiceModel();
					newProvidedService.ServiceName = service;
					newProvidedService.VendorId = currentVendorId;

					_context.ProvidedServiceTable.Add(newProvidedService);
					_context.SaveChanges();
                }

                return new OkObjectResult("Successfully Updated Profile");
			}
			catch (Exception ex)
			{
				return new BadRequestObjectResult(ex.Message);
			}
		}

	}


}
