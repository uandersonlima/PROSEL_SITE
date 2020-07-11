using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProselApp.Models
{
    public class Message
    {
        [Key]
        public int Messagecode { get; set; }
        [Required(ErrorMessage = "Informe o campo {0}", AllowEmptyStrings = false)]
        [MinLength(4, ErrorMessage = "Nome e sobrenome muito curto")]
        public string Sender { get; set; }
        [Required(ErrorMessage = "Informe o campo {0}", AllowEmptyStrings = false)]
        [EmailAddress(ErrorMessage = "Informe um e-mail válido")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Informe o campo {0}", AllowEmptyStrings = false)]
        [MinLength(14, ErrorMessage = "Senha deve conter no mínimo 6 caracteres")]
        public string Telephone { get; set; }
        [ForeignKey("User")]
        public string UserCpf { get; set; }
        public virtual User User { get; set; }
    }
}