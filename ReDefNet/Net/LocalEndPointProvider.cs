

using System;
using System.Net;
using System.Net.Sockets;

namespace ReDefNet.Net
{
    /// <summary>
    /// Represents a provider for local endpoints.
    /// </summary>
    public static class LocalEndPointProvider
    {
        /// <summary>
        /// Gets the first available local endpoint.
        /// </summary>
        /// <returns>The first available local endpoint.</returns>
        /// <exception cref="System.InvalidOperationException">Cannot find local IP address.</exception>
        public static IPEndPoint GetFirstAvailableLocalEndPoint()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());

            IPAddress resultIPAddress = null;

            foreach (var ipAddress in host.AddressList)
            {
                if (ipAddress.AddressFamily != AddressFamily.InterNetwork)
                {
                    continue;
                }

                resultIPAddress = ipAddress;
            }

            if (resultIPAddress == null)
            {
                throw new InvalidOperationException("Cannot find local IP address.");
            }

            int resultPort;

            using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
            {
                // Using socket 0 finds the first available port!
                var endPoint = new IPEndPoint(resultIPAddress, 0);

                socket.Bind(endPoint);

                resultPort = ((IPEndPoint)socket.LocalEndPoint).Port;
            }

            return new IPEndPoint(resultIPAddress, resultPort);
        }
    }
}