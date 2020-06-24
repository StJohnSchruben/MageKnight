using MKService.Queries;
using MKService.Updates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MKService.ModelFactories
{
    internal class MageKnightFactory : ModelFactoryBase<IUpdatableMageKnight>
    {
        public override IUpdatableMageKnight Create()
        {
            return new MageKnightQuery();
        }
    }
}
