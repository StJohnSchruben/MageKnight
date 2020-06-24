using System;
using Service;
using MKModel;
using System.Collections.ObjectModel;
using ReDefNet;

namespace MKService.Updates
{
    internal interface IUpdatableGame : IUpdatable, IGameModel, IQueryResponse
    {
        new Guid User1Id { get; set; }
        new Guid User2Id { get; set; }
        new Guid Id{ get; set; }
        new int TurnCount { get; set; }
    }

    internal interface IUpdatableGames : IUpdatable, IGameModels, IQueryResponse
    {
        new IObservableCollection<IUpdatableGame> Games { get; }
    }
}
