using GalaSoft.MvvmLight;
using MKModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MKViewModel
{
   public  interface IUserViewModel : INotifyPropertyChanged
    {
        IMageKnightBattleViewModel SelectedMageKnight { get; set; }
        IMageKnightBattleViewModel TargetedMageKnight { get; set; }

        IUser Model { get; }

        int Actions { get; set; }

    }

    public class UserViewModel : ViewModelBase, IUserViewModel, IUser
    {
        private IUser model;
        int actions;
        IMageKnightBattleViewModel selectedMageKnight;
        IMageKnightBattleViewModel targetedMageKnight;
        public UserViewModel(IUser model)
        {
            this.model = model;
            foreach (var mage in this.ActiveArmy)
            {
                mage.PropertyChanged += Mage_PropertyChanged;
            }
        }

        private void Mage_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals(nameof(IMageKnightBattleViewModel.ToggleRangeView)))
            {
                var m = sender as IMageKnightBattleViewModel;
                if (m.ToggleRangeView)
                {
                    this.SelectedMageKnight = m;
                }
            }
        }

        public List<IArmy> Armies { get => model.Armies; set => throw new NotImplementedException(); }
        public List<IMageKnightModel> MageKnights { get => model.MageKnights; set => throw new NotImplementedException(); }
        public ObservableCollection<IMageKnightBattleViewModel> ActiveArmy { get => model.ActiveArmy; set => throw new NotImplementedException(); }
        public IArmy SelectedArmy { get => model.SelectedArmy; set => throw new NotImplementedException(); }
        public IMageKnightBattleViewModel SelectedMageKnight { get => selectedMageKnight; set { this.Set(() => this.SelectedMageKnight, ref this.selectedMageKnight, value); } }
        public IUser Model => this.model;
        public int Actions { get => actions; set { this.Set(() => this.Actions, ref this.actions, value); } }

        public IMageKnightBattleViewModel TargetedMageKnight { get => targetedMageKnight; set { this.Set(() => this.TargetedMageKnight, ref this.targetedMageKnight, value); } }
    }
}
