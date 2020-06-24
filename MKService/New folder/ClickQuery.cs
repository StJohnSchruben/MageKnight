
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.Serialization;
using log4net;
using MKModel;
using MKService.Updates;
using ReDefNet;

namespace MKService.Queries
{
    [DataContract(Namespace = ServiceConstants.QueryNamespace)]
    public class ClickQuery : ObservableObject, IUpdatableClick
    {
        [DataMember]
        private SerializableObservableCollection<IUpdatableUser> users;
        public ClickQuery()
        {
            this.initialize();
        }

        public IReadOnlyObservableCollection<IUserModel> Users => this.users;

        IObservableCollection<IUpdatableUser> IUpdatableClick.Users => this.users;

        private void initialize()
        {
            users = new SerializableObservableCollection<IUpdatableUser>();
        }
    }
}
