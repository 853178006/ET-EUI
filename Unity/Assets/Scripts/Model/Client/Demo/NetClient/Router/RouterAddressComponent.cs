using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class RouterAddressComponent: Entity, IAwake<string, int>
    {
        /// <summary>
        /// RouterManager IP地址带端口
        /// </summary>
        public IPAddress RouterManagerIPAddress { get; set; }

        /// <summary>
        /// RouterManager IP
        /// </summary>
        public string RouterManagerHost;

        /// <summary>
        /// RouterManager 端口
        /// </summary>
        public int RouterManagerPort;

        public HttpGetRouterResponse Info;
        public int RouterIndex;
    }
}