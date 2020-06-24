using Service;
using MKService.DefaultModel;
using MKService.ModelFactories;
using MKService.Queries;
using MKService.Updates;


namespace MKService.QueryFactories
{
    internal class DialQueryFactory : QueryFactoryBase<DialQuery>
    {
        private DialQuery query;
        public DialQueryFactory(
             IModelFactoryResolver modelFactoryResolver, IDefaultModel defaultModel)
             : base(modelFactoryResolver, defaultModel)
        {
        }

        public override DialQuery Create()
        {
            if (this.query != null)
            {
                return this.query;
            }

            this.query = (DialQuery)this.ModelFactoryResolver.GetFactory<IUpdatableDial>().Create();
            return this.query;
        }
    }
}
