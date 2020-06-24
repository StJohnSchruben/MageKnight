

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ReDefNet
{
    /// <summary>
    /// Base class for an object that implements <see cref="INotifyPropertyChanged" />.
    /// </summary>
    [DebuggerStepThrough]
    [Serializable]
    public abstract class ObservableObject : INotifyPropertyChanged, INotifyPropertyChanging
    {
        /// <summary>
        /// Occurs when a property has changed.
        /// </summary>
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Occurs when a property value is changing.
        /// </summary>
        [field: NonSerialized]
        public event PropertyChangingEventHandler PropertyChanging;

        /// <summary>
        /// Raises the <see cref="PropertyChanged" /> event for the specified property.
        /// <para>
        /// If <paramref name="propertyName" /> does not refer to a valid property on the current object, an exception is
        /// thrown in DEBUG configuration only.
        /// </para>
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        public virtual void RaisePropertyChanged(string propertyName)
        {
            this.VerifyPropertyName(propertyName);

            var handler = this.PropertyChanged;

            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanging" /> event for the specified property.
        /// <para>
        /// If <paramref name="propertyName" /> does not refer to a valid property on the current object, an exception is
        /// thrown in DEBUG configuration only.
        /// </para>
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        public virtual void RaisePropertyChanging(string propertyName)
        {
            this.VerifyPropertyName(propertyName);

            var handler = this.PropertyChanging;

            handler?.Invoke(this, new PropertyChangingEventArgs(propertyName));
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanging" /> event for the specified property.
        /// </summary>
        /// <typeparam name="TProperty">The property type.</typeparam>
        /// <param name="propertyExpression">The property expression.</param>
        public virtual void RaisePropertyChanging<TProperty>(Expression<Func<TProperty>> propertyExpression)
        {
            if (propertyExpression == null)
            {
                throw new ArgumentNullException(nameof(propertyExpression));
            }

            var handler = this.PropertyChanging;

            if (handler == null)
            {
                return;
            }

            var propertyName = GetPropertyName(propertyExpression);

            handler.Invoke(this, new PropertyChangingEventArgs(propertyName));
        }

        /// <summary>
        /// Verifies that the specified property name exists on the current object. This method can be called before the
        /// property is used, for instance before calling <see cref="RaisePropertyChanged" />.
        /// <para>This method is only active in DEBUG configuration.</para>
        /// </summary>
        /// <param name="propertyName">The property name to verify.</param>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public void VerifyPropertyName(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                return;
            }

            var myType = this.GetType();

            if (myType.GetProperty(propertyName) == null)
            {
                return;
            }

            var descriptor = this as ICustomTypeDescriptor;

            if (descriptor != null)
            {
                if (descriptor.GetProperties()
                              .Cast<PropertyDescriptor>()
                              .Any(property => property.Name == propertyName))
                {
                    return;
                }
            }

            throw new ArgumentException("Property not found.", propertyName);
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanged" /> event for the property referred to by the specified property expression.
        /// </summary>
        /// <typeparam name="TProperty">The property type.</typeparam>
        /// <param name="propertyExpression">The property expression.</param>
        public virtual void RaisePropertyChanged<TProperty>(Expression<Func<TProperty>> propertyExpression)
        {
            if (propertyExpression == null)
            {
                throw new ArgumentNullException(nameof(propertyExpression));
            }

            var handler = this.PropertyChanged;

            if (handler == null)
            {
                return;
            }

            var propertyName = GetPropertyName(propertyExpression);

            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentException("Argument does not refer to a valid property.", nameof(propertyExpression));
            }

            handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Gets the name of the property referred to by the specified property expression.
        /// </summary>
        /// <typeparam name="TProperty">The property type.</typeparam>
        /// <param name="propertyExpression">The property expression.</param>
        /// <returns>
        /// The property name.
        /// </returns>
        /// <exception cref="ArgumentNullException">If <paramref name="propertyExpression" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">
        /// If <paramref name="propertyExpression" /> does not refer to a valid property.
        /// </exception>
        protected static string GetPropertyName<TProperty>(Expression<Func<TProperty>> propertyExpression)
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
        /// Sets the backing field for the specified property expression if the new value is different than the current value.
        /// </summary>
        /// <typeparam name="TProperty">The property and field type.</typeparam>
        /// <param name="propertyExpression">The property expression.</param>
        /// <param name="field">The field.</param>
        /// <param name="newValue">The new value.</param>
        /// <returns>
        /// <c>true</c>, if the backing field was set; otherwise, <c>false</c>.
        /// </returns>
        protected bool Set<TProperty>(Expression<Func<TProperty>> propertyExpression, ref TProperty field, TProperty newValue)
        {
            if (propertyExpression == null)
            {
                throw new ArgumentNullException(nameof(propertyExpression));
            }

            if (EqualityComparer<TProperty>.Default.Equals(field, newValue))
            {
                return false;
            }

            this.RaisePropertyChanging(propertyExpression);

            field = newValue;

            this.RaisePropertyChanged(propertyExpression);

            return true;
        }

        /// <summary>
        /// Sets the backing field for the specified property and raises the <see cref="PropertyChanging" /> and 
        /// <see cref="PropertyChanged" /> events if the new value is different than the current value.
        /// </summary>
        /// <typeparam name="TProperty">The property type.</typeparam>
        /// <param name="propertyName">The property name.</param>
        /// <param name="field">The backing field.</param>
        /// <param name="newValue">The new value.</param>
        /// <returns>
        /// <c>true</c>, if the backing field was set and the events were raised; otherwise, <c>false</c>.
        /// </returns>
        protected bool Set<TProperty>(string propertyName, ref TProperty field, TProperty newValue)
        {
            if (EqualityComparer<TProperty>.Default.Equals(field, newValue))
            {
                return false;
            }

            this.RaisePropertyChanging(propertyName);

            field = newValue;

            this.RaisePropertyChanged(propertyName);

            return true;
        }

        /// <summary>
        /// Calls the specified value setter related to the specified property expression if the new value is different
        /// than the current value. This method overload allows you to set other properties in addition to just fields.
        /// </summary>
        /// <typeparam name="TProperty">The property type.</typeparam>
        /// <param name="propertyExpression">The property expression.</param>
        /// <param name="getter">The delegate used to get the current value.</param>
        /// <param name="setter">The delegate used to set a new value.</param>
        /// <param name="newValue">The new value.</param>
        /// <returns>
        /// <c>true</c>, if the specified value setter was called; otherwise, <c>false</c>.
        /// </returns>
        protected bool Set<TProperty>(
            Expression<Func<TProperty>> propertyExpression,
            Func<TProperty> getter,
            Action<TProperty> setter,
            TProperty newValue)
        {
            if (propertyExpression == null)
            {
                throw new ArgumentNullException(nameof(propertyExpression));
            }

            if (getter == null)
            {
                throw new ArgumentNullException(nameof(getter));
            }

            if (setter == null)
            {
                throw new ArgumentNullException(nameof(setter));
            }

            var current = getter.Invoke();

            if (EqualityComparer<TProperty>.Default.Equals(current, newValue))
            {
                return false;
            }

            this.RaisePropertyChanging(propertyExpression);

            setter.Invoke(newValue);

            this.RaisePropertyChanged(propertyExpression);

            return true;
        }
    }
}