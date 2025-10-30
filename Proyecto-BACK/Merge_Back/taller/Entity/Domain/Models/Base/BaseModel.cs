namespace Entity.Domain.Models.Base
{
    public class BaseModel
    {
        public int id { get; set; }
        public bool active { get; set; }
        public bool is_deleted { get; set; }
        public DateTime created_date { get; set; }

    }
}
