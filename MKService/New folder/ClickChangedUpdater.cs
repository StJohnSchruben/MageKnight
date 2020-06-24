using MKService.Messages;
using MKService.ModelFactories;
using MKService.Updates;

namespace MKService.ModelUpdaters
{
    internal class ClickAddedUpdater : ModelUpdaterBase<IUpdatableClick, ClickAdd>
    {
        public ClickAddedUpdater(
               IModelFactoryResolver modelFactoryResolver)
               : base(modelFactoryResolver)
        {
        }

        protected override bool UpdateInternal(IUpdatableClick model, ClickAdd message)
        {
            var user = this.ModelFactoryResolver.GetFactory<IUpdatableMageKnight>().Create();

            return true;
        }
    }

}
