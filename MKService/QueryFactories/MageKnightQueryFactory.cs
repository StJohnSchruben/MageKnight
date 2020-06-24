using Service;
using MKService.DefaultModel;
using MKService.ModelFactories;
using MKService.Queries;
using MKService.Updates;

namespace MKService.QueryFactories
{
    internal class MageKnightQueryFactory : QueryFactoryBase<MageKnightQuery>
    {
        private MageKnightQuery query;
        public MageKnightQueryFactory(
             IModelFactoryResolver modelFactoryResolver, IDefaultModel defaultModel)
             : base(modelFactoryResolver, defaultModel)
        {
        }

        public override MageKnightQuery Create()
        {
            if (this.query != null)
            {
                return this.query;
            }

            this.query = (MageKnightQuery)this.ModelFactoryResolver.GetFactory<IUpdatableMageKnight>().Create();

            return this.query;
        }
    }
}
