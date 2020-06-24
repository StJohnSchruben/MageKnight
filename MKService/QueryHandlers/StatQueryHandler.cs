using Service;
using MKService.MessageHandlers;
using MKService.Queries;


namespace MKService.QueryHandlers
{
    internal class StatQueryHandler : QueryHandlerBase<StatQuery>
    {
        public StatQueryHandler(
              IQueryContract queryContract,
              ICommandContract commandContract,
              IQueryHandler nextQueryHandler,
              StatQuery defaultQueryResponse,
              IServerMessageHandlerResolver messageHandlerResolver)
              : base(queryContract, commandContract, nextQueryHandler, defaultQueryResponse, messageHandlerResolver)
        {
        }
    }
}
