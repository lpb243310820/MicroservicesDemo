using Abp.Application.Services;

namespace Lpb.UserCenter
{
    /// <summary>
    /// Derive your application services from this class.
    /// </summary>
    public abstract class UserCenterAppServiceBase : ApplicationService
    {
        protected UserCenterAppServiceBase()
        {
            LocalizationSourceName = UserCenterConsts.LocalizationSourceName;
        }
    }
}