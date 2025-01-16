using System;
using PhoneBookAPI.Core.Entities.Enums;

namespace PhoneBookAPI.Application.DTOs
{
    public class ContactInfoDto
    {
        public Guid Id { get; set; }
        public ContactType Type { get; set; }
        public string Value { get; set; }
    }

    public class AddContactInfoDto
    {
        public ContactType Type { get; set; }
        public string Value { get; set; }
    }
}