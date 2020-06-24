using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MKService
{
    public interface IClientServiceTypeProvider
    {
        /// <summary>
        /// Gets the IP end point.
        /// </summary>
        /// <value>The end point.</value>
        IPEndPoint EndPoint { get; }

        /// <summary>
        /// Gets the session time.
        /// </summary>
        /// <value>The session time.</value>
        //ISessionTime SessionTime { get; }

        /// <summary>
        /// Gets or sets a value indicating whether to use the service.
        /// </summary>
        /// <value>The use service.</value>
        bool UseService { get; set; }
    }
}
