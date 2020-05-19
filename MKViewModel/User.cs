using MKModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MKViewModel
{
    public interface IUser
    {
        List<IArmy> Armies { get; set; }
        List<IMageKnightModel> MageKnights { get; set; }

        ObservableCollection<IMageKnightBattleViewModel> ActiveArmy { get; set; }
        IArmy SelectedArmy { get; set; }
    }
    public class User : IUser
    {
        private List<IArmy> armies;
        private List<IMageKnightModel> mageKnights;
        private ObservableCollection<IMageKnightBattleViewModel> activeArmy;
        private IArmy selectedArmy;

        public User()
        {
            this.ActiveArmy = new ObservableCollection<IMageKnightBattleViewModel>();
            MageKnights = MageDB.GetMageKnights();
            Armies = new List<IArmy>();
            IArmy army = new Army();
            army.Units = new List<IMageKnightBattleViewModel>();
            army.Name = "Army1";
            this.SelectedArmy = army;
            Armies.Add(army);
        }

        public List<IArmy> Armies { get => this.armies; set => this.armies = value; }
        public List<IMageKnightModel> MageKnights { get => this.mageKnights; set => this.mageKnights = value; }
        public ObservableCollection<IMageKnightBattleViewModel> ActiveArmy { get => this.activeArmy; set => this.activeArmy = value; }
        public IArmy SelectedArmy { get => this.selectedArmy; set => this.selectedArmy = value; }
    }
}
