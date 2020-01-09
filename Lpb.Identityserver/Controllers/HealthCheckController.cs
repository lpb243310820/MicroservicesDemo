using Abp.Auditing;
using Microsoft.AspNetCore.Mvc;

namespace Lpb.Identityserver.Controllers
{
    [Route("[Controller]")]
    [DisableAuditing]
    public class HealthCheckController : ControllerBase
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