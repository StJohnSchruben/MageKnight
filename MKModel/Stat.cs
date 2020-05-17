using MKModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;

namespace MKModel
{
    public class Stat : IStat
    {
        private StatType statType;
        private int value = 0;
        private string ability = string.Empty;

        public Stat()
        {
        }

        public Stat(StatType statType )
        {
            this.StatType = statType;
        }

        public StatType StatType { get => this.statType; set => this.statType = value; }
        public int Value { get => this.value; set => this.value = value; }
        public string Ability { get => this.ability; set => this.ability = value; }
    }
}