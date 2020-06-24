

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;

namespace ReDefNet
{
    /// <summary>
    /// A serializable dynamic data collection that provides notifications when items get added, removed, or when the entire
    /// collection is refreshed. Essentially a wrapper around the built-in <see cref="ObservableCollection{T}" /> type, but
    /// that allows serialization. If the type given does not implement <see cref="INotifyCollectionChanged" />, this event
    /// will not work.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    [CollectionDataContract(Name = "SerializableObservableCollectionOf{0}", Namespace = "global")]
    public sealed class SerializableObservableCollection<T> : Collection<T>, IObservableCollection<T>
    {
        /// <summary>
        /// The count property name.
        /// </summary>
        private const string CountPropertyName = "Count";

        /// <summary>
        /// The name of the indexer property.
        /// </summary>
        private const string IndexerPropertyName = "Item[]";

        /// <summary>
        /// The name of the underlying observable collection in the <see cref="SerializationInfo" />.
        /// </summary>
        private const string SerializationCollectionName = "ObservableCollection";

        /// <summary>
        /// The monitor used to prevent reentrant calls to the collection.
        /// </summary>
        private SimpleMonitor monitor = new SimpleMonitor();

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializableObservableCollection{T}" /> class.
        /// </summary>
        public SerializableObservableCollection()
        {
            this.CanRaisePropertyChanged = typeof(T).GetInterfaces().Contains(typeof(INotifyPropertyChanged));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializableObservableCollection{T}" /> class.
        /// </summary>
        /// <param name="initialItems">The initial items.</param>
        public SerializableObservableCollection(IEnumerable<T> initialItems)
            : this()
        {
            initialItems.ValidateNotNull(nameof(initialItems));

            foreach (var initialItem in initialItems)
            {
                if (this.CanRaisePropertyChanged)
                {
                    ((INotifyPropertyChanged)initialItem).PropertyChanged += this.OnItemPropertyChanged;
                }

                this.Items.Add(initialItem);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializableObservableCollection{T}" /> class. This special constructor is
        /// used for deserialization only.
        /// </summary>
        /// <param name="info">The serialization information.</param>
        /// <param name="context">The serialization streaming context.</param>
        public SerializableObservableCollection(SerializationInfo info, StreamingContext context)
        {
            var deserializedCollection = (T[])info.GetValue(SerializationCollectionName, typeof(T[]));

            if (deserializedCollection == null)
            {
                return;
            }

            foreach (var deserializedItem in deserializedCollection)
            {
                if (this.CanRaisePropertyChanged)
                {
                    ((INotifyPropertyChanged)deserializedItem).PropertyChanged += this.OnItemPropertyChanged;
                }

                this.Items.Add(deserializedItem);
            }
        }

        /// <summary>
        /// Occurs when an item is added to the collection.
        /// </summary>
        public event EventHandler<NotifyCollectionChangedEventArgs> Added;

        /// <summary>
        /// Occurs when the collection changes. To only listen for more specific collection changes, use the <see cref="Added" />,
        /// <see cref="Removed" />, <see cref="Replaced" />, <see cref="Moved" />, or <see cref="Reset" /> events.
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <summary>
        /// Occurs when an item changed in the collection has changed. If the type given does not implement
        /// <see cref="INotifyCollectionChanged" />, this event will not work.
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs> ItemChanged;

        /// <summary>
        /// Occurs when items are moved in the collection.
        /// </summary>
        public event EventHandler<NotifyCollectionChangedEventArgs> Moved;

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Occurs when an item is removed from the collection.
        /// </summary>
        public event EventHandler<NotifyCollectionChangedEventArgs> Removed;

        /// <summary>
        /// Occurs when an item is replaced in the collection.
        /// </summary>
        public event EventHandler<NotifyCollectionChangedEventArgs> Replaced;

        /// <summary>
        /// Occurs when the collection is reset/cleared.
        /// </summary>
        public event EventHandler<NotifyCollectionChangedEventArgs> Reset;

        /// <summary>
        /// Gets a value indicating whether the collection is read-only.
        /// </summary>
        /// <value>
        /// <c>true</c>, if the collection is read-only; otherwise, <c>false</c>.
        /// </value>
        public bool IsReadOnly => false;

        /// <summary>
        /// Gets or sets a value indicating whether collection changed-related notification events should be suppressed.
        /// </summary>
        /// <value>
        /// <c>true</c>, if collection changed-related notification events should be suppressed; otherwise, <c>false</c>.
        /// </value>
        public bool SuppressNotifications { get; set; }

        /// <summary>
        /// Gets the number of elements in the collection.
        /// </summary>
        /// <value>
        /// The number of elements in the collection.
        /// </value>
        int IObservableCollection<T>.Count => this.Items.Count;

        /// <summary>
        /// Gets the number of elements in the collection.
        /// </summary>
        /// <value>
        /// The number of elements in the collection.
        /// </value>
        int ICollection<T>.Count => this.Items.Count;

        /// <summary>
        /// Gets the number of elements in the collection.
        /// </summary>
        /// <value>
        /// The number of elements in the collection.
        /// </value>
        int IReadOnlyCollection<T>.Count => this.Items.Count;

        /// <summary>
        /// Gets a value indicating whether the collection is read-only.
        /// </summary>
        /// <value>
        /// <c>true</c>, if the collection is read-only; otherwise, <c>false</c>.
        /// </value>
        bool IReadOnlyObservableCollection<T>.IsReadOnly => true;

        /// <summary>
        /// Gets a value indicating whether the types held can raise property changed.
        /// </summary>
        /// <value>
        /// <c>true</c> if the types held can raise property changed; otherwise, <c>false</c>.
        /// </value>
        private bool CanRaisePropertyChanged { get; }

        /// <summary>
        /// Gets or sets the item at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>
        /// The item at the specified index.
        /// </returns>
        public new T this[int index]
        {
            get
            {
                return this.Items[index];
            }

            set
            {
                if (this.Items[index] == null && value == null)
                {
                    return;
                }

                // ReSharper disable once RedundantNameQualifier
                if (object.ReferenceEquals(this.Items[index], value))
                {
                    return;
                }

                this.SetItem(index, value);
            }
        }

        /// <summary>
        /// Adds the specified item to the collection.
        /// </summary>
        /// <param name="item">The object to add.</param>
        public new void Add(T item)
        {
            item.ValidateNotNull(nameof(item));

            this.InsertItem(this.Items.Count, item);
        }

        /// <summary>
        /// Adds the specified range of items to the collection.
        /// </summary>
        /// <param name="items">The items.</param>
        public void AddRange(IEnumerable<T> items)
        {
            items.ValidateNotNull(nameof(items));

            foreach (var item in items)
            {
                this.Add(item);
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            foreach (var item in this.Items)
            {
                if (this.CanRaisePropertyChanged)
                {
                    ((INotifyPropertyChanged)item).PropertyChanged -= this.OnItemPropertyChanged;
                }
            }

            this.Items.Clear();

            if (this.monitor == null)
            {
                return;
            }

            this.monitor.Dispose();
            this.monitor = null;
        }

        /// <summary>
        /// Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo" /> with the data needed to serialize the
        /// target object.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> to populate with data.</param>
        /// <param name="context">
        /// The destination (see <see cref="T:System.Runtime.Serialization.StreamingContext" />) for this
        /// serialization.
        /// </param>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            var serializedCollection = this.Items.ToArray();

            info.AddValue(SerializationCollectionName, serializedCollection, typeof(T[]));
        }

        /// <summary>
        /// Inserts an item in the collection at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index.</param>
        /// <param name="item">The object to insert.</param>
        public new void Insert(int index, T item)
        {
            item.ValidateNotNull(nameof(item));

            this.InsertItem(index, item);
        }

        /// <summary>
        /// Moves the item at the specified index to a new location in the collection.
        /// </summary>
        /// <param name="oldIndex">The old index.</param>
        /// <param name="newIndex">The new index.</param>
        public void Move(int oldIndex, int newIndex)
        {
            this.MoveItem(oldIndex, newIndex);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the collection.
        /// </summary>
        /// <param name="item">The object to remove.</param>
        /// <returns>
        /// <c>true</c>, if <paramref name="item" /> was successfully removed from the collection; otherwise, <c>false</c>. This
        /// method also returns <c>false</c> if <paramref name="item" /> is not found in the collection.
        /// </returns>
        public new bool Remove(T item)
        {
            item.ValidateNotNull(nameof(item));

            var index = this.Items.IndexOf(item);

            if (index < 0)
            {
                return false;
            }

            this.RemoveItem(index);

            return true;
        }

        /// <summary>
        /// Clears the items.
        /// </summary>
        protected override void ClearItems()
        {
            this.CheckForReentrancy();

            foreach (var item in this.Items)
            {
                if (this.CanRaisePropertyChanged)
                {
                    ((INotifyPropertyChanged)item).PropertyChanged -= this.OnItemPropertyChanged;
                }
            }

            this.Items.Clear();

            if (this.SuppressNotifications)
            {
                return;
            }

            this.RaisePropertyChanged(CountPropertyName);
            this.RaisePropertyChanged(IndexerPropertyName);

            this.RaiseEvent(this.Reset, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <summary>
        /// Inserts the specified item at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="item">The item.</param>
        protected override void InsertItem(int index, T item)
        {
            this.CheckForReentrancy();
            if (this.CanRaisePropertyChanged)
            {
                ((INotifyPropertyChanged)item).PropertyChanged += this.OnItemPropertyChanged;
            }

            this.Items.Insert(index, item);

            if (this.SuppressNotifications)
            {
                return;
            }

            this.RaisePropertyChanged(CountPropertyName);
            this.RaisePropertyChanged(IndexerPropertyName);

            this.RaiseEvent(
                this.Added,
                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
        }

        /// <summary>
        /// Removes the item at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        protected override void RemoveItem(int index)
        {
            this.CheckForReentrancy();

            var removedItem = this.Items[index];
            if (this.CanRaisePropertyChanged)
            {
                ((INotifyPropertyChanged)removedItem).PropertyChanged -= this.OnItemPropertyChanged;
            }

            this.Items.RemoveAt(index);

            if (this.SuppressNotifications)
            {
                return;
            }

            this.RaisePropertyChanged(CountPropertyName);
            this.RaisePropertyChanged(IndexerPropertyName);

            this.RaiseEvent(
                this.Removed,
                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, removedItem, index));
        }

        /// <summary>
        /// Sets the specified item at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="item">The item.</param>
        protected override void SetItem(int index, T item)
        {
            this.CheckForReentrancy();

            var originalItem = this.Items[index];

            if (originalItem != null && this.CanRaisePropertyChanged)
            {
                ((INotifyPropertyChanged)originalItem).PropertyChanged -= this.OnItemPropertyChanged;
            }

            this.Items[index] = item;

            if (item != null && this.CanRaisePropertyChanged)
            {
                ((INotifyPropertyChanged)item).PropertyChanged += this.OnItemPropertyChanged;
            }

            if (this.SuppressNotifications)
            {
                return;
            }

            this.RaisePropertyChanged(IndexerPropertyName);

            this.RaiseEvent(
                this.Replaced,
                new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Replace,
                    originalItem,
                    item,
                    index));
        }

        /// <summary>
        /// Blocks reentrant calls for the life of the return value. This method is designed to be used in a using statement.
        /// </summary>
        /// <returns>
        /// The monitor.
        /// </returns>
        private IDisposable BlockReentrancy()
        {
            this.monitor.Enter();

            return this.monitor;
        }

        /// <summary>
        /// Checks and asserts for reentrant attempts to change the collection. A reentrant call is when the collection is updated
        /// while it is still notifying listeners of a previous update.
        /// </summary>
        private void CheckForReentrancy()
        {
            if (!this.monitor.IsBusy)
            {
                return;
            }

            //// We can allow changes if there's only one listener. The problem only arises if reentrant changes make the original event
            //// arguments invalid for later listeners.

            if ((this.Added != null && this.Added.GetInvocationList().Length > 1) ||
                (this.CollectionChanged != null && this.CollectionChanged.GetInvocationList().Length > 1) ||
                (this.Moved != null && this.Moved.GetInvocationList().Length > 1) ||
                (this.Removed != null && this.Removed.GetInvocationList().Length > 1) ||
                (this.Replaced != null && this.Replaced.GetInvocationList().Length > 1) ||
                (this.Reset != null && this.Reset.GetInvocationList().Length > 1))
            {
                throw new InvalidOperationException(
                    "Cannot change SerializableObservableCollection during an Added, CollectionChanged, " +
                    "Moved, Removed, Replaced, Reset event.");
            }
        }

        /// <summary>
        /// Moves the item at the specified old index to the specified new index.
        /// </summary>
        /// <param name="oldIndex">The old index.</param>
        /// <param name="newIndex">The new index.</param>
        private void MoveItem(int oldIndex, int newIndex)
        {
            this.CheckForReentrancy();

            var removedItem = this.Items[oldIndex];

            this.Items.RemoveAt(oldIndex);
            this.Items.Insert(newIndex, removedItem);

            if (this.SuppressNotifications)
            {
                return;
            }

            this.RaisePropertyChanged(IndexerPropertyName);

            this.RaiseEvent(
                this.Moved,
                new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Move,
                    removedItem,
                    newIndex,
                    oldIndex));
        }

        /// <summary>
        /// Called immediately after deserialization by the .NET Framework.
        /// </summary>
        /// <param name="context">The serialization streaming context.</param>
        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            // Since the constructor of a type isn't called during deserialization, this method acts as the constructor
            // during deserialization and ensures the class is initialized correctly.
            if (this.monitor == null)
            {
                this.monitor = new SimpleMonitor();
            }

            foreach (var item in this.Items)
            {
                if (this.CanRaisePropertyChanged)
                {
                    ((INotifyPropertyChanged)item).PropertyChanged += this.OnItemPropertyChanged;
                }
            }
        }

        /// <summary>
        /// Called when a property changes on an item in the collection.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="PropertyChangedEventArgs" /> instance containing the event data.</param>
        private void OnItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.ItemChanged.RaiseEvent(sender, e);
        }

        /// <summary>
        /// Raises the specified specialized event along with the general <see cref="CollectionChanged" /> event.
        /// </summary>
        /// <param name="eventHandler">The event handler.</param>
        /// <param name="e">The <see cref="NotifyCollectionChangedEventArgs" /> instance containing the event data.</param>
        private void RaiseEvent(
            EventHandler<NotifyCollectionChangedEventArgs> eventHandler,
            NotifyCollectionChangedEventArgs e)
        {
            var collectionChangedHandler = this.CollectionChanged;

            if (collectionChangedHandler != null)
            {
                using (this.BlockReentrancy())
                {
                    collectionChangedHandler.Invoke(this, e);
                }
            }

            var specializedHandler = eventHandler;

            if (specializedHandler == null)
            {
                return;
            }

            using (this.BlockReentrancy())
            {
                specializedHandler.Invoke(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanged" /> event for the specified property.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        private void RaisePropertyChanged(string propertyName)
        {
            var handler = this.PropertyChanged;

            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// A simple monitor that helps prevent reentrant calls to the collection.
        /// </summary>
        private sealed class SimpleMonitor : IDisposable
        {
            /// <summary>
            /// The number of times <see cref="Enter" /> was called.
            /// </summary>
            private int busyCount;

            /// <summary>
            /// Gets a value indicating whether the monitor is busy.
            /// </summary>
            /// <value>
            /// <c>true</c>, if the monitor is busy; otherwise, <c>false</c>.
            /// </value>
            public bool IsBusy => this.busyCount > 0;

            /// <summary>
            /// Signals that the monitor is no longer busy.
            /// </summary>
            public void Dispose()
            {
                --this.busyCount;
            }

            /// <summary>
            /// Signals that the monitor is busy.
            /// </summary>
            public void Enter()
            {
                ++this.busyCount;
            }
        }
    }
}