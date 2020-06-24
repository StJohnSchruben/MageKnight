
using MKModel;
using MKService.Updates;

namespace MKService.DefaultModel
{
    /// <summary>
    /// The Default Model.
    /// </summary>
    internal interface IDefaultModel
    {
        void LoadDataInfo(IUpdatableUserCollection model);
        void OpenBooster (IUpdatableUser model, BoosterPack set);
    }
}