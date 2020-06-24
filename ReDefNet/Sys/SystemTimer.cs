

using System.Timers;

namespace ReDefNet.Sys
{
    /// <summary>
    /// A component that generates recurring events.
    /// </summary>
    /// <remarks>A simple wrapper around <see cref="System.Timers.Timer" /> for testability.</remarks>
    public sealed class SystemTimer : ITimer
    {
        /// <summary>
        /// The underlying system timer.
        /// </summary>
        private Timer timer;

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemTimer" /> class.
        /// </summary>
        public SystemTimer()
        {
            this.timer = new Timer();
        }

        /// <summary>
        /// Occurs when the timer has elapsed.
        /// </summary>
        public event ElapsedEventHandler Elapsed
        {
            add
            {
                this.timer.Elapsed += value;
            }

            remove
            {
                this.timer.Elapsed -= value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the timer should raise the elapsed event each time after the
        /// configured interval elapses or only after the first time it elapses.
        /// </summary>
        /// <value>
        /// <c>true</c>, if it should continue to raise the elapsed event; <c>false</c>, to only raise it the first time.
        /// </value>
        public bool AutoReset
        {
            get
            {
                return this.timer.AutoReset;
            }

            set
            {
                this.timer.AutoReset = value;
            }
        }

        /// <summary>
        /// Gets or sets the interval, in milliseconds, at which to raise the elapsed event.
        /// </summary>
        /// <value>
        /// The interval, in milliseconds.
        /// </value>
        public double Interval
        {
            get
            {
                return this.timer.Interval;
            }

            set
            {
                this.timer.Interval = value;
            }
        }

        /// <summary>
        /// Releases all resources.
        /// </summary>
        public void Dispose()
        {
            if (this.timer == null)
            {
                return;
            }

            this.timer.Dispose();
            this.timer = null;
        }

        /// <summary>
        /// Starts the timer with the specified interval.
        /// </summary>
        public void Start()
        {
            this.timer.Start();
        }

        /// <summary>
        /// Stops the timer.
        /// </summary>
        public void Stop()
        {
            this.timer.Stop();
        }
    }
}