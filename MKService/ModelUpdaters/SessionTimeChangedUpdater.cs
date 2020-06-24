
using MKService.DefaultModel;
using MKService.Messages;
using MKService.ModelFactories;
using MKService.Updates;

namespace MKService.ModelUpdaters
{
    /// <summary>
    /// The updater for Session Time Changed Message.
    /// </summary>
    /// <seealso cref="MKService.ModelUpdaters.ModelUpdaterBase{MKService.Updates.IUpdatableSessionTime, MKService.Messages.SessionTimeChanged}" />
    internal class SessionTimeChangedUpdater : ModelUpdaterBase<IUpdatableSessionTime, SessionTimeChanged>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SessionTimeChangedUpdater" /> class.
        /// </summary>
        /// <param name="modelFactoryResolver">The model factory resolver.</param>
        public SessionTimeChangedUpdater(
            IModelFactoryResolver modelFactoryResolver, IDefaultModel defaultMode)
            : base(modelFactoryResolver, defaultMode)
        {
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
        protected override bool UpdateInternal(IUpdatableSessionTime model, SessionTimeChanged message)
        {
            if (model.SessionTime == message.NewSessionTime)
            {
                return false;
            }

            model.SessionTime = message.NewSessionTime;
            return true;
        }
    }
}