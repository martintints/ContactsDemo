using System.Collections.Generic;
using Contacts.API.Models.Result;
using Contacts.BL.DTOs.Result;
using Contacts.BL.Factories;

namespace Contacts.API.Factories
{
    public interface IBaseResultFactory<TDomain, TModel> : IBaseFactory<TDomain, TModel>
    {
        AbstractResultModel CreateResult(ResultDto<TDomain> resultDto);
        ResultBaseModel<TModel> CreateResult(TDomain domain);
        ResultBaseModel<IEnumerable<TModel>> CreateResult(IEnumerable<TDomain> entities);
    }
}
