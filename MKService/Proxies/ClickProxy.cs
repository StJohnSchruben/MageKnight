
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
    internal class ClickProxy : ProxyBase, IClick
    {
        private readonly SerializableObservableCollection<IStat> stats;
        private readonly IUpdatableClick model;
        public ClickProxy(
              IServiceClient serviceClient,
              IModelUpdaterResolver modelUpdaterResolver,
              IUpdatableClick model)
              : base(serviceClient, modelUpdaterResolver)
        {
            this.model = model;
            this.stats = new SerializableObservableCollection<IStat>();

            foreach (var stat in this.model.Stats)
            {
                this.stats.Add(new StatProxy(serviceClient, modelUpdaterResolver, stat));
            }

            this.model.Stats.CollectionChanged += Users_CollectionChanged;

            this.SetUpModelPropertyChangedPropagation(this.model);
        }

        private void Users_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (var stat in e.NewItems.Cast<IUpdatableStat>())
                {
                    this.stats.Add(new StatProxy(ServiceClient, ModelUpdaterResolver, stat));
                }
            }
            else
            {
                foreach (var stat in e.OldItems.Cast<IUpdatableStat>())
                {
                    this.stats.Remove(stat);
                }
            }
        }
        public IReadOnlyObservableCollection<IStat> Stats => this.model.Stats;

        public int Index => this.model.Index;

        public IStat Speed => this.model.Speed;

        public IStat Attack => this.model.Attack;

        public IStat Defense => this.model.Defense;

        public IStat Damage => this.model.Damage;
    }
}
