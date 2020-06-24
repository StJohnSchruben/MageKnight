
using MKService.Messages;
using MKService.ModelUpdaters;
using MKService.Queries;
using MKService.Updates;

namespace MKService.MessageHandlers
{
    internal class UserCollectionAddedHandler : ServerMessageHandlerBase<UserCollectionAdd, UserCollectionQuery, IUpdatableUserCollection>
    {
        public UserCollectionAddedHandler(IModelUpdaterResolver modelUpdaterResolver) : base(modelUpdaterResolver)
        {
        }

        protected override IUpdatableUserCollection LocateQueryComponent(UserCollectionAdd message, UserCollectionQuery query)
        {
            return query;
        }
    }
}
