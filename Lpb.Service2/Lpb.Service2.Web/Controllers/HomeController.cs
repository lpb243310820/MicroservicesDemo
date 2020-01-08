using Microsoft.AspNetCore.Mvc;

namespace Lpb.Service2.Web.Controllers
{
    public class HomeController : Service2ControllerBase
    {
        public ActionResult Index()
        {
            return Redirect("/swagger");
        }
    }
}