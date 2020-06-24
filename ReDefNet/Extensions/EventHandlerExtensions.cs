

using System;

namespace ReDefNet.Extensions
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
        [Obsolete("Use the extension methods in the root ReDefNet namespace instead.")]
        public static void RaiseEvent(this EventHandler e, object sender)
        {
            ReDefNet.EventHandlerExtensions.RaiseEvent(e, sender);
        }

        /// <summary>
        /// Raises the event.
        /// </summary>
        /// <typeparam name="TArgs">The event arguments type.</typeparam>
        /// <param name="e">The event.</param>
        /// <param name="sender">The event sender.</param>
        /// <param name="args">The event arguments.</param>
        [Obsolete("Use the extension methods in the root ReDefNet namespace instead.")]
        public static void RaiseEvent<TArgs>(this EventHandler<TArgs> e, object sender, TArgs args)
        {
            ReDefNet.EventHandlerExtensions.RaiseEvent(e, sender, args);
        }
    }
}