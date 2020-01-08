using CapRabbitMQ;
using Castle.Core.Logging;
using DotNetCore.CAP;
using System;

namespace Lpb.UserCenter.Web.Subscriber
{
    public class SubscriberService : ISubscriberService, ICapSubscribe
    {
        private readonly ILogger _logger;

        public SubscriberService(ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 心跳
        /// </summary>
        /// <param name="msg"></param>
        [CapSubscribe(MqTopic.Laiba_Heartbeat)]
        public void LaibaHeartbeat(string msg)
        {
            try
            {
                _logger.Debug($"Heartbeat：{msg}");
            }
            catch (Exception e)
            {
                _logger.Error($"LaibaHeartbeat函数Exception错误，{e.Message}");
            }
        }
    }

}
