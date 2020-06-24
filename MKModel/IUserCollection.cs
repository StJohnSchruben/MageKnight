using GalaSoft.MvvmLight;
using ReDefNet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MKModel
{
    public interface IUserCollection : INotifyPropertyChanged
    {
        IReadOnlyObservableCollection<IUserModel> Users { get; }
        IUserModel GetUser(Guid id);
        IUserModel CreateUser(string userName, string password);
        void SignIn(IUserModel user);
    }
}
