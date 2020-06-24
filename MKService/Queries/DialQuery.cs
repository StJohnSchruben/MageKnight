
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
    public class DialQuery : ObservableObject, IUpdatableDial
    {
        [DataMember]
        private SerializableObservableCollection<IUpdatableClick> clicks;

        [DataMember]
        private int clickIndex;

        [DataMember]
        private IClick click;

        [DataMember]
        private string name;

        public DialQuery()
        {
            this.initialize();
        }

        public int ClickIndex { get => this.clickIndex; set { this.Set(() => this.ClickIndex, ref clickIndex, value); } }
        public IClick Click { get => this.click; set { this.Set(() => this.Click, ref click, value); } }
        public string Name { get => this.name; set { this.Set(() => this.Name, ref name, value); } }

        IObservableCollection<IUpdatableClick> IUpdatableDial.Clicks => this.clicks; 

        IReadOnlyObservableCollection<IClick> IDial.Clicks => this.clicks; 

        private void initialize()
        {
            clicks = new SerializableObservableCollection<IUpdatableClick>();
        }
    }
}
