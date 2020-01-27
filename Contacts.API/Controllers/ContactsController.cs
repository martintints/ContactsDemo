using System.Collections.Generic;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using Contacts.API.Factories;
using Contacts.API.Factories.Contact;
using Contacts.API.Models.Contact;
using Contacts.API.Models.Result;
using Contacts.BL.DTOs.Result;
using Contacts.BL.Services.Contact;
using Contacts.Domain.Contact;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Contacts.API.Controllers
{
    [Produces(MediaTypeNames.Application.Json)]
    [ApiController]
    [Route("[controller]")]
    public class ContactsController : ControllerBase
    {
        private readonly IContactService _contactService;
        private readonly ICreateUpdateContactDtoFactory _createUpdateContactDtoFactory;

        private readonly IBaseResultFactory<ICollection<Contact>, ICollection<ContactModel>> _contactModelListFactory;
        private readonly IBaseResultFactory<Contact, ContactModel> _contactModelFactory;
        private readonly IBaseResultFactory<bool, bool> _deleteResultFactory;

        public ContactsController(
            IContactService contactService, 
            IBaseResultFactory<ICollection<Contact>, ICollection<ContactModel>> contactModelListFactory, 
            ICreateUpdateContactDtoFactory createUpdateContactDtoFactory, 
            IBaseResultFactory<Contact, ContactModel> contactModelFactory, 
            IBaseResultFactory<bool, bool> deleteResultFactory)
        {
            _contactService = contactService;
            _contactModelListFactory = contactModelListFactory;
            _createUpdateContactDtoFactory = createUpdateContactDtoFactory;
            _contactModelFactory = contactModelFactory;
            _deleteResultFactory = deleteResultFactory;
        }

        /// <summary>
        /// Get all contacts
        /// </summary>
        /// <remarks>
        /// Result is sorted by sequence in ascending order
        /// </remarks>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(ErrorBaseModel<ErrorCodeDto>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResultBaseModel<IEnumerable<ContactModel>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _contactService.GetAllAsync();
            var resultModel = _contactModelListFactory.CreateResult(result);

            if (!result.IsValid)
            {
                return NotFound(resultModel);
            }

            return Ok(resultModel);
        }

        /// <summary>
        /// Get contact
        /// </summary>
        /// <param name="contactId"></param>
        /// <returns></returns>
        [HttpGet("{contactId}")]
        [ProducesResponseType(typeof(ErrorBaseModel<ErrorCodeDto>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResultBaseModel<ContactModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(int contactId)
        {
            var result = await _contactService.GetAsync(contactId);
            var resultModel = _contactModelFactory.CreateResult(result);

            if (!result.IsValid)
            {
                return NotFound(resultModel);
            }

            return Ok(resultModel);
        }

        /// <summary>
        /// Create contact
        /// </summary>
        /// <remarks>
        /// Fields:
        /// * FirstName - required, length 100
        /// * LastName - required, length 100
        /// * Email - required, length 254. Must be email address (validator uses the same regular expression as used by the .NET Framework’s EmailAddressAttribute).
        /// * Sequence - required, precision 32,16. This field is used for frontend ordering only, backend does not maintain these values.
        /// * Phone - length 254
        /// </remarks>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ErrorBaseModel<ErrorCodeDto>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResultBaseModel<ContactModel>), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateContact([FromBody] CreateUpdateContactModel model, CancellationToken cancellationToken)
        {
            var dto = _createUpdateContactDtoFactory.Create(model);
            var result = await _contactService.CreateAsync(dto, cancellationToken);
            var resultModel = _contactModelFactory.CreateResult(result);

            if (!result.IsValid)
            {
                return BadRequest(resultModel);
            }

            return StatusCode(StatusCodes.Status201Created, resultModel);
        }

        /// <summary>
        /// Update contact
        /// </summary>
        /// <remarks>
        /// Fields:
        /// * Id - required
        /// * FirstName - required, length 100
        /// * LastName - required, length 100
        /// * Email - required, length 254. Must be email address (validator uses the same regular expression as used by the .NET Framework’s EmailAddressAttribute).
        /// * Sequence - required, precision 32,16. This field is used for frontend ordering only, backend does not maintain these values.
        /// * Phone - length 254
        /// </remarks>
        /// <param name="contactId"></param>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPatch("{contactId}")]
        [ProducesResponseType(typeof(ErrorBaseModel<ErrorCodeDto>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResultBaseModel<ContactModel>), StatusCodes.Status201Created)]
        public async Task<IActionResult> UpdateContact(
            int contactId, 
            [FromBody] CreateUpdateContactModel model,
            CancellationToken cancellationToken)
        {
            var dto = _createUpdateContactDtoFactory.Create(model, contactId);
            var result = await _contactService.UpdateAsync(dto, cancellationToken);
            var resultModel = _contactModelFactory.CreateResult(result);

            if (!result.IsValid)
            {
                return BadRequest(resultModel);
            }

            return StatusCode(StatusCodes.Status201Created, resultModel);
        }

        /// <summary>
        /// Delete contact
        /// </summary>
        /// <param name="contactId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete("{contactId}")]
        [ProducesResponseType(typeof(ErrorBaseModel<ErrorCodeDto>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteContact(int contactId, CancellationToken cancellationToken)
        {
            var result = await _contactService.DeleteAsync(contactId, cancellationToken);

            if (!result.IsValid)
            {
                var resultModel = _deleteResultFactory.CreateResult(result);
                return BadRequest(resultModel);
            }

            return Ok();
        }
    }

}
