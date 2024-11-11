using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MWL.IdentityService.Domain.Exceptions
{
    public class ValidationError : Exception
    {
        public ValidationError() { }
        public ValidationError(string message) : base(message) { }
        public ValidationError(string message, Exception innerException) : base(message, innerException) { }
    }
}
