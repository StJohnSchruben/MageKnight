

using System;
using System.Net;
using System.Threading.Tasks;

namespace ReDefNet.Net
{
    /// <summary>
    /// Represents a type that provides UDP network services.
    /// </summary>
    public interface IUdpClient : IDisposable
    {
        /// <summary>
        /// Binds to the specified network endpoint.
        /// </summary>
        /// <param name="endpoint">An endpoint that specifies the network endpoint on which you intend to receive data.</param>
        void Bind(IPEndPoint endpoint);

        /// <summary>
        /// Closes the UDP connection.
        /// </summary>
        void Close();

        /// <summary>
        /// Establishes a default remote host using the specified network endpoint.
        /// </summary>
        /// <param name="endpoint">An endpoint that specifies the network endpoint to which you intend to send data.</param>
        void Connect(IPEndPoint endpoint);

        /// <summary>
        /// Gets a UDP datagram that was sent by a remote host.
        /// </summary>
        /// <param name="endpoint">The remote endpoint.</param>
        /// <returns>The UDP datagram data.</returns>
        byte[] Receive(ref IPEndPoint endpoint);

        /// <summary>
        /// Asynchronously sends a UDP datagram to a remote host.
        /// </summary>
        /// <param name="datagram">The datagram.</param>
        /// <param name="bytes">The number of bytes in the datagram.</param>
        /// <returns>
        /// A task representing the asynchronous operation. The task result contains the number of bytes that were sent
        /// to the remote host.
        /// </returns>
        Task<int> SendAsync(byte[] datagram, int bytes);
    }
}