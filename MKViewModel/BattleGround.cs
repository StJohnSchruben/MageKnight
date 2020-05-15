using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace MKViewModel
{
    public class BattleGround : IBattleGround
    {
        private List<IPlayer> players;
        private ObservableCollection<IMageKnight> activeMageKnights;
        private ObservableCollection<IMageKnight> inactiveMageKnights;

        public BattleGround()
        {
            
        }

        public List<IPlayer> Players { get => players; set => this.players = value; }
        public ObservableCollection<IMageKnight> ActiveMageKnights { get => activeMageKnights; set=> this.activeMageKnights = value; }
        public ObservableCollection<IMageKnight> InactiveMageKnights { get => inactiveMageKnights; set => this.inactiveMageKnights = value; }
    }
}