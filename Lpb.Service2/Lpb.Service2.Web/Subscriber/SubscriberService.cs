using CapRabbitMQ;
using Castle.Core.Logging;
using DotNetCore.CAP;
using System;

namespace Lpb.Service2.Web.Subscriber
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
        [CapSubscribe(MqTopic.Lpb_Heartbeat)]
        public void LpbHeartbeat(string msg)
        {
            try
            {
                _logger.Debug($"Heartbeat：{msg}");
            }
            catch (Exception e)
            {
                _logger.Error($"LpbHeartbeat函数Exception错误，{e.Message}");
                //throw new UserFriendlyException("ProductInfoUpdateByBackStage", e);
            }
        }
    }

}
