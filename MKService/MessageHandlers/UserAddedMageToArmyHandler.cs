using MKService.Messages;
using MKService.ModelUpdaters;
using MKService.Queries;
using MKService.Updates;
using System.Linq;

namespace MKService.MessageHandlers
{
    internal class UserAddedMageToArmyHandler : ServerMessageHandlerBase<UserAddedMageToArmy, UserCollectionQuery, IUpdatableUser>
    {
        public UserAddedMageToArmyHandler(IModelUpdaterResolver modelUpdaterResolver) : base(modelUpdaterResolver)
        {
        }

        protected override IUpdatableUser LocateQueryComponent(UserAddedMageToArmy message, UserCollectionQuery query)
        {
            return (IUpdatableUser)query.GetUser(message.UserId);
        }
    }

    internal class UserAddedMageToInventoryHandler : ServerMessageHandlerBase<UserAddedMageToInventory, UserCollectionQuery, IUpdatableUser>
    {
        public UserAddedMageToInventoryHandler(IModelUpdaterResolver modelUpdaterResolver) : base(modelUpdaterResolver)
        {
        }

        protected override IUpdatableUser LocateQueryComponent(UserAddedMageToInventory message, UserCollectionQuery query)
        {
            return (IUpdatableUser)query.GetUser(message.UserId);
        }
    }
}
