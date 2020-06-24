
using MKModel;
using MKService.ModelUpdaters;
using MKService.Proxies;
using MKService.Queries;
namespace MKService.ProxyFactories
{
    internal interface IClickProxyFactory
    {
        IClick Create();
    }

    internal class ClickProxyFactory : IClickProxyFactory
    {
        private readonly IModelUpdaterResolver modelUpdaterResolver;


        private readonly IServiceClient serviceClient;

        public ClickProxyFactory(
            IServiceClient serviceClient,
            IModelUpdaterResolver modelUpdaterResolver)
        {

            this.serviceClient = serviceClient;

            this.modelUpdaterResolver = modelUpdaterResolver;
        }

        public IClick Create()
        {
            var query = this.serviceClient.Query<ClickQuery>();

            return new ClickProxy(this.serviceClient, this.modelUpdaterResolver, query);
        }
    }
}
