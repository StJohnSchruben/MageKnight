
using Service;
using MKService.DefaultModel;
using MKService.ModelFactories;
using MKService.Queries;
using MKService.Updates;

namespace MKService.QueryFactories
{
    /// <summary>
    /// The airplane query factory.
    /// </summary>
    /// <seealso cref="MKService.QueryFactories.QueryFactoryBase{MKService.Queries.SessionTimeQuery}" />
    internal class SessionTimeQueryFactory : QueryFactoryBase<SessionTimeQuery>
    {
        /// <summary>
        /// The query.
        /// </summary>
        private SessionTimeQuery query;

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionTimeQueryFactory" /> class.
        /// </summary>
        /// <param name="modelFactoryResolver">The model factory resolver.</param>
        /// <param name="defaultModel">The default model.</param>
        public SessionTimeQueryFactory(
            IModelFactoryResolver modelFactoryResolver, IDefaultModel defaultModel)
            : base(modelFactoryResolver, defaultModel)
        {
        }

        /// <summary>
        /// Creates a new a new instance of <see cref="!:TQuery" />.
        /// </summary>
        /// <returns>A new instance of <see cref="!:TQuery" />.</returns>
        public override SessionTimeQuery Create()
        {
            if (this.query != null)
            {
                return this.query;
            }

            this.query = (SessionTimeQuery)this.ModelFactoryResolver.GetFactory<IUpdatableSessionTime>().Create();

            return this.query;
        }
    }
}