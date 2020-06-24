
using MKService.DefaultModel;
using MKService.Messages;
using MKService.ModelFactories;
using MKService.Updates;


namespace MKService.ModelUpdaters
{
    internal class UserChangedUpdater : ModelUpdaterBase<IUpdatableUser, UserChanged>
    {
        public UserChangedUpdater(
            IModelFactoryResolver modelFactoryResolver, IDefaultModel defaultMode)
            : base(modelFactoryResolver, defaultMode)
        {
        }

        protected override bool UpdateInternal(IUpdatableUser model, UserChanged message)
        {

            if (model.Id != message.Id)
            {
                return false;
            }

            return true;
        }
    }

    internal class UserInventoryAddUpdater : ModelUpdaterBase<IUpdatableUser, UserInventoryAdd>
    {
        public UserInventoryAddUpdater(
            IModelFactoryResolver modelFactoryResolver, IDefaultModel defaultMode)
            : base(modelFactoryResolver, defaultMode)
        {
        }

        protected override bool UpdateInternal(IUpdatableUser model, UserInventoryAdd message)
        {
            var mage = this.ModelFactoryResolver.GetFactory<IUpdatableMageKnight>().Create();
            mage.Click = message.Click;
            mage.Dial = message.Dial;
            mage.Faction = message.Faction;
            mage.FrontArc = message.FrontArc;
            mage.Id = message.Id;
            mage.Index = message.Index;
            mage.ModelImage = message.ModelImage;
            mage.Name = message.Name;
            mage.PointValue = message.PointValue;
            mage.Range = message.Range;
            mage.Rank = message.Rank;
            mage.Set = message.Set;
            mage.Targets = message.Targets;
            model.Inventory.Add(mage);

            return true;
        }
    }

    internal class UserArmyAddUpdater : ModelUpdaterBase<IUpdatableUser, UserArmyAdd>
    {
        public UserArmyAddUpdater(
            IModelFactoryResolver modelFactoryResolver, IDefaultModel defaultMode)
            : base(modelFactoryResolver, defaultMode)
        {
        }

        protected override bool UpdateInternal(IUpdatableUser model, UserArmyAdd message)
        {
            var mage = this.ModelFactoryResolver.GetFactory<IUpdatableMageKnight>().Create();
            mage.Click = message.Click;
            mage.Dial = message.Dial;
            mage.Faction = message.Faction;
            mage.FrontArc = message.FrontArc;
            mage.Id = message.Id;
            mage.Index = message.Index;
            mage.ModelImage = message.ModelImage;
            mage.Name = message.Name;
            mage.PointValue = message.PointValue;
            mage.Range = message.Range;
            mage.Rank = message.Rank;
            mage.Set = message.Set;
            mage.Targets = message.Targets;
            model.Army.Add(mage);

            return true;
        }
    }
}
