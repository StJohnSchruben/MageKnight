using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace MKViewModel
{
    public interface IBattleGround
    {
        List<IPlayer> Players { get; set; }
        ObservableCollection<IMageKnight> ActiveMageKnights { get; set; }
        ObservableCollection<IMageKnight> InactiveMageKnights { get; set; }
    }
}