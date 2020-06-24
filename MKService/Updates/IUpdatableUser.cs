using System;
using Service;
using MKModel;
using System.Collections.ObjectModel;
using ReDefNet;

namespace MKService.Updates
{
    internal interface IUpdatableUser : IUpdatable, IUserModel, IQueryResponse
    {
        new IObservableCollection<IUpdatableMageKnight> Inventory { get; }
        new IObservableCollection<IUpdatableMageKnight> Army { get; }
        new int RebellionBoosterPacks { get; set; }
        new string UserName { get; set; }
        new string Password { get; set; }
        new Guid Id { get; set; }
        new bool IsSignedIn { get; set; }

    }
}
