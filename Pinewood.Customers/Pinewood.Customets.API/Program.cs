using Microsoft.EntityFrameworkCore;
using Pinewood.Customers.API.Mappers;
using Pinewood.Customers.Db;
using Pinewood.Customers.Models.DbModels;
using Pinewood.Customers.Models.Interfaces;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string ApiKey = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddScoped<IRepository<Customer>, CustomerRepository>();

builder.Services.AddDbContext<PinewoodCustomersDbContext>(options => options.UseSqlServer(ApiKey));

builder.Services.AddDbContext<PinewoodCustomersDbContext>(options =>
    options.UseSqlServer(ApiKey));

builder.Services.AddScoped<DatabaseInitializer>();

builder.Services.AddAutoMapper(typeof(CustomerMappingProfile));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var initializer = scope.ServiceProvider.GetRequiredService<DatabaseInitializer>();
    initializer.Initialize();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();