//-----------------------------------------------------------------------------
// <copyright company="Long Wave Inc.">
//   Copyright (c) 2018 Long Wave Inc. All rights reserved.
// </copyright>
// <license>
// PMA-271 Distribution Statement D
//   Distribution authorized to DoD and DoD Contractors only based on Administrative/Operational
//   Use as determined on 15 March 2001.  Other requests shall be referred to the Dept. of the Navy,
//   Program Executive Office for Air ASW, Assault and Special Mission Programs (PEO(A)).
//
// ITAR WARNING
//   This document contains technical data whose export is restricted by the Arms Export Control Act
//   (Title 22, U.S.C., Section 2751 et seq.) or the Export Administration Act of 1979, as amended,
//   Title 50, U.S.C., App. 2401 et seq.  Violations of these export laws are subject to severe
//   criminal penalties. Disseminate in accordance with provisions of DoD Directive 5230.25.
// </license>
//-----------------------------------------------------------------------------
// -------------------------------------------------------------------------------
// <copyright company="Long Wave Inc.">
//   Copyright (c) 2018 Long Wave Inc. All rights reserved.
// </copyright>
// <license>
// PMA-271 Distribution Statement D
//   Distribution authorized to DoD and DoD Contractors only based on Administrative/Operational
//   Use as determined on 15 March 2001.  Other requests shall be referred to the Dept. of the Navy,
//   Program Executive Office for Air ASW, Assault and Special Mission Programs (PEO(A)).
//
// ITAR WARNING
//   This document contains technical data whose export is restricted by the Arms Export Control Act
//   (Title 22, U.S.C., Section 2751 et seq.) or the Export Administration Act of 1979, as amended,
//   Title 50, U.S.C., App. 2401 et seq. Violations of these export laws are subject to severe
//   criminal penalties. Disseminate in accordance with provisions of DoD Directive 5230.25.
// </license>
// -------------------------------------------------------------------------------

using System;
using System.ServiceModel;

namespace ServerHost
{
    /// <summary>
    /// Represents a hosting server for the services infrastructure.
    /// </summary>
    public interface IServer : IDisposable
    {
        /// <summary>
        /// Occurs when the server is closed.
        /// </summary>
        event EventHandler Closed;

        /// <summary>
        /// Occurs when the server is faulted.
        /// </summary>
        event EventHandler Faulted;

        /// <summary>
        /// Occurs when the server is opened.
        /// </summary>
        event EventHandler Opened;

        /// <summary>
        /// Gets a value indicating whether the server is open.
        /// </summary>
        /// <value><c>true</c>, if the server is open; otherwise, <c>false</c>.</value>
        bool IsOpen { get; }

        /// <summary>
        /// Gets the current server state.
        /// </summary>
        /// <value>The server state.</value>
        CommunicationState State { get; }

        /// <summary>
        /// Closes the server.
        /// </summary>
        void Close();

        /// <summary>
        /// Opens the server.
        /// </summary>
        void Open();
    }
}