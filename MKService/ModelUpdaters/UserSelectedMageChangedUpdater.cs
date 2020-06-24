using MKModel;
using MKService.Messages;
using MKService.ModelFactories;
using MKService.Updates;
using MKService.DefaultModel;
using System.Linq;

namespace MKService.ModelUpdaters
{
    internal class UserSelectedMageChangedUpdater : ModelUpdaterBase<IUpdatableUser, UserSelectedMageChanged>
    {
        public UserSelectedMageChangedUpdater(
            IModelFactoryResolver modelFactoryResolver, IDefaultModel defaultMode)
            : base(modelFactoryResolver, defaultMode)
        {
        }

        protected override bool UpdateInternal(IUpdatableUser model, UserSelectedMageChanged message)
        {
            if (model.Id == message.UserId)
            {
                model.SelectedMage = model.Army.FirstOrDefault(x => x.InstantiatedId == message.MageId);
                return true;
            }

            return false;
        }
    }
}
