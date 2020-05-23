using GalaSoft.MvvmLight;
using MKModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace MKViewModel
{
    public interface IMageKnightBattleViewModel : INotifyPropertyChanged
    {
        bool ToggleRangeView { get; set; }
        IMageKnightModel Model { get; }
        IMageKnightBattleViewModel ViewModel { get; }
        int FacingAngle { get; set; }

        double XCord { get; set; }
        double YCord { get; set; }
    }

    public class MageKnightBattleViewModel : ViewModelBase, IMageKnightBattleViewModel, IMageKnightModel
    {
        bool toggleRangeView;
        IMageKnightModel model;
        int facingAngle;
        double xCord, yCord;
        public MageKnightBattleViewModel(IMageKnightModel model)
        {
            this.model = model;
        }

        public bool ToggleRangeView { get => this.toggleRangeView; set { this.Set(() => this.ToggleRangeView, ref this.toggleRangeView, value); } }
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
        public int PointValue => model.PointValue;
        public IMageKnightBattleViewModel ViewModel => this;

        public double XCord { get => this.xCord; set { this.Set(() => this.XCord, ref this.xCord, value); } }
        public double YCord { get => this.yCord; set { this.Set(() => this.YCord, ref this.yCord, value); } }

        string IMageKnightModel.Set => throw new NotImplementedException();
    }
}
