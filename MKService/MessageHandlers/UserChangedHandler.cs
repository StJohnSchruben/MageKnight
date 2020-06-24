
using MKService.Messages;
using MKService.ModelUpdaters;
using MKService.Queries;
using MKService.Updates;

namespace MKService.MessageHandlers
{
    internal class UserChangedHandler : ServerMessageHandlerBase<UserChanged, UserQuery, IUpdatableUser>
    {
        public UserChangedHandler(IModelUpdaterResolver modelUpdaterResolver) : base(modelUpdaterResolver)
        {
        }

        protected override IUpdatableUser LocateQueryComponent(UserChanged message, UserQuery query)
        {
            return query;
        }
    }

    internal class UserInventoryAddHandler : ServerMessageHandlerBase<UserInventoryAdd, UserQuery, IUpdatableUser>
    {
        public UserInventoryAddHandler(IModelUpdaterResolver modelUpdaterResolver) : base(modelUpdaterResolver)
        {
        }

        protected override IUpdatableUser LocateQueryComponent(UserInventoryAdd message, UserQuery query)
        {
            return query;
        }
    }

    internal class UserArmyAddHandler : ServerMessageHandlerBase<UserArmyAdd, UserQuery, IUpdatableUser>
    {
        public UserArmyAddHandler(IModelUpdaterResolver modelUpdaterResolver) : base(modelUpdaterResolver)
        {
        }

        protected override IUpdatableUser LocateQueryComponent(UserArmyAdd message, UserQuery query)
        {
            return query;
        }
    }
}
