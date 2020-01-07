using System.Net;

namespace UseConsul
{
    /// <summary>
    /// 服务注册
    /// </summary>
    public class ServiceRegisterOptions
    {
        public string ServiceName { get; set; }

        public string ServiceIpAddress { get; set; }

        public int ServicePort { get; set; }

        public ConsulRegisterOptions Consul { get; set; }
    }

    public class ConsulRegisterOptions
    {
        public string HttpEndPoint { get; set; }
    }



    /// <summary>
    /// 服务发现
    /// </summary>
    public class ConsulDisvoveryOptions
    {
        public string Address { get; set; }

        public int Port { get; set; }

        public IPEndPoint ToIPEndPoint()
        {
            return new IPEndPoint(IPAddress.Parse(Address), Port);
        }
    }
}
