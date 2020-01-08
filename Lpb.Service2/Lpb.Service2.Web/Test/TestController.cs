using Lpb.Service2.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Lpb.Service2.Web.Test
{
    [Route("api/[controller]/[action]")]
    public class TestController : Service2ControllerBase
    {
        /// <summary>
        /// 不需要角色，有授权就行
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> Index()
        {
            return await Task.FromResult("No Authorize");
        }

        /// <summary>
        /// 不需要角色，有授权就行
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<string> Index0()
        {
            return await Task.FromResult("Authorize");
        }

        /// <summary>
        /// customer或者backstage角色都可以访问
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "customer,doctor")]
        public async Task<string> Index1()
        {
            return await Task.FromResult("customer,doctor");
        }

        /// <summary>
        /// 需要doctor角色
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "doctor")]
        public async Task<string> Index2()
        {
            return await Task.FromResult("doctor");
        }

        /// <summary>
        /// customer且backstage角色可以访问
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "customer")]
        public async Task<string> Index3()
        {
            return await Task.FromResult("customer");
        }

        /// <summary>
        /// customer且doctor角色可以访问
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "customer")]
        [Authorize(Roles = "doctor")]
        public async Task<string> Index4()
        {
            return await Task.FromResult("customer,doctor");
        }

    }
}