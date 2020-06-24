
using System;
using Service;
using MKService.Updates;

namespace MKService.ModelUpdaters
{
    /// <summary>
    /// Represents a type whose sole responsibility is to update the model in response to a command message in a
    /// consistent manner. This ensures consistency in how the server message handlers and the client proxies are
    /// updating their respective models in response to command messages.
    /// </summary>
    internal interface IModelUpdater
    {
        /// <summary>
        /// Determine if the updater can update the specified model type with the specified command message type.
        /// </summary>
        /// <param name="modelType">The model type.</param>
        /// <param name="messageType">The command message type.</param>
        /// <returns>
        /// <c>true</c>, if the updater can the specified model type with the specified command message type; otherwise, <c>false</c>.
        /// </returns>
        bool CanUpdate(Type modelType, Type messageType);

        /// <summary>
        /// Updates the specified model with the specified command message.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="message">The command message.</param>
        /// <returns>
        /// <c>true</c>, if any updates were actually made to the specified model; <c>false</c>, if no changes were
        /// actually made.
        /// </returns>
        bool Update(IUpdatable model, IMessage message);
    }
}