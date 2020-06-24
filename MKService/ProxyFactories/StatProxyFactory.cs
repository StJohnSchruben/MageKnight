
using MKModel;
using MKService.ModelUpdaters;
using MKService.Proxies;
using MKService.Queries;
namespace MKService.ProxyFactories
{
    internal interface IStatProxyFactory
    {
        IStat Create();
    }

    internal class StatProxyFactory : IStatProxyFactory
    {
        private readonly IModelUpdaterResolver modelUpdaterResolver;


        private readonly IServiceClient serviceClient;

        public StatProxyFactory(
            IServiceClient serviceClient,
            IModelUpdaterResolver modelUpdaterResolver)
        {

            this.serviceClient = serviceClient;

            this.modelUpdaterResolver = modelUpdaterResolver;
        }

        public IStat Create()
        {
            var query = this.serviceClient.Query<StatQuery>();

            return new StatProxy(this.serviceClient, this.modelUpdaterResolver, query);
        }
    }
}
