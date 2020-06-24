

using System.Collections.Generic;
using System.ComponentModel;

namespace ReDefNet
{
    /// <summary>
    /// Represents a dynamic data collection that provides notifications when items get added, removed, or when the entire
    /// collection is refreshed.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    public interface IObservableCollection<T> : IList<T>, IReadOnlyObservableCollection<T> 
    {
        /// <summary>
        /// Gets the number of elements in the collection.
        /// </summary>
        /// <value>
        /// The number of elements in the collection.
        /// </value>
        new int Count { get; }

        /// <summary>
        /// Gets a value indicating whether the collection is read-only.
        /// </summary>
        /// <value>
        /// <c>true</c>, if the collection is read-only; otherwise, <c>false</c>.
        /// </value>
        new bool IsReadOnly { get; }

        /// <summary>
        /// Adds the specified range of items to the collection.
        /// </summary>
        /// <param name="items">The items.</param>
        void AddRange(IEnumerable<T> items);

        /// <summary>
        /// Moves the item at the specified index to a new location in the collection.
        /// </summary>
        /// <param name="oldIndex">The old index.</param>
        /// <param name="newIndex">The new index.</param>
        void Move(int oldIndex, int newIndex);
    }
}