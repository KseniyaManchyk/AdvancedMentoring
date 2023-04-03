using System.Runtime.Serialization;

namespace CatalogService.Domain.ExceptionHandling
{
    public class NotFoundException : Exception
    {
        private const string _notFoundMessage = "{0} was not found in the database.";

        public NotFoundException()
        {
        }

        public NotFoundException(string? entityName) : base(string.Format(_notFoundMessage, entityName))
        {
        }

        public NotFoundException(string? entityName, Exception? innerException) : base(string.Format(_notFoundMessage, entityName), innerException)
        {
        }

        protected NotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
