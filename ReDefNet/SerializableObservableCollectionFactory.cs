

using System.Collections.Generic;
using System.ComponentModel;

namespace ReDefNet
{
    /// <summary>
    /// A factory for creating instances of <see cref="IObservableCollection{T}" />.
    /// </summary>
    public class SerializableObservableCollectionFactory : IObservableCollectionFactory
    {
        /// <summary>
        /// Creates an empty instance of the <see cref="IObservableCollection{T}" /> object.
        /// </summary>
        /// <typeparam name="T">The type of object in the collection.</typeparam>
        /// <returns>
        /// Returns an empty instance of the <see cref="IObservableCollection{T}" /> object.
        /// </returns>
        public IObservableCollection<T> Create<T>() where T : class, INotifyPropertyChanged
        {
            return new SerializableObservableCollection<T>(new T[0]);
        }

        /// <summary>
        /// Creates an instance of the <see cref="IObservableCollection{T}" /> object filled with the given
        /// <paramref name="collection" />.
        /// </summary>
        /// <typeparam name="T">The type of object in the collection.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <returns>
        /// Returns an instance of the <see cref="IObservableCollection{T}" /> object filled with the given
        /// <paramref name="collection" />.
        /// </returns>
        public IObservableCollection<T> Create<T>(IEnumerable<T> collection) where T : class, INotifyPropertyChanged
        {
            return new SerializableObservableCollection<T>(collection);
        }
    }
}