
using System;
using log4net;
using MKService.Messages;
using MKService.ModelUpdaters;
using MKService.Updates;
using MKModel;


namespace MKService.Proxies
{
    internal class MageKnightProxy : ProxyBase, IMageKnightModel
    {  
        private readonly ILog log = LogManager.GetLogger(nameof(MageKnightProxy));
        private readonly IUpdatableMageKnight model;
        public MageKnightProxy(
            IServiceClient serviceClient,
            IModelUpdaterResolver modelUpdaterResolver,
            IUpdatableMageKnight model)
            : base(serviceClient, modelUpdaterResolver)
        {
            this.model = model;

            this.SetUpModelPropertyChangedPropagation(this.model);

            this.SubscribeToMessage<MageKnightChanged>(this.Handle);

            this.SubscribeToMessage<MageKnightCoordinatesChanged>(this.Handle);
        }
        public Guid InstantiatedId => this.model.InstantiatedId;
        string IMageKnightModel.Set => this.model.Set;
        public string Name => this.model.Name;
        public int Index => this.model.Index;
        public int Range => this.model.Range;
        public int PointValue => this.model.PointValue;
        public int FrontArc => this.model.FrontArc;
        public int Targets => this.model.Targets;
        public int Click => this.model.Click;
        public string Faction => this.model.Faction;
        public string Rank => this.model.Rank;
        public byte[] ModelImage => this.model.ModelImage;
        public IDial Dial => this.model.Dial;
        public Guid Id
        {
            get => this.model.Id;
            set
            {
                var message = new MageKnightChanged
                {
                    NewModel = this.model
                };

                bool changed = this.ModelUpdaterResolver.GetUpdater<IUpdatableMageKnight, MageKnightChanged>().Update(this.model, message);

                if (changed)
                {
                    this.ServiceClient.PublishAsync<MageKnightChanged>(message);
                }
            }
        }
        public double XCoordinate
        {
            get
            {
                return this.model.XCoordinate;
            }
            set
            {
                this.model.XCoordinate = value;
            }
        }
        public double YCoordinate
        {
            get
            {
                return this.model.YCoordinate;
            }
            set
            {
                this.model.YCoordinate = value;
            }
        }
        public void UpdateMageKnightCoordinates(IUserModel user, Guid instantiatedId, double xCoordinate, double yCoordinate)
        {
            var message = new MageKnightCoordinatesChanged
            {
                UserId = user.Id,
                InstantiatedMageId = instantiatedId,
                XCoordinate = xCoordinate,
                YCoordinate = yCoordinate
            };

            var changed = this.ModelUpdaterResolver.GetUpdater<IUpdatableMageKnight, MageKnightCoordinatesChanged>().Update(this.model, message);

            if (changed)
            {
                this.ServiceClient.PublishAsync<MageKnightCoordinatesChanged>(message);
            }
        }
        private void Handle(MageKnightChanged message)
        {
            this.ModelUpdaterResolver.GetUpdater<IUpdatableMageKnight, MageKnightChanged>().Update(this.model, message);
        }
        private void Handle(MageKnightCoordinatesChanged message)
        {
            this.ModelUpdaterResolver.GetUpdater<IUpdatableMageKnight, MageKnightCoordinatesChanged>().Update(this.model, message);
        }
    }
}
