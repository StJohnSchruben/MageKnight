
using System;
using log4net;
using MKService.Messages;
using MKService.ModelUpdaters;
using MKService.Updates;
using MKModel;
using System.Collections.ObjectModel;
using ReDefNet;
using System.Linq;

namespace MKService.Proxies
{
    internal class UserProxy : ProxyBase, IUserModel
    {
        private readonly ILog log = LogManager.GetLogger(nameof(UserProxy));
        private readonly SerializableObservableCollection<IMageKnightModel> inventoryProxies;
        private readonly SerializableObservableCollection<IMageKnightModel> armyProxies;
        private readonly IUpdatableUser model;

        public UserProxy(
            IServiceClient serviceClient,
            IModelUpdaterResolver modelUpdaterResolver,
            IUpdatableUser model)
            : base(serviceClient, modelUpdaterResolver)
        {
            this.model = model;
            this.inventoryProxies = new SerializableObservableCollection<IMageKnightModel>();
            this.armyProxies = new SerializableObservableCollection<IMageKnightModel>();
            foreach (var mage in this.model.Inventory)
            {
                this.inventoryProxies.Add(new MageKnightProxy(serviceClient, modelUpdaterResolver, mage));
            }
            foreach (var mage in this.model.Army)
            {
                this.armyProxies.Add(new MageKnightProxy(serviceClient, modelUpdaterResolver, mage));
            }

            this.model.Inventory.CollectionChanged += Inventory_CollectionChanged;
            this.model.Army.CollectionChanged += Army_CollectionChanged;

            this.SetUpModelPropertyChangedPropagation(this.model);

            this.SubscribeToMessage<UserChanged>(this.Handle);
            this.SubscribeToMessage<UserInventoryAdd>(this.Handle);
            this.SubscribeToMessage<UserAddedMageToArmy>(this.Handle);
            this.SubscribeToMessage<UserAddedMageToInventory>(this.Handle);
        }

        private void Army_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (var mage in e.NewItems.Cast<IUpdatableMageKnight>())
                {
                    this.armyProxies.Add(new MageKnightProxy(ServiceClient, ModelUpdaterResolver, mage));
                }
            }
            else
            {
                foreach (var mage in e.OldItems.Cast<IUpdatableMageKnight>())
                {
                    this.armyProxies.Remove(mage);
                }
            }
        }

        private void Inventory_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (var mage in e.NewItems.Cast<IUpdatableMageKnight>())
                {
                    this.inventoryProxies.Add(new MageKnightProxy(ServiceClient, ModelUpdaterResolver, mage));
                }
            }
            else
            {
                foreach (var mage in e.OldItems.Cast<IUpdatableMageKnight>())
                {
                    this.inventoryProxies.Remove(mage);
                }
            }
        }

        //todo: fix the sets on these to actually set the model.
        public Guid Id
        {
            get => this.model.Id;
            set
            {
                var message = new UserChanged
                {
                    Id = this.model.Id
                };

                bool changed = this.ModelUpdaterResolver.GetUpdater<IUpdatableUser, UserChanged>().Update(this.model, message);

                if (changed)
                {
                    this.ServiceClient.PublishAsync<UserChanged>(message);
                }
            }
        }
        public IReadOnlyObservableCollection<IMageKnightModel> Inventory => this.inventoryProxies;
        public IReadOnlyObservableCollection<IMageKnightModel> Army => this.armyProxies;

        public int RebellionBoosterPacks
        {
            get => this.model.RebellionBoosterPacks;
            set
            {
                var message = new UserBoosterPackCountChanged
                {
                    UserId = this.model.Id,
                    Set = BoosterPack.Rebellion,
                    Count = value
                };

                this.model.RebellionBoosterPacks = value;

                //bool changed = this.ModelUpdaterResolver.GetUpdater<IUpdatableUser, UserBoosterPackCountChanged>().Update(this.model, message);

                this.ServiceClient.PublishAsync<UserBoosterPackCountChanged>(message);
            }
        }


        public string UserName
        {
            get => this.model.UserName;
            set
            {
                var message = new UserChanged
                {
                    Id = this.model.Id
                };

                bool changed = this.ModelUpdaterResolver.GetUpdater<IUpdatableUser, UserChanged>().Update(this.model, message);

                if (changed)
                {
                    this.ServiceClient.PublishAsync<UserChanged>(message);
                }
            }
        }


        public string Password
        {
            get => this.model.Password;
            set
            {
                var message = new UserChanged
                {
                    Id = this.model.Id
                };

                bool changed = this.ModelUpdaterResolver.GetUpdater<IUpdatableUser, UserChanged>().Update(this.model, message);

                if (changed)
                {
                    this.ServiceClient.PublishAsync<UserChanged>(message);
                }
            }
        }

        public IMageKnightModel SelectedMage 
        {
            get => this.model.SelectedMage;

            set
            {
                this.model.SelectedMage = value;
                var message = new UserSelectedMageChanged
                {
                    UserId = this.model.Id,
                    MageId = this.model.SelectedMage.InstantiatedId
                };

                this.ServiceClient.PublishAsync<UserSelectedMageChanged>(message);
            }
        }

        public bool IsSignedIn
        {
            get => this.model.IsSignedIn;
            set
            {
                var message = new UserIsSignedInChanged
                {
                    UserId = this.model.Id,
                    IsSignedIn = value
                };
                var changed = this.ModelUpdaterResolver.GetUpdater<IUpdatableUser, UserIsSignedInChanged>().Update(this.model, message);
                
                if (changed)
                {
                    this.ServiceClient.PublishAsync<UserIsSignedInChanged>(message);
                }
            }
        }

        public void UpdateInventory(ObservableCollection<IMageKnightModel> inventory)
        {
            foreach (var mage in inventory)
            {
                var message = new UserInventoryAdd
                {
                    Click = mage.Click,
                    Dial = mage.Dial,
                    Faction = mage.Faction,
                    FrontArc = mage.FrontArc,
                    Id = mage.Id,
                    Index = mage.Index,
                    ModelImage = mage.ModelImage,
                    Name = mage.Name,
                    PointValue = mage.PointValue,
                    Range = mage.Range,
                    Rank = mage.Rank,
                    Set = mage.Set,
                    Targets = mage.Targets,
                };

                var changed = this.ModelUpdaterResolver.GetUpdater<IUpdatableUser, UserInventoryAdd>().Update(this.model, message);
                if (changed)
                {
                    this.ServiceClient.PublishAsync<UserInventoryAdd>(message);
                }

                RaisePropertyChanged(nameof(IUserModel.Inventory));
            }

        }

        private void Handle(UserInventoryAdd message)
        {
            this.ModelUpdaterResolver.GetUpdater<IUpdatableUser, UserInventoryAdd>().Update(this.model, message);
        }

        private void Handle(UserChanged message)
        {
            this.ModelUpdaterResolver.GetUpdater<IUpdatableUser, UserChanged>().Update(this.model, message);
        }

        public void SignIn(string userName, string password)
        {
            this.UserName = userName;
            this.Password = password;
        }

        private void Handle(UserAddedMageToArmy message)
        {
            if (message.UserId == this.model.Id)
            {
                this.ModelUpdaterResolver.GetUpdater<IUpdatableUser, UserAddedMageToArmy>().Update(this.model, message);
            }
        }
        private void Handle(UserAddedMageToInventory message)
        {
            if (message.UserId == this.model.Id)
            {
                this.ModelUpdaterResolver.GetUpdater<IUpdatableUser, UserAddedMageToInventory>().Update(this.model, message);
            }
        }

        public void AddMageToArmy(Guid id, Guid instantiatedId)
        {
            var message = new UserAddedMageToArmy
            {
                Id = id,
                UserId = this.model.Id,
                InstantiatedId = instantiatedId
            };

            var changed = this.ModelUpdaterResolver.GetUpdater<IUpdatableUser, UserAddedMageToArmy>().Update(this.model, message);
            if (changed)
            {
                this.ServiceClient.PublishAsync<UserAddedMageToArmy>(message);
            }
        }

        public void AddMageToInventory(Guid id)
        {
            var message = new UserAddedMageToInventory
            {
                Id = id,
                UserId = this.model.Id,
            };

            this.ServiceClient.PublishAsync<UserAddedMageToInventory>(message);
        }

        public void OpenBooserPack(BoosterPack rebellion)
        {
            throw new NotImplementedException();
        }
    }
}
