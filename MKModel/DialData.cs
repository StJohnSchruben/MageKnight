using GalaSoft.MvvmLight;
using MKModel;
using ReDefNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace MKModel
{
    public interface ISessionTime : INotifyPropertyChanged
    {
        DateTime SessionTime { get; }

        void ChangeSessionTime(short year, short month, short day, short hour, short minute, short second);
    }

    public class StatData : ViewModelBase
    {
        private StatType statType;
        private int value = 0;
        private string ability = string.Empty;

        public StatData()
        {
        }

        public StatData(StatType statType)
        {
            this.StatType = statType;
        }
        public StatData(IStat stat)
        {
            this.StatType = stat.StatType;
            this.Ability = stat.Ability;
            this.Value = stat.Value;
        }

        public StatType StatType { get => this.statType; set => this.statType = value; }
        public int Value { get => this.value; set => this.value = value; }
        public string Ability { get => this.ability; set => this.ability = value; }
    }

    public class ClickData : ViewModelBase
    {
        private IObservableCollection<StatData> stats = new SerializableObservableCollection<StatData>();
        private int index;

        public ClickData(StatData speed, StatData attack, StatData defense, StatData damage)
        {
            this.stats.Add(speed);
            this.stats.Add(attack);
            this.stats.Add(defense);
            this.stats.Add(damage);
        }
        public ClickData(StatData speed, StatData attack, StatData defense, StatData damage, int index)
        {
            this.index = index;
            this.stats.Add(speed);
            this.stats.Add(attack);
            this.stats.Add(defense);
            this.stats.Add(damage);
        }

        public IObservableCollection<StatData> Stats { get => this.stats; }

        public int Index { get => this.index; set => this.index = value; }

        public StatData Speed => stats.First(x => x.StatType == StatType.Speed);

        public StatData Attack => stats.First(x => x.StatType == StatType.Attack);

        public StatData Defense => stats.First(x => x.StatType == StatType.Defense);

        public StatData Damage => stats.First(x => x.StatType == StatType.Damage);
    }

    public class DialData : ViewModelBase
    {
        private IObservableCollection<ClickData> clicks;
        private int clickIndex;
        public MageData data;
        public DialData(MageData data)
        {
            clicks = new SerializableObservableCollection<ClickData>();
            this.data = data;
            if (data.Name.StartsWith("temp"))
            {
                this.Clicks.Add(new ClickData(new StatData(StatType.Speed), new StatData(StatType.Attack), new StatData(StatType.Defense), new StatData(StatType.Damage)));
            }
        }

        public IObservableCollection<ClickData> Clicks
        {
            get => this.clicks;
            set
            {
                this.Set(() => this.Clicks, ref this.clicks, value);
            }
        }
        public int ClickIndex
        {
            get => this.clickIndex;
            set
            {
                if (!(value >= this.Clicks.Count) && !(value < 0))
                    this.Set(() => this.ClickIndex, ref this.clickIndex, value);
                this.RaisePropertyChanged(nameof(this.Click));
            }
        }

        public string Name => this.data.Name;

        public ClickData Click => this.Clicks.FirstOrDefault(x => x.Index == this.clickIndex);
    }
}