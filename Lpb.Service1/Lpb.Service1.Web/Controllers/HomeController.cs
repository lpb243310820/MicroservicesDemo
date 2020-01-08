using Microsoft.AspNetCore.Mvc;

namespace Lpb.Service1.Web.Controllers
{
    public class HomeController : Service1ControllerBase
    {
        public ActionResult Index()
        {
            return Redirect("/swagger");
        }
    }
}