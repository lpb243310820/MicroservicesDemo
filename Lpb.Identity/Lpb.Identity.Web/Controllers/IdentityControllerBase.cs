using Abp.AspNetCore.Mvc.Controllers;

namespace Lpb.Identity.Web.Controllers
{
    public abstract class IdentityControllerBase : AbpController
    {
        protected IdentityControllerBase()
        {
            LocalizationSourceName = IdentityConsts.LocalizationSourceName;
        }
    }
}