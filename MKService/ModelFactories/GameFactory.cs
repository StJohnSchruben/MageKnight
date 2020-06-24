using MKService.Queries;
using MKService.Updates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MKService.ModelFactories
{
    internal class GameFactory : ModelFactoryBase<IUpdatableGame>
    {
        public override IUpdatableGame Create()
        {
            return new GameQuery();
        }
    }

    internal class GamesFactory : ModelFactoryBase<IUpdatableGames>
    {
        public override IUpdatableGames Create()
        {
            return new GamesQuery();
        }
    }
}
