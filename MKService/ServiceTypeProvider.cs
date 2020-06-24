using System;
using System.Net;
using System.Collections.Generic;
using MKService.QueryFactories;
using MKService.ModelFactories;
using MKService.QueryHandlers;
using System.Net.Sockets;
using MKService.DefaultModel;
using System.Threading;
using MKService.ModelUpdaters;
using MKService.MessageHandlers;
using MKService.ProxyFactories;
using MKModel;
using MKService.Queries;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace MKService
{
    public class ServiceTypeProvider : IClientServiceTypeProvider, IServerServiceTypeProvider
    {
        public const int DefaultPort = 9000;

        private static readonly Lazy<ServiceTypeProvider> instance = new Lazy<ServiceTypeProvider>(() => new ServiceTypeProvider());

        private readonly IDefaultModel defaultModel;
       
        public Guid LoggedInUserId { get; set; }
        public IGameModel GameModel { get; set; }

        private readonly ModelFactoryResolver modelFactoryResolver;
        private readonly ModelUpdaterResolver modelUpdaterResolver;
        private readonly QueryFactoryResolver queryFactoryResolver;

        private readonly Lazy<ISessionTime> sessionTime;
        private readonly Lazy<ISessionTimeProxyFactory> sessionTimeProxyFactory;

        private Lazy<IUserModel> user;
        private Lazy<IUserModelProxyFactory> userProxyFactory;

        private Lazy<IUserCollection> userCollection;
        private Lazy<IUserCollectionProxyFactory> userCollectionProxyFactory;

        private readonly Lazy<IMageKnightModel> mageKnight;
        private readonly Lazy<IMageKnightModelProxyFactory> mageKnightProxyFactory;

       private readonly Lazy<IGameModel> game;
        private readonly Lazy<IGameModelProxyFactory> gameProxyFactory;

        private readonly Lazy<IGameModels> games;
        private readonly Lazy<IGamesModelProxyFactory> gamesProxyFactory;

        private readonly Lazy<IServiceClient> serviceClient;

        private ServiceTypeProvider()
        {
            var modelFactories = new IModelFactory[]
            {
                new SessionTimeFactory(),
                new MageKnightFactory(),
                new UserFactory(),
                new UserCollectionFactory(),
                new GameFactory(),
                new GamesFactory(),
                new DialFactory(),
                new StatFactory(),
                new ClickFactory(),
            };

            this.modelFactoryResolver = new ModelFactoryResolver(modelFactories);
            this.defaultModel = new DefaultModel.DefaultModel(this.modelFactoryResolver);

            IEnumerable<IQueryFactory> queryFactories;
            queryFactories = new IQueryFactory[]
                {
                    new SessionTimeQueryFactory(this.modelFactoryResolver, this.defaultModel),
                    new MageKnightQueryFactory(this.modelFactoryResolver, this.defaultModel),
                    new UserQueryFactory(this.modelFactoryResolver, this.defaultModel),
                    new UserCollectionQueryFactory(this.modelFactoryResolver, this.defaultModel),
                    new GameQueryFactory(this.modelFactoryResolver, this.defaultModel),
                    new GamesQueryFactory(this.modelFactoryResolver, this.defaultModel),
                    new DialQueryFactory(this.modelFactoryResolver, this.defaultModel),
                    new ClickQueryFactory(this.modelFactoryResolver, this.defaultModel),
                    new StatQueryFactory(this.modelFactoryResolver, this.defaultModel),
                };

            this.queryFactoryResolver = new QueryFactoryResolver(queryFactories);

            var modelUpdaters = new IModelUpdater[]
            {
                new SessionTimeChangedUpdater(this.modelFactoryResolver, this.defaultModel),
                new MageKnightChangedUpdater(this.modelFactoryResolver, this.defaultModel),
                new UserChangedUpdater(this.modelFactoryResolver, this.defaultModel),
                new UserCollectionAddedUpdater(this.modelFactoryResolver, this.defaultModel),
                new UserInventoryAddUpdater(this.modelFactoryResolver, this.defaultModel),
                new UserArmyAddUpdater(this.modelFactoryResolver, this.defaultModel),
                new UserAddedMageToArmyUpdater(this.modelFactoryResolver, this.defaultModel),
                new GameChangedModelUpdater(this.modelFactoryResolver, this.defaultModel),
                new GamesChangedModelUpdater(this.modelFactoryResolver, this.defaultModel),
                new UserSignUpUpdater(this.modelFactoryResolver, this.defaultModel),
                new UserBoosterPackCountChangedUpdater(this.modelFactoryResolver, this.defaultModel),
                new UserSignInUpdater(this.modelFactoryResolver, this.defaultModel),
                new UserIsSignedInChangedUpdater(this.modelFactoryResolver, this.defaultModel),
                new GameJoinedUpdater(this.modelFactoryResolver, this.defaultModel),
                new MageKnightCoordinatesChangedUpdater(this.modelFactoryResolver, this.defaultModel),
                new UserSelectedMageChangedUpdater(this.modelFactoryResolver, this.defaultModel),
                new UserAddedMageToInventoryUpdater(this.modelFactoryResolver, this.defaultModel),
            };

            this.modelUpdaterResolver = new ModelUpdaterResolver(modelUpdaters);

            var serverMessageHandlers = new IServerMessageHandler[]
            {
                new SessionTimeChangedHandler(this.modelUpdaterResolver),
                new MageKnightChangedHandler(this.modelUpdaterResolver),
                new UserChangedHandler(this.modelUpdaterResolver),
                new UserCollectionAddedHandler(this.modelUpdaterResolver),
                new UserInventoryAddHandler(this.modelUpdaterResolver),
                new UserAddedMageToArmyHandler(this.modelUpdaterResolver),
                new UserArmyAddHandler(this.modelUpdaterResolver),
                new GameChangedHandler(this.modelUpdaterResolver),
                new GamesChangedHandler(this.modelUpdaterResolver),
                new UserSignUpHandler(this.modelUpdaterResolver),
                new UserBoosterPackCountChangedHandler(this.modelUpdaterResolver),
                new UserSignInHandler(this.modelUpdaterResolver),
                new UserIsSignedInChangedHandler(this.modelUpdaterResolver),
                new GameJoinedHandler(this.modelUpdaterResolver),
                new MageKnightCoordinatesChangedHandler(this.modelUpdaterResolver),
                new UserSelectedMageChangedHandler(this.modelUpdaterResolver),
                new UserAddedMageToInventoryHandler(this.modelUpdaterResolver),
            };

            var serverMessageHandlerResolver = new ServerMessageHandlerResolver(serverMessageHandlers);

            this.QueryHandlerCollectionFactory = new QueryHandlerCollectionFactory(
                this.queryFactoryResolver,
                serverMessageHandlerResolver);

            this.serviceClient = new Lazy<IServiceClient>(
                this.CreateServiceClient,
                LazyThreadSafetyMode.ExecutionAndPublication);

            this.sessionTimeProxyFactory = new Lazy<ISessionTimeProxyFactory>(
                this.CreateSessionTimeProxyFactory,
                LazyThreadSafetyMode.ExecutionAndPublication);

            this.sessionTime = new Lazy<ISessionTime>(
                this.CreateSessionTime,
                LazyThreadSafetyMode.ExecutionAndPublication);

            this.mageKnightProxyFactory = new Lazy<IMageKnightModelProxyFactory>(
              this.CreateMageKnightProxyFactory,
              LazyThreadSafetyMode.ExecutionAndPublication);

            this.mageKnight = new Lazy<IMageKnightModel>(
                this.CreateMageKnight,
                LazyThreadSafetyMode.ExecutionAndPublication);

            this.userProxyFactory = new Lazy<IUserModelProxyFactory>(
             this.CreateUserProxyFactory,
             LazyThreadSafetyMode.ExecutionAndPublication);

            this.user = new Lazy<IUserModel>(
                this.CreateUser,
                LazyThreadSafetyMode.ExecutionAndPublication);

            this.userCollectionProxyFactory = new Lazy<IUserCollectionProxyFactory>(
           this.CreateUserCollectionProxyFactory,
           LazyThreadSafetyMode.ExecutionAndPublication);

            this.userCollection = new Lazy<IUserCollection>(
                this.CreateUserCollection,
                LazyThreadSafetyMode.ExecutionAndPublication);

            this.gameProxyFactory = new Lazy<IGameModelProxyFactory>(
             this.CreateGameProxyFactory,
             LazyThreadSafetyMode.ExecutionAndPublication);

            this.game = new Lazy<IGameModel>(
                this.CreateGame,
                LazyThreadSafetyMode.ExecutionAndPublication);

            this.gamesProxyFactory = new Lazy<IGamesModelProxyFactory>(
                 this.CreateGamesProxyFactory,
                 LazyThreadSafetyMode.ExecutionAndPublication);

            this.games = new Lazy<IGameModels>(
                this.CreateGames,
                LazyThreadSafetyMode.ExecutionAndPublication);
        }

        public static ServiceTypeProvider Instance => instance.Value;

        public IPEndPoint EndPoint { get; set; }

        public bool InComMode { get; set; }

        public ISessionTime SessionTime => this.sessionTime.Value;
        public IUserModel User => this.user.Value;
        public IUserCollection UserCollection => this.userCollection.Value;
        public IUserModel LoggedInUser => this.UserCollection.Users.FirstOrDefault(x=> x.Id == this.LoggedInUserId);
        public IGameModels Games => this.games.Value;
        public IGameModel Game
        {
            get
            {
                var game1 = Games.Games.FirstOrDefault(x => x.User1Id == this.LoggedInUserId);
                

                if (game1 != null)
                {
                    return game1;
                }
                else 
                {
                    IEnumerable<IGameModel> joinableGames = Games.Games.Where(x => x.User2Id != null);
                    foreach (var game in joinableGames)
                    {
                        if (game.User2Id == this.LoggedInUserId)
                        {
                            return game;
                        }
                    }
                }
                return null;
            }
        }
        public bool UseService { get; set; }

        public IQueryHandlerCollectionFactory QueryHandlerCollectionFactory { get; }
    
    private static IPEndPoint GetDefaultLocalEndpoint()
        {
            var address = IPAddress.Parse("127.0.0.1");

            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ipAddress in host.AddressList)
            {
                if (ipAddress.AddressFamily != AddressFamily.InterNetwork)
                {
                    continue;
                }

                address = ipAddress;
            }

            return new IPEndPoint(address, 9000);
        }

        private IServiceClient CreateServiceClient()
        {
            //if (!this.UseService)
            //{
            //    return new NullServiceClient(this.queryFactoryResolver);
            //}

            if (this.EndPoint == null)
            {
                this.EndPoint = GetDefaultLocalEndpoint();
            }

            return new ServiceClient(new ClientChannelProvider(this.EndPoint, new DeviceServiceBootstrapper(this)));
        }

        private ISessionTime CreateSessionTime()
        {
            return this.sessionTimeProxyFactory.Value.Create(this.InComMode);
        }

        private ISessionTimeProxyFactory CreateSessionTimeProxyFactory()
        {
            return new SessionTimeProxyFactory(this.serviceClient.Value, this.modelUpdaterResolver);
        }

        private IMageKnightModel CreateMageKnight()
        {
            return this.mageKnightProxyFactory.Value.Create();
        }

        private IMageKnightModelProxyFactory CreateMageKnightProxyFactory()
        {
            return new MageKnightModelProxyFactory(this.serviceClient.Value, this.modelUpdaterResolver);
        }

        private IUserModel CreateUser()
        {
            return this.userProxyFactory.Value.Create();
        }

        private IUserModelProxyFactory CreateUserProxyFactory()
        {
            return new UserProxyFactory(this.serviceClient.Value, this.modelUpdaterResolver);
        }

        private IUserCollection CreateUserCollection()
        {
            return this.userCollectionProxyFactory.Value.Create();
        }

        private IUserCollectionProxyFactory CreateUserCollectionProxyFactory()
        {
            return new UserCollectionProxyFactory(this.serviceClient.Value, this.modelUpdaterResolver);
        }

        private IGameModel CreateGame()
        {
            return this.gameProxyFactory.Value.Create();
        }

        private IGameModelProxyFactory CreateGameProxyFactory()
        {
            return new GameProxyFactory(this.serviceClient.Value, this.modelUpdaterResolver);
        }

        private IGameModels CreateGames()
        {
            return this.gamesProxyFactory.Value.Create();
        }

        private IGamesModelProxyFactory CreateGamesProxyFactory()
        {
            return new GamesProxyFactory(this.serviceClient.Value, this.modelUpdaterResolver);
        }
    }
}
