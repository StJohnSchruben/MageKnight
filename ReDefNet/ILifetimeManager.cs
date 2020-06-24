

using System;
using System.Threading.Tasks;

namespace ReDefNet
{
    /// <summary>
    /// Represents a type responsible for the asynchronous creation and destruction of an item instance or instances.
    /// </summary>
    public interface ILifetimeManager : IDisposable
    {
        /// <summary>
        /// Asynchronously destroys the instance(s).
        /// </summary>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        Task DestroyAsync();

        /// <summary>
        /// Asynchronously initializes the instance(s).
        /// </summary>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        Task InitializeAsync();
    }
}