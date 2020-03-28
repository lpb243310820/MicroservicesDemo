using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Runtime.Caching;
using Abp.Threading.BackgroundWorkers;
using Abp.Threading.Timers;
using Abp.Timing;
using Abp.UI;
using Abp.Web.Models;
using Lpb.Identity.TokenCleanup;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Lpb.Identity.Web.BackgroundWorker
{
    public class IdentityBackgroundWorker : PeriodicBackgroundWorkerBase, ISingletonDependency
    {
        private readonly ITokenCleanupAppService _tokenCleanupAppService;

        public IdentityBackgroundWorker(AbpTimer timer, ITokenCleanupAppService tokenCleanupAppService)
            : base(timer)
        {
            timer.Period = 1000 * 60 * 30; //seconds (good for tests, but normally will be more)

            _tokenCleanupAppService = tokenCleanupAppService;
        }

        protected override void DoWork()
        {
            if (Clock.Now.Hour != 3 && Clock.Now.Hour != 4) return;

            Logger.Debug("IdentityBackgroundWorker执行开始：" + Clock.Now.ToString("O"));

            try
            {
                _tokenCleanupAppService.TokenCleanup();
            }
            catch (Exception ex)
            {
                Logger.Fatal($"IdentityBackgroundWorker执行错误{ex.StackTrace}");
            }
        }

        
    }
}