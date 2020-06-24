using MKService.Messages;
using MKService.ModelFactories;
using MKService.Updates;
using System.Linq;
using MKService.DefaultModel;

namespace MKService.ModelUpdaters
{
    internal class GameJoinedUpdater : ModelUpdaterBase<IUpdatableGame, GameJoined>
    {
        public GameJoinedUpdater(
            IModelFactoryResolver modelFactoryResolver, IDefaultModel defaultMode)
            : base(modelFactoryResolver, defaultMode)
        {
        }

        protected override bool UpdateInternal(IUpdatableGame model, GameJoined message)
        {
            model.User2Id = ServiceTypeProvider.Instance.UserCollection.Users.FirstOrDefault(x => x.Id ==  message.User2Id).Id;
            return true;
        }
    }
}
