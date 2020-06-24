
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
    public class ClickQuery : ObservableObject, IUpdatableClick
    {
        [DataMember]
        private SerializableObservableCollection<IUpdatableStat> stats;

        [DataMember]
        private int index;

        [DataMember]
        private IStat speed;

        [DataMember]
        private IStat attack;

        [DataMember]
        private IStat defense;

        [DataMember]
        private IStat damage;

        public ClickQuery()
        {
            this.initialize();
        }

        public IReadOnlyObservableCollection<IStat> Stats => this.stats; 
        public int Index { get => this.index; set { this.Set(() => this.Index, ref index, value); } }
        public IStat Speed { get => this.speed; set { this.Set(() => this.Speed, ref speed, value); } }
        public IStat Attack { get => this.attack; set { this.Set(() => this.Attack, ref attack, value); } }
        public IStat Defense { get => this.defense; set { this.Set(() => this.Defense, ref defense, value); } }
        public IStat Damage { get => this.damage; set { this.Set(() => this.Damage, ref damage, value); } }

        IObservableCollection<IUpdatableStat> IUpdatableClick.Stats => this.stats;

        private void initialize()
        {
            stats = new SerializableObservableCollection<IUpdatableStat>();
        }
    }
}
