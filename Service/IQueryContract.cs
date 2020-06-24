

using System.ServiceModel;

namespace Service
{
    /// <summary>
    /// Represents the service contract for queries.
    /// </summary>
    [ServiceContract(CallbackContract = typeof(IQueryCallbackContract), Namespace = ServiceConstants.QueryNamespace)]
    public interface IQueryContract
    {
        /// <summary>
        /// Occurs when a new query is submitted.
        /// </summary>
        event QueryEventHandler QuerySubmitted;

        /// <summary>
        /// Executes a direct query which does not utilize callbacks.
        /// </summary>
        /// <param name="queryType">The query type.</param>
        /// <param name="queryDef">The query definition.</param>
        /// <returns>
        /// The query response of the specified query type.
        /// </returns>
        [OperationContract]
        IQueryResponse DirectQuery(string queryType, IQueryDef queryDef);

        /// <summary>
        /// Executes a query using callbacks.
        /// </summary>
        /// <param name="queryType">The query type.</param>
        /// <param name="queryDef">The query definition.</param>
        [OperationContract(IsOneWay = true)]
        void Query(string queryType, IQueryDef queryDef);
    }
}