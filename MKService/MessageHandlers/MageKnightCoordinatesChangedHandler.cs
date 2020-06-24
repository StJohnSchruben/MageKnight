using MKService.Messages;
using MKService.ModelUpdaters;
using MKService.Queries;
using MKService.Updates;

namespace MKService.MessageHandlers
{
    internal class MageKnightCoordinatesChangedHandler : ServerMessageHandlerBase<MageKnightCoordinatesChanged, MageKnightQuery, IUpdatableMageKnight>
    {
        public MageKnightCoordinatesChangedHandler(IModelUpdaterResolver modelUpdaterResolver) : base(modelUpdaterResolver)
        {
        }

        protected override IUpdatableMageKnight LocateQueryComponent(MageKnightCoordinatesChanged message, MageKnightQuery query)
        {
            return query;
        }
    }
}
