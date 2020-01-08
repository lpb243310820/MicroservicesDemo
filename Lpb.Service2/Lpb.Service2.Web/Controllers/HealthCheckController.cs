using Abp.Auditing;
using Microsoft.AspNetCore.Mvc;

namespace Lpb.Service2.Web.Controllers
{
    [Route("[Controller]")]
    [DisableAuditing]
    public class HealthCheckController : Service2ControllerBase
    {
        [HttpGet("")]
        [HttpHead("")]
        public IActionResult Ping()
        {
            //Logger.Debug("Consul健康检查：HealthCheck" + Clock.Now.ToString("O"));
            return Ok();
        }
    }
}