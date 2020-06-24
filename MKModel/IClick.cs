using ReDefNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace MKModel
{
    public interface IClick : INotifyPropertyChanged
    {
        IReadOnlyObservableCollection<IStat> Stats { get; }
        int Index { get; }
        IStat Speed { get; }
        IStat Attack { get; }
        IStat Defense { get; }
        IStat Damage { get; }
    }
}