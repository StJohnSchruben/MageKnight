using MKService.Queries;
using MKService.Updates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MKService.ModelFactories
{
    internal class UserFactory : ModelFactoryBase<IUpdatableUser>
    {
        public override IUpdatableUser Create()
        {
            return new UserQuery();
        }
    }
}
