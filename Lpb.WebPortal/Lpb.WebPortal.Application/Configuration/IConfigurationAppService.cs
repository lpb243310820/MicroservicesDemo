using System.Threading.Tasks;
using Lpb.WebPortal.Configuration.Dto;

namespace Lpb.WebPortal.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
