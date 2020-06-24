
using System.Collections.Generic;
using Service;

namespace MKService.QueryHandlers
{
    /// <summary>
    /// The query handler collection factory interface.
    /// </summary>
    public interface IQueryHandlerCollectionFactory
    {
        /// <summary>
        /// Creates the specified query contract.
        /// </summary>
        /// <param name="queryContract">The query contract.</param>
        /// <param name="commandContract">The command contract.</param>
        /// <param name="rootHandler">The root handler.</param>
        /// <returns>The IList.</returns>
        IList<IQueryHandler> Create(
            IQueryContract queryContract,
            ICommandContract commandContract,
            IQueryHandler rootHandler);
    }
}