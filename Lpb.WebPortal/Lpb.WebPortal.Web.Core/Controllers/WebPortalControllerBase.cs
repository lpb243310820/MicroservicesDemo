using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace Lpb.WebPortal.Controllers
{
    public abstract class WebPortalControllerBase: AbpController
    {
        protected WebPortalControllerBase()
        {
            LocalizationSourceName = WebPortalConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
