using MKService.Messages;
using MKService.ModelUpdaters;
using MKService.Queries;
using MKService.Updates;

namespace MKService.MessageHandlers
{
    internal class UserSignUpHandler : ServerMessageHandlerBase<UserSignUp, UserCollectionQuery, IUpdatableUserCollection>
    {
        public UserSignUpHandler(IModelUpdaterResolver modelUpdaterResolver) : base(modelUpdaterResolver)
        {
        }

        protected override IUpdatableUserCollection LocateQueryComponent(UserSignUp message, UserCollectionQuery query)
        {
            return query;
        }
    }
}
