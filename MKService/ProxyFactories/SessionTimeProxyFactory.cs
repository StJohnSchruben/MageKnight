

using MKModel;
using MKService.ModelUpdaters;
using MKService.Proxies;
using MKService.Queries;

namespace MKService.ProxyFactories
{
    /// <summary>
    /// The Session Time Proxy Factory.
    /// </summary>
    /// <seealso cref="MKService.ProxyFactories.ISessionTimeProxyFactory" />
    internal class SessionTimeProxyFactory : ISessionTimeProxyFactory
    {
        /// <summary>
        /// The model updater resolver.
        /// </summary>
        private readonly IModelUpdaterResolver modelUpdaterResolver;

        /// <summary>
        /// The service client.
        /// </summary>
        private readonly IServiceClient serviceClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionTimeProxyFactory" /> class.
        /// </summary>
        /// <param name="serviceClient">The service client.</param>
        /// <param name="modelUpdaterResolver">The model updater resolver.</param>
        public SessionTimeProxyFactory(
            IServiceClient serviceClient,
            IModelUpdaterResolver modelUpdaterResolver)
        {

            this.serviceClient = serviceClient;

            this.modelUpdaterResolver = modelUpdaterResolver;
        }

        /// <summary>
        /// Creates a new instance of a Session Time object.
        /// </summary>
        /// <param name="inComMode">If set to <c>true</c> [in COM mode].</param>
        /// <returns>
        /// Session Time Object.
        /// </returns>
        public ISessionTime Create(bool inComMode)
        {
            var query = this.serviceClient.Query<SessionTimeQuery>();

            return new SessionTimeProxy(this.serviceClient, this.modelUpdaterResolver, query, true);
        }
    }
}