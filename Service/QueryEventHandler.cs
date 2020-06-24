

using System.Runtime.InteropServices;

namespace Service
{
    /// <summary>
    /// Delegate handler of a query event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="QueryArgs" /> instance containing the event data.</param>
    [ComVisible(true)]
    public delegate void QueryEventHandler(object sender, QueryArgs e);
}