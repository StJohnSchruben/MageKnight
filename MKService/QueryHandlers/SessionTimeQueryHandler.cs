using Service;
using MKService.MessageHandlers;
using MKService.Queries;

namespace MKService.QueryHandlers
{
    /// <summary>
    /// The airplane query handler.
    /// </summary>
    /// <seealso cref="MKService.QueryHandlers.QueryHandlerBase{MKService.Queries.SessionTimeQuery}" />
    internal class SessionTimeQueryHandler : QueryHandlerBase<SessionTimeQuery>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SessionTimeQueryHandler" /> class.
        /// </summary>
        /// <param name="queryContract">The query contract.</param>
        /// <param name="commandContract">The command contract.</param>
        /// <param name="nextQueryHandler">The next query handler in the chain.</param>
        /// <param name="defaultQueryResponse">The default query response.</param>
        /// <param name="messageHandlerResolver">The message handler resolver.</param>
        public SessionTimeQueryHandler(
            IQueryContract queryContract,
            ICommandContract commandContract,
            IQueryHandler nextQueryHandler,
            SessionTimeQuery defaultQueryResponse,
            IServerMessageHandlerResolver messageHandlerResolver)
            : base(queryContract, commandContract, nextQueryHandler, defaultQueryResponse, messageHandlerResolver)
        {
        }
    }
}