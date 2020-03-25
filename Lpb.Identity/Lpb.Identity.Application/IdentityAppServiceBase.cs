using Abp.Application.Services;

namespace Lpb.Identity
{
    /// <summary>
    /// Derive your application services from this class.
    /// </summary>
    public abstract class IdentityAppServiceBase : ApplicationService
    {
        protected IdentityAppServiceBase()
        {
            LocalizationSourceName = IdentityConsts.LocalizationSourceName;
        }
    }
}