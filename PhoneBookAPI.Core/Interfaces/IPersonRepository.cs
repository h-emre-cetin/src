using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PhoneBookAPI.Core.Entities;

namespace PhoneBookAPI.Core.Interfaces
{
    public interface IPersonRepository : IRepository<Person>
    {
        Task<IEnumerable<Person>> GetByLocationAsync(string location);
        Task<Dictionary<string, (int PersonCount, int PhoneNumberCount)>> GetLocationStatisticsAsync();
    }
}