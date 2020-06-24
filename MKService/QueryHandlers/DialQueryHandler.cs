using Service;
using MKService.MessageHandlers;
using MKService.Queries;


namespace MKService.QueryHandlers
{
    internal class DialQueryHandler : QueryHandlerBase<DialQuery>
    {
        public DialQueryHandler(
              IQueryContract queryContract,
              ICommandContract commandContract,
              IQueryHandler nextQueryHandler,
              DialQuery defaultQueryResponse,
              IServerMessageHandlerResolver messageHandlerResolver)
              : base(queryContract, commandContract, nextQueryHandler, defaultQueryResponse, messageHandlerResolver)
        {
        }
    }
}
