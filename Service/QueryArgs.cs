

using System;

namespace Service
{
    /// <summary>
    /// Event data for a query event.
    /// </summary>
    public class QueryArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the query callback.
        /// </summary>
        /// <value>
        /// The query callback.
        /// </value>
        public IQueryCallbackContract Callback { get; set; }

        /// <summary>
        /// Gets or sets the query definition.
        /// </summary>
        /// <value>
        /// The query definition.
        /// </value>
        public IQueryDef QueryDefinition { get; set; }

        /// <summary>
        /// Gets or sets the query type.
        /// </summary>
        /// <value>
        /// The query type.
        /// </value>
        public string QueryType { get; set; }
    }
}