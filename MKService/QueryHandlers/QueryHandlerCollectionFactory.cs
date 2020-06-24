
using System.Collections.Generic;

using Service;
using MKService.MessageHandlers;
using MKService.Queries;
using MKService.QueryFactories;

namespace MKService.QueryHandlers
{
    /// <summary>
    /// The query handler collection factory.
    /// </summary>
    /// <seealso cref="MKService.QueryHandlers.IQueryHandlerCollectionFactory"/>
    internal class QueryHandlerCollectionFactory : IQueryHandlerCollectionFactory
    {
        /// <summary>
        /// The message handler resolver.
        /// </summary>
        private readonly IServerMessageHandlerResolver messageHandlerResolver;

        /// <summary>
        /// The query factory resolver.
        /// </summary>
        private readonly IQueryFactoryResolver queryFactoryResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryHandlerCollectionFactory" /> class.
        /// </summary>
        /// <param name="queryFactoryResolver">The model factory resolver.</param>
        /// <param name="messageHandlerResolver">The message handler resolver.</param>
        public QueryHandlerCollectionFactory(
            IQueryFactoryResolver queryFactoryResolver,
            IServerMessageHandlerResolver messageHandlerResolver)
        {
            this.queryFactoryResolver = queryFactoryResolver;
            this.messageHandlerResolver = messageHandlerResolver;
        }

        /// <summary>
        /// Creates a list of query handlers.
        /// </summary>
        /// <param name="queryContract">The query contract.</param>
        /// <param name="commandContract">The command contract.</param>
        /// <param name="rootHandler">The root query handler.</param>
        /// <returns>A list of query handlers.</returns>
        public IList<IQueryHandler> Create(
            IQueryContract queryContract,
            ICommandContract commandContract,
            IQueryHandler rootHandler)
        {
            var result = new List<IQueryHandler>
            {
                new SessionTimeQueryHandler(
                    queryContract,
                    commandContract,
                    rootHandler,
                    this.queryFactoryResolver.GetFactory<SessionTimeQuery>().Create(),
                    this.messageHandlerResolver),
                new MageKnightQueryHandler(
                    queryContract,
                    commandContract,
                    rootHandler,
                    this.queryFactoryResolver.GetFactory<MageKnightQuery>().Create(),
                    this.messageHandlerResolver),
                new UserQueryHandler(
                    queryContract,
                    commandContract,
                    rootHandler,
                    this.queryFactoryResolver.GetFactory<UserQuery>().Create(),
                    this.messageHandlerResolver),
                new DialQueryHandler(
                    queryContract,
                    commandContract,
                    rootHandler,
                    this.queryFactoryResolver.GetFactory<DialQuery>().Create(),
                    this.messageHandlerResolver),
                new ClickQueryHandler(
                    queryContract,
                    commandContract,
                    rootHandler,
                    this.queryFactoryResolver.GetFactory<ClickQuery>().Create(),
                    this.messageHandlerResolver),
                new StatQueryHandler(
                    queryContract,
                    commandContract,
                    rootHandler,
                    this.queryFactoryResolver.GetFactory<StatQuery>().Create(),
                    this.messageHandlerResolver),
                 new UserCollectionQueryHandler(
                    queryContract,
                    commandContract,
                    rootHandler,
                    this.queryFactoryResolver.GetFactory<UserCollectionQuery>().Create(),
                    this.messageHandlerResolver),
                new GameQueryHandler(
                    queryContract,
                    commandContract,
                    rootHandler,
                    this.queryFactoryResolver.GetFactory<GameQuery>().Create(),
                    this.messageHandlerResolver),
                new GamesQueryHandler(
                    queryContract,
                    commandContract,
                    rootHandler,
                    this.queryFactoryResolver.GetFactory<GamesQuery>().Create(),
                    this.messageHandlerResolver),
            };

            return result;
        }
    }
}