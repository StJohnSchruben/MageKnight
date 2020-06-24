
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using log4net;
using MKModel;
using MKService.Updates;
using ReDefNet;

namespace MKService.Queries
{
    [DataContract(Namespace = ServiceConstants.QueryNamespace)]
    public class UserCollectionQuery : ObservableObject, IUpdatableUserCollection
    {
        [DataMember]
        private SerializableObservableCollection<IUpdatableUser> users;
        public UserCollectionQuery()
        {
            this.initialize();
        }

        public IReadOnlyObservableCollection<IUserModel> Users => this.users;

        IObservableCollection<IUpdatableUser> IUpdatableUserCollection.Users => this.users;

        public IUserModel CreateUser(string userName, string password)
        {
            throw new NotImplementedException();
        }

        public IUserModel GetUser(Guid id)
        {
            return this.Users.FirstOrDefault(x => x.Id == id);
        }

        public void SignIn(IUserModel user)
        {
            throw new NotImplementedException();
        }

        private void initialize()
        {
            users = new SerializableObservableCollection<IUpdatableUser>();
        }
    }
}
