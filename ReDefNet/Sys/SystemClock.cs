

using System;

namespace ReDefNet.Sys
{
    /// <summary>
    /// The system clock.
    /// </summary>
    public sealed class SystemClock : IClock
    {
        /// <summary>
        /// Gets the current date and time.
        /// </summary>
        /// <value>
        /// The date and time.
        /// </value>
        public DateTime Now => DateTime.Now;
    }
}