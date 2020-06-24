

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace ReDefNet
{
    /// <summary>
    /// Represents a read-only dynamic data collection that provides notifications when items get added, removed, or when
    /// the entire collection is refreshed. If the type given does not implement <see cref="INotifyCollectionChanged" />,
    /// this event will not work.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    public interface IReadOnlyObservableCollection<out T>
        : IReadOnlyCollection<T>, INotifyCollectionChanged,
          INotifyPropertyChanged, IDisposable
    {
        /// <summary>
        /// Occurs when an item is added to the collection.
        /// </summary>
        event EventHandler<NotifyCollectionChangedEventArgs> Added;

        /// <summary>
        /// Occurs when the collection changes. To only listen for more specific collection changes, use the
        /// <see cref="Added" />, <see cref="Removed" />, <see cref="Replaced" />, <see cref="Moved" />, or
        /// <see cref="Reset" /> events.
        /// </summary>
        new event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <summary>
        /// Occurs when an item changed in the collection has changed. If the type given does not implement
        /// <see cref="INotifyCollectionChanged" />, this event will not work.
        /// </summary>
        event EventHandler<PropertyChangedEventArgs> ItemChanged;

        /// <summary>
        /// Occurs when items are moved in the collection.
        /// </summary>
        event EventHandler<NotifyCollectionChangedEventArgs> Moved;

        /// <summary>
        /// Occurs when an item is removed from the collection.
        /// </summary>
        event EventHandler<NotifyCollectionChangedEventArgs> Removed;

        /// <summary>
        /// Occurs when an item is replaced in the collection.
        /// </summary>
        event EventHandler<NotifyCollectionChangedEventArgs> Replaced;

        /// <summary>
        /// Occurs when the collection is reset/cleared.
        /// </summary>
        event EventHandler<NotifyCollectionChangedEventArgs> Reset;

        /// <summary>
        /// Gets a value indicating whether the collection is read-only.
        /// </summary>
        /// <value><c>true</c>, if the collection is read-only; otherwise, <c>false</c>.</value>
        bool IsReadOnly { get; }

        /// <summary>
        /// Gets or sets a value indicating whether collection changed-related notification events should be suppressed.
        /// </summary>
        /// <value>
        /// <c>true</c>, if collection changed-related notification events should be suppressed; otherwise, <c>false</c>.
        /// </value>
        bool SuppressNotifications { get; set; }
    }
}