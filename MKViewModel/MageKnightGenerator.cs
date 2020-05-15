using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MKModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MKViewModel
{
    public class MageKnightGenerator : ViewModelBase, IMageKnightGenerator
    {
        private string name;
        private int index;
        private int pointValue;
        private int range;
        private string set;
        private string faction;
        private int frontArc;
        private int rearArc;
        private int click;
        private IStats stats;
        private IRank rank;
        private MageData mage;
        public MageKnightGenerator( )
        {
            this.mage = new MageData();
        }

        public MageData Mage
        {
            get => this.mage;
            set => this.Set(() => this.Mage , ref this.mage, value); 
        }

        public ICommand Enter => new RelayCommand(this.EnterClicked);

        private void EnterClicked()
        {
            if (this.validate())
            {
                //MageKnight mageKnight = MageDB.GetMageKnight(this.Name);
                //this.mage.Name = mageKnight.Name;
                //this.mage.Index = mageKnight.Index;
                //this.mage.Range = mageKnight.Range;
                //this.mage.PointValue = mageKnight.PointValue;
                //this.mage.FrontArc = mageKnight.FrontArc;
                //this.mage.Faction = mageKnight.Faction;
                //this.mage.RearArc= mageKnight.RearArc;
                MageDB.EditMageKnight(this.mage);
            }
        }

        private bool validate()
        {
            if (this.Name == string.Empty ||
                this.Faction == string.Empty ||
                this.Set == string.Empty)
            {
                return false;
            }

            return true;
        }

        public ICommand Edit => new RelayCommand(this.EditClicked);

        private void EditClicked()
        {
        }

        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                this.name = value;
            }
        }

        public int Index
        {
            get
            {
                return this.mage.Index;
            }

            set
            {
                this.mage.Index = value;
            }
        }
        public int PointValue
        {
            get
            {
                return this.mage.PointValue;
            }

            set
            {
                this.mage.PointValue = value;
            }
        }
        public int Range
        {
            get
            {
                return this.mage.Range;
            }

            set
            {
                this.mage.Range = value;
            }
        }
        public string Set
        {
            get
            {
                return this.mage.Set;
            }

            set
            {
                this.mage.Set = value;
            }
        }
        public string Faction
        {
            get
            {
                return this.mage.Faction;
            }

            set
            {
                this.mage.Faction = value;
            }
        }

        public int FrontArc
        {
            get
            {
                return this.mage.FrontArc;
            }

            set
            {
                this.mage.FrontArc = value;
            }
        }
        public int RearArc
        {
            get
            {
                return this.mage.RearArc;
            }

            set
            {
                this.mage.RearArc = value;
            }
        }
        public int Click
        {
            get
            {
                return this.mage.Click;
            }

            set
            {
                this.mage.Click = value;
            }
        }

        public IStats Stats
        {
            get
            {
                return this.mage.Stats;
            }

            set
            {
                this.mage.Stats = value;
            }
        }

        public IRank Rank
        {
            get
            {
                return this.mage.Rank;
            }

            set
            {
                this.mage.Rank = value;
            }
        }
    }
}
