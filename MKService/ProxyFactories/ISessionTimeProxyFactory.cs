

using MKModel;

namespace MKService.ProxyFactories
{
    /// <summary>
    /// Interface for Session Time Proxy Factory.
    /// </summary>
    internal interface ISessionTimeProxyFactory
    {
        /// <summary>
        /// Creates a new instance of a session time.
        /// </summary>
        /// <param name="inComMode">If set to <c>true</c> [in COM mode].</param>
        /// <returns>
        /// Session Time object.
        /// </returns>
        ISessionTime Create(bool inComMode);
    }
}