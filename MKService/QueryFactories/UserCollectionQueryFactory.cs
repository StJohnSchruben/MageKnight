using Service;
using MKService.DefaultModel;
using MKService.ModelFactories;
using MKService.Queries;
using MKService.Updates;


namespace MKService.QueryFactories
{
    internal class UserCollectionQueryFactory : QueryFactoryBase<UserCollectionQuery>
    {
        private UserCollectionQuery query;
        public UserCollectionQueryFactory(
             IModelFactoryResolver modelFactoryResolver, IDefaultModel defaultModel)
             : base(modelFactoryResolver, defaultModel)
        {
        }

        public override UserCollectionQuery Create()
        {
            if (this.query != null)
            {
                return this.query;
            }

            this.query = (UserCollectionQuery)this.ModelFactoryResolver.GetFactory<IUpdatableUserCollection>().Create();
            this.DefaultModel.LoadDataInfo(this.query);
            return this.query;
        }
    }
}
