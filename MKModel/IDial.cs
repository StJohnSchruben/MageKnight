using MKModel;
using ReDefNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace MKModel
{
    public interface IDial : INotifyPropertyChanged
    {
        IReadOnlyObservableCollection<IClick> Clicks { get; }
        int ClickIndex { get; set; }

        IClick Click { get; set; }

        string Name { get; set; }
    }
}