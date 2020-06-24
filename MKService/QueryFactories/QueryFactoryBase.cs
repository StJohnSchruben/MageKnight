
using System;

using Service;
using MKService.DefaultModel;
using MKService.ModelFactories;

namespace MKService.QueryFactories
{
    /// <summary>
    /// The query factory base.
    /// </summary>
    /// <typeparam name="TQuery">The type of the query.</typeparam>
    /// <seealso cref="MKService.QueryFactories.IQueryFactory{TQuery}" />
    internal abstract class QueryFactoryBase<TQuery> : IQueryFactory<TQuery> where TQuery : class, IQueryResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QueryFactoryBase{TQuery}" /> class.
        /// </summary>
        /// <param name="modelFactoryResolver">The model factory resolver.</param>
        /// <param name="defaultModel">The default model.</param>
        protected QueryFactoryBase(
            IModelFactoryResolver modelFactoryResolver,
            IDefaultModel defaultModel)
        {
            this.ModelFactoryResolver = modelFactoryResolver;
            this.DefaultModel = defaultModel;
        }

        /// <summary>
        /// Gets the default model.
        /// </summary>
        /// <value>
        /// The default model.
        /// </value>
        public IDefaultModel DefaultModel { get; }

        /// <summary>
        /// Gets the type that can be created by the factory.
        /// </summary>
        /// <value>
        /// The type that can be created by the factory.
        /// </value>
        public Type SupportedType => typeof(TQuery);

        /// <summary>
        /// Gets the model factory resolver.
        /// </summary>
        /// <value>
        /// The model factory resolver.
        /// </value>
        protected IModelFactoryResolver ModelFactoryResolver { get; }

        /// <summary>
        /// Creates a new a new instance of <typeparamref name="TQuery"/>.
        /// </summary>
        /// <returns>
        /// A new instance of <typeparamref name="TQuery"/>.
        /// </returns>
        public abstract TQuery Create();
    }
}