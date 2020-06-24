
using Service;

namespace MKService.QueryFactories
{
    /// <summary>
    /// The generic query factory interface.
    /// </summary>
    /// <typeparam name="TQuery">The type of the query.</typeparam>
    /// <seealso cref="MKService.QueryFactories.IQueryFactory" />
    internal interface IQueryFactory<out TQuery> : IQueryFactory where TQuery : class, IQueryResponse
    {
        /// <summary>
        /// Creates a new a new instance of <see cref="TQuery" />.
        /// </summary>
        /// <returns>A new instance of <see cref="TQuery" />.</returns>
        TQuery Create();
    }
}