

using System;
using System.Diagnostics;
using log4net;

using Service;
using MKService.ModelFactories;
using MKService.Updates;
using MKService.DefaultModel;

namespace MKService.ModelUpdaters
{
    /// <summary>
    /// Base class for all model updaters.
    /// </summary>
    /// <typeparam name="TModel">The model type.</typeparam>
    /// <typeparam name="TMessage">The command message type.</typeparam>
    internal abstract class ModelUpdaterBase<TModel, TMessage> : IModelUpdater<TModel, TMessage>
        where TModel : IUpdatable where TMessage : IMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModelUpdaterBase{TModel, TMessage}" /> class.
        /// </summary>
        /// <param name="modelFactoryResolver">The model factory resolver.</param>
        protected ModelUpdaterBase(IModelFactoryResolver modelFactoryResolver,
            IDefaultModel defaultModel)
        {
            this.ModelFactoryResolver = modelFactoryResolver;
            this.DefaultModel = defaultModel;
            this.Log = LogManager.GetLogger(this.GetType());
        }
        public IDefaultModel DefaultModel { get; }

        /// <summary>
        /// Gets the log.
        /// </summary>
        /// <value>
        /// The log.
        /// </value>
        protected ILog Log { get; }

        /// <summary>
        /// Gets the model factory resolver.
        /// </summary>
        /// <value>The model factory resolver.</value>
        protected IModelFactoryResolver ModelFactoryResolver { get; }

        /// <summary>
        /// Determine if the updater can update the specified model type with the specified command message type.
        /// </summary>
        /// <param name="modelType">The model type.</param>
        /// <param name="messageType">The command message type.</param>
        /// <returns>
        /// <c>true</c>, if the updater can the specified model type with the specified command message type; otherwise,
        /// <c>false</c>.
        /// </returns>
        public bool CanUpdate(Type modelType, Type messageType)
        {
            return typeof(TModel) == modelType && typeof(TMessage) == messageType;
        }

        /// <summary>
        /// Updates the specified model with the specified command message.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="message">The command message.</param>
        /// <returns>
        /// <c>true</c>, if any updates were actually made to the specified model; <c>false</c>, if no changes were
        /// actually made.
        /// </returns>
        public bool Update(TModel model, TMessage message)
        {
            this.Log.DebugFormat(
                "Updating model of type '{0}' with message of type '{1}'.",
                typeof(TModel).FullName,
                typeof(TMessage).FullName);

            Debug.WriteLine($"Updating '{model.GetType().Name}' with message '{message.GetType().Name}'.");

            return this.UpdateInternal(model, message);
        }

        /// <summary>
        /// Updates the specified model with the specified command message.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="message">The command message.</param>
        /// <returns>
        /// <c>true</c>, if any updates were actually made to the specified model; <c>false</c>, if no changes were
        /// actually made.
        /// </returns>
        public bool Update(IUpdatable model, IMessage message)
        {
            return this.Update((TModel)model, (TMessage)message);
        }

        /// <summary>
        /// Called by the base class when an update should take place. The specified model and command message are
        /// guaranteed to not be <c>null</c> when this method is called.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="message">The command message.</param>
        /// <returns>
        /// <c>true</c>, if any updates were actually made to the specified model; <c>false</c>, if no changes were
        /// actually made.
        /// </returns>
        protected abstract bool UpdateInternal(TModel model, TMessage message);
    }
}