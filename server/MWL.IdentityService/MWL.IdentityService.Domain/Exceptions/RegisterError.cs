using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MWL.IdentityService.Domain.Exceptions
{
    public class RegisterError : Exception
    {
        public RegisterError() { }
        public RegisterError(string message) : base(message) { }
        public RegisterError(string message, Exception innerException) : base(message, innerException) { }
        public RegisterError(IEnumerable<IdentityError> errors) : base(string.Join(", ", errors.Select(e => e.Description))) { }
    }
}
