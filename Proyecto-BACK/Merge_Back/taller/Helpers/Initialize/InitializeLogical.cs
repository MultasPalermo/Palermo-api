using Entity.Domain.Interfaces;
using Entity.Domain.Models.Base;

namespace Helpers.Initialize
{
    public static class InitializeLogical
    {
        public static void InitializeLogicalState(this object entity)
        {
            if (entity is BaseModel softDeletable)
            {
                softDeletable.is_deleted = false;
                softDeletable.active = true;
            }
        }
    }
}
