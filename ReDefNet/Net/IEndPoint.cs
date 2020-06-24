

using System.Net;

namespace ReDefNet.Net
{
    /// <summary>
    /// Represents a provider of an endpoint.
    /// </summary>
    public interface IEndPoint
    {
        /// <summary>
        /// Gets the endpoint.
        /// </summary>
        /// <value>
        /// The endpoint.
        /// </value>
        IPEndPoint EndPoint { get; }
    }
}