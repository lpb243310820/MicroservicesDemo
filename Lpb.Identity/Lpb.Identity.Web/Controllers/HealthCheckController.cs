using Abp.Auditing;
using Microsoft.AspNetCore.Mvc;

namespace Lpb.Identity.Web.Controllers
{
    [Route("[Controller]")]
    [DisableAuditing]
    public class HealthCheckController : IdentityControllerBase
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