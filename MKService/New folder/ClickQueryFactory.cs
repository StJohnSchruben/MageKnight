using Service;
using MKService.DefaultModel;
using MKService.ModelFactories;
using MKService.Queries;
using MKService.Updates;


namespace MKService.QueryFactories
{
    internal class ClickQueryFactory : QueryFactoryBase<ClickQuery>
    {
        private ClickQuery query;
        public ClickQueryFactory(
             IModelFactoryResolver modelFactoryResolver, IDefaultModel defaultModel)
             : base(modelFactoryResolver, defaultModel)
        {
        }

        public override ClickQuery Create()
        {
            if (this.query != null)
            {
                return this.query;
            }

            this.query = (ClickQuery)this.ModelFactoryResolver.GetFactory<IUpdatableClick>().Create();
            this.DefaultModel.LoadDataInfo(this.query);
            return this.query;
        }
    }
}
