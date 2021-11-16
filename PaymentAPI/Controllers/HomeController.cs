using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PaymentAPI.Models;

namespace PaymentAPI.Controllers
{
    /// <summary>
    /// This is the starter-kit that we use in the training course IdentityServer in Production  
    /// by https://www.tn-data.se
    /// </summary>
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            if (Request.Headers["accept"] == "application/json")
            {
                //Return JSON response
                return Problem(title: "Server error",
                detail: "Please contact support");
            }
            else
            {
                return View(new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ??
                HttpContext.TraceIdentifier
                });
            }
        }
    }
}
