
using Service;
using MKService.Updates;

namespace MKService.ModelUpdaters
{
    /// <summary>
    /// Represents a type that can resolve a <see cref="IModelUpdater" /> for a given model type and command message type.
    /// </summary>
    internal interface IModelUpdaterResolver
    {
        /// <summary>
        /// Gets the model updater that can handle updating the specified model type with the specified command message
        /// type. This method will intentionally throw an exception if no updater was found.
        /// </summary>
        /// <typeparam name="TModel">The model type.</typeparam>
        /// <typeparam name="TMessage">The command message type.</typeparam>
        /// <returns>The model updater.</returns>
        IModelUpdater<TModel, TMessage> GetUpdater<TModel, TMessage>()
            where TModel : IUpdatable where TMessage : IMessage;
    }
}