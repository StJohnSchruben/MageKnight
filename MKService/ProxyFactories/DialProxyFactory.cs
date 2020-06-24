
using MKModel;
using MKService.ModelUpdaters;
using MKService.Proxies;
using MKService.Queries;
namespace MKService.ProxyFactories
{
    internal interface IDialProxyFactory
    {
        IDial Create();
    }

    internal class DialProxyFactory : IDialProxyFactory
    {
        private readonly IModelUpdaterResolver modelUpdaterResolver;


        private readonly IServiceClient serviceClient;

        public DialProxyFactory(
            IServiceClient serviceClient,
            IModelUpdaterResolver modelUpdaterResolver)
        {

            this.serviceClient = serviceClient;

            this.modelUpdaterResolver = modelUpdaterResolver;
        }

        public IDial Create()
        {
            var query = this.serviceClient.Query<DialQuery>();

            return new DialProxy(this.serviceClient, this.modelUpdaterResolver, query);
        }
    }
}
