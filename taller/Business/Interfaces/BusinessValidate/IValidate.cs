using Business.Interfaces.BusinessBasic;

namespace Business.Interfaces.BusinessValidate
{
    public interface IValidate<TDto, TDtoGet> : IBusiness<TDto, TDtoGet>
    {
        Task<TDto?> CreateValidateAsync(TDto dto);
        Task<TDto?> UpdateValidateAsync(TDto dto);
    }
}
