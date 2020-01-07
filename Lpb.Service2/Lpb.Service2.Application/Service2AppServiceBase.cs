using Abp.Application.Services;

namespace Lpb.Service2
{
    /// <summary>
    /// Derive your application services from this class.
    /// </summary>
    public abstract class Service2AppServiceBase : ApplicationService
    {
        protected Service2AppServiceBase()
        {
            LocalizationSourceName = Service2Consts.LocalizationSourceName;
        }
    }
}