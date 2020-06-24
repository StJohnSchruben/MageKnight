

using MKModel;
using MKService.ModelUpdaters;
using MKService.Proxies;
using MKService.Queries;

namespace MKService.ProxyFactories
{
    internal class GameProxyFactory : IGameModelProxyFactory
    {
        private readonly IModelUpdaterResolver modelUpdaterResolver;


        private readonly IServiceClient serviceClient;

        public GameProxyFactory(
            IServiceClient serviceClient,
            IModelUpdaterResolver modelUpdaterResolver)
        {

            this.serviceClient = serviceClient;

            this.modelUpdaterResolver = modelUpdaterResolver;
        }

        public IGameModel Create()
        {
            var query = this.serviceClient.Query<GameQuery>();

            return new GameProxy(this.serviceClient, this.modelUpdaterResolver, query);
        }
    }

    internal class GamesProxyFactory : IGamesModelProxyFactory
    {
        private readonly IModelUpdaterResolver modelUpdaterResolver;


        private readonly IServiceClient serviceClient;

        public GamesProxyFactory(
            IServiceClient serviceClient,
            IModelUpdaterResolver modelUpdaterResolver)
        {

            this.serviceClient = serviceClient;

            this.modelUpdaterResolver = modelUpdaterResolver;
        }

        public IGameModels Create()
        {
            var query = this.serviceClient.Query<GamesQuery>();

            return new GamesProxy(this.serviceClient, this.modelUpdaterResolver, query);
        }
    }
}
