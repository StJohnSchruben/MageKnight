

using System;

namespace JetBrains.Annotations
{
    /// <summary>
    /// A ReSharper annotation attribute that aids in code analysis. See ReSharper code annotation documentation for details.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    internal class ContractAnnotationAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContractAnnotationAttribute" /> class.
        /// </summary>
        /// <param name="contract">The contract.</param>
        public ContractAnnotationAttribute(string contract)
            : this(contract, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContractAnnotationAttribute" /> class.
        /// </summary>
        /// <param name="contract">The contract.</param>
        /// <param name="forceFullStates"><c>true</c>, if full states should be forced; otherwise, <c>false</c>.</param>
        public ContractAnnotationAttribute(string contract, bool forceFullStates)
        {
            this.Contract = contract;
            this.ForceFullStates = forceFullStates;
        }

        /// <summary>
        /// Gets the contract.
        /// </summary>
        /// <value>
        /// The contract.
        /// </value>
        public string Contract { get; }

        /// <summary>
        /// Gets a value indicating whether full states should be forced.
        /// </summary>
        /// <value>
        /// <c>true</c>, if full states should be forced; otherwise, <c>false</c>.
        /// </value>
        public bool ForceFullStates { get; }
    }
}