using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace MKModel
{
    public interface IMageKnightModel : INotifyPropertyChanged
    {
        Guid Id { get; }
        Guid InstantiatedId { get; }
        string Name { get; }
        int Index { get; }
        int Range { get; }
        int PointValue { get; }
        int FrontArc { get; }
        int Targets { get; }
        int Click { get; }
        string Set { get; }
        string Faction { get; }
        string Rank { get; }
        byte[] ModelImage { get; }
        IDial Dial { get; }
        double XCoordinate { get; set; }
        double YCoordinate { get; set; }
        void UpdateMageKnightCoordinates(IUserModel user, Guid instantiatedId, double xCoordinate, double yCoordinate);
    }
}