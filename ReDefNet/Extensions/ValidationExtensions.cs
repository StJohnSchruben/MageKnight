

using System;
using JetBrains.Annotations;

namespace ReDefNet.Extensions
{
    /// <summary>
    /// Extension methods for argument validation.
    /// </summary>
    public static class ValidationExtensions
    {
        /// <summary>
        /// Validates the argument is not null.
        /// </summary>
        /// <typeparam name="TArgument">The argument type.</typeparam>
        /// <param name="argument">The argument value.</param>
        /// <param name="name">The argument name.</param>
        [ContractAnnotation("halt <= argument: null")]
        [Obsolete("Use the validation extension methods in the root ReDefNet namespace.")]
        public static void ValidateNotNull<TArgument>([NoEnumeration] this TArgument argument, string name)
        {
            ReDefNet.ValidationExtensions.ValidateNotNull(argument, name);
        }
    }
}