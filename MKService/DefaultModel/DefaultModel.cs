using MKModel;
using MKService.ModelFactories;
using MKService.Updates;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Windows.Controls;

namespace MKService.DefaultModel
{
    /// <summary>
    /// Default Model.
    /// </summary>
    /// <seealso cref="MKService.DefaultModel.IDefaultModel" />
    internal class DefaultModel : IDefaultModel
    {
        private ModelFactoryResolver modelFactoryResolver;

        private readonly IUpdatableUserCollection userCollection;
       
        public DefaultModel(ModelFactoryResolver modelFactoryResolver)
        {
            this.modelFactoryResolver = modelFactoryResolver;

            this.userCollection = this.modelFactoryResolver.GetFactory<IUpdatableUserCollection>().Create();
            this.BuildData();
        }

        private void BuildData()
        {
           
            ObservableCollection<UserData> users = UserDataDBService.GetUsers();
            foreach(UserData userData in users)
            {
                var user = this.modelFactoryResolver.GetFactory<IUpdatableUser>().Create();
                user.UserName = userData.UserName;
                user.Password = userData.Password;
                user.UserName = userData.UserName;
                user.Id = userData.Id;
                user.RebellionBoosterPacks = userData.RebellionBoosterPacks;

                ObservableCollection<MageKnightData> inventory = UserDataDBService.GetUserInventory(user.Id);
                foreach(var magedata in inventory)
                {
                    var mage = this.modelFactoryResolver.GetFactory<IUpdatableMageKnight>().Create();
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
                    var dial = this.modelFactoryResolver.GetFactory<IUpdatableDial>().Create();
                    foreach (var click in magedata.Dial.Clicks)
                    {
                        var query = this.modelFactoryResolver.GetFactory<IUpdatableClick>().Create();
                        query.Attack = this.modelFactoryResolver.GetFactory<IUpdatableStat>().Create();
                        query.Speed = this.modelFactoryResolver.GetFactory<IUpdatableStat>().Create();
                        query.Defense = this.modelFactoryResolver.GetFactory<IUpdatableStat>().Create();
                        query.Damage = this.modelFactoryResolver.GetFactory<IUpdatableStat>().Create();

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

                        foreach(var stat in click.Stats)
                        {
                            var statQuery = this.modelFactoryResolver.GetFactory<IUpdatableStat>().Create();
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
                    user.Inventory.Add(mage);
                }

                this.userCollection.Users.Add(user);
            }
        }

        public void LoadDataInfo(IUpdatableUserCollection userCollection)
        {
            userCollection.Users.AddRange(this.userCollection.Users);
        }

        public void OpenBooster(IUpdatableUser model, BoosterPack set)
        {
            var user = userCollection.Users.FirstOrDefault(x=> x.Id == model.Id);
            try
            {
                for (int i = 0; i < 5; i++)
                {
                    MageKnightData magedata = MageDB.GetRandomMage(set);
                    UserDataDB.AddMageToInventory(magedata.Id, user.Id);

                    var mage = this.modelFactoryResolver.GetFactory<IUpdatableMageKnight>().Create();
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
                    var dial = this.modelFactoryResolver.GetFactory<IUpdatableDial>().Create();
                    foreach (var click in magedata.Dial.Clicks)
                    {
                        var query = this.modelFactoryResolver.GetFactory<IUpdatableClick>().Create();
                        query.Attack = this.modelFactoryResolver.GetFactory<IUpdatableStat>().Create();
                        query.Speed = this.modelFactoryResolver.GetFactory<IUpdatableStat>().Create();
                        query.Defense = this.modelFactoryResolver.GetFactory<IUpdatableStat>().Create();
                        query.Damage = this.modelFactoryResolver.GetFactory<IUpdatableStat>().Create();

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
                            var statQuery = this.modelFactoryResolver.GetFactory<IUpdatableStat>().Create();
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
                    ServiceTypeProvider.Instance.UserCollection.Users.FirstOrDefault(x => x.Id == user.Id).AddMageToInventory(mage.Id);
                    user.Inventory.Add(mage);
                }
            }
            catch (Exception e)
            {
                ;
            }
            

            UserDataDB.UpdateUsersBoosterPackCount(set, user.Id, user.RebellionBoosterPacks);
        }
    }
}