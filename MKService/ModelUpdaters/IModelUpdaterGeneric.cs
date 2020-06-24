
using Service;
using MKService.Updates;

namespace MKService.ModelUpdaters
{
    /// <summary>
    /// Represents a type whose sole responsibility is to update the model in response to a command message in a
    /// consistent manner. This ensures consistency in how the server message handlers and the client proxies are
    /// updating their respective models in response to command messages.
    /// </summary>
    /// <typeparam name="TModel">The model type.</typeparam>
    /// <typeparam name="TMessage">The command message type.</typeparam>
    internal interface IModelUpdater<in TModel, in TMessage> : IModelUpdater
        where TModel : IUpdatable where TMessage : IMessage
    {
        /// <summary>
        /// Updates the specified model with the specified command message.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="message">The command message.</param>
        /// <returns>
        /// <c>true</c>, if any updates were actually made to the specified model; <c>false</c>, if no changes were
        /// actually made.
        /// </returns>
        bool Update(TModel model, TMessage message);
    }
}