using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MKModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MKViewModel
{
    public interface IMageKnightBattleViewModel : INotifyPropertyChanged
    {
        bool ToggleRangeView { get; set; }
        IMageKnightModel Model { get; }
        IMageKnightBattleViewModel ViewModel { get; }
        int FacingAngle { get; set; }
        int ClickIndex { get; set; }
        double XCord { get; set; }
        double YCord { get; set; }
        ICommand ClickUp { get; }
        ICommand ClickDown { get; }
        ICommand Attack { get; }
        ICommand Move { get; }
        ICommand Capture { get; }
        ICommand Flight { get; }
        ICommand FlameLightining { get; }
        ICommand Charge { get; }  
        ICommand Healing { get; }
        ICommand WheaponMaster { get; }
        ICommand MagicLevitation { get; }
        ICommand MagicBlast { get; }
        ICommand ShockWave { get; }
        ICommand Aquatic { get; }
        ICommand Command { get; }
        ICommand MagicHealing { get; }
        ICommand Bound { get; }
        ICommand Stealth { get; }
        ICommand Necromancy { get; }
    }

    public class MageKnightBattleViewModel : ViewModelBase, IMageKnightBattleViewModel, IMageKnightModel
    {
        bool toggleRangeView;
        IMageKnightModel model;
        int facingAngle, clickIndex;
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

        public int ClickIndex { get => this.clickIndex; set { this.Set(() => this.ClickIndex, ref this.clickIndex, value); } }

        public ICommand ClickUp => new RelayCommand(this.ClickUpClicked);

        private void ClickUpClicked()
        {
            this.Dial.ClickIndex++;
            this.RaisePropertyChanged(nameof(this.Dial.ClickIndex));
        }

        public ICommand ClickDown => new RelayCommand(this.ClickDounClicked);

        private void ClickDounClicked()
        {
            this.Dial.ClickIndex--;
            this.RaisePropertyChanged(nameof(this.Dial.ClickIndex));
        }

        string IMageKnightModel.Set => throw new NotImplementedException();

        public ICommand Attack => new RelayCommand(this.AttackClicked);

        private void AttackClicked()
        {
            throw new NotImplementedException();
        }

        public ICommand Move => new RelayCommand(this.MoveClicked);

        private void MoveClicked()
        {
            throw new NotImplementedException();
        }

        public ICommand Flight => new RelayCommand(this.FlightClicked);

        private void FlightClicked()
        {
            throw new NotImplementedException();
        }

        public ICommand FlameLightining => new RelayCommand(this.FlameLightiningClicked);

        private void FlameLightiningClicked()
        {
            throw new NotImplementedException();
        }

        public ICommand Charge => new RelayCommand(this.ChargeClicked);

        private void ChargeClicked()
        {
            throw new NotImplementedException();
        }

        public ICommand Healing => new RelayCommand(this.HealingClicked);

        private void HealingClicked()
        {
            throw new NotImplementedException();
        }

        public ICommand WheaponMaster => new RelayCommand(this.WheaponMasterClicked);

        private void WheaponMasterClicked()
        {
            throw new NotImplementedException();
        }

        public ICommand MagicLevitation => new RelayCommand(this.MagicLevitationClicked);

        private void MagicLevitationClicked()
        {
            throw new NotImplementedException();
        }

        public ICommand MagicBlast => new RelayCommand(this.MagicBlastClicked);

        private void MagicBlastClicked()
        {
            throw new NotImplementedException();
        }

        public ICommand ShockWave => new RelayCommand(this.ShockWaveClicked);

        private void ShockWaveClicked()
        {
            throw new NotImplementedException();
        }

        public ICommand Aquatic => new RelayCommand(this.AquaticClicked);

        private void AquaticClicked()
        {
            throw new NotImplementedException();
        }

        public ICommand Command => new RelayCommand(this.CommandClicked);

        private void CommandClicked()
        {
            throw new NotImplementedException();
        }

        public ICommand MagicHealing => new RelayCommand(this.MagicHealingClicked);

        private void MagicHealingClicked()
        {
            throw new NotImplementedException();
        }

        public ICommand Bound => new RelayCommand(this.BoundClicked);

        private void BoundClicked()
        {
            throw new NotImplementedException();
        }

        public ICommand Stealth => new RelayCommand(this.StealthClicked);

        private void StealthClicked()
        {
            throw new NotImplementedException();
        }

        public ICommand Necromancy => new RelayCommand(this.NecromancyClicked);

        private void NecromancyClicked()
        {
            throw new NotImplementedException();
        }

        public ICommand Capture => new RelayCommand(this.CaptureClicked);

        private void CaptureClicked()
        {
            throw new NotImplementedException();
        }
    }
}
