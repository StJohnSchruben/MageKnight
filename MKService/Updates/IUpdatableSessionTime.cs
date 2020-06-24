
using System;
using Service;

using MKModel;

namespace MKService.Updates
{
    /// <summary>
    /// The Interface for the Updatable Session Time Model.
    /// </summary>
    /// <seealso cref="MKService.Updates.IUpdatable" />
    /// <seealso cref="MKService.Model.ISessionTime" />
    internal interface IUpdatableSessionTime : IUpdatable, ISessionTime, IQueryResponse
    {
        /// <summary>
        /// Gets or sets the session time.
        /// </summary>
        /// <value>
        /// The session time.
        /// </value>
        new DateTime SessionTime { get; set; }
    }
}