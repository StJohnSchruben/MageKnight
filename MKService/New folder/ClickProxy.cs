
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
        private readonly SerializableObservableCollection<IUserModel> users;
        private readonly IUpdatableClick model;
        public ClickProxy(
              IServiceClient serviceClient,
              IModelUpdaterResolver modelUpdaterResolver,
              IUpdatableClick model)
              : base(serviceClient, modelUpdaterResolver)
        {
            this.model = model;
            this.users = new SerializableObservableCollection<IUserModel>();

            foreach (var dude in this.model.Users)
            {
                this.users.Add(new UserProxy(serviceClient, modelUpdaterResolver, dude));
            }

            this.model.Users.CollectionChanged += Users_CollectionChanged;

            this.SetUpModelPropertyChangedPropagation(this.model);

            this.SubscribeToMessage<ClickAdd>(this.Handle);
        }

        private void Users_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (var dude in e.NewItems.Cast<IUpdatableUser>())
                {
                    this.users.Add(new UserProxy(ServiceClient, ModelUpdaterResolver, dude));
                }
            }
            else
            {
                foreach (var dude in e.OldItems.Cast<IUpdatableUser>())
                {
                    this.users.Remove(dude);
                }
            }
        }

        public IReadOnlyObservableCollection<IUserModel> Users => this.users;

        private void Handle(ClickAdd message)
        {
            this.ModelUpdaterResolver.GetUpdater<IUpdatableClick, ClickAdd>().Update(this.model, message);
        }
    }
}
