using Abp.AspNetCore.Mvc.Controllers;

namespace Lpb.Gateway.Web.Controllers
{
    public abstract class GatewayControllerBase: AbpController
    {
        protected GatewayControllerBase()
        {
            LocalizationSourceName = GatewayConsts.LocalizationSourceName;
        }
    }
}