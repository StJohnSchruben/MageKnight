﻿using MKModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MKViewModel
{
    public interface IArmyBuilder
    {
        IUser User { get; set; }
        IArmy SelectedArmy { get; set; }
        ICommand NewArmy { get; }
        ICommand DeleteArmy { get; }
        ICommand AddToArmy { get; }
        ICommand RemoveFromArmy { get; }
        ICommand ApplyToBoard { get; }
        ObservableCollection<IMageKnightBattleViewModel> CurrentModels { get; set; }

        IMageKnightModel SelectedMageKnight { get; set; }
    }
}
