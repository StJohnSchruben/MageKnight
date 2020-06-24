
using System;
using log4net;
using MKService.Messages;
using MKService.ModelUpdaters;
using MKService.Updates;
using MKModel;
using System.Collections.ObjectModel;
using ReDefNet;
using System.Linq;
using MKService.Queries;

namespace MKService.Proxies
{
    internal class UserCollectionProxy : ProxyBase, IUserCollection
    {
        private readonly SerializableObservableCollection<IUserModel> users;
        private readonly IUpdatableUserCollection model;
        public UserCollectionProxy(
              IServiceClient serviceClient,
              IModelUpdaterResolver modelUpdaterResolver,
              IUpdatableUserCollection model)
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

            this.SubscribeToMessage<UserCollectionAdd>(this.Handle);
            this.SubscribeToMessage<UserSignUp>(this.Handle);
            this.SubscribeToMessage<UserSignIn>(this.Handle);
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

        private void Handle(UserCollectionAdd message)
        {
            this.ModelUpdaterResolver.GetUpdater<IUpdatableUserCollection, UserCollectionAdd>().Update(this.model, message);
        }
        private void Handle(UserSignIn message)
        {
            this.ModelUpdaterResolver.GetUpdater<IUpdatableUser, UserSignIn>().Update(this.model.Users.FirstOrDefault(x => x.Id == message.UserId), message);
        }

        private void Handle(UserSignUp message)
        {
            var newUser = new UserQuery();
            newUser.UserName = message.UserName;
            newUser.Password = message.Password;
            newUser.RebellionBoosterPacks = 5;
            newUser.Id = message.Id;
            model.Users.Add(newUser);
        }
        public IUserModel GetUser(Guid id)
        {
            throw new NotImplementedException();
        }

        public IUserModel CreateUser(string userName, string password)
        {
            var message = new UserSignUp
            {
                UserName = userName,
                Password = password,
                Id = Guid.NewGuid()
            };

            var newUser = new UserQuery();
            newUser.UserName = message.UserName;
            newUser.Password = message.Password;
            newUser.RebellionBoosterPacks = 5;
            newUser.Id = message.Id;
            model.Users.Add(newUser);

            this.ServiceClient.PublishAsync<UserSignUp>(message);

            return this.Users.FirstOrDefault(x => x.Id == message.Id);
        }

        public void SignIn(IUserModel user)
        {
            var message = new UserSignIn 
            {
                IsSignedIn = !user.IsSignedIn,
                UserId = user.Id,
            };

            var changed = this.ModelUpdaterResolver.GetUpdater<IUpdatableUser, UserSignIn>().Update(this.model.Users.FirstOrDefault(x=> x.Id == user.Id), message);
  
            if (changed)
            {
                this.ServiceClient.PublishAsync<UserSignIn>(message);
            }        
        }
    }
}
