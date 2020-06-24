

using System;
using System.Timers;

namespace ReDefNet.Sys
{
    /// <summary>
    /// Represents a component that generates recurring events.
    /// </summary>
    public interface ITimer : IDisposable
    {
        /// <summary>
        /// Occurs when the timer has elapsed.
        /// </summary>
        event ElapsedEventHandler Elapsed;

        /// <summary>
        /// Gets or sets a value indicating whether the timer should raise the elapsed event each time after the
        /// configured interval elapses or only after the first time it elapses.
        /// </summary>
        /// <value>
        /// <c>true</c>, if it should continue to raise the elapsed event; <c>false</c>, to only raise it the first time.
        /// </value>
        bool AutoReset { get; set; }

        /// <summary>
        /// Gets or sets the interval, in milliseconds, at which to raise the elapsed event.
        /// </summary>
        /// <value>
        /// The interval, in milliseconds.
        /// </value>
        double Interval { get; set; }

        /// <summary>
        /// Starts the timer with the specified interval.
        /// </summary>
        void Start();

        /// <summary>
        /// Stops the timer.
        /// </summary>
        void Stop();
    }
}