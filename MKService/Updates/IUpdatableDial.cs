using System;
using Service;
using MKModel;
using System.Collections.ObjectModel;
using ReDefNet;
using MKService.Updates;

namespace MKService.Updates
{
    internal interface IUpdatableDial : IUpdatable, IDial, IQueryResponse
    {
        new IObservableCollection<IUpdatableClick> Clicks { get; }
        new int ClickIndex { get; set; }

        new IClick Click { get; set; }

        new string Name { get; set; }
    }
}