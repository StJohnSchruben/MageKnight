using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MKModel
{
    public interface IClick
    {
        System.Collections.Generic.List<IStat> Stats { get; }
        int Index { get; }
        IStat Speed { get; }
        IStat Attack { get; }
        IStat Defense { get; }
        IStat Damage { get; }
    }
}