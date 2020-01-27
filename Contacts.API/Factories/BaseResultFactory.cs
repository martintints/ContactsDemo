using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Contacts.API.Models.Result;
using Contacts.BL.DTOs.Result;
using Contacts.BL.Factories;

namespace Contacts.API.Factories
{
    public class BaseResultFactory<TDomain, TModel> : BaseFactory<TDomain, TModel>, IBaseResultFactory<TDomain, TModel>
    {

        public BaseResultFactory(IMapper mapper) : base(mapper)
        {
        }

        /// <summary>
        /// Create an abstract result model that could contain either errors or data.
        /// </summary>
        /// <param name="resultDto"></param>
        /// <returns></returns>
        public virtual AbstractResultModel CreateResult(ResultDto<TDomain> resultDto)
        {
            if (!resultDto.IsValid)
            {
                return new ErrorBaseModel<ErrorCodeDto>(resultDto.Errors.Keys.Distinct());
            }

            return CreateResult(resultDto.Data);
        }

        public virtual ResultBaseModel<TModel> CreateResult(TDomain domain) => new ResultBaseModel<TModel>(Create(domain));

        public virtual ResultBaseModel<IEnumerable<TModel>> CreateResult(IEnumerable<TDomain> entities) => new ResultBaseModel<IEnumerable<TModel>>(entities.Select(Create));
    }

}
