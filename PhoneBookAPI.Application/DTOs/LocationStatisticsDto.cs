namespace PhoneBookAPI.Application.DTOs
{
    public class LocationStatisticsDto
    {
        public string Location { get; set; }
        public int PersonCount { get; set; }
        public int PhoneNumberCount { get; set; }
    }
}