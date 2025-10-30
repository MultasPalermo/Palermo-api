

using Data.Interfaces.DataBasic;
using Entity.Domain.Enums;

namespace Business.Strategy.StrategyDeletes.Interfaces
{
    public interface IStrategyDeletes<T> where T : class
    {
        DeleteType Type { get; }
        Task<bool> DeleteAsync(int id, IData<T> repository);
    }
}
