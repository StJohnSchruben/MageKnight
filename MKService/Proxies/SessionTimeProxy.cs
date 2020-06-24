
using System;
using log4net;
using MKService.Messages;
using MKService.ModelUpdaters;
using MKService.Updates;
using MKModel;

namespace MKService.Proxies
{


    /// <summary>
    /// The Session Time Proxy.
    /// </summary>
    /// <seealso cref="MKService.Proxies.ProxyBase" />
    /// <seealso cref="MKService.Model.ISessionTime" />
    internal class SessionTimeProxy : ProxyBase, ISessionTime
    {
        /// <summary>
        /// The log.
        /// </summary>
        private readonly ILog log = LogManager.GetLogger(nameof(SessionTimeProxy));

        /// <summary>
        /// The model.
        /// </summary>
        private readonly IUpdatableSessionTime model;

        /// <summary>
        /// The timer.
        /// </summary>
        //private ITimer timer;

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionTimeProxy" /> class.
        /// </summary>
        /// <param name="serviceClient">The service client.</param>
        /// <param name="modelUpdaterResolver">The model updater resolver.</param>
        /// <param name="model">The model.</param>
        /// <param name="inComMode">If set to <c>true</c> [in COM mode].</param>
        public SessionTimeProxy(
            IServiceClient serviceClient,
            IModelUpdaterResolver modelUpdaterResolver,
            IUpdatableSessionTime model,
            bool inComMode)
            : base(serviceClient, modelUpdaterResolver)
        {
            this.model = model;

            this.SetUpModelPropertyChangedPropagation(this.model);

            if (!inComMode)
            {
                //this.timer = new SystemTimer { Interval = 1000 };
                //this.timer.Elapsed += this.Timer_Elapsed;
                //this.timer.Start();
            }

            this.SubscribeToMessage<SessionTimeChanged>(this.Handle);
        }

        /// <summary>
        /// Gets the session time.
        /// </summary>
        /// <value>The session time.</value>
        public DateTime SessionTime => this.model.SessionTime;

        /// <summary>
        /// Changes the session time.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="month">The month.</param>
        /// <param name="day">The day.</param>
        /// <param name="hour">The hour.</param>
        /// <param name="minute">The minute.</param>
        /// <param name="second">The second.</param>
        public void ChangeSessionTime(short year, short month, short day, short hour, short minute, short second)
        {
            ////this.log.Debug($"SessionTimeProxy ASDASDASD(ChangeSessionTime)");
            ////this.log.Debug($"Year: {year} Month: {month} Day: {day} Hour: {hour} Minute: {minute} Second: {second}");

            var message = new SessionTimeChanged
            {
                NewSessionTime = new DateTime(year, month, day, hour, minute, second)
            };

            var changed = this.ModelUpdaterResolver.GetUpdater<IUpdatableSessionTime, SessionTimeChanged>().Update(this.model, message);

            if (changed)
            {
                this.ServiceClient.PublishAsync(message);
            }
        }

        /// <summary>
        /// Handles the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        private void Handle(SessionTimeChanged message)
        {
            this.ModelUpdaterResolver.GetUpdater<IUpdatableSessionTime, SessionTimeChanged>().Update(this.model, message);
        }

        /// <summary>
        /// Handles the Elapsed event of the Timer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Timers.ElapsedEventArgs" /> instance containing the event data.</param>
        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            ////this.log.Debug($"TIMER ELAPSED ASDASDASD");
            this.model.SessionTime = DateTime.Now;
        }
    }
}