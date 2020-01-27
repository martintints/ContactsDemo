using AutoMapper;

namespace Contacts.BL.Factories
{
    public class BaseFactory<TIn, TOut> : IBaseFactory<TIn, TOut>
    {
        protected readonly IMapper Mapper;

        public BaseFactory(IMapper mapper)
        {
            Mapper = mapper;
        }

        public virtual TOut Create(TIn model) => Mapper.Map<TOut>(model);
    }
}
