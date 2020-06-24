using MKModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace MKViewModel
{
    public interface IArmy 
    {
        List<IMageKnightBattleViewModel> Units { get; set; }
        int PointTotal { get; }
        string Name { get; set; }
    }

    public class Army : IArmy
    {
        private List<IMageKnightBattleViewModel> units;
        private int pointTotal;
        private string name;
        public Army()
        {
            units = new List<IMageKnightBattleViewModel>();
        }
        public List<IMageKnightBattleViewModel> Units { get => this.units; set => this.units = value; }
        public int PointTotal { get => this.CalculatePointTotal(); }
        public string Name { get => this.name; set => this.name = value; }
        private int CalculatePointTotal()
        {
            return pointTotal;
        }
    }
}
