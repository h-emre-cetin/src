using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PhoneBookAPI.Application.DTOs;

namespace PhoneBookAPI.Application.Services
{
    public interface IPersonService
    {
        Task<PersonDto> CreatePersonAsync(CreatePersonDto createPersonDto);
        Task<PersonDto> GetPersonByIdAsync(Guid id);
        Task<IEnumerable<PersonDto>> GetAllPersonsAsync();
        Task UpdatePersonAsync(Guid id, UpdatePersonDto updatePersonDto);
        Task DeletePersonAsync(Guid id);
        Task<PersonDto> AddContactInfoAsync(Guid personId, AddContactInfoDto contactInfoDto);
        Task RemoveContactInfoAsync(Guid personId, Guid contactInfoId);
        Task<IEnumerable<LocationStatisticsDto>> GetLocationStatisticsAsync();
    }
}