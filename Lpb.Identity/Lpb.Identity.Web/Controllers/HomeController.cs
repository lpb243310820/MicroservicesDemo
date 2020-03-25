using Microsoft.AspNetCore.Mvc;

namespace Lpb.Identity.Web.Controllers
{
    public class HomeController : IdentityControllerBase
    {
        public ActionResult Index()
        {
            return Redirect("/swagger");
        }
    }
}