using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ProjectLauncher
{
    public interface IServiceTypeSelector
    {
        /// <summary>
        /// Gets or sets the first octet of the <see cref="IPAddress" />.
        /// </summary>
        /// <value>
        /// The first octet of the <see cref="IPAddress" />.
        /// </value>
        byte FirstOctet { get; set; }

        /// <summary>
        /// Gets or sets the forth octet of the <see cref="IPAddress" />.
        /// </summary>
        /// <value>
        /// The forth octet of the <see cref="IPAddress" />.
        /// </value>
        byte ForthOctet { get; set; }

        /// <summary>
        /// Gets the local IP address.
        /// </summary>
        /// <value>
        /// The local IP address.
        /// </value>
        IPAddress LocalIP { get; }

        /// <summary>
        /// Gets or sets the second octet of the <see cref="IPAddress" />.
        /// </summary>
        /// <value>
        /// The second octet of the <see cref="IPAddress" />.
        /// </value>
        byte SecondOctet { get; set; }

        /// <summary>
        /// Gets or sets the third octet of the <see cref="IPAddress" />.
        /// </summary>
        /// <value>
        /// The third octet of the <see cref="IPAddress" />.
        /// </value>
        byte ThirdOctet { get; set; }
    }
}
