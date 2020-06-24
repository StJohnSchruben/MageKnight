
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Service;
using MKService.Updates;

namespace MKService.ModelUpdaters
{
    /// <summary>
    /// A type that can resolve a <see cref="IModelUpdater" /> for a given model type and command message type.
    /// </summary>
    internal class ModelUpdaterResolver : IModelUpdaterResolver
    {
        /// <summary>
        /// The model updaters.
        /// </summary>
        private readonly IEnumerable<IModelUpdater> updaters;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelUpdaterResolver" /> class.
        /// </summary>
        /// <param name="updaters">The model updaters.</param>
        public ModelUpdaterResolver(IEnumerable<IModelUpdater> updaters)
        {
            this.updaters = updaters;
        }

        /// <summary>
        /// Gets the model updater that can handle updating the specified model type with the specified command message
        /// type. This method will intentionally throw an exception if no updater was found.
        /// </summary>
        /// <typeparam name="TModel">The model type.</typeparam>
        /// <typeparam name="TMessage">The command message type.</typeparam>
        /// <returns>The model updater.</returns>
        public IModelUpdater<TModel, TMessage> GetUpdater<TModel, TMessage>()
            where TModel : IUpdatable where TMessage : IMessage
        {
            var result = this.updaters.Where(x => x.CanUpdate(typeof(TModel), typeof(TMessage))).ToList();

            Debug.WriteLine(
                $"Searching for a model updater that can update '{typeof(TModel).Name}' " +
                $"with message '{typeof(TMessage).Name}'.");

            if (result.Count == 0)
            {
                throw new ArgumentException(
                    $"Could not locate a model updater that can update a model type '{typeof(TModel)}' with " +
                    $"command message type '{typeof(TMessage)}'. Did you forget to write a model updater?");
            }

            if (result.Count > 1)
            {
                throw new ArgumentException(
                    $"Found more than one model updater that can update a model type '{typeof(TModel)}' with " +
                    $"command message type '{typeof(TMessage)}'. This is ambiguous.");
            }

            return (IModelUpdater<TModel, TMessage>)result[0];
        }
    }
}