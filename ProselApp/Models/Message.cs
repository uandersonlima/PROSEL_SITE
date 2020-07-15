using System;
using System.ComponentModel.DataAnnotations;

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
        [MinLength(14, ErrorMessage = "Telefone inválido")]

        public string Telephone { get; set; }

        [MinLength(50, ErrorMessage="Texto pequeno, porfavor digite no mínimo {1} caracteres")]
        public string Description {get; set;}
        public DateTime TimeReceived {get; set;}
        public DateTime? ViewedTime {get; set;}

    }
}