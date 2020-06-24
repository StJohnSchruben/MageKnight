
using MKService.Queries;
using MKService.Updates;

namespace MKService.ModelFactories
{
    /// <summary>
    /// The satellite collection factory.
    /// </summary>
    internal class SessionTimeFactory : ModelFactoryBase<IUpdatableSessionTime>
    {
        /// <summary>
        /// Creates a new satellite collection query.
        /// </summary>
        /// <returns>Updatable satellite collection.</returns>
        public override IUpdatableSessionTime Create()
        {
            return new SessionTimeQuery();
        }
    }
}