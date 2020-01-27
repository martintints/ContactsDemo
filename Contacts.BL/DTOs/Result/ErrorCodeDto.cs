namespace Contacts.BL.DTOs.Result
{
    public enum ErrorCodeDto
    {
        GeneralInternalError,
        GeneralNotFound,
        GeneralForbidden,
        GeneralValidationError,
        /// <summary>
        /// Concurrent update of a particular table row has occurred.
        /// </summary>
        GeneralConcurrencyConflict,
        GeneralAlreadyExists
    }
}
