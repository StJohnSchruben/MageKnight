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
        new IObservableCollection<IUpdatableUser> Users { get; }
    }
}