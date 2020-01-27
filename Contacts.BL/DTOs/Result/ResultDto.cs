using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Contacts.Common.Extensions;
using FluentValidation.Results;

namespace Contacts.BL.DTOs.Result
{
    public class ResultDto<T>
    {
        private readonly IDictionary<ErrorCodeDto, string> _errors;
        /// <summary>
        /// Possible result errors. All error codes should be unique, ie. multiple messages can't use same error code.
        /// This requirement could be removed by using a list for values, but doesn't seem to be needed ATM.
        /// </summary>
        public IReadOnlyDictionary<ErrorCodeDto, string> Errors;
        public T Data { get; private set; }

        public ResultDto()
        {
            _errors = new Dictionary<ErrorCodeDto, string>();
            Errors = new ReadOnlyDictionary<ErrorCodeDto, string>(_errors);
        }

        public ResultDto(T data) : this()
        {
            Data = data;
        }

        public ResultDto(ValidationResult validationResult) : this()
        {
            AddError(validationResult);
        }

        public ResultDto(IDictionary<ErrorCodeDto, string> errors)
        {
            _errors = errors;
            Errors = new ReadOnlyDictionary<ErrorCodeDto, string>(_errors);
        }

        public bool IsValid => !_errors.Any();

        public void AddError(ErrorCodeDto errorCodeDto, string message = null)
        {
            // NB! Existing error with same code gets overwritten. Check comment above for reason.
            _errors[errorCodeDto] = message;
        }

        public void AddError(ValidationResult validationResult)
        {
            validationResult.Errors.ForEach(AddError);
        }

        /// <summary>
        /// This method currently assumes that only one error for same code is generated - underlying dictionary uses the Add()
        /// method which throws for multiple adds for same key.
        /// </summary>
        /// <param name="error"></param>
        public void AddError(ValidationFailure error)
        {
            if (error.ErrorCode != null && Enum.TryParse(typeof(ErrorCodeDto), error.ErrorCode, out var code))
            {
                AddError((ErrorCodeDto)code, error.ErrorMessage);
            }
            else
            {
                AddError(ErrorCodeDto.GeneralValidationError, error.ErrorMessage);
            }
        }

        public void AddData(T data)
        {
            Data = data;
        }
    }

}
