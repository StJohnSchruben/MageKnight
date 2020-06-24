
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
    internal class DialProxy : ProxyBase, IDial
    {
        private readonly SerializableObservableCollection<IClick> clicks;
        private readonly IUpdatableDial model;

        public DialProxy(
              IServiceClient serviceClient,
              IModelUpdaterResolver modelUpdaterResolver,
              IUpdatableDial model)
              : base(serviceClient, modelUpdaterResolver)
        {
            this.model = model;
            this.clicks = new SerializableObservableCollection<IClick>();

            foreach (var click in this.model.Clicks)
            {
                this.clicks.Add(new ClickProxy(serviceClient, modelUpdaterResolver, click));
            }

            this.model.Clicks.CollectionChanged += Users_CollectionChanged;

            this.SetUpModelPropertyChangedPropagation(this.model);

            this.SubscribeToMessage<DialAdd>(this.Handle);
        }


        public IReadOnlyObservableCollection<IClick> Clicks => throw new NotImplementedException();

        public int ClickIndex { get => this.model.ClickIndex; set => throw new NotImplementedException(); }
        public IClick Click { get => this.model.Click; set => throw new NotImplementedException(); }
        public string Name { get => this.model.Name; set => throw new NotImplementedException(); }
        private void Users_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (var click in e.NewItems.Cast<IUpdatableClick>())
                {
                    this.clicks.Add(new ClickProxy(ServiceClient, ModelUpdaterResolver, click));
                }
            }
            else
            {
                foreach (var click in e.OldItems.Cast<IUpdatableClick>())
                {
                    this.clicks.Remove(click);
                }
            }
        }

        private void Handle(DialAdd message)
        {
            this.ModelUpdaterResolver.GetUpdater<IUpdatableDial, DialAdd>().Update(this.model, message);
        }
    }
}
