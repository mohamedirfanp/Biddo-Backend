using Biddo.Models;

namespace Biddo.Services.Models
{
    public class MailDto
    {

        public string Subject { get; set; } = string.Empty;

        public string Body { get; set; } = string.Empty;

        public List<string> ToList { get; set; } = null;

    }
}
