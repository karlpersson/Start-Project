using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

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
    }
}
