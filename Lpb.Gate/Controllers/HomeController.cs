using Microsoft.AspNetCore.Mvc;

namespace Lpb.Gateway.Web.Controllers
{
    public class HomeController : GatewayControllerBase
    {
        public ActionResult Index()
        {
            return Redirect("/swagger");
        }
    }
}