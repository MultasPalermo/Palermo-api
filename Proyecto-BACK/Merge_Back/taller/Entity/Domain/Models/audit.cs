namespace Entity.Domain.Models
{
    public class audit
    {
        public int id { get; set; }
        public string Entity { get; set; }
        public string action { get; set; }
        public string keyValues { get; set; }
        public string? oldValues { get; set; }
        public string? newValues { get; set; }
        public string? userId { get; set; }
        public DateTime dateTime { get; set; } 
        //public string changedColumns { get; set; }
    }
}
