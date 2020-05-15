using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MKModel
{
    public class MageData
    {
        private string name = string.Empty;
        private int index = 0;
        private int pointValue = 0;
        private int range = 0;
        private string set = string.Empty;
        private string faction = string.Empty;
        private int frontArc = 0;
        private int rearArc = 0;
        private int click = 0;
        private IStats stats;
        private IRank rank;

        public MageData()
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
                return this.index;
            }

            set
            {
                this.index = value;
            }
        }
        public int PointValue
        {
            get
            {
                return this.pointValue;
            }

            set
            {
                this.pointValue = value;
            }
        }
        public int Range
        {
            get
            {
                return this.range;
            }

            set
            {
                this.range = value;
            }
        }
        public string Set
        {
            get
            {
                return this.set;
            }

            set
            {
                this.set = value;
            }
        }
        public string Faction
        {
            get
            {
                return this.faction;
            }

            set
            {
                this.faction = value;
            }
        }

        public int FrontArc
        {
            get
            {
                return this.frontArc;
            }

            set
            {
                this.frontArc = value;
            }
        }
        public int RearArc
        {
            get
            {
                return this.rearArc;
            }

            set
            {
                this.rearArc = value;
            }
        }
        public int Click
        {
            get
            {
                return this.click;
            }

            set
            {
                this.click = value;
            }
        }

        public IStats Stats
        {
            get
            {
                return this.stats;
            }

            set
            {
                this.stats = value;
            }
        }

        public IRank Rank
        {
            get
            {
                return this.rank;
            }

            set
            {
                this.rank = value;
            }
        }
    }
}
