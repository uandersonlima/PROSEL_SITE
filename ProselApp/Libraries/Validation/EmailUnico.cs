using ProselApp.Models;
using ProselApp.Repositories.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ProselApp.Libraries.Validation
{
    public sealed class  EmailUnico : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult("Digite o e-mail!");
            }
            string Email = (value as string).Trim();

            IUserRepository usuarioRepos = (IUserRepository)validationContext.GetService(typeof(IUserRepository));
            List<User> usuarios = usuarioRepos.GetUserByEmail(Email);

            User objCliente = (User)validationContext.ObjectInstance;

            if (usuarios.Count > 1)
            {
                return new ValidationResult("E-mail já existente!");
            }
            if (usuarios.Count == 1 && objCliente.Cpf != usuarios.First().Cpf)
            {
                return new ValidationResult("E-mail já existente!");
            }


            return ValidationResult.Success;
        }
    }
}
