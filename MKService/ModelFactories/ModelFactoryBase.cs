

using System;
using MKService.Updates;

namespace MKService.ModelFactories
{
    /// <summary>
    /// Base class for all model factories.
    /// </summary>
    /// <typeparam name="TModel">The type that can be created by the factory.</typeparam>
    internal abstract class ModelFactoryBase<TModel> : IModelFactory<TModel> where TModel : IUpdatable
    {
        /// <summary>
        /// Gets the type that can be created by the factory.
        /// </summary>
        /// <value>The type that can be created by the factory.</value>
        public Type SupportedType => typeof(TModel);

        /// <summary>
        /// Creates a new a new instance of the model type.
        /// </summary>
        /// <returns>A new instance of the model type.</returns>
        public abstract TModel Create();
    }
}