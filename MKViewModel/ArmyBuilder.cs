using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using log4net;
using Microsoft.Win32;
using MKModel;
using MKService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MKViewModel
{
    public class ArmyBuilder : ViewModelBase, IArmyBuilder
    {
        private IUserModel user;
        private IGameModels games;
        private IGameModel selectedGame;
        private IMageKnightModel selectedMageKnight;
        private List<Guid> selectedArmy = new List<Guid>();
        private ObservableCollection<byte[]> currentModels = new ObservableCollection<byte[]>();
        private ObservableCollection<BoosterPack> booster = new ObservableCollection<BoosterPack>();
        private IUserViewModel userViewModel;
        private bool isAppliedToBoard;
        private IGameModel game;
        private readonly ILog log = LogManager.GetLogger(typeof(DeviceServiceBootstrapper));
        public ArmyBuilder(IUserModel user, IGameModels games)
        {
            this.User = user;
            this.Games = games;
            this.User.PropertyChanged += User_PropertyChanged;
            this.User.Inventory.CollectionChanged += Inventory_CollectionChanged;
            for(int i = 0; i < user.RebellionBoosterPacks; i++)
            {
                this.Boosters.Add(BoosterPack.Rebellion);
            }
            games.PropertyChanged += Games_PropertyChanged;
            games.Games.CollectionChanged += Games_CollectionChanged;
        }

        private void Inventory_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            ;
        }

        public ICommand NewArmy => new RelayCommand(this.NewArmyClicked);
        public ICommand DeleteArmy => new RelayCommand(this.DeleteArmyClicked);
        public ICommand AddToArmy => new RelayCommand(this.AddToArmyClicked);
        public ICommand RemoveFromArmy => new RelayCommand(this.RemoveFromArmyClicked);
        public ICommand OpenBoosters => new RelayCommand(this.OpenBoostesClicked);
        public ICommand JoinGame => new RelayCommand(this.JoinGameClicked);
        public ICommand HostGame => new RelayCommand(this.HostGameClicked);
        public ObservableCollection<BoosterPack> Boosters { get => this.booster; set { this.Set(() => this.Boosters, ref this.booster, value); } }
        public IMageKnightModel SelectedMageKnight { get => this.selectedMageKnight; set { this.Set(() => this.SelectedMageKnight, ref this.selectedMageKnight, value); } }
        public ObservableCollection<byte[]> CurrentModels { get => this.currentModels; set { this.Set(() => this.CurrentModels, ref this.currentModels, value); } }
        public bool IsAppliedToBoard { get => this.isAppliedToBoard; set { this.Set(() => this.IsAppliedToBoard, ref this.isAppliedToBoard, value); } }
        public IGameModels Games { get => this.games; set { this.Set(() => this.Games, ref this.games, value); } }
        public IGameModel SelectedGame { get => this.selectedGame; set { this.Set(() => this.SelectedGame, ref this.selectedGame, value); } }
        public IUserModel User { get { return this.user; } set { this.Set(() => this.User, ref this.user, value); } }
        public List<Guid> SelectedArmyGuids { get => this.selectedArmy; set { this.Set(() => this.SelectedArmyGuids, ref this.selectedArmy, value); } }
        private void GameModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IGameModel.User2Id))
            {
                this.IsAppliedToBoard = true;
            }
        }
     
        private void Games_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            foreach (var game in e.NewItems.Cast<IGameModel>())
            {
                if (game.User1Id == this.user.Id || (selectedGame != null && game.Id == selectedGame.Id))
                {
                    this.game = game;
                    ServiceTypeProvider.Instance.GameModel = this.game;
                    this.game.PropertyChanged += GameModel_PropertyChanged;
                }
            }

            this.RaisePropertyChanged(nameof(IGameModels.Games));
        }

        private void User_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IUserModel.RebellionBoosterPacks))
            {
                this.Boosters.Clear();
                for (int i = 0; i < user.RebellionBoosterPacks; i++)
                {
                    this.Boosters.Add(BoosterPack.Rebellion);
                }
            }

            if (e.PropertyName == nameof(IUserModel.IsSignedIn) && this.user.IsSignedIn)
            {
                this.IsAppliedToBoard = false;
            }
        }
        private void JoinGameClicked()
        {
            if (this.SelectedGame.User1Id == this.user.Id)
            {
                this.game = this.SelectedGame;
                ServiceTypeProvider.Instance.GameModel = this.game;
                this.game.PropertyChanged += GameModel_PropertyChanged;
                this.Games.JoinGame(this.SelectedGame, this.user);
            }
            else
            {
                this.AddMageKnightsToUsersArmy();
                this.game = this.SelectedGame;
                ServiceTypeProvider.Instance.GameModel = this.game;
                this.game.PropertyChanged += GameModel_PropertyChanged;
                this.Games.JoinGame(this.SelectedGame, this.user);
            }
        }
        private void AddToArmyClicked()
        {
            this.selectedArmy.Add(SelectedMageKnight.Id);
            this.CurrentModels.Add(SelectedMageKnight.ModelImage);
            this.RaisePropertyChanged("CurrentModels");
        }
        private void AddMageKnightsToUsersArmy()
        {
            foreach (var id in selectedArmy)
            {
                this.User.AddMageToArmy(id, Guid.NewGuid());
            }
        }
        private void HostGameClicked()
        {
            this.AddMageKnightsToUsersArmy();
            this.Games.HostGame(this.User);
        }
        private void OpenBoostesClicked()
        {
            this.user.RebellionBoosterPacks -= 1;
        }
        private void NewArmyClicked()
        {
        }
        private void DeleteArmyClicked()
        {
        }
        private void RemoveFromArmyClicked()
        {

        }
        private void Game_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
        }
        private void Games_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
        }
    }
}
