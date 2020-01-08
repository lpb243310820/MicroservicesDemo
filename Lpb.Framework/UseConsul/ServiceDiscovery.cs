namespace UseConsul
{
    public class ServiceDiscoveryOptions
    {
        /// <summary>
        /// 5001
        /// </summary>
        public string Service1Name { get; set; }
        /// <summary>
        /// 5002
        /// </summary>
        public string Service2Name { get; set; }
        /// <summary>
        /// 5003
        /// </summary>
        public string IdentityServiceName { get; set; }
        /// <summary>
        /// 5004
        /// </summary>
        public string UserServiceName { get; set; }

        public string LocalDebugAddress { get; set; }

        public ConsulDisvoveryOptions ConsulDnsEndpoint { get; set; }
    }
}
