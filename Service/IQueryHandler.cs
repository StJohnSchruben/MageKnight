

namespace Service
{
    /// <summary>
    /// Represents a handler of queries.
    /// </summary>
    /// <remarks>
    /// This type is designed to use the chain of responsibility pattern.
    /// </remarks>
    public interface IQueryHandler
    {
        /// <summary>
        /// Adds the specified query handler.
        /// </summary>
        /// <param name="handler">The query handler.</param>
        /// <remarks>
        /// If the internal next handler is <c>null</c>, this method should set the next internal handler to the specified handler;
        /// otherwise, it should pass the specified handler to the next internal handler's <see cref="AddHandler" /> method.
        /// </remarks>
        void AddHandler(IQueryHandler handler);

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
        IQueryResponse Handle(string queryType, IQueryDef queryDef);
    }
}