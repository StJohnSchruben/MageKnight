using MKService.DefaultModel;
using MKService.Messages;
using MKService.ModelFactories;
using MKService.Updates;

namespace MKService.ModelUpdaters
{ 
    internal class MageKnightCoordinatesChangedUpdater : ModelUpdaterBase<IUpdatableMageKnight, MageKnightCoordinatesChanged>
    {
        public MageKnightCoordinatesChangedUpdater(
            IModelFactoryResolver modelFactoryResolver, IDefaultModel defaultMode)
            : base(modelFactoryResolver, defaultMode)
        {
        }
        protected override bool UpdateInternal(IUpdatableMageKnight model, MageKnightCoordinatesChanged message)
        {
            if (model.InstantiatedId != message.InstantiatedMageId)
            {
                return false;
            }

            model.XCoordinate = message.XCoordinate;
            model.YCoordinate = message.YCoordinate;

            return true;
        }
    }
}
