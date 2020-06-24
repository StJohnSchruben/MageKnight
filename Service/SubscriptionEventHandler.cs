

using System.Runtime.InteropServices;

namespace Service
{
    /// <summary>
    /// Delegate handler for a command message subscription event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="SubscribedEventArgs" /> instance containing the event data.</param>
    [ComVisible(true)]
    public delegate void SubscriptionEventHandler(object sender, SubscribedEventArgs e);
}