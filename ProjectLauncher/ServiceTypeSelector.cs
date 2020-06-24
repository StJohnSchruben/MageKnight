using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ProjectLauncher
{
    public class ServiceTypeSelector : IServiceTypeSelector
    {
        /// <summary>
        /// The local IP address.
        /// </summary>
        private readonly IPAddress localIPAddress;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceTypeSelector" /> class.
        /// </summary>
        public ServiceTypeSelector()
        {
            this.localIPAddress = this.GetLocalIpAddress();
            var addressArray = this.localIPAddress.GetAddressBytes();
            this.FirstOctet = addressArray[0];
            this.SecondOctet = addressArray[1];
            this.ThirdOctet = addressArray[2];
            this.ForthOctet = addressArray[3];
        }

        /// <summary>
        /// Gets or sets the first octet of the <see cref="IPAddress" />.
        /// </summary>
        /// <value>
        /// The first octet of the <see cref="IPAddress" />.
        /// </value>
        public byte FirstOctet { get; set; }

        /// <summary>
        /// Gets or sets the forth octet of the <see cref="IPAddress" />.
        /// </summary>
        /// <value>
        /// The forth octet of the <see cref="IPAddress" />.
        /// </value>
        public byte ForthOctet { get; set; }

        /// <summary>
        /// Gets the local IP address.
        /// </summary>
        /// <value>
        /// The local IP address.
        /// </value>
        public IPAddress LocalIP => this.localIPAddress;

        /// <summary>
        /// Gets or sets the second octet of the <see cref="IPAddress" />.
        /// </summary>
        /// <value>
        /// The second octet of the <see cref="IPAddress" />.
        /// </value>
        public byte SecondOctet { get; set; }

        /// <summary>
        /// Gets or sets the third octet of the <see cref="IPAddress" />.
        /// </summary>
        /// <value>
        /// The third octet of the <see cref="IPAddress" />.
        /// </value>
        public byte ThirdOctet { get; set; }

        /// <summary>
        /// Gets the local IP address.
        /// </summary>
        /// <returns>
        /// The local <see cref="IPAddress" /> of the machine if able, otherwise throws exception.
        /// </returns>
        /// <exception cref="System.InvalidOperationException">No network adapters with an IPV4 address in the system.</exception>
        private IPAddress GetLocalIpAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return ip;
                }
            }

            throw new InvalidOperationException("No network adapters with an IPv4 address in the system.");
        }
    }
}
