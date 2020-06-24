

using System.ServiceModel;

namespace Service
{
    /// <summary>
    /// Base class for query service contracts.
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public abstract class QueryContractBase : IQueryContract, IQueryHandler
    {
        /// <summary>
        /// The query handler.
        /// </summary>
        private IQueryHandler queryHandler;

        /// <summary>
        /// Occurs when a new query is submitted.
        /// </summary>
        public event QueryEventHandler QuerySubmitted;

        /// <summary>
        /// Adds the handler.
        /// </summary>
        /// <param name="handler">The handler.</param>
        public void AddHandler(IQueryHandler handler)
        {
            if (this.queryHandler == null)
            {
                this.queryHandler = handler;
            }
            else
            {
                this.queryHandler.AddHandler(handler);
            }
        }

        /// <summary>
        /// Executes a direct query which does not utilize callbacks.
        /// </summary>
        /// <param name="queryType">The query type.</param>
        /// <param name="queryDef">The query definition.</param>
        /// <returns>
        /// The query response of the specified query type.
        /// </returns>
        public IQueryResponse DirectQuery(string queryType, IQueryDef queryDef)
        {
            return this.queryHandler?.Handle(queryType, queryDef);
        }

        /// <summary>
        /// Handles the specified query.
        /// </summary>
        /// <param name="queryType">The query type.</param>
        /// <param name="queryDef">The query definition.</param>
        /// <returns>
        /// The query response of the specified query type.
        /// </returns>
        /// <remarks>
        /// This method should handle the message. If the internal next handler is <c>null</c>, this method should also return
        /// <c>null</c>; otherwise, it should return the result from the next internal handler's <see cref="Handle" /> method.
        /// </remarks>
        public IQueryResponse Handle(string queryType, IQueryDef queryDef)
        {
            return null;
        }

        /// <summary>
        /// Executes a query using callbacks.
        /// </summary>
        /// <param name="queryType">The query type.</param>
        /// <param name="queryDef">The query definition.</param>
        public void Query(string queryType, IQueryDef queryDef)
        {
            var handler = this.QuerySubmitted;

            if (handler == null)
            {
                return;
            }

            var callbackChannel = OperationContext.Current.GetCallbackChannel<IQueryCallbackContract>();

            handler.Invoke(
                this,
                new QueryArgs
                {
                    QueryType = queryType,
                    QueryDefinition = queryDef,
                    Callback = callbackChannel
                });
        }
    }
}