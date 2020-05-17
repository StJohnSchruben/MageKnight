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

        ObservableCollection<IMageKnightModel> ActiveArmy { get; set; }
        IArmy SelectedArmy { get; set; }
    }
    public class User : IUser
    {
        private List<IArmy> armies;
        private List<IMageKnightModel> mageKnights;
        private ObservableCollection<IMageKnightModel> activeArmy;
        private IArmy selectedArmy;

        public User()
        {
            this.ActiveArmy = new ObservableCollection<IMageKnightModel>();
            MageKnights = MageDB.GetMageKnights();
            Armies = new List<IArmy>();
            IArmy army = new Army();
            army.Units = new List<IMageKnightModel>();
            army.Name = "Army1";
            this.SelectedArmy = army;
            Armies.Add(army);
        }

        public List<IArmy> Armies { get => this.armies; set => this.armies = value; }
        public List<IMageKnightModel> MageKnights { get => this.mageKnights; set => this.mageKnights = value; }
        public ObservableCollection<IMageKnightModel> ActiveArmy { get => this.activeArmy; set => this.activeArmy = value; }
        public IArmy SelectedArmy { get => this.selectedArmy; set => this.selectedArmy = value; }
    }
}
