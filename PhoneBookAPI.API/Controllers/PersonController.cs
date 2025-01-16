using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PhoneBookAPI.Application.DTOs;
using PhoneBookAPI.Application.Services;

namespace PhoneBookAPI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly IPersonService _personService;

        public PersonController(IPersonService personService)
        {
            _personService = personService;
        }

        [HttpPost]
        public async Task<ActionResult<PersonDto>> CreatePerson([FromBody] CreatePersonDto createPersonDto)
        {
            var result = await _personService.CreatePersonAsync(createPersonDto);
            return CreatedAtAction(nameof(GetPerson), new { id = result.Id }, result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PersonDto>> GetPerson(Guid id)
        {
            var person = await _personService.GetPersonByIdAsync(id);
            if (person == null)
                return NotFound();

            return Ok(person);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PersonDto>>> GetAllPersons()
        {
            var persons = await _personService.GetAllPersonsAsync();
            return Ok(persons);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePerson(Guid id, [FromBody] UpdatePersonDto updatePersonDto)
        {
            try
            {
                await _personService.UpdatePersonAsync(id, updatePersonDto);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerson(Guid id)
        {
            await _personService.DeletePersonAsync(id);
            return NoContent();
        }

        [HttpPost("{id}/contacts")]
        public async Task<ActionResult<PersonDto>> AddContactInfo(Guid id, [FromBody] AddContactInfoDto contactInfoDto)
        {
            try
            {
                var result = await _personService.AddContactInfoAsync(id, contactInfoDto);
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}/contacts/{contactId}")]
        public async Task<IActionResult> RemoveContactInfo(Guid id, Guid contactId)
        {
            try
            {
                await _personService.RemoveContactInfoAsync(id, contactId);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}