using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MKViewModel
{
    public class NewGame
    {
        private IBattleGround battleGround;

        public NewGame()
        {
            IPlayer player = new Player();
            player.Name = "john";
            List<IMageKnight> army = new List<IMageKnight>();

            IMageKnight mage = new StandardMageKnight();
            army.Add(mage);

            player.Army = army;
            this.battleGround = new BattleGround();
            battleGround.ActiveMageKnights = new System.Collections.ObjectModel.ObservableCollection<IMageKnight>();
            foreach (var knight in army)
            {
                battleGround.ActiveMageKnights.Add(knight);
            }
        }

        public IBattleGround TheBattleGround => this.battleGround;
    }
}
