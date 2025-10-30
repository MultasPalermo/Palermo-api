using Business.Strategy.StrategyDeletes.Interfaces;
using Data.Interfaces.DataBasic;
using Entity.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Strategy.StrategyDeletes.Implement
{
    public class DeleteLogical<T> : IStrategyDeletes<T> where T : class
    {
        public DeleteType Type => DeleteType.Logical;
        public async Task<bool> DeleteAsync(int id, IData<T> repository)
        {
            // Aseguramos que el repositorio implemente IDataExtend<T>
            return await repository.DeleteLogicAsync(id);

        }
    }
}
