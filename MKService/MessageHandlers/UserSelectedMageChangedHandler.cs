using MKService.MessageHandlers;
using MKService.Messages;
using MKService.ModelUpdaters;
using MKService.Queries;
using MKService.Updates;

namespace MKService.MessageHandlers
{ 
    internal class UserSelectedMageChangedHandler : ServerMessageHandlerBase<UserSelectedMageChanged, UserCollectionQuery, IUpdatableUser>
    {
        public UserSelectedMageChangedHandler(IModelUpdaterResolver modelUpdaterResolver) : base(modelUpdaterResolver)
        {
        }

        protected override IUpdatableUser LocateQueryComponent(UserSelectedMageChanged message, UserCollectionQuery query)
        {
            return (IUpdatableUser)query.GetUser(message.UserId);
        }
    }
}
