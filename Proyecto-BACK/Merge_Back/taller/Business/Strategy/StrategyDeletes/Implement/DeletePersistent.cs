using Business.Strategy.StrategyDeletes.Interfaces;
using Data.Interfaces.DataBasic;
using Entity.Domain.Enums;

namespace Business.Strategy.StrategyDeletes.Implement
{
    public class DeletePersistent<T> : IStrategyDeletes<T> where T : class
    {
        public DeleteType Type => DeleteType.Persistent;
        public async Task<bool> DeleteAsync(int id, IData<T> repository)
        {
            return await repository.DeleteAsync(id);
        }
    }
}
