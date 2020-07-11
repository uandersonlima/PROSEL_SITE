using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ProselApp.Models;
using ProselApp.Repositories.Interfaces;

namespace ProselApp.Libraries.Validation
{
    public sealed class  CPFUnico : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult("Digite o CPF");
            }
            string CPF = (value as string).Trim();

            IUserRepository usuarioRepos = (IUserRepository)validationContext.GetService(typeof(IUserRepository));
            User user = usuarioRepos.GetByCpfAsync(CPF).Result;

            User objCliente = (User)validationContext.ObjectInstance;

            if (user != null)
            {
                return new ValidationResult("CPF já cadastrado, entre em contato em caso de dúvidas");
            }

            return ValidationResult.Success;
        }
    }
}