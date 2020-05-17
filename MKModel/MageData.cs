using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace MKModel
{
    public class MageData
    {
        private string name = string.Empty;
        private int index = 0;
        private int pointValue = 0;
        private int range = 0;
        private string set = string.Empty;
        private int rarity = 0;
        private string faction = string.Empty;
        private int frontArc = 0;
        private int targets = 0;
        private int click = 0;
        private string rank;
        private Guid id;
        private byte[] image;
        private string priceValue;
        private IDial dial;

        public MageData()
        {

        }
        public string PriceValue
        {
            get
            {
                return this.priceValue;
            }

            set
            {
                this.priceValue = value;
            }
        }
        public byte[] ModelImage
        {
            get
            {
                return this.image;
            }

            set
            {
                this.image = value;
            }
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

        public int Rarity
        {
            get
            {
                return this.rarity;
            }

            set
            {
                this.rarity = value;
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
        public int Targets
        {
            get
            {
                return this.targets;
            }

            set
            {
                this.targets = value;
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
        public IDial Dial { get => this.dial; set => this.dial = value; }

        public string Rank
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

        public Guid Id
        {
            get
            {
                return this.id;
            }

            set
            {
                this.id = value;
            }
        }

    }
}
