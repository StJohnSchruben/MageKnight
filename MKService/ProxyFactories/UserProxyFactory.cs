

using MKModel;
using MKService.ModelUpdaters;
using MKService.Proxies;
using MKService.Queries;

namespace MKService.ProxyFactories
{
    internal class UserProxyFactory : IUserModelProxyFactory
    {
        private readonly IModelUpdaterResolver modelUpdaterResolver;


        private readonly IServiceClient serviceClient;

        public UserProxyFactory(
            IServiceClient serviceClient,
            IModelUpdaterResolver modelUpdaterResolver)
        {

            this.serviceClient = serviceClient;

            this.modelUpdaterResolver = modelUpdaterResolver;
        }

        public IUserModel Create()
        {
            var query = this.serviceClient.Query<UserQuery>();

            return new UserProxy(this.serviceClient, this.modelUpdaterResolver, query);
        }
    }
}
