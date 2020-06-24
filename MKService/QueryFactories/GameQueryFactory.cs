using Service;
using MKService.DefaultModel;
using MKService.ModelFactories;
using MKService.Queries;
using MKService.Updates;

namespace MKService.QueryFactories
{
    internal class GameQueryFactory : QueryFactoryBase<GameQuery>
    {
        private GameQuery query;
        public GameQueryFactory(
             IModelFactoryResolver modelFactoryResolver, IDefaultModel defaultModel)
             : base(modelFactoryResolver, defaultModel)
        {
        }

        public override GameQuery Create()
        {
            if (this.query != null)
            {
                return this.query;
            }

            this.query = (GameQuery)this.ModelFactoryResolver.GetFactory<IUpdatableGame>().Create();

            return this.query;
        }
    }

    internal class GamesQueryFactory : QueryFactoryBase<GamesQuery>
    {
        private GamesQuery query;
        public GamesQueryFactory(
             IModelFactoryResolver modelFactoryResolver, IDefaultModel defaultModel)
             : base(modelFactoryResolver, defaultModel)
        {
        }

        public override GamesQuery Create()
        {
            if (this.query != null)
            {
                return this.query;
            }

            this.query = (GamesQuery)this.ModelFactoryResolver.GetFactory<IUpdatableGames>().Create();

            return this.query;
        }
    }
}
