using GalaSoft.MvvmLight;
using MKModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace MKModel
{
    public class Dial : ViewModelBase, IDial
    {
        private List<IClick> clicks = new List<IClick>();
        private int clickIndex;
        public MageData data;
        public Dial(MageData data)
        {
            this.data = data;
        }

        public List<IClick> Clicks { get => this.clicks; set => this.clicks = value; }
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

        public IClick Click => this.Clicks.First(x=>x.Index == this.clickIndex);

        public string Name => this.data.Name;
    }
}