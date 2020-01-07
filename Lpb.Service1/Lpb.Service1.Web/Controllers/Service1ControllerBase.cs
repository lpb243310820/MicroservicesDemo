using Abp.AspNetCore.Mvc.Controllers;

namespace Lpb.Service1.Web.Controllers
{
    public abstract class Service1ControllerBase: AbpController
    {
        protected Service1ControllerBase()
        {
            LocalizationSourceName = Service1Consts.LocalizationSourceName;
        }
    }
}