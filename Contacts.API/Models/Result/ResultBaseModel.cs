namespace Contacts.API.Models.Result
{
    public class ResultBaseModel<T> : AbstractResultModel
    {
        public ResultBaseModel(T data)
        {
            Data = data;
        }
        public T Data { get; set; }
    }
}
