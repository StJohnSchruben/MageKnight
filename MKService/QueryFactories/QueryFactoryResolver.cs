
using System;
using System.Collections.Generic;

using Service;

namespace MKService.QueryFactories
{
    /// <summary>
    /// The query factory resolver.
    /// </summary>
    /// <seealso cref="MKService.QueryFactories.IQueryFactoryResolver" />
    internal class QueryFactoryResolver : IQueryFactoryResolver
    {
        /// <summary>
        /// The factories.
        /// </summary>
        private readonly Dictionary<Type, IQueryFactory> factories = new Dictionary<Type, IQueryFactory>();

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryFactoryResolver" /> class.
        /// </summary>
        /// <param name="factories">The query factories.</param>
        public QueryFactoryResolver(IEnumerable<IQueryFactory> factories)
        {
            foreach (var factory in factories)
            {
                this.factories.Add(factory.SupportedType, factory);
            }
        }

        /// <summary>
        /// Gets the query factory for the specified query type. This method will intentionally throw an exception if no
        /// factory could be found.
        /// </summary>
        /// <typeparam name="TQuery">The query type.</typeparam>
        /// <returns>The factory for the specified query type.</returns>
        /// <exception cref="System.ArgumentException">The ArgumentException.</exception>
        public IQueryFactory<TQuery> GetFactory<TQuery>() where TQuery : class, IQueryResponse
        {
            if (!this.factories.ContainsKey(typeof(TQuery)))
            {
                throw new ArgumentException(
                    $"Could not locate a query factory for the type, '{typeof(TQuery)}'. " +
                    "Did you forget to write a query factory?");
            }

            return (IQueryFactory<TQuery>)this.factories[typeof(TQuery)];
        }
    }
}