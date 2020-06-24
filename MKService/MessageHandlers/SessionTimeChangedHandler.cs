
using MKService.Messages;
using MKService.ModelUpdaters;
using MKService.Queries;
using MKService.Updates;

namespace MKService.MessageHandlers
{
    /// <summary>
    /// Service Handler for the Session Time Changed Message.
    /// </summary>
    /// <seealso cref="MKService.MessageHandlers.ServerMessageHandlerBase{MKService.Messages.SessionTimeChanged, MKService.Queries.SessionTimeQuery, MKService.Updates.IUpdatableSessionTime}" />
    internal class SessionTimeChangedHandler : ServerMessageHandlerBase<SessionTimeChanged, SessionTimeQuery, IUpdatableSessionTime>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SessionTimeChangedHandler" /> class.
        /// </summary>
        /// <param name="modelUpdaterResolver">The model updater resolver.</param>
        public SessionTimeChangedHandler(IModelUpdaterResolver modelUpdaterResolver) : base(modelUpdaterResolver)
        {
        }

        /// <summary>
        /// Locates the query or query component of the specified query that needs to be updated in response to the
        /// specified message. This method should not actually update the query as this is handled in the model updater.
        /// It is safe to return <c>null</c> from this method.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="query">The query to be updated.</param>
        /// <returns>
        /// If the query or query component affected by the specified message is found, the query or query component;
        /// otherwise, <c>null</c>.
        /// </returns>
        protected override IUpdatableSessionTime LocateQueryComponent(SessionTimeChanged message, SessionTimeQuery query)
        {
            return query;
        }
    }
}