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
    public interface IGameModels : INotifyPropertyChanged 
    {
        IReadOnlyObservableCollection<IGameModel> Games { get; }
        IGameModel HostGame(IUserModel user);
        void JoinGame(IGameModel game, IUserModel user);
        IGameModel GetGame(Guid gameId);
    }
}
