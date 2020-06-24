

using System.Globalization;
using System.Net;

namespace ReDefNet
{
    /// <summary>
    /// Extension methods for working with <see cref="IPEndPoint" />.
    /// </summary>
    public static class IPEndPointExtensions
    {
        /// <summary>
        /// Attempts to parse an <see cref="IPEndPoint" /> from the string.
        /// </summary>
        /// <param name="value">The string value.</param>
        /// <param name="endpoint">The parsed endpoint.</param>
        /// <param name="defaultPort">The default port to use if no port is specified. If <c>null</c>, no port will be specified.</param>
        /// <returns>
        /// <c>true</c>, if the string was parsed successfully; otherwise, <c>false</c>.
        /// </returns>
        public static bool TryParseIPEndPoint(this string value, out IPEndPoint endpoint, int? defaultPort = null)
        {
            if (string.IsNullOrEmpty(value))
            {
                endpoint = null;

                return false;
            }

            var pieces = value.Split(':');

            IPAddress address;
            if (!IPAddress.TryParse(pieces[0], out address))
            {
                endpoint = null;

                return false;
            }

            if (pieces.Length == 1)
            {
                endpoint = defaultPort.HasValue
                    ? new IPEndPoint(address, defaultPort.Value)
                    : new IPEndPoint(address, 0);

                return true;
            }

            int port;
            if (!int.TryParse(pieces[1], NumberStyles.None, NumberFormatInfo.CurrentInfo, out port))
            {
                endpoint = null;

                return false;
            }

            endpoint = new IPEndPoint(address, port);

            return true;
        }
    }
}