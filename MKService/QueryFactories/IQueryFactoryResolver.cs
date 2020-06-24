
using Service;

namespace MKService.QueryFactories
{
    /// <summary>
    /// The query factory resolver interface.
    /// </summary>
    internal interface IQueryFactoryResolver
    {
        /// <summary>
        /// Gets the factory.
        /// </summary>
        /// <typeparam name="TQuery">The type of the query.</typeparam>
        /// <returns>The query factory.</returns>
        IQueryFactory<TQuery> GetFactory<TQuery>() where TQuery : class, IQueryResponse;
    }
}