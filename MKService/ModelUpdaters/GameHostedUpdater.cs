using MKService.Messages;
using MKService.ModelFactories;
using MKService.Updates;
using MKService.DefaultModel;

namespace MKService.ModelUpdaters
{
    internal class GameHostedUpdater : ModelUpdaterBase<IUpdatableGames, GameHosted>
    {
        public GameHostedUpdater(
            IModelFactoryResolver modelFactoryResolver, IDefaultModel defaultMode)
            : base(modelFactoryResolver, defaultMode)
        {
        }

        protected override bool UpdateInternal(IUpdatableGames model, GameHosted message)
        {
            var newGame = this.ModelFactoryResolver.GetFactory<IUpdatableGame>().Create();
            newGame.Id = message.GameId;
            newGame.User1Id = ServiceTypeProvider.Instance.UserCollection.GetUser(message.User1Id).Id;
            return true;
        }
    }
}
