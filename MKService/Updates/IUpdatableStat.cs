using System;
using Service;
using MKModel;
using System.Collections.ObjectModel;
using ReDefNet;
using MKService.Updates;

namespace MKService.Updates
{
    internal interface IUpdatableStat : IUpdatable, IStat, IQueryResponse
    {
        new StatType StatType { get; set; }
        new int Value { get; set; }

        new string Ability { get; set; }
    }
}