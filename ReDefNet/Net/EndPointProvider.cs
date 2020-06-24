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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using ReDefNet.Net;

namespace ReDefNet.Net
{
    /// <summary>
    /// EndPoint provider.
    /// </summary>
    public class EndPointProvider : IEndPointProvider
    {
        /// <summary>
        /// The saved address.
        /// </summary>
        IPAddress savedAddress;

        /// <summary>
        /// The starting port.
        /// </summary>
        int startingPort;

        /// <summary>
        /// The ending port.
        /// </summary>
        int endingPort;

        /// <summary>
        /// The master port.
        /// </summary>
        int masterPort;

        /// <summary>
        /// The end point collection.
        /// </summary>
        List<IPEndPoint> endPointCollection;

        /// <summary>
        /// Initializes a new instance of the <see cref="EndPointProvider"/> class.
        /// </summary>
        public EndPointProvider() : this(new IPAddress(new byte[] { 192, 168, 17, 95 }))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EndPointProvider"/> class.
        /// </summary>
        /// <param name="address">The address.</param>
        public EndPointProvider(IPAddress address, int startPort = -1, int endPort = -1)
        {
            this.endPointCollection = new List<IPEndPoint>();
            address.ValidateNotNull(nameof(address));
            this.savedAddress = address;
            this.startingPort = startPort == -1 ? 35000 : startPort;
            this.endingPort = endPort == -1 ? 35500 : endingPort;
        }

        /// <summary>
        /// Gets the ip address.
        /// </summary>
        /// <returns>Returns the IpAddress.</returns>
        private IPAddress GetIPAddress()
        {
            var address = IPAddress.Parse("127.0.0.1");

            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ipAddress in host.AddressList)
            {
                if (ipAddress.AddressFamily != AddressFamily.InterNetwork)
                {
                    continue;
                }

                return ipAddress;
            }

            return null;
        }

        /// <summary>
        /// Ports this instance.
        /// </summary>
        /// <returns>The correct Port Value.</returns>
        private int SetPort()
        {
            int portValue = 35000;
            int maxPortValue = 35500;

            IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] tcpEndpoints = properties.GetActiveTcpListeners();

            List<int> usedPorts = tcpEndpoints.Select(x => x.Port).ToList<int>();

            if (!this.endPointCollection.Count.Equals(0))
            {
                var trialValue = this.endPointCollection.Max(x => x.Port) + 1;
                if (trialValue > portValue)
                {
                    portValue = trialValue;
                }
            }

            for (int port = portValue; port < maxPortValue; port++)
            {
                if (!usedPorts.Contains(port))
                {
                    portValue = port;
                    break;
                }
            }

            return portValue;
        }

        /// <summary>
        /// Ports this instance.
        /// </summary>
        /// <returns>The correct Port Value.</returns>
        private int SetPort(int value)
        {
            int interval = 100;
            int portValue = 35000 + (interval * value);
            int maxPortValue = 35000 + (interval * value) + 99;

            IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] tcpEndpoints = properties.GetActiveTcpListeners();

            List<int> usedPorts = tcpEndpoints.Select(x => x.Port).ToList<int>();

            if (!this.endPointCollection.Count.Equals(0))
            {
                var trialValue = this.endPointCollection.Max(x => x.Port) + 1;
                if (trialValue > portValue)
                {
                    portValue = trialValue;
                }
            }

            for (int port = portValue; port < maxPortValue; port++)
            {
                if (!usedPorts.Contains(port))
                {
                    portValue = port;
                    break;
                }
            }

            return portValue;
        }

        /// <summary>
        /// Gets the ip end point.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <returns> The Ip Endpoint.</returns>
        public IPEndPoint GetIPEndPoint()
        {
            IPEndPoint end = new IPEndPoint(this.savedAddress, SetPort());
            this.endPointCollection.Add(end);
            return end;
        }

        /// <summary>
        /// Gets the ip end point.
        /// </summary>
        /// <param name="port">The port.</param>
        /// <returns>The IPEndpoint.</returns>
        public IPEndPoint GetIPEndPoint(int port)
        {
            var result = port % 9000;
            IPEndPoint end = new IPEndPoint(this.savedAddress, SetPort(result));
            this.endPointCollection.Add(end);
            return end;
        }
    }
}
