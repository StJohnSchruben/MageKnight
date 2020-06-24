
using System;
using log4net;
using MKService.Messages;
using MKService.ModelUpdaters;
using MKService.Updates;
using MKModel;
using System.Collections.ObjectModel;
using ReDefNet;
using System.Linq;
using MKService.Queries;

namespace MKService.Proxies
{
    internal class GameProxy : ProxyBase, IGameModel
    {
        private readonly ILog log = LogManager.GetLogger(nameof(GameProxy));

        private readonly IUpdatableGame model;

        public GameProxy(
            IServiceClient serviceClient,
            IModelUpdaterResolver modelUpdaterResolver,
            IUpdatableGame model)
            : base(serviceClient, modelUpdaterResolver)
        {
            this.model = model;

            this.SetUpModelPropertyChangedPropagation(this.model);
            this.SubscribeToMessage<GameChanged>(this.Handle);
            this.SubscribeToMessage<GameJoined>(this.Handle);
            var query = this.ServiceClient.Query<UserQuery>();
        }

        public Guid User1Id
        {
            get
            {
                return this.model.User1Id;
            }
            set
            {
                var message = new GameChanged
                {
                    User1Id = value,
                    User2Id = this.model.User2Id,
                    GameId = this.model.Id,
                    TurnCount = this.model.TurnCount
                };

                bool changed = this.ModelUpdaterResolver.GetUpdater<IUpdatableGame, GameChanged>().Update(this.model, message);

                if (changed)
                {
                    this.ServiceClient.PublishAsync<GameChanged>(message);
                }
            }
        }

        public Guid User2Id
        {
            get
            {
                return this.model.User2Id;
            }
            set
            {
                var message = new GameChanged
                {
                    User1Id = this.model.User1Id,
                    User2Id = value,
                    GameId = this.model.Id,
                    TurnCount = this.model.TurnCount
                };

                bool changed = this.ModelUpdaterResolver.GetUpdater<IUpdatableGame, GameChanged>().Update(this.model, message);

                if (changed)
                {
                    this.ServiceClient.PublishAsync<GameChanged>(message);
                }
            }
        }

        public int TurnCount
        {
            get
            {
                return this.model.TurnCount;
            }
            set
            {
                var message = new GameChanged
                {
                    User1Id = this.model.User1Id,
                    User2Id = this.model.User2Id,
                    GameId = this.model.Id,
                    TurnCount = value
                };

                bool changed = this.ModelUpdaterResolver.GetUpdater<IUpdatableGame, GameChanged>().Update(this.model, message);

                if (changed)
                {
                    this.ServiceClient.PublishAsync<GameChanged>(message);
                }
            }
        }

        public Guid Id { get => this.model.Id; }

        private void Handle(GameJoined message)
        {
            this.ModelUpdaterResolver.GetUpdater<IUpdatableGame, GameJoined>().Update(this.model, message);
        }

        private void Handle(GameChanged message)
        {
            this.ModelUpdaterResolver.GetUpdater<IUpdatableGame, GameChanged>().Update(this.model, message);
        }
    }

    internal class GamesProxy : ProxyBase, IGameModels
    {
        private readonly ILog log = LogManager.GetLogger(nameof(GamesProxy));

        private readonly IUpdatableGames model;

        private SerializableObservableCollection<IGameModel> gamesProxies;

        public GamesProxy(
            IServiceClient serviceClient,
            IModelUpdaterResolver modelUpdaterResolver,
            IUpdatableGames model)
            : base(serviceClient, modelUpdaterResolver)
        {
            this.model = model;
            this.gamesProxies = new SerializableObservableCollection<IGameModel>();

            foreach (var game in this.model.Games)
            {
                this.gamesProxies.Add(new GameProxy(serviceClient, modelUpdaterResolver, game));
            }

            this.SetUpModelPropertyChangedPropagation(this.model);

            this.SubscribeToMessage<GameHosted>(this.Handle);
            this.SubscribeToMessage<GamesChanged>(this.Handle);
            this.model.Games.CollectionChanged += Games_CollectionChanged;
        }

        private void Games_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (var game in e.NewItems.Cast<IUpdatableGame>())
                {
                    this.gamesProxies.Add(new GameProxy(ServiceClient, ModelUpdaterResolver, game));
                }
            }
            else
            {
                foreach (var game in e.OldItems.Cast<IUpdatableGame>())
                {
                    this.gamesProxies.Remove(game);
                }
            }
          
        }

        public IReadOnlyObservableCollection<IGameModel> Games => this.gamesProxies;

        public IGameModel HostGame(IUserModel user)
        {
            var message = new GamesChanged()
            {
                Id = user.Id
            };

            var changed = this.ModelUpdaterResolver.GetUpdater<IUpdatableGames, GamesChanged>().Update(this.model, message);

            if (changed)
            {
                this.ServiceClient.PublishAsync<GamesChanged>(message);
                return this.model.Games.FirstOrDefault(x => x.User1Id == user.Id);
            }

            return null;
        }

        public void JoinGame(IGameModel game, IUserModel user)
        {
            var message = new GameJoined()
            {
                User2Id = user.Id,
                GameId = game.Id
            };

            var changed = this.ModelUpdaterResolver.GetUpdater<IUpdatableGame, GameJoined>().Update(this.model.Games.FirstOrDefault(x=> x.Id == message.GameId), message);

            if (changed)
            {
                this.ServiceClient.PublishAsync<GameJoined>(message);
            }
        }

        private void Handle(GameHosted message)
        {
            this.log.Debug("Game hosted message handled by games collection proxy");
            this.ModelUpdaterResolver.GetUpdater<IUpdatableGames, GamesChanged>().Update(this.model, message);
        }

        private void Handle(GamesChanged message)
        {
            this.ModelUpdaterResolver.GetUpdater<IUpdatableGames, GamesChanged>().Update(this.model, message);
        }

        public IGameModel GetGame(Guid gameId)
        {
            return this.Games.FirstOrDefault(x => x.Id == gameId);
        }
    }
}
