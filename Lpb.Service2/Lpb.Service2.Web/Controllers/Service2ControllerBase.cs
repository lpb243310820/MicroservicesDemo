using Abp.AspNetCore.Mvc.Controllers;

namespace Lpb.Service2.Web.Controllers
{
    public abstract class Service2ControllerBase: AbpController
    {
        protected Service2ControllerBase()
        {
            LocalizationSourceName = Service2Consts.LocalizationSourceName;
        }
    }
}