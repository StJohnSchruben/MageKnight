using System;
using System.Runtime.Serialization;
using log4net;
using MKModel;
using MKService.Updates;
using ReDefNet;

namespace MKService.Queries
{
    [DataContract(Namespace = ServiceConstants.QueryNamespace)]
    public class MageKnightQuery : ObservableObject, IUpdatableMageKnight
    {
        private readonly ILog log = LogManager.GetLogger(nameof(MageKnightQuery));
        public MageKnightQuery()
        {
            this.initialize();
        }

        private void initialize()
        {
            this.instantiatedId = Guid.NewGuid();
        }

        [DataMember]
        private Guid id;

        [DataMember]
        private Guid instantiatedId;

        [DataMember]
        private string name;

        [DataMember]
        private int index;

        [DataMember]
        private int range;

        [DataMember]
        private int pointValue;

        [DataMember]
        private int frontArc;

        [DataMember]
        private int targets;

        [DataMember]
        private int click;

        [DataMember]
        private string setName;

        [DataMember]
        private string faction;

        [DataMember]
        private string rank;

        [DataMember]
        private byte[] modelImage;

        [DataMember]
        private IDial dial;

        [DataMember]
        private double xCoordinate;

        [DataMember]
        private double yCoordinate;

        public Guid Id { get { return this.id; } set { this.Set(() => this.Id, ref this.id, value); } }
        public string Name { get { return this.name; } set { this.Set(() => this.Name, ref this.name, value); } }
        public int Index { get { return this.index; } set { this.Set(() => this.Index, ref this.index, value); } }
        public int Range { get { return this.range; } set { this.Set(() => this.Range, ref this.range, value); } }
        public int PointValue { get { return this.pointValue; } set { this.Set(() => this.PointValue, ref this.pointValue, value); } }
        public int FrontArc { get { return this.frontArc; } set { this.Set(() => this.FrontArc, ref this.frontArc, value); } }
        public int Targets { get { return this.targets; } set { this.Set(() => this.Targets, ref this.targets, value); } }
        public int Click { get { return this.click; } set { this.Set(() => this.Click, ref this.click, value); } }
        public string Faction { get { return this.faction; } set { this.Set(() => this.Faction, ref this.faction, value); } }
        public string Rank { get { return this.rank; } set { this.Set(() => this.Rank, ref this.rank, value); } }
        public string Set { get { return this.setName; } set { this.Set(() => this.Set, ref this.setName, value); } }
        public byte[] ModelImage { get { return this.modelImage; } set { this.Set(() => this.ModelImage, ref this.modelImage, value); } }
        public IDial Dial { get { return this.dial; } set { this.Set(() => this.Dial, ref this.dial, value); } }
        public Guid InstantiatedId { get { return this.instantiatedId; } }
        Guid IMageKnightModel.InstantiatedId => this.instantiatedId;
        public double XCoordinate { get { return this.xCoordinate; } set { this.Set(() => this.XCoordinate, ref this.xCoordinate, value); } }
        public double YCoordinate { get { return this.yCoordinate; } set { this.Set(() => this.YCoordinate, ref this.yCoordinate, value); } }
        public void UpdateMageKnightCoordinates(IUserModel user, Guid instantiatedId, double xCoordinate, double yCoordinate)
        {
            throw new NotImplementedException();
        }
    }
}
