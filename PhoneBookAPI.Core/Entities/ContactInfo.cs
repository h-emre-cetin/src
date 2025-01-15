using System;
using PhoneBookAPI.Core.Entities.Enums;

namespace PhoneBookAPI.Core.Entities
{
    public class ContactInfo
    {
        public Guid Id { get; set; }
        public ContactType Type { get; set; }
        public string Value { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }

        public ContactInfo()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
            IsActive = true;
        }
    }
}