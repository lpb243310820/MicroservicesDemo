using Abp.AspNetCore.Mvc.Views;

namespace Lpb.Service1.Web.Views
{
    public abstract class Service1RazorPage<TModel> : AbpRazorPage<TModel>
    {
        protected Service1RazorPage()
        {
            LocalizationSourceName = Service1Consts.LocalizationSourceName;
        }
    }
}
