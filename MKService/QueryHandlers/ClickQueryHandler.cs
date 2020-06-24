using Service;
using MKService.MessageHandlers;
using MKService.Queries;


namespace MKService.QueryHandlers
{
    internal class ClickQueryHandler : QueryHandlerBase<ClickQuery>
    {
        public ClickQueryHandler(
              IQueryContract queryContract,
              ICommandContract commandContract,
              IQueryHandler nextQueryHandler,
              ClickQuery defaultQueryResponse,
              IServerMessageHandlerResolver messageHandlerResolver)
              : base(queryContract, commandContract, nextQueryHandler, defaultQueryResponse, messageHandlerResolver)
        {
        }
    }
}
