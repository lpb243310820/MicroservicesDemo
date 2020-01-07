using Abp.AutoMapper;
using Lpb.WebPortal.Authentication.External;

namespace Lpb.WebPortal.Models.TokenAuth
{
    [AutoMapFrom(typeof(ExternalLoginProviderInfo))]
    public class ExternalLoginProviderInfoModel
    {
        public string Name { get; set; }

        public string ClientId { get; set; }
    }
}
