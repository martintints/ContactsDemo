namespace Contacts.BL.DTOs.Contact
{
    public class CreateUpdateContactDto
    {
        public bool IsUpdate { get; set; }
        public int? ContactId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public decimal Sequence { get; set; }
    }
}
