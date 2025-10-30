using Business.Strategy.StrategyGet.Iterfaz;
using Data.Interfaces.DataBasic;
using Entity.Domain.Enums;

namespace Business.Strategy.StrategyGet.Implement
{
    public class StrategyGetDeletes<T> : IStrategyGetAll<T> where T : class
    {
        public GetAllType Type => GetAllType.GetAllDeletes;


        public async Task<IEnumerable<T>> GetAll(IData<T> repository)
        {
                return await repository.GetDeletes();
        }

    }
}
