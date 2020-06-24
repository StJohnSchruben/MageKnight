

using System;

namespace ReDefNet.Sys
{
    /// <summary>
    /// Represents a source of date and time.
    /// </summary>
    public interface IClock
    {
        /// <summary>
        /// Gets the current date and time.
        /// </summary>
        /// <value>
        /// The date and time.
        /// </value>
        DateTime Now { get; }
    }
}