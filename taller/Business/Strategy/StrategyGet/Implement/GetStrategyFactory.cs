using Business.Strategy.StrategyGet.Iterfaz;
using Data.Interfaces.DataBasic;
using Entity.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Strategy.StrategyGet.Implement
{
    public static class GetStrategyFactory
    {
        public static IStrategyGetAll<T> GetStrategyGet<T>(IData<T> repository,GetAllType getAllType) where T : class
        {
            return getAllType switch
            {
                GetAllType.GetAll => new StrategyGetAll<T>(),
                GetAllType.GetAllDeletes when repository is IData<T> => new StrategyGetDeletes<T>(),
                _ => throw new InvalidOperationException("Tipo de get no soprtado en este repositorio")
            };
        }
    }
}
