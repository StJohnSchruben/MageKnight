

using System;

namespace ReDefNet
{
    /// <summary>
    /// Extension methods for working with event handlers.
    /// </summary>
    public static class EventHandlerExtensions
    {
        /// <summary>
        /// Raises the event.
        /// </summary>
        /// <param name="e">The event.</param>
        /// <param name="sender">The event sender.</param>
        public static void RaiseEvent(this EventHandler e, object sender)
        {
            var handler = e;

            handler?.Invoke(sender, EventArgs.Empty);
        }

        /// <summary>
        /// Raises the event.
        /// </summary>
        /// <typeparam name="TArgs">The event arguments type.</typeparam>
        /// <param name="e">The event.</param>
        /// <param name="sender">The event sender.</param>
        /// <param name="args">The event arguments.</param>
        public static void RaiseEvent<TArgs>(this EventHandler<TArgs> e, object sender, TArgs args)
        {
            var handler = e;

            handler?.Invoke(sender, args);
        }
    }
}