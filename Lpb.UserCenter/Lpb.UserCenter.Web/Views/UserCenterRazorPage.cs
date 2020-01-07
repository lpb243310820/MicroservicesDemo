using Abp.AspNetCore.Mvc.Views;

namespace Lpb.UserCenter.Web.Views
{
    public abstract class UserCenterRazorPage<TModel> : AbpRazorPage<TModel>
    {
        protected UserCenterRazorPage()
        {
            LocalizationSourceName = UserCenterConsts.LocalizationSourceName;
        }
    }
}
