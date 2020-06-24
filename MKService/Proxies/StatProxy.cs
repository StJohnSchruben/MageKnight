
using System;
using log4net;
using MKService.Messages;
using MKService.ModelUpdaters;
using MKService.Updates;
using MKModel;
using System.Collections.ObjectModel;
using ReDefNet;
using System.Linq;


namespace MKService.Proxies
{
    internal class StatProxy : ProxyBase, IStat
    {
        private readonly IUpdatableStat model;
        public StatProxy(
              IServiceClient serviceClient,
              IModelUpdaterResolver modelUpdaterResolver,
              IUpdatableStat model)
              : base(serviceClient, modelUpdaterResolver)
        {
            this.model = model;

            this.SetUpModelPropertyChangedPropagation(this.model);
        }

        public StatType StatType { get => this.model.StatType; set => this.model.StatType = value; }
        public int Value { get => this.model.Value; set => this.model.Value = value; }
        public string Ability { get => this.model.Ability; set => this.model.Ability = value; }
    }
}
