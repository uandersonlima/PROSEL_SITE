using System.ComponentModel.DataAnnotations.Schema;
using ProselApp.Models.AcessCode;

namespace ProselApp.Models.ViewModel
{
    [NotMapped]
    public class UserCode
    {
         public User User {get; set;}
         public AccessCode AccessCode { get; set; }
    }
}