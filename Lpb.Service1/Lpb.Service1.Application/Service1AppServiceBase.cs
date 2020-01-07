using Abp.Application.Services;

namespace Lpb.Service1
{
    /// <summary>
    /// Derive your application services from this class.
    /// </summary>
    public abstract class Service1AppServiceBase : ApplicationService
    {
        protected Service1AppServiceBase()
        {
            LocalizationSourceName = Service1Consts.LocalizationSourceName;
        }
    }
}