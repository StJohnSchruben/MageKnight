using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MKModel
{
    public interface IGameModel : INotifyPropertyChanged
    {
        Guid User1Id { get; set; }
        Guid User2Id { get; set; }
        int TurnCount { get; }
        Guid Id { get; }
    }
}
