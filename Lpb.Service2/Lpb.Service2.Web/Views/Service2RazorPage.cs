using Abp.AspNetCore.Mvc.Views;

namespace Lpb.Service2.Web.Views
{
    public abstract class Service2RazorPage<TModel> : AbpRazorPage<TModel>
    {
        protected Service2RazorPage()
        {
            LocalizationSourceName = Service2Consts.LocalizationSourceName;
        }
    }
}
