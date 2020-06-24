

using MKModel;
using MKService.ModelUpdaters;
using MKService.Proxies;
using MKService.Queries;


namespace MKService.ProxyFactories
{
    internal class MageKnightModelProxyFactory : IMageKnightModelProxyFactory
    {
        private readonly IModelUpdaterResolver modelUpdaterResolver;


        private readonly IServiceClient serviceClient;

        public MageKnightModelProxyFactory(
            IServiceClient serviceClient,
            IModelUpdaterResolver modelUpdaterResolver)
        {

            this.serviceClient = serviceClient;

            this.modelUpdaterResolver = modelUpdaterResolver;
        }

        public IMageKnightModel Create()
        {
            var query = this.serviceClient.Query<MageKnightQuery>();

            return new MageKnightProxy(this.serviceClient, this.modelUpdaterResolver, query);
        }
    }
}
