

using System.ServiceModel;

namespace Service
{
    /// <summary>
    /// Represents the service contract for query callbacks.
    /// </summary>
    [ServiceContract(Namespace = ServiceConstants.QueryNamespace)]
    public interface IQueryCallbackContract
    {
        /// <summary>
        /// Handles the specified query.
        /// </summary>
        /// <param name="query">The query.</param>
        [OperationContract(IsOneWay = true)]
        void HandleQueryResponse(IQueryResponse query);
    }
}