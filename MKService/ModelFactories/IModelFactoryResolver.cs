using MKService.Updates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MKService.ModelFactories
{
    internal interface IModelFactoryResolver
    {
        /// <summary>
        /// Gets the model factory for the specified model type. This method will intentionally throw an
        /// exception if no factory could be found.
        /// </summary>
        /// <typeparam name="TModel">The model type.</typeparam>
        /// <returns>
        /// The factory for the specified model type.
        /// </returns>
        IModelFactory<TModel> GetFactory<TModel>() where TModel : IUpdatable;
    }
}
