using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProselApp.Models.DTO
{
    [NotMapped]
    public class Password
    {
        public string Cpf { get; set; }
        [Required(ErrorMessage = "Informe o campo {0}", AllowEmptyStrings = false)]
        [MinLength(6, ErrorMessage = "Senha deve conter no mínimo 6 caracteres")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "Informe o campo {0}", AllowEmptyStrings = false)]
        [MinLength(6, ErrorMessage = "Senha deve conter no mínimo 6 caracteres")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }


        [Required(ErrorMessage = "Informe o campo {0}", AllowEmptyStrings = false)]
        [Display(Name = "Confirme a senha")]
        [Compare("NewPassword", ErrorMessage = "Senhas diferentes")]
        [DataType(DataType.Password)]
        public string NewPasswordConfirmation { get; set; }
    }
}