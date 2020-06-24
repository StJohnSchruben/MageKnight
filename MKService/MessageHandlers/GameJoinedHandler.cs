
using MKService.Messages;
using MKService.ModelUpdaters;
using MKService.Queries;
using MKService.Updates;


namespace MKService.MessageHandlers
{
    internal class GameJoinedHandler : ServerMessageHandlerBase<GameJoined, GamesQuery, IUpdatableGame>
    {
        public GameJoinedHandler(IModelUpdaterResolver modelUpdaterResolver) : base(modelUpdaterResolver)
        {
        }

        protected override IUpdatableGame LocateQueryComponent(GameJoined message, GamesQuery query)
        {
            return (IUpdatableGame)query.GetGame(message.GameId);
        }
    }
}
