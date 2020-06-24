
using MKService.Messages;
using MKService.ModelFactories;
using MKService.Updates;
using System.Linq;
using MKService.DefaultModel;

namespace MKService.ModelUpdaters
{
    internal class GameChangedModelUpdater : ModelUpdaterBase<IUpdatableGame, GameChanged>
    {
        public GameChangedModelUpdater(
            IModelFactoryResolver modelFactoryResolver, IDefaultModel defaultMode)
            : base(modelFactoryResolver, defaultMode)
        {
        }

        protected override bool UpdateInternal(IUpdatableGame model, GameChanged message)
        {
            return true;
        }
    }

    internal class GamesChangedModelUpdater : ModelUpdaterBase<IUpdatableGames, GamesChanged>
    {
        public GamesChangedModelUpdater(
            IModelFactoryResolver modelFactoryResolver, IDefaultModel defaultMode)
            : base(modelFactoryResolver, defaultMode)
        {
        }

        protected override bool UpdateInternal(IUpdatableGames model, GamesChanged message)
        {
            var game =  this.ModelFactoryResolver.GetFactory<IUpdatableGame>().Create();
            game.User1Id = ServiceTypeProvider.Instance.UserCollection.Users.FirstOrDefault(x => x.Id == message.Id).Id;
            model.Games.Add(game);
            return true;
        }
    }
}
