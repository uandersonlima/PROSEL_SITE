using System;

namespace ProselApp.Services.Exceptions
{
    public class DbConcurrencyException : ApplicationException
    {
        public DbConcurrencyException (string message) : base(message)
        {

        }
    }
}
