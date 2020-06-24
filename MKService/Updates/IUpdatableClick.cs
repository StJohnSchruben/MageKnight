using System;
using Service;
using MKModel;
using System.Collections.ObjectModel;
using ReDefNet;
using MKService.Updates;

namespace MKService.Updates
{
    internal interface IUpdatableClick : IUpdatable, IClick, IQueryResponse
    {
        new IObservableCollection<IUpdatableStat> Stats { get; }
        new int Index { get; set; }
        new IStat Speed { get; set; }
        new IStat Attack { get; set; }
        new IStat Defense { get; set; }
        new IStat Damage { get; set; }
    }
}