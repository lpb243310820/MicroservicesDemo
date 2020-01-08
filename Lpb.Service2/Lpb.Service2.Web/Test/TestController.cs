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
        /// ����Ҫ��ɫ������Ȩ����
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> Index()
        {
            return await Task.FromResult("No Authorize");
        }

        /// <summary>
        /// ����Ҫ��ɫ������Ȩ����
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<string> Index0()
        {
            return await Task.FromResult("Authorize");
        }

        /// <summary>
        /// customer����backstage��ɫ�����Է���
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "customer,doctor")]
        public async Task<string> Index1()
        {
            return await Task.FromResult("customer,doctor");
        }

        /// <summary>
        /// ��Ҫdoctor��ɫ
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "doctor")]
        public async Task<string> Index2()
        {
            return await Task.FromResult("doctor");
        }

        /// <summary>
        /// customer��backstage��ɫ���Է���
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "customer")]
        public async Task<string> Index3()
        {
            return await Task.FromResult("customer");
        }

        /// <summary>
        /// customer��doctor��ɫ���Է���
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