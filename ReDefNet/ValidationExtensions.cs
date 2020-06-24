

using System;
using JetBrains.Annotations;

namespace ReDefNet
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
        public static void ValidateNotNull<TArgument>([NoEnumeration] this TArgument argument, string name)
        {
            if (argument == null)
            {
                throw new ArgumentNullException(name);
            }
        }
    }
}