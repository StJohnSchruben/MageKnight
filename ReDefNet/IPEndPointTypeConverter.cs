

using System;
using System.ComponentModel;
using System.Globalization;
using System.Net;

namespace ReDefNet
{
    /// <summary>
    /// A type converter that supports converting from a string to an <see cref="IPEndPoint" /> and from an
    /// <see cref="IPEndPoint" /> to a string.
    /// </summary>
    public class IPEndPointTypeConverter : TypeConverter
    {
        /// <summary>
        /// Determines whether the converter can convert from the specified source type to an <see cref="IPEndPoint" /> using the
        /// specified context.
        /// </summary>
        /// <param name="context">The format context.</param>
        /// <param name="sourceType">The source type.</param>
        /// <returns>
        /// <c>true</c>, if the converter can convert from the specified source type to an <see cref="IPEndPoint" />; otherwise,
        /// <c>false</c>.
        /// </returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }

        /// <summary>
        /// Converts the specified object to an <see cref="IPEndPoint" /> using the specified context and culture information.
        /// </summary>
        /// <param name="context">The format context.</param>
        /// <param name="culture">The current culture information.</param>
        /// <param name="value">The value to convert.</param>
        /// <returns>
        /// An <see cref="IPEndPoint" /> that represents the converted value.
        /// </returns>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var str = value as string;

            if (str == null)
            {
                return base.ConvertFrom(context, culture, value);
            }

            if (string.IsNullOrWhiteSpace(str))
            {
                return null;
            }

            var pieces = str.Split(':');

            if (pieces.Length != 2)
            {
                throw new FormatException("Invalid IPEndPoint format.");
            }

            IPAddress address;
            if (!IPAddress.TryParse(pieces[0], out address))
            {
                throw new FormatException("Invalid IPEndPoint format.");
            }

            int port;
            if (!int.TryParse(pieces[1], NumberStyles.None, NumberFormatInfo.CurrentInfo, out port))
            {
                throw new FormatException("Invalid IPEndPoint format.");
            }

            return new IPEndPoint(address, port);
        }
    }
}