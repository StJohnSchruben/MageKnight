using MKService.DefaultModel;
using MKService.Messages;
using MKService.ModelFactories;
using MKService.Updates;

namespace MKService.ModelUpdaters
{
    internal class UserCollectionAddedUpdater : ModelUpdaterBase<IUpdatableUserCollection, UserCollectionAdd>
    {
        public UserCollectionAddedUpdater(
            IModelFactoryResolver modelFactoryResolver, IDefaultModel defaultMode)
            : base(modelFactoryResolver, defaultMode)
        {
        }

        protected override bool UpdateInternal(IUpdatableUserCollection model, UserCollectionAdd message)
        {
            var user = this.ModelFactoryResolver.GetFactory<IUpdatableMageKnight>().Create();

            return true;
        }
    }

}
