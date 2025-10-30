namespace Entity.DTOs.Select.ModelSecuritySelectDto
{
    public class UserSelectDto
    {
        public int id { get; set; }
        public string? email { get; set; }
        public int? documentTypeId { get; set; }
        public string? TypeDocument { get; set; }
        public string? documentNumber { get; set; }
    }
}
