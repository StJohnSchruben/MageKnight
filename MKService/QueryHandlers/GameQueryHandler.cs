using Service;
using MKService.MessageHandlers;
using MKService.Queries;


namespace MKService.QueryHandlers
{
    internal class GameQueryHandler : QueryHandlerBase<GameQuery>
    {
        public GameQueryHandler(
               IQueryContract queryContract,
               ICommandContract commandContract,
               IQueryHandler nextQueryHandler,
               GameQuery defaultQueryResponse,
               IServerMessageHandlerResolver messageHandlerResolver)
               : base(queryContract, commandContract, nextQueryHandler, defaultQueryResponse, messageHandlerResolver)
        {
        }
    }
    internal class GamesQueryHandler : QueryHandlerBase<GamesQuery>
    {
        public GamesQueryHandler(
               IQueryContract queryContract,
               ICommandContract commandContract,
               IQueryHandler nextQueryHandler,
               GamesQuery defaultQueryResponse,
               IServerMessageHandlerResolver messageHandlerResolver)
               : base(queryContract, commandContract, nextQueryHandler, defaultQueryResponse, messageHandlerResolver)
        {
        }
    }
}
