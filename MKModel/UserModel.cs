using GalaSoft.MvvmLight;
using ReDefNet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MKModel
{
    public interface IUserModel : INotifyPropertyChanged
    {
        IReadOnlyObservableCollection<IMageKnightModel> Inventory { get; }
        IReadOnlyObservableCollection<IMageKnightModel> Army { get; }

        IMageKnightModel SelectedMage { get; set; }

        int RebellionBoosterPacks { get; set; }

        string UserName { get; set; }
        string Password { get; set; }
        Guid Id { get; set; }
        bool IsSignedIn { get; set; }

        void UpdateInventory(ObservableCollection<IMageKnightModel> inventory);

        void SignIn(string userName, string password);

        void AddMageToArmy(Guid id, Guid instantiatedId);

        void AddMageToInventory(Guid id);

        void OpenBooserPack(BoosterPack rebellion);
    }

    public class UserModel : ViewModelBase
    {
        UserData user;
        SerializableObservableCollection<IMageKnightModel> inventory = new SerializableObservableCollection<IMageKnightModel>();
        SerializableObservableCollection<IMageKnightModel> army = new SerializableObservableCollection<IMageKnightModel>();
        public UserModel(UserData user)
        {
            this.user = user;
        }

        public string UserName => this.user.UserName;
        public string Password => this.user.Password;
        public Guid Id => this.user.Id;
        public int RebellionBoosterPacks => this.user.RebellionBoosterPacks;
        //public int WhirlWindBoosterPacks => this.user.WhirlWindBoosterPacks;
        //public int LancersBoosterPacks => this.user.LancersBoosterPacks;
        //public int UnlimitedBoosterPacks => this.user.UnlimitedBoosterPacks;
        //public int SinisterBoosterPacks => this.user.SinisterBoosterPacks;
        //public int MinionsBoosterPacks => this.user.MinionsBoosterPacks;
        //public int UprisingBoosterPacks => this.user.UprisingBoosterPacks;
    }
}
