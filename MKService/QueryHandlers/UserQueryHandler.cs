
using Service;
using MKService.MessageHandlers;
using MKService.Queries;

namespace MKService.QueryHandlers
{
    internal class UserQueryHandler : QueryHandlerBase<UserQuery>
    {
        public UserQueryHandler(
              IQueryContract queryContract,
              ICommandContract commandContract,
              IQueryHandler nextQueryHandler,
              UserQuery defaultQueryResponse,
              IServerMessageHandlerResolver messageHandlerResolver)
              : base(queryContract, commandContract, nextQueryHandler, defaultQueryResponse, messageHandlerResolver)
        {
        }
    }
}
