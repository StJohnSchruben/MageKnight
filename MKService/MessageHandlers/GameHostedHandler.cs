using MKService.Messages;
using MKService.ModelUpdaters;
using MKService.Queries;
using MKService.Updates;

namespace MKService.MessageHandlers
{
    internal class GameHostedHandler : ServerMessageHandlerBase<GameHosted, GamesQuery, IUpdatableGames>
    {
        public GameHostedHandler(IModelUpdaterResolver modelUpdaterResolver) : base(modelUpdaterResolver)
        {
        }

        protected override IUpdatableGames LocateQueryComponent(GameHosted message, GamesQuery query)
        {
            return query;
        }
    }
}
