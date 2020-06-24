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
using System.ServiceModel;


namespace Service.Contract
{
    /// <summary>
    /// Represents the service host instance for the <see cref="ICommandContract" />.
    /// </summary>
    /// <seealso cref="Service.ICommandContract" />
    /// <seealso cref="Service.CommandContractBase" />
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single)]
    public class CommandService : CommandContractBase, ICommandContract
    {
    }
}