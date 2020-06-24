using Service;
using MKService.DefaultModel;
using MKService.ModelFactories;
using MKService.Queries;
using MKService.Updates;

namespace MKService.QueryFactories
{
    internal class UserQueryFactory : QueryFactoryBase<UserQuery>
    {
        private UserQuery query;
        public UserQueryFactory(
             IModelFactoryResolver modelFactoryResolver, IDefaultModel defaultModel)
             : base(modelFactoryResolver, defaultModel)
        {
        }

        public override UserQuery Create()
        {
            if (this.query != null)
            {
                return this.query;
            }

            this.query = (UserQuery)this.ModelFactoryResolver.GetFactory<IUpdatableUser>().Create();
            //this.DefaultModel.LoadDataInfo(this.query);
            return this.query;
        }
    }
}
