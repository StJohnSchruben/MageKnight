using MKService.DefaultModel;
using MKService.Messages;
using MKService.ModelFactories;
using MKService.Updates;

namespace MKService.ModelUpdaters
{
    internal class DialAddedUpdater : ModelUpdaterBase<IUpdatableDial, DialAdd>
    {
        public DialAddedUpdater(
            IModelFactoryResolver modelFactoryResolver, IDefaultModel defaultMode)
            : base(modelFactoryResolver, defaultMode)
        {
        }

        protected override bool UpdateInternal(IUpdatableDial model, DialAdd message)
        {
            var user = this.ModelFactoryResolver.GetFactory<IUpdatableMageKnight>().Create();

            return true;
        }
    }

}
