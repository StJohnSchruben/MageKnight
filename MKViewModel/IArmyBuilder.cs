using MKModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MKViewModel
{
    public interface IArmyBuilder : INotifyPropertyChanged
    {
        IUserModel User { get; set; }
        IGameModels Games { get; set; }
        IGameModel SelectedGame { get; set; }
        ICommand NewArmy { get; }
        ICommand DeleteArmy { get; }
        ICommand AddToArmy { get; }
        ICommand RemoveFromArmy { get; }
        ICommand HostGame { get; }
        ICommand JoinGame { get; }
        ICommand OpenBoosters { get; }
        ObservableCollection<byte []> CurrentModels { get; set; }
        ObservableCollection<BoosterPack> Boosters { get; set; }
        bool IsAppliedToBoard { get; set; }
        IMageKnightModel SelectedMageKnight { get; set; }
    }
}
