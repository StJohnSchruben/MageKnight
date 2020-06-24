using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MKModel;
using ReDefNet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MKViewModel
{
    public enum ActionMode 
    {
        Pass = 0,
        Attack = 1,
        AttackFormation = 2,
        Move = 3,
        MoveFormation = 4
    }

    public interface IMageKnightBattleViewModel : INotifyPropertyChanged, IMageKnightModel
    {
        new Guid InstantiatedId { get; }
        Guid UserId { get; set; }
        bool WasActiveThisTurn { get; set; }
        bool IsPushed { get; set; }
        bool IsInBaseContact { get; set; }
        bool IsSelected { get; set; }
        bool IsMoving { get; }
        bool IsInRange { get; set; }
        double Size { get; }
        Point CenterPoint { get; }
        Point TopLeftPoint { get; }
        Point MoveStartingPoint { get; set; }
        Point TargetStartingPoint { get; set; }
        bool IsMovingBorder { get; set; }
        bool IsTargeting { get;  }
        bool ToggleRangeView { get; set; }
        IMageKnightModel Model { get; }
        IMageKnightBattleViewModel ViewModel { get; }
        ObservableCollection<IMageKnightBattleViewModel> TargetedFigures { get; set; }
        ObservableCollection<IMageKnightBattleViewModel> EnemyBaseContact { get; set; }
        ObservableCollection<IMageKnightBattleViewModel> FrendlyBaseContact { get; set; }
        ObservableCollection<IMageKnightBattleViewModel> MoveFormationParticipants { get; set; }
        ObservableCollection<IMageKnightBattleViewModel> AttackFormationParticipants { get; set; }
        int FacingAngle { get; set; }
        int PreMoveFacingAngle { get; set; }
        int ClickIndex { get; set; }
        int ActionCount { get; set; }
        double XCord { get; set; }
        double YCord { get; set; }
        double PreMoveXCord { get; set; }
        double PreMoveYCord { get; set; }
        ICommand Cancel { get; }
        ICommand MovementFormation { get; }
        ICommand AttackFormation { get; }
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
        ICommand Regeneration { get; }
        ICommand Quickness { get; }
        ICommand Push { get; }
        ICommand BreakAway { get; }
        ActionMode ActionMode { get; set; }
        void AddTarget(IMageKnightBattleViewModel target);
    }

    public class MageKnightBattleViewModel : ViewModelBase, IMageKnightBattleViewModel
    {
        private bool toggleRangeView, isInBaseContact, isMovingBorder, isSelected, isInRange, wasActiveThisTurn, isPushed;
        private IMageKnightModel model;
        private int facingAngle, preMoveFacingAngle, clickIndex;
        private int actionCount;
        private double xCord, yCord, preMoveXCord, preMoveYCord;
        private ActionMode actionMode;
        private Point moveStartingPoint, targetStartingPoint;
        private Guid instantiatedId;
        private Guid userId;
        private IMageKnightBattleViewModel target;
        private ObservableCollection<IMageKnightBattleViewModel> moveFormationParticipants = new ObservableCollection<IMageKnightBattleViewModel>();
        private ObservableCollection<IMageKnightBattleViewModel> enemyBaseContact = new ObservableCollection<IMageKnightBattleViewModel>();
        private ObservableCollection<IMageKnightBattleViewModel> frendlyBaseContact = new ObservableCollection<IMageKnightBattleViewModel>();
        private ObservableCollection<IMageKnightBattleViewModel> targetedFigures = new ObservableCollection<IMageKnightBattleViewModel>();
        private IUserModel user;
        public MageKnightBattleViewModel(IMageKnightModel model, IUserModel user)
        {
            this.model = model;
            this.user = user;
            this.model.PropertyChanged += Model_PropertyChanged;
        }
        public MageKnightBattleViewModel(IMageKnightBattleViewModel clone)
        {
            MageData data = new MageData("temp" + clone.Model.InstantiatedId.ToString());
            data.Range = clone.Model.Range;
            data.Dial = new DialData(data);
            data.Dial.Click.Speed.Value = clone.Dial.Click.Speed.Value;

            this.model = new MageDataForMovingBoundry(data); 
            if (clone.ActionMode == ActionMode.Move || clone.ActionMode == ActionMode.MoveFormation)
            {
                this.IsMovingBorder = true;
            }

            this.RaisePropertyChanged(nameof(this.IsMovingBorder));
            this.XCord = clone.XCord;
            this.YCord = clone.YCord;
        }
        private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IMageKnightModel.XCoordinate))
            {
                this.XCord = this.model.XCoordinate;
            }
            else if (e.PropertyName == nameof(IMageKnightModel.YCoordinate))
            {
                this.YCord = this.model.YCoordinate;
            }
        }
        public double XCord 
        { 
            get => this.xCord; 
            set 
            {
                //if (value >= -950 && value <= 2600)
                {
                    this.Set(() => this.XCord, ref this.xCord, value);
                }
            } 
        }
        public double YCord
        {
            get => this.yCord;
            set 
            {
                //if (value >= -950 && value <= 2600)
                {
                    this.Set(() => this.YCord, ref this.yCord, value);
                }
            } 
        }
        private void CancelClicked()
        {
            switch (ActionMode) 
            {
                case ActionMode.Move:
                    this.FacingAngle = this.PreMoveFacingAngle;
                    this.XCord = this.PreMoveXCord;
                    this.YCord = this.PreMoveYCord;
                    
                    this.Move.Execute(null);
                    break;

                case ActionMode.MoveFormation:
                    this.FacingAngle = this.PreMoveFacingAngle;
                    this.XCord = this.PreMoveXCord;
                    this.YCord = this.PreMoveYCord;
                    foreach (IMageKnightBattleViewModel mage in this.MoveFormationParticipants)
                    {
                        mage.Cancel.Execute(null);
                    }

                    this.moveFormationParticipants.Clear();

                    this.MovementFormation.Execute(null);
                    break;

                case ActionMode.Attack:
                    break;
                case ActionMode.AttackFormation:
                    break;
            }

            this.ActionCount--;
            this.WasActiveThisTurn = false;
        }
        private void ClickUpClicked()
        {
            this.Dial.ClickIndex++;
            this.RaisePropertyChanged(nameof(this.Dial.ClickIndex));
        }
        private void ClickDounClicked()
        {
            this.Dial.ClickIndex--;
            this.RaisePropertyChanged(nameof(this.Dial.ClickIndex));
        }
        private void MovementFormationClicked()
        {
            foreach (IMageKnightBattleViewModel mage in this.MoveFormationParticipants)
            {
                mage.Move.Execute(null);
            }

            this.MoveFormationParticipants.Clear();

            if (this.ActionMode != ActionMode.MoveFormation)
            {
                this.ActionMode = ActionMode.MoveFormation;
                this.MoveStartingPoint = this.CenterPoint;
                this.PreMoveFacingAngle = this.FacingAngle;
                this.PreMoveXCord = this.XCord;
                this.PreMoveYCord = this.YCord;
            }
            else
            {
                this.ActionMode = ActionMode.Pass;
                this.ActionCount++;
                this.WasActiveThisTurn = true;
                if (this.IsPushed)
                {
                    this.ClickUp.Execute(null);
                }
            }
        }
        private void AttackFormationClicked()
        {
            if (this.ActionMode != ActionMode.AttackFormation)
            {
                this.ActionMode = ActionMode.AttackFormation;
                this.TargetStartingPoint = new Point(this.XCord, this.YCord);
                this.PreMoveFacingAngle = this.FacingAngle;
            }
            else
            {
                this.ActionMode = ActionMode.Pass;
                this.ActionCount++;
                this.WasActiveThisTurn = true;
            }
        }
        private void AttackClicked()
        {
            if (this.Targets != null)
            {
                this.AttackTarget();
            }

            if (this.ActionMode != ActionMode.Attack)
            {
                this.ActionMode = ActionMode.Attack;
                this.TargetStartingPoint = new Point(this.XCord, this.YCord);
            }
            else
            {
                this.ActionMode = ActionMode.Pass;
                this.ActionCount++;
            }
        }
        private void AttackTarget()
        {
            foreach (var mage in this.TargetedFigures)
            {
                mage.ClickUp.Execute(null);
            }

            this.TargetedFigures.Clear();
        }
        private void MoveClicked()
        {
            if (this.ActionMode != ActionMode.Move)
            {
                this.ActionMode = ActionMode.Move;
                this.MoveStartingPoint = this.CenterPoint;
                this.PreMoveFacingAngle = this.FacingAngle;
                this.PreMoveXCord = this.XCord;
                this.PreMoveYCord = this.YCord;
            }
            else
            {
                this.model.UpdateMageKnightCoordinates(this.user, this.model.InstantiatedId, this.XCord, this.YCord);
                this.ActionMode = ActionMode.Pass;
                this.ActionCount++;
            }
        }
        public void AddTarget(IMageKnightBattleViewModel target)
        {
            if (this.TargetedFigures.Count < this.Targets)
            {
                this.TargetedFigures.Add(target);
                this.RaisePropertyChanged(nameof(this.TargetedFigures));
            }
        }
        private void PushClicked()
        {
            this.IsPushed = true;
        }
        public void UpdateMageKnightCoordinates(IUserModel user, Guid instantiatedId, double xCoordinate, double yCoordinate)
        {
             new RelayCommand(this.somethingclicked);
        }
        public bool IsSelected 
        { 
            get => this.isSelected; 
            set
            { 
                var changed = this.Set(() => this.IsSelected, ref this.isSelected, value);

                if (changed && value)
                {
                    this.user.SelectedMage = this.model;
                }
            }
        }
        public Point CenterPoint
        {
            get
            {
                double range = this.Range;
                double speed = this.Dial.Click.Speed.Value;
                double max = Math.Max(range, speed);
                double height = max * 100;
                double x = height;
                double y = height;
                return new Point(this.XCord + x, this.YCord + y);
            }
        }
        public double Size
        {
            get
            {
                double range = this.Range;
                double speed = this.Dial.Click.Speed.Value;
                double max = Math.Max(range, speed);
                return max * 100;
            }
        }
        public ICommand Attack => new RelayCommand(this.AttackClicked);
        public ICommand ClickDown => new RelayCommand(this.ClickDounClicked);
        public ICommand ClickUp => new RelayCommand(this.ClickUpClicked);
        public ICommand Cancel => new RelayCommand(this.CancelClicked);
        public ICommand MovementFormation => new RelayCommand(this.MovementFormationClicked);
        public ICommand Push => new RelayCommand(this.PushClicked);
        public ICommand Move => new RelayCommand(this.MoveClicked);
        public ICommand AttackFormation => new RelayCommand(this.AttackFormationClicked);
        public ICommand Flight => new RelayCommand(this.somethingclicked);
        public ICommand FlameLightining =>  new RelayCommand(this.somethingclicked);
        public ICommand Charge =>  new RelayCommand(this.somethingclicked);
        public ICommand Healing =>  new RelayCommand(this.somethingclicked);
        public ICommand WheaponMaster =>  new RelayCommand(this.somethingclicked);
        public ICommand MagicLevitation =>  new RelayCommand(this.somethingclicked);
        public ICommand MagicBlast =>  new RelayCommand(this.somethingclicked);
        public ICommand ShockWave =>  new RelayCommand(this.somethingclicked);
        public ICommand Aquatic =>  new RelayCommand(this.somethingclicked);
        public ICommand Command =>  new RelayCommand(this.somethingclicked);
        public ICommand MagicHealing =>  new RelayCommand(this.somethingclicked);
        public ICommand Bound =>  new RelayCommand(this.somethingclicked);
        public ICommand Stealth =>  new RelayCommand(this.somethingclicked);
        public ICommand Necromancy =>  new RelayCommand(this.somethingclicked);
        public ICommand Capture =>  new RelayCommand(this.somethingclicked);
        public ICommand Regeneration =>  new RelayCommand(this.somethingclicked);
        public ICommand Quickness =>  new RelayCommand(this.somethingclicked);
        public ICommand BreakAway =>  new RelayCommand(this.somethingclicked);
        public ObservableCollection<IMageKnightBattleViewModel> EnemyBaseContact { get => this.enemyBaseContact; set { this.Set(() => this.EnemyBaseContact, ref this.enemyBaseContact, value); } }
        public ObservableCollection<IMageKnightBattleViewModel> FrendlyBaseContact { get => this.frendlyBaseContact; set { this.Set(() => this.FrendlyBaseContact, ref this.frendlyBaseContact, value); } }
        public ObservableCollection<IMageKnightBattleViewModel> MoveFormationParticipants { get => this.moveFormationParticipants; set { this.Set(() => this.MoveFormationParticipants, ref this.moveFormationParticipants, value); } }
        public ObservableCollection<IMageKnightBattleViewModel> AttackFormationParticipants { get => throw new NotImplementedException(); set =>  new RelayCommand(this.somethingclicked); }
        public ObservableCollection<IMageKnightBattleViewModel> TargetedFigures { get => this.targetedFigures; set { this.Set(() => this.TargetedFigures, ref this.targetedFigures, value); } }
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
        string IMageKnightModel.Set => this.model.Set;
        Guid IMageKnightModel.Id => this.model.Id;
        public IMageKnightBattleViewModel ViewModel => this;
        public Point TopLeftPoint { get { return new Point(this.XCord, this.YCord); } }
        public bool IsMoving { get { return this.ActionMode == ActionMode.Move ? true : false; } }
        public bool IsTargeting { get { return this.ActionMode == ActionMode.Attack ? true : false; } }
        public ActionMode ActionMode { get => this.actionMode; set { this.Set(() => this.ActionMode, ref this.actionMode, value); } }
        public bool ToggleRangeView { get => this.toggleRangeView; set { this.Set(() => this.ToggleRangeView, ref this.toggleRangeView, value); } }
        public IMageKnightModel Model { get => this.model; }
        public int FacingAngle { get => this.facingAngle; set { this.Set(() => this.FacingAngle, ref this.facingAngle, value); } }
        public bool IsInBaseContact { get => this.isInBaseContact; set { this.Set(() => this.IsInBaseContact, ref this.isInBaseContact, value); } }
        public int ClickIndex { get => this.clickIndex; set { this.Set(() => this.ClickIndex, ref this.clickIndex, value); } }
        public Point MoveStartingPoint { get => this.moveStartingPoint; set { this.Set(() => this.MoveStartingPoint, ref this.moveStartingPoint, value); } }
        public Point TargetStartingPoint { get => this.targetStartingPoint; set { this.Set(() => this.TargetStartingPoint, ref this.targetStartingPoint, value); } }
        public bool IsMovingBorder { get => this.isMovingBorder; set { this.Set(() => this.IsMovingBorder, ref this.isMovingBorder, value); } }
        public int ActionCount { get => this.actionCount; set { this.Set(() => this.ActionCount, ref this.actionCount, value); } }
        public bool IsInRange { get => this.isInRange; set { this.Set(() => this.IsInRange, ref this.isInRange, value); } }
        public Guid UserId { get => this.userId; set { this.Set(() => this.UserId, ref this.userId, value); } }
        public int PreMoveFacingAngle { get => this.preMoveFacingAngle; set { this.Set(() => this.PreMoveFacingAngle, ref this.preMoveFacingAngle, value); } }
        public double PreMoveXCord { get => this.preMoveXCord; set { this.Set(() => this.PreMoveXCord, ref this.preMoveXCord, value); } }
        public double PreMoveYCord { get => this.preMoveYCord; set { this.Set(() => this.PreMoveYCord, ref this.preMoveYCord, value); } }
        public bool WasActiveThisTurn { get => this.wasActiveThisTurn; set { this.Set(() => this.WasActiveThisTurn, ref this.wasActiveThisTurn, value); } }
        public bool IsPushed { get => this.isPushed; set { this.Set(() => this.IsPushed, ref this.isPushed, value); } }
        public Guid InstantiatedId { get => this.model.InstantiatedId; }
        public double XCoordinate { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double YCoordinate { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        private void somethingclicked()
        {
            ;
        }

    }

    #region
    public class StatDataForMovingBoundry : IStat
    {
        StatData data;
        public StatDataForMovingBoundry(StatData data)
        {
            this.data = data;
        }
        public StatType StatType { get => data.StatType; set => data.StatType = value; }
        public int Value { get => data.Value; set => data.Value = value; }
        public string Ability { get => data.Ability; set => data.Ability = value; }
        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class ClickDataForMovingBoundary : IClick
    {
        private MageData data;
        public SerializableObservableCollection<IStat> stats = new SerializableObservableCollection<IStat>();
        public ClickDataForMovingBoundary(MageData data)
        {
            this.data = data;
            foreach (var stat in data.Dial.Click.Stats)
            {
                stats.Add(new StatDataForMovingBoundry(stat));
            }
        }
        public IReadOnlyObservableCollection<IStat> Stats => this.stats;
        public int Index => data.Index;
        public IStat Speed => Stats.FirstOrDefault(x => x.StatType == StatType.Speed);
        public IStat Attack => Stats.FirstOrDefault(x => x.StatType == StatType.Attack);
        public IStat Defense => Stats.FirstOrDefault(x => x.StatType == StatType.Defense);
        public IStat Damage => Stats.FirstOrDefault(x => x.StatType == StatType.Damage);
        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class DialDataForMovingBoundary : IDial
    {
        private MageData data;
        public SerializableObservableCollection<IClick> clicks = new SerializableObservableCollection<IClick>();
        public DialDataForMovingBoundary(MageData data)
        {
            this.data = data;
            foreach (var click in data.Dial.Clicks)
            {
                clicks.Add(new ClickDataForMovingBoundary(data));
            }
        }
        public IReadOnlyObservableCollection<IClick> Clicks => this.clicks;
        public int ClickIndex { get => this.data.Dial.ClickIndex; set => this.data.Dial.ClickIndex = value; }
        public IClick Click { get => clicks.ElementAt(this.ClickIndex); set { } }
        public string Name { get => this.data.Dial.Name; set { } }
        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class MageDataForMovingBoundry : IMageKnightModel
    {
        private MageData data;
        private DialDataForMovingBoundary dial;
        public MageDataForMovingBoundry(MageData data)
        {
            this.data = data;
            this.dial = new DialDataForMovingBoundary(data);
        }
        public Guid Id => data.Id;
        public string Name => data.Name;
        public int Index => data.Index;
        public int Range => data.Range;
        public int PointValue => data.PointValue;
        public int FrontArc => data.FrontArc;
        public int Targets => data.Targets;
        public int Click => data.Click;
        public string Set => data.Set;
        public string Faction => data.Faction;
        public string Rank => data.Rank;
        public byte[] ModelImage => data.ModelImage;
        public IDial Dial => this.dial;
        public Guid InstantiatedId => throw new NotImplementedException();
        public double XCoordinate { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double YCoordinate { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public event PropertyChangedEventHandler PropertyChanged;
        public void UpdateMageKnightCoordinates(IUserModel user, Guid instantiatedId, double xCoordinate, double yCoordinate)
        {
            throw new NotImplementedException();
        }
    }
    #endregion
}
