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
        int RearArc { get; set; }
        int Click { get; set; }
        IStats Stats { get; set; }
        IRank Rank { get; set; }
    }
}
