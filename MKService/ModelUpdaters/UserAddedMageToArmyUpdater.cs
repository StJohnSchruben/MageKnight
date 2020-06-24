using MKService.Messages;
using MKService.ModelFactories;
using MKService.Updates;
using System.Linq;
using MKService.DefaultModel;
using MKModel;

namespace MKService.ModelUpdaters
{
    internal class UserAddedMageToArmyUpdater : ModelUpdaterBase<IUpdatableUser, UserAddedMageToArmy>
    {
        public UserAddedMageToArmyUpdater(
            IModelFactoryResolver modelFactoryResolver, IDefaultModel defaultMode)
            : base(modelFactoryResolver, defaultMode)
        {
        }

        protected override bool UpdateInternal(IUpdatableUser model, UserAddedMageToArmy message)
        {
            if (message.UserId != model.Id)
            {
                return false;
            }

            var query = model.Inventory.FirstOrDefault(x => x.Id == message.Id);
            var mage = this.ModelFactoryResolver.GetFactory<IUpdatableMageKnight>().Create();
            var dial = this.ModelFactoryResolver.GetFactory<IUpdatableDial>().Create();
            foreach (var click in query.Dial.Clicks)
            {
                var clickQuery = this.ModelFactoryResolver.GetFactory<IUpdatableClick>().Create();
                clickQuery.Attack = this.ModelFactoryResolver.GetFactory<IUpdatableStat>().Create();
                clickQuery.Speed = this.ModelFactoryResolver.GetFactory<IUpdatableStat>().Create();
                clickQuery.Defense = this.ModelFactoryResolver.GetFactory<IUpdatableStat>().Create();
                clickQuery.Damage = this.ModelFactoryResolver.GetFactory<IUpdatableStat>().Create();

                clickQuery.Attack.Value = click.Attack.Value;
                clickQuery.Attack.Ability = click.Attack.Ability;
                clickQuery.Attack.StatType = click.Attack.StatType;
                clickQuery.Speed.Value = click.Speed.Value;
                clickQuery.Speed.Ability = click.Speed.Ability;
                clickQuery.Speed.StatType = click.Speed.StatType;
                clickQuery.Defense.Value = click.Defense.Value;
                clickQuery.Defense.Ability = click.Defense.Ability;
                clickQuery.Defense.StatType = click.Defense.StatType;
                clickQuery.Damage.Value = click.Damage.Value;
                clickQuery.Damage.Ability = click.Damage.Ability;
                clickQuery.Damage.StatType = click.Damage.StatType;
                clickQuery.Index = click.Index;

                foreach (var stat in click.Stats)
                {
                    var statQuery = this.ModelFactoryResolver.GetFactory<IUpdatableStat>().Create();
                    statQuery.Ability = stat.Ability;
                    statQuery.Value = stat.Value;
                    statQuery.StatType = stat.StatType;
                    clickQuery.Stats.Add(statQuery);
                }

                dial.Clicks.Add(clickQuery);
            }
            mage.Dial = dial; 
            mage.Click = query.Click;
            mage.Faction = query.Faction;
            mage.FrontArc = query.FrontArc;
            mage.Id = query.Id;
            mage.Index = query.Index;
            mage.ModelImage = query.ModelImage;
            mage.Name = query.Name;
            mage.PointValue = query.PointValue;
            mage.Range = query.Range;
            mage.Rank = query.Rank;
            mage.Set = query.Set;
            mage.Targets = query.Targets;
            mage.InstantiatedId = message.InstantiatedId;
            model.Army.Add(query);
            return true;
        }
    }
    internal class UserAddedMageToInventoryUpdater : ModelUpdaterBase<IUpdatableUser, UserAddedMageToInventory>
    {
        public UserAddedMageToInventoryUpdater(
            IModelFactoryResolver modelFactoryResolver, IDefaultModel defaultMode)
            : base(modelFactoryResolver, defaultMode)
        {
        }

        protected override bool UpdateInternal(IUpdatableUser model, UserAddedMageToInventory message)
        {
            if (message.UserId != model.Id)
            {
                return false;
            }

            MageKnightData magedata = MageDB.GetMageKnight(message.Id);
            var mage = this.ModelFactoryResolver.GetFactory<IUpdatableMageKnight>().Create();
            mage.Faction = magedata.Faction;
            mage.FrontArc = magedata.FrontArc;
            mage.Id = magedata.Id;
            mage.Index = magedata.Index;
            mage.ModelImage = magedata.ModelImage;
            mage.Name = magedata.Name;
            mage.PointValue = magedata.PointValue;
            mage.Range = magedata.Range;
            mage.Rank = magedata.Rank;
            mage.Set = magedata.Set;
            var dial = this.ModelFactoryResolver.GetFactory<IUpdatableDial>().Create();
            foreach (var click in magedata.Dial.Clicks)
            {
                var query = this.ModelFactoryResolver.GetFactory<IUpdatableClick>().Create();
                query.Attack = this.ModelFactoryResolver.GetFactory<IUpdatableStat>().Create();
                query.Speed = this.ModelFactoryResolver.GetFactory<IUpdatableStat>().Create();
                query.Defense = this.ModelFactoryResolver.GetFactory<IUpdatableStat>().Create();
                query.Damage = this.ModelFactoryResolver.GetFactory<IUpdatableStat>().Create();

                query.Attack.Value = click.Attack.Value;
                query.Attack.Ability = click.Attack.Ability;
                query.Attack.StatType = click.Attack.StatType;
                query.Speed.Value = click.Speed.Value;
                query.Speed.Ability = click.Speed.Ability;
                query.Speed.StatType = click.Speed.StatType;
                query.Defense.Value = click.Defense.Value;
                query.Defense.Ability = click.Defense.Ability;
                query.Defense.StatType = click.Defense.StatType;
                query.Damage.Value = click.Damage.Value;
                query.Damage.Ability = click.Damage.Ability;
                query.Damage.StatType = click.Damage.StatType;
                query.Index = click.Index;

                foreach (var stat in click.Stats)
                {
                    var statQuery = this.ModelFactoryResolver.GetFactory<IUpdatableStat>().Create();
                    statQuery.Ability = stat.Ability;
                    statQuery.Value = stat.Value;
                    statQuery.StatType = stat.StatType;
                    query.Stats.Add(statQuery);
                }

                dial.Clicks.Add(query);
            }

            dial.ClickIndex = magedata.Dial.ClickIndex;
            dial.Click = dial.Clicks.First();
            dial.Name = magedata.Dial.Name;
            mage.Dial = dial;
            model.Inventory.Add(mage);
            return true;
        }
    }
}
