

using System;
using System.Collections.Generic;
using System.Diagnostics;

using MKService.Updates;

namespace MKService.ModelFactories
{
    /// <summary>
    /// A type that can resolve a <see cref="IModelFactory" /> for a given type.
    /// </summary>
    internal class ModelFactoryResolver : IModelFactoryResolver
    {
        /// <summary>
        /// The model factories.
        /// </summary>
        private readonly Dictionary<Type, IModelFactory> factories = new Dictionary<Type, IModelFactory>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelFactoryResolver" /> class.
        /// </summary>
        /// <param name="factories">The model factories.</param>
        public ModelFactoryResolver(IEnumerable<IModelFactory> factories)
        {
            foreach (var factory in factories)
            {
                this.factories.Add(factory.SupportedType, factory);
            }
        }

        /// <summary>
        /// Gets the model factory for the specified model type. This method will intentionally throw an exception if no
        /// factory could be found.
        /// </summary>
        /// <typeparam name="TModel">The model type.</typeparam>
        /// <returns>The factory for the specified model type.</returns>
        public IModelFactory<TModel> GetFactory<TModel>() where TModel : IUpdatable
        {
            Debug.WriteLine(
                $"Searching for a model factory that can create instances of '{typeof(TModel).Name}'.");

            if (!this.factories.ContainsKey(typeof(TModel)))
            {
                throw new ArgumentException(
                    $"Could not locate a model factory for the type, '{typeof(TModel)}'. " +
                    "Did you forget to write a model factory?");
            }

            return (IModelFactory<TModel>)this.factories[typeof(TModel)];
        }
    }
}