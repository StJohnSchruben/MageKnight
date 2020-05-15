using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MKViewModel
{
    public interface IStat
    {
        StatType StatType { get; set; }
        int Value { get; set; }
    }
}