using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MKModel
{
    public interface IMageKnightModel 
    {
        string Name { get; }
        int Index { get; }
        int Range { get; }
        string Set { get; }
        string Faction { get; }
        byte[] ModelImage { get; }
        
        int FrontArc { get; }
        int Targets { get; }
        int Click { get; }
        IDial Dial { get; }

        string Rank { get;  }
    }
}