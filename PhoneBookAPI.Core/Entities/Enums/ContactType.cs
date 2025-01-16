using System.Text.Json;
using System.Text.Json.Serialization;

namespace PhoneBookAPI.Core.Entities.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ContactType
    {
        PhoneNumber,
        Email,
        Location
    }
}