
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using MKModel;
using MKService.Updates;
using ReDefNet;

namespace MKService.Queries
{
    [DataContract(Namespace = ServiceConstants.QueryNamespace)]
    public class GameQuery : ObservableObject, IUpdatableGame
    {
        [DataMember]
        private Guid user1Id;
        [DataMember]
        private Guid user2Id;
        [DataMember]
        private int turnCount;
        [DataMember]
        private Guid id = Guid.NewGuid();

        public GameQuery()
        {
            this.initialize();
        }

        private void initialize()
        {
        }

        public Guid User1Id
        {
            get
            { 
                return this.user1Id;
            } 
            set 
            { 
                this.Set(() => this.User1Id, ref this.user1Id, value);
            } 
        }

        public Guid User2Id
        {
            get
            { 
                return this.user2Id;
            } 
            set 
            { 
                this.Set(() => this.User2Id, ref this.user2Id, value);
            } 
        }

        public int TurnCount { get { return this.turnCount; } set { this.Set(() => this.TurnCount, ref this.turnCount, value); } }

        public Guid Id { get { return this.id; } set { this.Set(() => this.Id, ref this.id, value); } }
    }

    [DataContract(Namespace = ServiceConstants.QueryNamespace)]
    public class GamesQuery : ObservableObject, IUpdatableGames
    {
        [DataMember]
        private SerializableObservableCollection<IUpdatableGame> games;

        public GamesQuery()
        {
            this.initialize();
        }

        public IReadOnlyObservableCollection<IGameModel> Games => this.games;

        IObservableCollection<IUpdatableGame> IUpdatableGames.Games => this.games;

        public IGameModel GetGame(Guid gameId)
        {
            return this.Games.FirstOrDefault(x => x.Id == gameId);
        }

        public IGameModel HostGame(IUserModel user)
        {
            throw new NotImplementedException();
        }

        public void JoinGame(IGameModel game, IUserModel user)
        {
            throw new NotImplementedException();
        }

        private void initialize()
        {
            games = new SerializableObservableCollection<IUpdatableGame>();
        }
    }
}
