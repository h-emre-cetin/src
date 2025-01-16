using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PhoneBookAPI.Application.DTOs;
using PhoneBookAPI.Core.Entities;
using PhoneBookAPI.Core.Interfaces;

namespace PhoneBookAPI.Application.Services
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _personRepository;
        private readonly ICacheService _cacheService;
        private const string LocationStatsCacheKey = "location_stats";

        public PersonService(IPersonRepository personRepository, ICacheService cacheService)
        {
            _personRepository = personRepository;
            _cacheService = cacheService;
        }

        public async Task<PersonDto> CreatePersonAsync(CreatePersonDto createPersonDto)
        {
            var person = new Person
            {
                FirstName = createPersonDto.FirstName,
                LastName = createPersonDto.LastName,
                Company = createPersonDto.Company,
                ContactInformation = createPersonDto.ContactInformation.Select(ci => new ContactInfo
                {
                    Type = ci.Type,
                    Value = ci.Value
                }).ToList()
            };

            await _personRepository.AddAsync(person);
            return MapToDto(person);
        }

        public async Task<PersonDto> GetPersonByIdAsync(Guid id)
        {
            var person = await _personRepository.GetByIdAsync(id);
            return person != null ? MapToDto(person) : null;
        }

        public async Task<IEnumerable<PersonDto>> GetAllPersonsAsync()
        {
            var persons = await _personRepository.GetAllAsync();
            return persons.Select(MapToDto);
        }

        public async Task UpdatePersonAsync(Guid id, UpdatePersonDto updatePersonDto)
        {
            var person = await _personRepository.GetByIdAsync(id);
            if (person == null)
                throw new KeyNotFoundException($"Person with ID {id} not found.");

            person.FirstName = updatePersonDto.FirstName;
            person.LastName = updatePersonDto.LastName;
            person.Company = updatePersonDto.Company;
            person.ContactInformation = updatePersonDto.ContactInformation.Select(ci => new ContactInfo
            {
                Type = ci.Type,
                Value = ci.Value,
                IsActive = true
            }).ToList();

            await _personRepository.UpdateAsync(person);
            await InvalidateLocationStatsCache();
        }

        public async Task DeletePersonAsync(Guid id)
        {
            await _personRepository.DeleteAsync(id);
            await InvalidateLocationStatsCache();
        }

        public async Task<PersonDto> AddContactInfoAsync(Guid personId, AddContactInfoDto contactInfoDto)
        {
            var person = await _personRepository.GetByIdAsync(personId);
            if (person == null)
                throw new KeyNotFoundException($"Person with ID {personId} not found.");

            var contactInfo = new ContactInfo
            {
                Type = contactInfoDto.Type,
                Value = contactInfoDto.Value
            };

            person.ContactInformation.Add(contactInfo);
            await _personRepository.UpdateAsync(person);
            await InvalidateLocationStatsCache();

            return MapToDto(person);
        }

        public async Task RemoveContactInfoAsync(Guid personId, Guid contactInfoId)
        {
            var person = await _personRepository.GetByIdAsync(personId);
            if (person == null)
                throw new KeyNotFoundException($"Person with ID {personId} not found.");

            var contactInfo = person.ContactInformation.FirstOrDefault(c => c.Id == contactInfoId);
            if (contactInfo == null)
                throw new KeyNotFoundException($"Contact info with ID {contactInfoId} not found.");

            contactInfo.IsActive = false;
            await _personRepository.UpdateAsync(person);
            await InvalidateLocationStatsCache();
        }

        public async Task<IEnumerable<LocationStatisticsDto>> GetLocationStatisticsAsync()
        {
            var cachedStats = await _cacheService.GetAsync<IEnumerable<LocationStatisticsDto>>(LocationStatsCacheKey);
            if (cachedStats != null)
                return cachedStats;

            var stats = await _personRepository.GetLocationStatisticsAsync();
            var result = stats.Select(s => new LocationStatisticsDto
            {
                Location = s.Key,
                PersonCount = s.Value.PersonCount,
                PhoneNumberCount = s.Value.PhoneNumberCount
            }).OrderByDescending(s => s.PersonCount);

            await _cacheService.SetAsync(LocationStatsCacheKey, result, TimeSpan.FromMinutes(5));
            return result;
        }

        private async Task InvalidateLocationStatsCache()
        {
            await _cacheService.RemoveAsync(LocationStatsCacheKey);
        }

        private static PersonDto MapToDto(Person person)
        {
            return new PersonDto
            {
                Id = person.Id,
                FirstName = person.FirstName,
                LastName = person.LastName,
                Company = person.Company,
                ContactInformation = person.ContactInformation
                    .Where(c => c.IsActive)
                    .Select(c => new ContactInfoDto
                    {
                        Id = c.Id,
                        Type = c.Type,
                        Value = c.Value
                    }).ToList()
            };
        }
    }
}