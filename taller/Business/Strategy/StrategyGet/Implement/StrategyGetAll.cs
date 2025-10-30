

using Business.Strategy.StrategyGet.Iterfaz;
using Data.Interfaces.DataBasic;
using Entity.Domain.Enums;

namespace Business.Strategy.StrategyGet.Implement
{
    public class StrategyGetAll<T> : IStrategyGetAll<T> where T : class
    {
        public GetAllType Type => GetAllType.GetAll;

        public async Task<IEnumerable<T>> GetAll(IData<T> repository)
        {
            return await repository.GetAllAsync();
        }
    }
}
