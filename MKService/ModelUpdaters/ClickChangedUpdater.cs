using MKService.DefaultModel;
using MKService.Messages;
using MKService.ModelFactories;
using MKService.Updates;

namespace MKService.ModelUpdaters
{
    internal class ClickAddedUpdater : ModelUpdaterBase<IUpdatableClick, ClickAdd>
    {
        public ClickAddedUpdater(
            IModelFactoryResolver modelFactoryResolver, IDefaultModel defaultMode)
            : base(modelFactoryResolver, defaultMode)
        {
        }

        protected override bool UpdateInternal(IUpdatableClick model, ClickAdd message)
        {
            var user = this.ModelFactoryResolver.GetFactory<IUpdatableMageKnight>().Create();

            return true;
        }
    }
}
