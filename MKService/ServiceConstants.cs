using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MKService
{
    public static class ServiceConstants
    {
        /// <summary>
        /// The message namespace.
        /// </summary>
        public const string MessageNamespace = "MKService.Messages";

        /// <summary>
        /// The query namespace.
        /// </summary>
        public const string QueryNamespace = "MKService.Queries";

        /// <summary>
        /// The query method invocation exception.
        /// </summary>
        internal const string QueryMethodInvocationException =
        "This method should be called on the proxy, not the query or query component.";
    }
}
