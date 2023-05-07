using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Biddo.Models;
using Microsoft.AspNetCore.Authorization;
using Biddo.Services.HelpServices;

namespace Biddo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HelpController : ControllerBase
    {
        //public HelpController(private helpService : HelpService)
        //{
        //    _helpService = HelpService;
        //}

        //// To Get the Tickets
        //[HttpGet("get/Tickets"), Authorize]
        //public async Task<IEnumerable<QueryModel>> GetTickets()
        //{
        //    var response = 
        //}
    }
}
