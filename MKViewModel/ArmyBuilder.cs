using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using MKModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MKViewModel
{
    public class ArmyBuilder : ViewModelBase, IArmyBuilder
    {
        private IUser user;
        private IMageKnightModel selectedMageKnight;
        private IArmy selectedArmy;
        private ObservableCollection<IMageKnightBattleViewModel> currentModels = new ObservableCollection<IMageKnightBattleViewModel>();

        public ArmyBuilder(IUser user)
        {
            this.User = user;
            //this.user.MageKnights = MageDB.GetMageKnights();
            //this.user.Armies = new List<IArmy>();
            //IArmy army = new Army();
            //army.Units = new List<IMageKnightModel>();
            //army.Name = "Army1";
            //this.user.Armies.Add(army);
            this.SelectedArmy = this.User.SelectedArmy;

            this.SelectedMageKnight = user.MageKnights.First();
            this.AddToArmyCllicked();
            this.SelectedMageKnight = user.MageKnights.Last();
            this.AddToArmyCllicked();
            this.ApplyToBoardClicked();
        }

        public IUser User
        {
            get 
            {
                return this.user;
            }
            set
            {
                this.Set(() => this.User, ref this.user, value);
            }
        }

        public IArmy SelectedArmy 
        { 
            get => this.selectedArmy;
            set 
            {
                this.CurrentModels.Clear();

                this.Set(() => this.SelectedArmy, ref this.selectedArmy, value);
                foreach (var model in selectedArmy.Units)
                {
                    this.CurrentModels.Add(model);
                }
            } 
        }

        public ICommand NewArmy => new RelayCommand(this.NewArmyClicked);

        private void NewArmyClicked()
        {
        }

        public ICommand DeleteArmy => new RelayCommand(this.DeleteArmyClicked);

        private void DeleteArmyClicked()
        {
        }

        public ICommand AddToArmy => new RelayCommand(this.AddToArmyCllicked);

        private void AddToArmyCllicked()
        {
            this.selectedArmy.Units.Add(new MageKnightBattleViewModel(SelectedMageKnight));
            this.CurrentModels.Add(new MageKnightBattleViewModel(SelectedMageKnight));

            RaisePropertyChanged("CurrentModels");
        }

        public ICommand RemoveFromArmy => new RelayCommand(this.RemoveFromArmyClicked);

        public IMageKnightModel SelectedMageKnight { get => this.selectedMageKnight; set { this.Set(() => this.SelectedMageKnight, ref this.selectedMageKnight, value); } }

        public ObservableCollection<IMageKnightBattleViewModel> CurrentModels { get => this.currentModels; set { this.Set(() => this.CurrentModels, ref this.currentModels, value); } }

        public ICommand ApplyToBoard => new RelayCommand(this.ApplyToBoardClicked);

        private void ApplyToBoardClicked()
        {
            foreach (var model in selectedArmy.Units)
            {
                this.User.ActiveArmy.Add(model);
            }
        }

        private void RemoveFromArmyClicked()
        {

        }
    }
}
