using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MKModel
{
    public class MageKnight : IMageKnightModel
    {
        MageData data;
        public MageKnight(MageData data)
        {
            this.data = data;
        }

        public string Name => this.data.Name;

        public int Index => this.data.Index;

        public int PointValue => this.data.PointValue;

        public int Range => this.data.Range;

        public string Set => this.data.Set;

        public string Faction => this.data.Faction;

        public string PriceValue => this.data.PriceValue;

        public int FrontArc => this.data.FrontArc;

        public int Targets => this.data.Targets;

        public int Click => this.data.Click;
        public int Rarity => this.data.Rarity;

        public string Rank => this.data.Rank;

        public IDial Dial => this.data.Dial;

        public byte[] ModelImage => this.data.ModelImage;
    }
}
