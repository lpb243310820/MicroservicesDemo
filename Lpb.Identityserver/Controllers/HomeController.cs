using Microsoft.AspNetCore.Mvc;

namespace Lpb.Identityserver.Controllers
{
    public class HomeController : ControllerBase
    {
        public ActionResult Index()
        {
            return Redirect("/swagger");
        }
    }
}