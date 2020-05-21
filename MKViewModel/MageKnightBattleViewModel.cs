using GalaSoft.MvvmLight;
using MKModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace MKViewModel
{
    public interface IMageKnightBattleViewModel 
    {
        bool IsSelected { get; set; }
        IMageKnightModel Model { get; }
        IMageKnightBattleViewModel ViewModel { get; }
        int FacingAngle { get; set; }
    }

    public class MageKnightBattleViewModel : ViewModelBase, IMageKnightBattleViewModel, IMageKnightModel
    {
        bool isSelected;
        IMageKnightModel model;
        int facingAngle;

        public MageKnightBattleViewModel(IMageKnightModel model)
        {
            this.model = model;
        }

        public bool IsSelected { get => this.isSelected; set { this.Set(() => this.IsSelected, ref this.isSelected, value); } }
        public IMageKnightModel Model { get => this.model; }
        public int FacingAngle { get => this.facingAngle; set { this.Set(() => this.FacingAngle, ref this.facingAngle, value); } }

        public string Name => model.Name;

        public int Index => model.Index;

        public int Range => model.Range;

        public string Faction => model.Faction;

        public byte[] ModelImage => model.ModelImage;

        public int FrontArc => model.FrontArc;

        public int Targets => model.Targets;

        public int Click => model.Click;

        public IDial Dial => model.Dial;

        public string Rank => model.Rank;

        public IMageKnightBattleViewModel ViewModel => this;

        string IMageKnightModel.Set => throw new NotImplementedException();
    }
}
