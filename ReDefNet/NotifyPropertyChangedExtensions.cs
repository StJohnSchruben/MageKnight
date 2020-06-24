

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;

namespace ReDefNet
{
    /// <summary>
    /// Extension methods for working with types that implement <see cref="INotifyPropertyChanged" />.
    /// </summary>
    public static class NotifyPropertyChangedExtensions
    {
        /// <summary>
        /// Subscribes to property changes for the property referred to by the specified property expression.
        /// </summary>
        /// <typeparam name="T">The type containing the property.</typeparam>
        /// <typeparam name="TProperty">The property.</typeparam>
        /// <param name="observable">The observable object.</param>
        /// <param name="propertyExpression">The property expression.</param>
        /// <param name="handler">The property changed handler.</param>
        [DebuggerStepThrough]
        public static void SubscribeToChanges<T, TProperty>(
            this T observable,
            Expression<Func<T, TProperty>> propertyExpression,
            PropertyChangedEventHandler handler)
            where T : INotifyPropertyChanged
        {
            if (observable == null)
            {
                throw new ArgumentNullException(nameof(observable));
            }

            if (propertyExpression == null)
            {
                throw new ArgumentNullException(nameof(propertyExpression));
            }

            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            var propertyName = observable.GetPropertyName(propertyExpression);

            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentException("Argument does not refer to a valid property.", nameof(propertyExpression));
            }

            observable.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == propertyName)
                {
                    handler.Invoke(observable, e);
                }
            };
        }

        /// <summary>
        /// Gets the name of the property referred to by the specified property expression.
        /// </summary>
        /// <typeparam name="TProperty">The property type.</typeparam>
        /// <param name="observable">The observable object.</param>
        /// <param name="propertyExpression">The property expression.</param>
        /// <returns>The property name.</returns>
        [DebuggerStepThrough]
        internal static string GetPropertyName<TProperty>(
            this INotifyPropertyChanged observable,
            Expression<Func<TProperty>> propertyExpression)
        {
            if (propertyExpression == null)
            {
                throw new ArgumentNullException(nameof(propertyExpression));
            }

            var memberExpression = propertyExpression.Body as MemberExpression;

            if (memberExpression == null)
            {
                throw new ArgumentException("Argument does not refer to a valid property.", nameof(propertyExpression));
            }

            var property = memberExpression.Member as PropertyInfo;

            if (property == null)
            {
                throw new ArgumentException("Argument does not refer to a valid property.", nameof(propertyExpression));
            }

            return property.Name;
        }

        /// <summary>
        /// Gets the name of the property referred to by the specified property expression.
        /// </summary>
        /// <typeparam name="T">The type containing the property.</typeparam>
        /// <typeparam name="TProperty">The property type.</typeparam>
        /// <param name="observable">The observable object.</param>
        /// <param name="propertyExpression">The property expression.</param>
        /// <returns>The property name.</returns>
        internal static string GetPropertyName<T, TProperty>(
            this T observable,
            Expression<Func<T, TProperty>> propertyExpression)
            where T : INotifyPropertyChanged
        {
            if (propertyExpression == null)
            {
                throw new ArgumentNullException(nameof(propertyExpression));
            }

            var memberExpression = propertyExpression.Body as MemberExpression;

            if (memberExpression == null)
            {
                throw new ArgumentException("Argument does not refer to a valid property.", nameof(propertyExpression));
            }

            var property = memberExpression.Member as PropertyInfo;

            if (property == null)
            {
                throw new ArgumentException("Argument does not refer to a valid property.", nameof(propertyExpression));
            }

            return property.Name;
        }
    }
}