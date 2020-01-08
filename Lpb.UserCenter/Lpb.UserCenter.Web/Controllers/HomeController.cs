using Microsoft.AspNetCore.Mvc;

namespace Lpb.UserCenter.Web.Controllers
{
    public class HomeController : UserCenterControllerBase
    {
        public ActionResult Index()
        {
            return Redirect("/swagger");
        }
    }
}