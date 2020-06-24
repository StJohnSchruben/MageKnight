using System;
using Service;
using MKModel;

namespace MKService.Updates
{
    internal interface IUpdatableMageKnight : IUpdatable, IMageKnightModel, IQueryResponse
    {
        new Guid Id { get; set; }
        new Guid InstantiatedId { get; }
        new string Name { get; set; }
        new int Index { get; set; }
        new int Range { get; set; }
        new int PointValue { get; set; }
        new int FrontArc { get; set; }
        new int Targets { get; set; }
        new int Click { get; set; }
        new string Set { get; set; }
        new string Faction { get; set; }
        new string Rank { get; set; }
        new byte[] ModelImage { get; set; }
        new IDial Dial { get; set; }
    }
}
