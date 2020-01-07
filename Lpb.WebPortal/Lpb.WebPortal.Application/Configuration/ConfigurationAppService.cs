using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using Lpb.WebPortal.Configuration.Dto;

namespace Lpb.WebPortal.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : WebPortalAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
