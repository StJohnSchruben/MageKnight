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
        int PointValue { get; }
        int Range { get; }
        string Set { get; }
        string Faction { get; }

        int FrontArc { get; }
        int RearArc { get; }
        int Click { get; }

        IStats Stats { get; }

        IRank Rank { get;  }
    }
}