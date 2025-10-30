using Business.Interfaces.BusinessBasic;
using Entity.Domain.Models.Implements.Entities;
using Entity.DTOs.Default.AnexarMulta; // si acá están tus DTOs
// ^ Ajusta el using según dónde estén tus DTOs (UserInfractionDto, UserInfractionSelectDto)

namespace Business.Interfaces.IBusinessImplements.Entities
{
    public interface IUserInfractionServices : IBusiness<UserInfractionDto, UserInfractionSelectDto>
    {
        Task<IEnumerable<UserInfractionSelectDto>> GetByDocumentAsync(int documentTypeId, string documentNumber);
        Task<UserInfractionSelectDto> GetByIdAsyncPdf(int id);
        Task<UserInfractionSelectDto> CreateWithPersonAsync(CreateInfractionRequestDto dto);
        Task<IEnumerable<UserInfractionSelectDto>> GetByTypeInfractionAsync(int typeInfractionId);
        Task<UserInfractionSelectDto?> GetFirstByDocumentAsync(int documentTypeId, string documentNumber);
    }
}
