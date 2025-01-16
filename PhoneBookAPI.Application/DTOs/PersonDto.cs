using System;
using System.Collections.Generic;

namespace PhoneBookAPI.Application.DTOs
{
    public class PersonDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Company { get; set; }
        public List<ContactInfoDto> ContactInformation { get; set; }
    }

    public class CreatePersonDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Company { get; set; }
    public List<AddContactInfoDto> ContactInformation { get; set; } = new List<AddContactInfoDto>();
}

    public class UpdatePersonDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Company { get; set; }
    public List<AddContactInfoDto> ContactInformation { get; set; } = new List<AddContactInfoDto>();
}
}