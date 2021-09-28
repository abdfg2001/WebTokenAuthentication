using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebTokenAuthentication.Controllers
{
    [Route("/api/protected")]
    [Authorize(Roles = "Admin")]
    public class AuthorizeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Content("authorized");
        }
    }
}
