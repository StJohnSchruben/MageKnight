
using MKService.Messages;
using MKService.ModelUpdaters;
using MKService.Queries;
using MKService.Updates;

namespace MKService.MessageHandlers
{
    internal class GameChangedHandler : ServerMessageHandlerBase<GameChanged, GameQuery, IUpdatableGame>
    {
        public GameChangedHandler(IModelUpdaterResolver modelUpdaterResolver) : base(modelUpdaterResolver)
        {
        }

        protected override IUpdatableGame LocateQueryComponent(GameChanged message, GameQuery query)
        {
            return query;
        }
    }
    internal class GamesChangedHandler : ServerMessageHandlerBase<GamesChanged, GamesQuery, IUpdatableGames>
    {
        public GamesChangedHandler(IModelUpdaterResolver modelUpdaterResolver) : base(modelUpdaterResolver)
        {
        }

        protected override IUpdatableGames LocateQueryComponent(GamesChanged message, GamesQuery query)
        {
            return query;
        }
    }
}
