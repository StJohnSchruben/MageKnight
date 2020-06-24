using MKService.QueryHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MKService
{
    public interface IServerServiceTypeProvider
    {
        /// <summary>
        /// Gets the query handler collection factory.
        /// </summary>
        /// <value>The query handler collection factory.</value>
        IQueryHandlerCollectionFactory QueryHandlerCollectionFactory { get; }
    }
}
