

using System.Runtime.InteropServices;

namespace Service
{
    /// <summary>
    /// Delegate handler of a command message publishing event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="PublishedEventArgs" /> instance containing the event data.</param>
    [ComVisible(true)]
    public delegate void PublishedEventHandler(object sender, PublishedEventArgs e);
}