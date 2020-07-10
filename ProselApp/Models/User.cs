using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ProselApp.Libraries.Validation;


namespace ProselApp.Models
{
    public class User
    {
        [Key]
        [CPF(ErrorMessage = "Informe um {0} válido")]
        [Required(ErrorMessage = "Informe o campo {0}", AllowEmptyStrings = false)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#########'/'##}")]
        public string Cpf { get; set; }
        
        [Required(ErrorMessage = "Informe o campo {0}", AllowEmptyStrings = false)]
        [MinLength(4, ErrorMessage = "Nome e sobrenome muito curto")]
        public string Name { get; set; }
        public string Username { get; set; }

        [EmailUnico]
        [Required(ErrorMessage = "Informe o campo {0}", AllowEmptyStrings = false)]
        [EmailAddress(ErrorMessage = "Informe um e-mail válido")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Informe o campo {0}", AllowEmptyStrings = false)]
        [MinLength(14, ErrorMessage = "Senha deve conter no mínimo 6 caracteres")]
        public string Telephone { get; set; }

        [Required(ErrorMessage = "Informe o campo {0}", AllowEmptyStrings = false)]
        [MinLength(6, ErrorMessage = "Senha deve conter no mínimo 6 caracteres")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Informe o campo {0}", AllowEmptyStrings = false)]
        [Display(Name = "Confirme a senha")]
        [Compare("Senha", ErrorMessage = "Senhas diferentes")]
        [DataType(DataType.Password)]
        public string PasswordConfirmation { get; set; }
        public bool AccountStatus { get; set; }
        public bool receive_emails { get; set; }
        public string AccessType { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
    }
}