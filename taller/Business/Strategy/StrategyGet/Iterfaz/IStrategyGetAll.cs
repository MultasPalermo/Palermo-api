using Data.Interfaces.DataBasic;
using Entity.Domain.Enums;

namespace Business.Strategy.StrategyGet.Iterfaz
{
    public interface IStrategyGetAll<T> where T : class
    {
        GetAllType Type { get; }
        public Task<IEnumerable<T>> GetAll(IData<T> repository);
    }
}
