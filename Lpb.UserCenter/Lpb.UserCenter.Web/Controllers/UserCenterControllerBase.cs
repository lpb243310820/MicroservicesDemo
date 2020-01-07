using Abp.AspNetCore.Mvc.Controllers;

namespace Lpb.UserCenter.Web.Controllers
{
    public abstract class UserCenterControllerBase: AbpController
    {
        protected UserCenterControllerBase()
        {
            LocalizationSourceName = UserCenterConsts.LocalizationSourceName;
        }
    }
}