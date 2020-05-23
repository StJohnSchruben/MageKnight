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

        IUser Model { get; }

    }
    public class UserViewModel : ViewModelBase, IUserViewModel, IUser
    {
        private IUser model;
        IMageKnightBattleViewModel selectedMageKnight;
        public UserViewModel(IUser model)
        {
            this.model = model;
            foreach(var mage in this.ActiveArmy)
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
        public  IMageKnightBattleViewModel SelectedMageKnight { get => selectedMageKnight; set => selectedMageKnight = value; }

        public IUser Model => this.model;
    }
}
