using MKModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MKViewModel
{
    public interface IMageKnightGenerator
    {
        IMageKnightModel SelectedMageKnight {get;set;}
        MageData Mage { get; set; }
        ICommand Enter { get; }
        ICommand Edit { get; }
        string Name { get; set; }
        int Index { get; set; }
        int PointValue { get; set; }
        int Range { get; set; }
        string Set { get; set; }
        string Faction { get; set; }
        int FrontArc { get; set; }
        int Targets { get; set; }
        int Click { get; set; }
        string Rank { get; set; }

        List<IMageKnightModel> MageKnights { get; set; }
    }
}
