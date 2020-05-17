using MKModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Remoting.Messaging;
using System.Text;

namespace MKModel
{
    public class Click : IClick
    {
        private List<IStat> stats = new List<IStat>();
        private int index;

        public Click(IStat speed, IStat attack, IStat defense, IStat damage)
        {
            this.stats.Add(speed);
            this.stats.Add(attack);
            this.stats.Add(defense);
            this.stats.Add(damage);
        }
        public Click(IStat speed, IStat attack, IStat defense, IStat damage, int index)
        {
            this.index = index;
            this.stats.Add(speed);
            this.stats.Add(attack);
            this.stats.Add(defense);
            this.stats.Add(damage);
        }
        public List<IStat> Stats { get => this.stats; }

        public IStat Speed => stats.First(x=> x.StatType == StatType.Speed);

        public IStat Attack => stats.First(x => x.StatType == StatType.Attack);

        public IStat Defense => stats.First(x => x.StatType == StatType.Defense);

        public IStat Damage => stats.First(x => x.StatType == StatType.Damage);

        public int Index 
        {
            get
            {
                return this.index;
            }

            set
            {
                this.index = value;
            }
        }

    }
}