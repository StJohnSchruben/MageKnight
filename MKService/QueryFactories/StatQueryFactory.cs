using Service;
using MKService.DefaultModel;
using MKService.ModelFactories;
using MKService.Queries;
using MKService.Updates;


namespace MKService.QueryFactories
{
    internal class StatQueryFactory : QueryFactoryBase<StatQuery>
    {
        private StatQuery query;
        public StatQueryFactory(
             IModelFactoryResolver modelFactoryResolver, IDefaultModel defaultModel)
             : base(modelFactoryResolver, defaultModel)
        {
        }

        public override StatQuery Create()
        {
            if (this.query != null)
            {
                return this.query;
            }

            this.query = (StatQuery)this.ModelFactoryResolver.GetFactory<IUpdatableStat>().Create();
            //this.DefaultModel.LoadDataInfo(this.query);
            return this.query;
        }
    }
}
