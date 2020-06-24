
using MKService.Messages;
using MKService.ModelUpdaters;
using MKService.Queries;
using MKService.Updates;

namespace MKService.MessageHandlers
{
    internal class MageKnightChangedHandler : ServerMessageHandlerBase<MageKnightChanged, MageKnightQuery, IUpdatableMageKnight>
    {
        public MageKnightChangedHandler(IModelUpdaterResolver modelUpdaterResolver) : base(modelUpdaterResolver)
        {
        }

        protected override IUpdatableMageKnight LocateQueryComponent(MageKnightChanged message, MageKnightQuery query)
        {
            return query;
        }
    }
}
