using System;


namespace ProselApp.Models
{
    public class Token
    {
        public int Id { get; set; }
        public string SecurityToken { get; set; }
        public User User { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime TokenExpiration { get; set; }
    }
}
