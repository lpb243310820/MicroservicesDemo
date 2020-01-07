using Microsoft.AspNetCore.Mvc;
using System;

namespace Lpb.Identityserver.Controllers
{
    public class HomeController : ControllerBase
    {
        public ActionResult Index()
        {
            return new ContentResult() { Content = $"{123}{this.GetType().ToString()}_{DateTime.Now.ToString()}" };
        }
    }
}