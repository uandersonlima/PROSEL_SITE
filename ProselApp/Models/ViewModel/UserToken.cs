using System.ComponentModel.DataAnnotations.Schema;

namespace ProselApp.Models.ViewModel
{
    [NotMapped]
    public class UserToken
    {
        public User User { get; set; }
        public Token Token { get; set; }
    }
}