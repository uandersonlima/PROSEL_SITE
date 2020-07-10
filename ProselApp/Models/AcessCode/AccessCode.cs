using System;
using System.ComponentModel.DataAnnotations;

namespace ProselApp.Models.AcessCode
{
    public class AccessCode
    {
        [Key]
        public int Code { get; set; }
        public string UserCpf { get; set; }
        public virtual User User { get; set; }
        [Required(ErrorMessageResourceType = typeof(Message), ErrorMessageResourceName = "MSG_E001")]
        [Display(Name = "Código de Verificação")]
        public string Key { get; set; }
        public string CodeType { get; set; }
        public DateTime GenDate { get; set; }
    }
}