using MKService.Messages;
using MKService.ModelUpdaters;
using MKService.Queries;
using MKService.Updates;
using System.Linq;

namespace MKService.MessageHandlers
{
    internal class UserBoosterPackCountChangedHandler : ServerMessageHandlerBase<UserBoosterPackCountChanged, UserCollectionQuery, IUpdatableUser>
    {
        public UserBoosterPackCountChangedHandler(IModelUpdaterResolver modelUpdaterResolver) : base(modelUpdaterResolver)
        {
        }

        protected override IUpdatableUser LocateQueryComponent(UserBoosterPackCountChanged message, UserCollectionQuery query)
        {
            return (IUpdatableUser)query.GetUser(message.UserId);
        }
    }
}
