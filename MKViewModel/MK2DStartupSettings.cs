using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MKViewModel
{
    public static class MK2DStartupSettings
    {
        /// <summary>
        /// The log.
        /// </summary>
        private static ILog log = LogManager.GetLogger(typeof(MK2DStartupSettings));

        /// <summary>
        /// The service end point.
        /// </summary>
        private static IPEndPoint serviceEndPoint;

        /// <summary>
        /// The WST position number.
        /// </summary>
        private static ushort wstPositionNumber;

        /// <summary>
        /// Gets or sets a value indicating whether the TXP-A will use service.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the TXP-A will use service; otherwise, <c>false</c>.
        /// </value>
        public static bool UseService { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is in COM mode.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is COM mode; otherwise, <c>false</c>.
        /// </value>
        public static bool InComMode { get; set; }

        /// <summary>
        /// Gets the workstation position.
        /// </summary>
        /// <value>
        /// The workstation position.
        /// </value>
        public static ushort WorkstationPosition
        {
            get { return wstPositionNumber; }
        }

        /// <summary>
        /// Gets or sets the WST service end point.
        /// </summary>
        /// <value>
        /// The WST service end point.
        /// </value>
        public static IPEndPoint WstServiceEndPoint
        {
            get
            {
                if (UseService)
                {
                    return serviceEndPoint ?? (serviceEndPoint = GetDefaultLocalServiceEndpoint());
                }

                log.Debug($"our wst endpoint returned localhost because use service is false");
                return serviceEndPoint ?? (serviceEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9000));
            }

            set
            {
                log.Debug($"our wst endpoint was set to {value.ToString()}");
                serviceEndPoint = value;
            }
        }

        /// <summary>
        /// Gets the default local service endpoint.
        /// </summary>
        /// <returns>
        /// Returns the new IPEndPoint.
        /// </returns>
        private static IPEndPoint GetDefaultLocalServiceEndpoint()
        {
            var address = IPAddress.Parse("127.0.0.1");

            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ipAddress in host.AddressList)
            {
                if (ipAddress.AddressFamily != AddressFamily.InterNetwork)
                {
                    continue;
                }

                address = ipAddress;
            }

            log.Debug($"our wst endpoint returned {address.ToString()}");
            return new IPEndPoint(address, 9000);
        }
    }
}
