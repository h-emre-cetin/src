namespace PhoneBookAPI.Infrastructure.Data.Configurations
{
    public class MongoDbSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string PersonCollectionName { get; set; }
    }
}