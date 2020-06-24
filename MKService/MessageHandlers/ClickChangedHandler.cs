
using MKService.Messages;
using MKService.ModelUpdaters;
using MKService.Queries;
using MKService.Updates;

namespace MKService.MessageHandlers
{
    internal class ClickAddedHandler : ServerMessageHandlerBase<ClickAdd, ClickQuery, IUpdatableClick>
    {
        public ClickAddedHandler(IModelUpdaterResolver modelUpdaterResolver) : base(modelUpdaterResolver)
        {
        }

        protected override IUpdatableClick LocateQueryComponent(ClickAdd message, ClickQuery query)
        {
            return query;
        }
    }
}
