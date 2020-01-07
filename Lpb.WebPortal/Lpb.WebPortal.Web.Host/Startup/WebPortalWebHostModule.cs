using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Lpb.WebPortal.Configuration;

namespace Lpb.WebPortal.Web.Host.Startup
{
    [DependsOn(
       typeof(WebPortalWebCoreModule))]
    public class WebPortalWebHostModule: AbpModule
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public WebPortalWebHostModule(IWebHostEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(WebPortalWebHostModule).GetAssembly());
        }
    }
}
