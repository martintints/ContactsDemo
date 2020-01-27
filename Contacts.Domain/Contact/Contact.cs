using Contacts.Domain.Common;

namespace Contacts.Domain.Contact
{
    public class Contact : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        // ReSharper disable once UnusedMember.Global
        public string FullName => $"{FirstName} {LastName}";
        public string Email { get; set; }
        public string Phone { get; set; }
        public decimal Sequence { get; set; }
    }
}
