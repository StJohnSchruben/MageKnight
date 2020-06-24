using MKService.Queries;
using MKService.Updates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MKService.ModelFactories
{

    internal class DialFactory : ModelFactoryBase<IUpdatableDial>
    {
        public override IUpdatableDial Create()
        {
            return new DialQuery();
        }
    }
}
