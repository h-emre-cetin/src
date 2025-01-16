using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using PhoneBookAPI.Infrastructure.Data;
using PhoneBookAPI.Infrastructure.Data.Configurations;
using PhoneBookAPI.Infrastructure.Repositories;
using PhoneBookAPI.Infrastructure.Cache;
using PhoneBookAPI.Application.Services;
using PhoneBookAPI.Core.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configure MongoDB
var mongoDbSettings = builder.Configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>();
builder.Services.AddSingleton(mongoDbSettings);
builder.Services.AddSingleton<MongoDbContext>();
builder.Services.AddScoped<IPersonRepository, PersonRepository>();

// Configure Redis
var redisConnectionString = builder.Configuration.GetValue<string>("RedisSettings:ConnectionString");
builder.Services.AddSingleton<ICacheService>(new RedisCacheService(redisConnectionString));

// Configure Application Services
builder.Services.AddScoped<IPersonService, PersonService>();

// Configure Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "PhoneBook API",
        Version = "v1",
        Description = "A simple phone book API with MongoDB and Redis"
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();