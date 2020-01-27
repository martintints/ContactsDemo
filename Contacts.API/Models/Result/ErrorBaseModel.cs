using System.Collections.Generic;

namespace Contacts.API.Models.Result
{
    public class ErrorBaseModel<T> : AbstractResultModel
    {
        public IEnumerable<T> Errors { get; }

        public ErrorBaseModel(IEnumerable<T> errors)
        {
            Errors = errors;
        }

        public ErrorBaseModel(T error)
        {
            Errors = new List<T> { error };
        }
    }
}
