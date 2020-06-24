

using System.Net;
using System.ServiceModel;

namespace ReDefNet.Net
{
    /// <summary>
    /// Represents a provider of an endpoint address.
    /// </summary>
    public interface IEndpointAddressProvider
    {
        /// <summary>
        /// Gets the endpoint address for the specified endpoint.
        /// </summary>
        /// <param name="endPoint">The endpoint.</param>
        /// <returns>
        /// The endpoint address.
        /// </returns>
        EndpointAddress GetEndpointAddress(IPEndPoint endPoint);
    }
}