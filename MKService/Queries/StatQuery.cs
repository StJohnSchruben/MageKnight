
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.Serialization;
using log4net;
using MKModel;
using MKService.Updates;
using ReDefNet;

namespace MKService.Queries
{
    [DataContract(Namespace = ServiceConstants.QueryNamespace)]
    public class StatQuery : ObservableObject, IUpdatableStat
    {
        [DataMember]
        private StatType statType;
        [DataMember]
        private int value;
        [DataMember]
        private string ability;
        public StatQuery()
        {
            this.initialize();
        }

        public StatType StatType { get => this.statType; set { this.Set(() => this.StatType, ref statType, value); }  }
        public int Value { get => this.value; set { this.Set(() => this.Value, ref this.value, value); } }
        public string Ability { get => this.ability; set { this.Set(() => this.Ability, ref ability, value); } }

        private void initialize()
        {
        }
    }
}
