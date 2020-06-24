
using MKService.Messages;
using MKService.ModelUpdaters;
using MKService.Queries;
using MKService.Updates;

namespace MKService.MessageHandlers
{
    internal class DialAddedHandler : ServerMessageHandlerBase<DialAdd, DialQuery, IUpdatableDial>
    {
        public DialAddedHandler(IModelUpdaterResolver modelUpdaterResolver) : base(modelUpdaterResolver)
        {
        }

        protected override IUpdatableDial LocateQueryComponent(DialAdd message, DialQuery query)
        {
            return query;
        }
    }
}
