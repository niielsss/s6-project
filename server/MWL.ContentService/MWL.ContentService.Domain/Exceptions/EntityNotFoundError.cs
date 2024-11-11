using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MWL.ContentService.Domain.Exceptions
{
    public class EntityNotFoundError : Exception
    {
        public EntityNotFoundError() { }
        public EntityNotFoundError(string message) : base(message) { }
        public EntityNotFoundError(int id) : base($"Entity with id {id} was not found.") { }
        public EntityNotFoundError(string message, Exception innerException) : base(message, innerException) { }
    }
}
