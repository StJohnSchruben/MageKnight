using Service;
using MKService.MessageHandlers;
using MKService.Queries;


namespace MKService.QueryHandlers
{
    internal class UserCollectionQueryHandler : QueryHandlerBase<UserCollectionQuery>
    {
        public UserCollectionQueryHandler(
              IQueryContract queryContract,
              ICommandContract commandContract,
              IQueryHandler nextQueryHandler,
              UserCollectionQuery defaultQueryResponse,
              IServerMessageHandlerResolver messageHandlerResolver)
              : base(queryContract, commandContract, nextQueryHandler, defaultQueryResponse, messageHandlerResolver)
        {
        }
    }
}
