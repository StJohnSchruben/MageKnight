using GalaSoft.MvvmLight;
using MKModel;
using ReDefNet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace MKViewModel
{
   public  interface IUserViewModel : INotifyPropertyChanged
    {
        IMageKnightBattleViewModel SelectedMageKnight { get; set; }
        IMageKnightBattleViewModel TargetedMageKnight { get; set; }

        IObservableCollection <IMageKnightBattleViewModel> ArmyViewModels { get; }

        IUserModel Opponent { get; set; }

        IUserModel Model { get; }

        int Actions { get; set; }

        void AddMage(IMageKnightBattleViewModel mage);
    }

    public class UserViewModel : ViewModelBase, IUserViewModel
    {
        private IUserModel model;
        IUserModel opponent;
        int actions;
        IMageKnightBattleViewModel selectedMageKnight;
        IMageKnightBattleViewModel targetedMageKnight;
        private SerializableObservableCollection<IMageKnightBattleViewModel> army = new SerializableObservableCollection<IMageKnightBattleViewModel>(); 

        public UserViewModel(IUserModel model)
        {
            this.model = model;

            //foreach(var mage in this.model.Army)
            //{
            //    var mageViewModel = new MageKnightBattleViewModel(mage, model);
            //    mageViewModel.PropertyChanged += Mage_PropertyChanged;

            //    int i = this.model.Army.Count;

            //    double range = mage.Range;
            //    double speed = mage.Dial.Click.Speed.Value;
            //    double max = Math.Max(range, speed);
            //    double height = max * 100;
            //    double x = height - 50 - 1100;
            //    double y = height - 50 - 200;

            //    mageViewModel.XCord = -x + 100 * i;
            //    mageViewModel.YCord = -y;
            //    this.army.Add(mageViewModel);
            //}

            this.model.PropertyChanged += Model_PropertyChanged;
        }

        private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IUserModel.SelectedMage))
            {
                this.SelectedMageKnight = this.army.FirstOrDefault(x => x.Model.Id == this.model.SelectedMage.Id);
            }
        }

        private void Mage_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals(nameof(IMageKnightBattleViewModel.IsSelected)))
            {
                var m = sender as IMageKnightBattleViewModel;
                if (m.IsSelected)
                {
                    if (this.SelectedMageKnight != null && this.SelectedMageKnight.ActionMode != ActionMode.Pass)
                    {
                        m.IsSelected = false;
                        return;
                    }

                    this.SelectedMageKnight = m;
                }
            }
        }
   
        public void AddMageKnightToInventory(IMageKnightBattleViewModel mage)
        {
           //this.model.AddMageKnightToInventory(mage);
        }

        public void AddMage(IMageKnightBattleViewModel mage)
        {
            //mage.UserId = this.UserId;
            //mage.PropertyChanged += Mage_PropertyChanged;

            //int i = this.model.ActiveArmy.Count;
            
            //double range = mage.Range;
            //double speed = mage.Dial.Click.Speed.Value;
            //double max = Math.Max(range, speed);
            //double height = max * 100;
            //double x = height - 50 - 1100;
            //double y = height - 50 - 200;

            //mage.XCord = -x + 100 * i;
            //mage.YCord = -y;
            //this.model.ActiveArmies.Add(mage);
        }

        public void UpdateInventory(ObservableCollection<IMageKnightModel> inventory)
        {
            throw new NotImplementedException();
        }

        public IObservableCollection<IMageKnightBattleViewModel> ArmyViewModels => this.army;

        public IMageKnightBattleViewModel SelectedMageKnight { get => selectedMageKnight; set { this.Set(() => this.SelectedMageKnight, ref this.selectedMageKnight, value); } }
        public IUserModel Model => this.model;
        public int Actions { get => actions; set { this.Set(() => this.Actions, ref this.actions, value); } }

        public IMageKnightBattleViewModel TargetedMageKnight { get => targetedMageKnight; set { this.Set(() => this.TargetedMageKnight, ref this.targetedMageKnight, value); } }

        public IUserModel Opponent { get => this.opponent; set { this.Set(() => this.Opponent, ref this.opponent, value); } }

        public int BoosterPacks => throw new NotImplementedException();

        public string Password => throw new NotImplementedException();

        public Guid Id => throw new NotImplementedException();
    }
}
