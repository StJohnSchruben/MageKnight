
using Service;
using MKService.MessageHandlers;
using MKService.Queries;

namespace MKService.QueryHandlers
{
    internal class MageKnightQueryHandler : QueryHandlerBase<MageKnightQuery>
    {
        public MageKnightQueryHandler(
           IQueryContract queryContract,
           ICommandContract commandContract,
           IQueryHandler nextQueryHandler,
           MageKnightQuery defaultQueryResponse,
           IServerMessageHandlerResolver messageHandlerResolver)
           : base(queryContract, commandContract, nextQueryHandler, defaultQueryResponse, messageHandlerResolver)
        {
        }
    }
}
