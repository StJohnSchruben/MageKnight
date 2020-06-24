using MKService.Messages;
using MKService.ModelUpdaters;
using MKService.Queries;
using MKService.Updates;

namespace MKService.MessageHandlers
{
    internal class UserIsSignedInChangedHandler : ServerMessageHandlerBase<UserIsSignedInChanged, UserQuery, IUpdatableUser>
    {
        public UserIsSignedInChangedHandler(IModelUpdaterResolver modelUpdaterResolver) : base(modelUpdaterResolver)
        {
        }

        protected override IUpdatableUser LocateQueryComponent(UserIsSignedInChanged message, UserQuery query)
        {
            return query;
        }
    }
}
