

using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ReDefNet.Net
{
    /// <summary>
    /// A type that provides UDP network services.
    /// </summary>
    public sealed class SystemUdpClient : IUdpClient
    {
        /// <summary>
        /// The UDP client.
        /// </summary>
        private UdpClient client;

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemUdpClient" /> class.
        /// </summary>
        public SystemUdpClient()
        {
            this.client = new UdpClient();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemUdpClient"/> class.
        /// </summary>
        /// <param name="endPoint">The end point.</param>
        public SystemUdpClient(IPEndPoint endPoint)
        {
            this.client = new UdpClient(endPoint);
        }

        /// <summary>
        /// Binds to the specified network endpoint.
        /// </summary>
        /// <param name="endpoint">An endpoint that specifies the network endpoint on which you intend to receive data.</param>
        public void Bind(IPEndPoint endpoint)
        {
            if (this.client.Client.IsBound)
            {
                return;
            }

            this.client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            this.client.Client.Bind(endpoint);
        }

        /// <summary>
        /// Closes the UDP connection.
        /// </summary>
        public void Close()
        {
            this.Dispose();
        }

        /// <summary>
        /// Establishes a default remote host using the specified network endpoint.
        /// </summary>
        /// <param name="endpoint">An endpoint that specifies the network endpoint to which you intend to send data.</param>
        public void Connect(IPEndPoint endpoint)
        {
            this.client.Connect(endpoint);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (this.client == null)
            {
                return;
            }

            this.client.Close();
            this.client = null;
        }

        /// <summary>
        /// Gets a UDP datagram that was sent by a remote host.
        /// </summary>
        /// <param name="endpoint">The remote endpoint.</param>
        /// <returns>The UDP datagram data.</returns>
        public byte[] Receive(ref IPEndPoint endpoint)
        {
            return this.client.Receive(ref endpoint);
        }

        /// <summary>
        /// Asynchronously sends a UDP datagram to a remote host.
        /// </summary>
        /// <param name="datagram">The datagram.</param>
        /// <param name="bytes">The number of bytes in the datagram.</param>
        /// <returns>
        /// A task representing the asynchronous operation. The task result contains the number of bytes that were sent
        /// to the remote host.
        /// </returns>
        public async Task<int> SendAsync(byte[] datagram, int bytes)
        {
            return await this.client.SendAsync(datagram, bytes);
        }
    }
}