using MKService.Messages;
using MKService.ModelUpdaters;
using MKService.Queries;
using MKService.Updates;

namespace MKService.MessageHandlers
{
    internal class UserSignInHandler : ServerMessageHandlerBase<UserSignIn, UserCollectionQuery, IUpdatableUser>
    {
        public UserSignInHandler(IModelUpdaterResolver modelUpdaterResolver) : base(modelUpdaterResolver)
        {
        }

        protected override IUpdatableUser LocateQueryComponent(UserSignIn message, UserCollectionQuery query)
        {
            return (IUpdatableUser)query.GetUser(message.UserId);
        }
    }
}
