

using System;

namespace JetBrains.Annotations
{
    /// <summary>
    /// A ReSharper annotation attribute that aids in code analysis. See ReSharper code annotation documentation for details.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    internal class NoEnumerationAttribute : Attribute
    {
    }
}