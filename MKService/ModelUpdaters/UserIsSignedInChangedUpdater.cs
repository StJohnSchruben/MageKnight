
using MKService.Messages;
using MKService.ModelFactories;
using MKService.Updates;
using MKService.DefaultModel;


namespace MKService.ModelUpdaters
{
    internal class UserIsSignedInChangedUpdater : ModelUpdaterBase<IUpdatableUser, UserIsSignedInChanged>
    {
        public UserIsSignedInChangedUpdater(
            IModelFactoryResolver modelFactoryResolver, IDefaultModel defaultMode)
            : base(modelFactoryResolver, defaultMode)
        {
        }

        protected override bool UpdateInternal(IUpdatableUser model, UserIsSignedInChanged message)
        {
            if (message.UserId != model.Id)
            {
                return false;
            }

            model.IsSignedIn = message.IsSignedIn;

            return true;
        }
    }
}
