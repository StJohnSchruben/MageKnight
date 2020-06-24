using MKService.Updates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MKService.ModelFactories
{
    internal interface IModelFactory
    {
        /// <summary>
        /// Gets the type that can be created by the factory.
        /// </summary>
        /// <value>
        /// The type that can be created by the factory.
        /// </value>
        Type SupportedType { get; }
    }

    internal interface IModelFactory<out TModel> : IModelFactory where TModel : IUpdatable
    {
        /// <summary>
        /// Creates a new a new instance of <see cref="TModel" />.
        /// </summary>
        /// <returns>
        /// A new instance of <see cref="TModel" />.
        /// </returns>
        TModel Create();
    }
}
