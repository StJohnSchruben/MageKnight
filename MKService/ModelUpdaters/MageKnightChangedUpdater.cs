
using MKService.DefaultModel;
using MKService.Messages;
using MKService.ModelFactories;
using MKService.Updates;

namespace MKService.ModelUpdaters
{
    internal class MageKnightChangedUpdater : ModelUpdaterBase<IUpdatableMageKnight, MageKnightChanged>
    {
        public MageKnightChangedUpdater(
            IModelFactoryResolver modelFactoryResolver, IDefaultModel defaultMode)
            : base(modelFactoryResolver, defaultMode)
        {
        }
        protected override bool UpdateInternal(IUpdatableMageKnight model, MageKnightChanged message)
        {

            if(model.Id != message.NewModel.Id)
            {
                return false;
            }

            model.Name = message.NewModel.Name;
            model.Index = message.NewModel.Index;
            model.Range = message.NewModel.Range;
            model.PointValue = message.NewModel.PointValue;
            model.FrontArc = message.NewModel.FrontArc;
            model.Targets = message.NewModel.Targets;
            model.Click = message.NewModel.Click;
            model.Set = message.NewModel.Set;
            model.Faction = message.NewModel.Faction;
            model.Rank = message.NewModel.Rank;
            model.ModelImage = message.NewModel.ModelImage;

            return true;
        }
    }
}
