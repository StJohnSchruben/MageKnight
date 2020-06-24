
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.Serialization;
using log4net;
using MKModel;
using MKService.Updates;
using ReDefNet;

namespace MKService.Queries
{
    [DataContract(Namespace = ServiceConstants.QueryNamespace)]
    public class UserQuery : ObservableObject, IUpdatableUser
    {
        private readonly ILog log = LogManager.GetLogger(nameof(UserQuery));

        [DataMember]
        private SerializableObservableCollection<IUpdatableMageKnight> inventory;

        [DataMember]
        private SerializableObservableCollection<IUpdatableMageKnight> army;

        [DataMember]
        private int boosterPacks;

        [DataMember]
        private string userName;

        [DataMember]
        private string password;

        [DataMember]
        private Guid id;

        [DataMember]
        private IMageKnightModel selectedMage;

        [DataMember]
        private bool isSignedIn;

        public UserQuery()
        {
            this.initialize();
        }

        private void initialize()
        {
            inventory = new SerializableObservableCollection<IUpdatableMageKnight>();
            army = new SerializableObservableCollection<IUpdatableMageKnight>();

        }
        public void UpdateInventory(ObservableCollection<IMageKnightModel> inventory)
        {
            throw new NotImplementedException();
        }
        public void SignIn(string userName, string password)
        {
            throw new NotImplementedException();
        }
        public void AddMageToArmy(Guid id, Guid instantiatedId)
        {
            throw new NotImplementedException();
        }
        public void AddMageToInventory(Guid id)
        {
            throw new NotImplementedException();
        }
        public void OpenBooserPack(BoosterPack rebellion)
        {
            throw new NotImplementedException();
        }
        public IReadOnlyObservableCollection<IMageKnightModel> Inventory { get { return this.inventory; } }
        IObservableCollection<IUpdatableMageKnight> IUpdatableUser.Inventory { get { return this.inventory; } }
        IObservableCollection<IUpdatableMageKnight> IUpdatableUser.Army { get { return this.army; } }
        public IReadOnlyObservableCollection<IMageKnightModel> Army { get { return this.army; } }

        public int RebellionBoosterPacks { get { return this.boosterPacks; } set { this.Set(() => this.RebellionBoosterPacks, ref this.boosterPacks, value); } }

        public string UserName { get { return this.userName; } set { this.Set(() => this.UserName, ref this.userName, value); } }

        public string Password { get { return this.password; } set { this.Set(() => this.Password, ref this.password, value); } }

        public Guid Id { get { return this.id; } set { this.Set(() => this.Id, ref this.id, value); } }

        public IMageKnightModel SelectedMage { get { return this.selectedMage; } set { this.Set(() => this.SelectedMage, ref this.selectedMage, value); } }

        public bool IsSignedIn { get { return this.isSignedIn; } set { this.Set(() => this.IsSignedIn, ref this.isSignedIn, value); } }
    }
}