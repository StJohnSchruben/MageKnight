
using System;

namespace MKService.QueryFactories
{
    /// <summary>
    /// The query factory interface.
    /// </summary>
    internal interface IQueryFactory
    {
        /// <summary>
        /// Gets the type of the supported.
        /// </summary>
        /// <value>The type of the supported.</value>
        Type SupportedType { get; }
    }
}