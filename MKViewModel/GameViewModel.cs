using GalaSoft.MvvmLight;
using MKModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MKViewModel
{
    public interface IGameViewModel
    {
        int TurnCount { get; set; }
        IUserModel User1 { get; }
        IUserModel User2 { get; }
        IUserModel UserActiveThisTurn { get; set; }
        ObservableCollection<IMageKnightBattleViewModel> ActiveMageKnights { get; }

        IMageKnightBattleViewModel User1SelectedMageKnight { get; set; }
    }

    public class GameViewModel : ViewModelBase, IGameViewModel
    {
        private int turnCount;
        private IGameModel gameModel;
        private IUserModel userActiveThisTurn;
        private ObservableCollection<IMageKnightBattleViewModel> activeMageKnights = new ObservableCollection<IMageKnightBattleViewModel>();
        private IUserViewModel userViewModel;
        private IMageKnightBattleViewModel user1SelectedMageKnight;
        private IUserCollection userCollection;
        public GameViewModel(IGameModel gameModel, IUserViewModel userViewModel, IUserCollection userCollection)
        {
            this.gameModel = gameModel;
            this.userViewModel = userViewModel;
            this.userCollection = userCollection;
            this.gameModel.PropertyChanged += GameModel_PropertyChanged;
            this.User1.PropertyChanged += User1_PropertyChanged;
            this.User2.PropertyChanged += User1_PropertyChanged;
            if (User1.Army.Count() != 0)
            {
                int i = 0;
                foreach(var mage in User1.Army)
                {
                    var mageViewModel = new MageKnightBattleViewModel(mage, this.User1);
                    mageViewModel.PropertyChanged += MageViewModel_PropertyChanged;
                    double range = mageViewModel.Range;
                    double speed = mageViewModel.Dial.Click.Speed.Value;
                    double max = Math.Max(range, speed);
                    double height = max * 100;
                    double x = height - 50 - 1100;
                    double y = height - 50 - 200;

                    mageViewModel.XCord = -x + 100 * i;
                    mageViewModel.YCord = -y;
                    i++;
                    userViewModel.ArmyViewModels.Add(mageViewModel);
                    activeMageKnights.Add(mageViewModel);
                }
            }
            if (User2.Army.Count() != 0 && User2.Id != User1.Id)
            {
                int i = 0;
                foreach (var mage in User2.Army)
                {
                    var mageViewModel = new MageKnightBattleViewModel(mage, this.User2);
                    double range = mageViewModel.Range;
                    double speed = mageViewModel.Dial.Click.Speed.Value;
                    double max = Math.Max(range, speed);
                    double height = max * 100;
                    double x = height - 50 - 1100;
                    double y = height - 50 - 3700;

                    mageViewModel.XCord = -x + 100 * i;
                    mageViewModel.YCord = -y;
                    i++;
                    mageViewModel.PropertyChanged += MageViewModel_PropertyChanged;
                    userViewModel.ArmyViewModels.Add(mageViewModel);
                    activeMageKnights.Add(mageViewModel);
                }
            }
        }

        private void MageViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IMageKnightBattleViewModel.ActionMode))
            {
                try
                {
                    var m = sender as IMageKnightBattleViewModel;
                    this.activeMageKnights.Remove(this.activeMageKnights.FirstOrDefault(x => x.Name == "temp" + m.Model.InstantiatedId.ToString()));
                }
                catch
                {
                    ;
                }

                IMageKnightBattleViewModel data = sender as IMageKnightBattleViewModel;
                if (data.ActionMode == ActionMode.Move || data.ActionMode == ActionMode.MoveFormation)
                {
                    var boundry = new MageKnightBattleViewModel(data);
                    this.ActiveMageKnights.Add(boundry);
                }

                this.SyncBaseContactCollections();
            }

            if (e.PropertyName == nameof(IMageKnightBattleViewModel.IsSelected))
            {
                var mage = sender as IMageKnightBattleViewModel;
                if (mage.IsSelected)
                {
                    this.User1SelectedMageKnight = this.ActiveMageKnights.FirstOrDefault(x => x.Model.InstantiatedId == mage.Model.InstantiatedId);
                }
            }
        }

        private void SyncBaseContactCollections()
        {
            foreach(var mage in this.ActiveMageKnights)
            {
                foreach (var mage2 in this.ActiveMageKnights)
                {
                    Vector diff = mage.CenterPoint - mage2.CenterPoint;
                    if (Math.Abs(diff.Length) <= 100)
                    {
                        if (mage.UserId == mage2.UserId)
                        {
                            mage.FrendlyBaseContact.Add(mage2);
                            mage2.FrendlyBaseContact.Add(mage);
                        }
                        else
                        {
                            mage.EnemyBaseContact.Add(mage2);
                            mage2.EnemyBaseContact.Add(mage);
                        }
                    }
                }
            }
        }

        private void User2_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IUserModel.Army))
            {
                int i = 0;
                foreach (var mage in User2.Army)
                {
                    var mageViewModel = new MageKnightBattleViewModel(mage, this.User2);
                    double range = mageViewModel.Range;
                    double speed = mageViewModel.Dial.Click.Speed.Value;
                    double max = Math.Max(range, speed);
                    double height = max * 100;
                    double x = height - 50 - 1100;
                    double y = height - 50 - 3700;

                    mageViewModel.XCord = -x + 100 * i;
                    mageViewModel.YCord = -y;
                    i++;
                    mageViewModel.PropertyChanged += MageViewModel_PropertyChanged;
                    userViewModel.ArmyViewModels.Add(mageViewModel);
                    activeMageKnights.Add(mageViewModel);
                }
            }
        }

        private void User1_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            
        }

        private void GameModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IGameModel.User1Id))
            {
                foreach (var mage in this.User1.Army)
                {
                    var mageViewModel = new MageKnightBattleViewModel(mage, this.User1);

                    mageViewModel.PropertyChanged += MageViewModel_PropertyChanged;
                    userViewModel.ArmyViewModels.Add(mageViewModel);
                    this.ActiveMageKnights.Add(mageViewModel);
                }
            }

            if (e.PropertyName == nameof(IGameModel.User2Id))
            {
                int i = 0;
                foreach (var mage in User2.Army)
                {
                    var mageViewModel = new MageKnightBattleViewModel(mage, this.User2);
                    double range = mageViewModel.Range;
                    double speed = mageViewModel.Dial.Click.Speed.Value;
                    double max = Math.Max(range, speed);
                    double height = max * 100;
                    double x = height - 50 - 1100;
                    double y = height - 50 - 3700;

                    mageViewModel.XCord = -x + 100 * i;
                    mageViewModel.YCord = -y;
                    i++;
                    mageViewModel.PropertyChanged += MageViewModel_PropertyChanged;
                    userViewModel.ArmyViewModels.Add(mageViewModel);
                    activeMageKnights.Add(mageViewModel);
                }
            }
        }

        public int TurnCount { get => this.turnCount; set => this.Set(() => this.TurnCount, ref this.turnCount, value); }
        public IUserModel User1 => this.userCollection.Users.FirstOrDefault(x => x.Id ==  gameModel.User1Id);
        public IUserModel User2 => this.userCollection.Users.FirstOrDefault(x => x.Id == gameModel.User2Id);
        public IUserModel UserActiveThisTurn { get => this.userActiveThisTurn; set => this.Set(() => this.UserActiveThisTurn, ref this.userActiveThisTurn, value); }
        public ObservableCollection<IMageKnightBattleViewModel> ActiveMageKnights => this.activeMageKnights;
        public IMageKnightBattleViewModel User1SelectedMageKnight { get => this.user1SelectedMageKnight; set => this.Set(() => this.User1SelectedMageKnight, ref this.user1SelectedMageKnight, value); }
    }
}
