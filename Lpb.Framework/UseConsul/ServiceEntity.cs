using System;
using System.Collections.Generic;
using System.Text;

namespace UseConsul
{
    /// <summary>
    /// 服务实体
    /// </summary>
    public class ServiceEntity
    {
        /// <summary>
        /// IP地址
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 服务名称
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// Consul的IP
        /// </summary>
        public string ConsulIP { get; set; }

        /// <summary>
        /// Consul的端口
        /// </summary>
        public int ConsulPort { get; set; }
    }
}
