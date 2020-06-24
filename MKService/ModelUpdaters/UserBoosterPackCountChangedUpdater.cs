using MKModel;
using MKService.Messages;
using MKService.ModelFactories;
using MKService.Updates;
using MKService.DefaultModel;

namespace MKService.ModelUpdaters
{
    internal class UserBoosterPackCountChangedUpdater : ModelUpdaterBase<IUpdatableUser, UserBoosterPackCountChanged>
    {
        public UserBoosterPackCountChangedUpdater(
            IModelFactoryResolver modelFactoryResolver, IDefaultModel defaultMode)
            : base(modelFactoryResolver, defaultMode)
        {
        }

        protected override bool UpdateInternal(IUpdatableUser model, UserBoosterPackCountChanged message)
        {
            bool changed = false;
            switch (message.Set) 
            {
                case BoosterPack.Rebellion:
                    if (model.RebellionBoosterPacks != message.Count && message.Count >= 0)
                    {
                        if (model.RebellionBoosterPacks > message.Count)
                        {
                            model.RebellionBoosterPacks = message.Count;
                            this.DefaultModel.OpenBooster(model, message.Set);
                        }
                       
                        changed = true;
                    }
                    break;
            }

            return changed;
        }
    }
}
