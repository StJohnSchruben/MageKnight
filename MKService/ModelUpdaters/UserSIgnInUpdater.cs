using MKModel;
using MKService.Messages;
using MKService.ModelFactories;
using MKService.Updates;
using MKService.DefaultModel;

namespace MKService.ModelUpdaters
{
    internal class UserSignInUpdater : ModelUpdaterBase<IUpdatableUser, UserSignIn>
    {
        public UserSignInUpdater(
            IModelFactoryResolver modelFactoryResolver, IDefaultModel defaultMode)
            : base(modelFactoryResolver, defaultMode)
        {
        }

        protected override bool UpdateInternal(IUpdatableUser model, UserSignIn message)
        {
            if (model.Id == message.UserId && model.IsSignedIn != message.IsSignedIn)
            {
                model.IsSignedIn = message.IsSignedIn;
                return true;
            }

            return false;
        }
    }
}
