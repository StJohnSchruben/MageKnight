
using MKModel;
using MKService.ModelUpdaters;
using MKService.Proxies;
using MKService.Queries;
namespace MKService.ProxyFactories
{
    internal interface IUserCollectionProxyFactory
    {
        IUserCollection Create();
    }

    internal class UserCollectionProxyFactory : IUserCollectionProxyFactory
    {
        private readonly IModelUpdaterResolver modelUpdaterResolver;


        private readonly IServiceClient serviceClient;

        public UserCollectionProxyFactory(
            IServiceClient serviceClient,
            IModelUpdaterResolver modelUpdaterResolver)
        {

            this.serviceClient = serviceClient;

            this.modelUpdaterResolver = modelUpdaterResolver;
        }

        public IUserCollection Create()
        {
            var query = this.serviceClient.Query<UserCollectionQuery>();

            return new UserCollectionProxy(this.serviceClient, this.modelUpdaterResolver, query);
        }
    }
}
