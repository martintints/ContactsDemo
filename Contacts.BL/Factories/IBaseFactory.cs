namespace Contacts.BL.Factories
{
    public interface IBaseFactory<in TIn, out TOut>
    {
        TOut Create(TIn domain);
    }
}
