namespace Biddo.Services.Models
{
    public class VendorSignDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string CompanyName { get; set; }
        public string CompanyGSTNumber { get; set; }
        public string CompanyAddress { get; set; }

        public List<string> ServiceProvide { get; set; }

        public bool isServiceChange { get; set; } = false;


    }
}
