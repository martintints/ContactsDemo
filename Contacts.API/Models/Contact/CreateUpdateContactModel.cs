namespace Contacts.API.Models.Contact
{
    public class CreateUpdateContactModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public decimal Sequence { get; set; }
    }
}
