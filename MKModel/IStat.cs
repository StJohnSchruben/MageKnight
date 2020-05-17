using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MKModel
{
    public interface IStat
    {
        StatType StatType { get; set; }
        int Value { get; set; }

        string Ability { get; set; } 
    }
}