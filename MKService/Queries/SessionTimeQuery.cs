
using System;
using System.ComponentModel;
using System.Runtime.Serialization;
//using GalaSoft.MvvmLight;
using MKService.Updates;
using ReDefNet;

namespace MKService.Queries
{
    /// <summary>
    /// The Updatable Model for Session Time.
    /// </summary>
    /// <seealso cref="Lwi.ObservableObject" />
    /// <seealso cref="MKService.Updates.IUpdatableSessionTime" />
    [DataContract(Namespace = ServiceConstants.QueryNamespace)]
    public class SessionTimeQuery : ObservableObject, IUpdatableSessionTime
    {
        /// <summary>
        /// The session time.
        /// </summary>
        [DataMember]
        private DateTime sessionTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionTimeQuery"/> class.
        /// </summary>
        public SessionTimeQuery()
        {
        }

        /// <summary>
        /// Gets or sets the session time.
        /// </summary>
        /// <value>
        /// The session time.
        /// </value>
        public DateTime SessionTime
        {
            get { return this.sessionTime; }
            set { this.Set(() => this.SessionTime, ref this.sessionTime, value); }
        }

        /// <summary>
        /// Changes the session time.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="month">The month.</param>
        /// <param name="day">The day.</param>
        /// <param name="hour">The hour.</param>
        /// <param name="minute">The minute.</param>
        /// <param name="second">The second.</param>
        /// <exception cref="System.InvalidOperationException">Query should not be used to call functions for model updates.</exception>
        public void ChangeSessionTime(short year, short month, short day, short hour, short minute, short second)
        {
            throw new InvalidOperationException(ServiceConstants.QueryMethodInvocationException);
        }
    }
}