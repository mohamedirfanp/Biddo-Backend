using Biddo.Services.Models;
using Microsoft.AspNetCore.Mvc;

namespace Biddo.Services.MailServices
{
    public interface IMailService
    {
        IActionResult SendMail(MailDto request);
    }
}
