using laundryServer.Business.Interfaces;
using laundryServer.Business;
using laundryServer.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Get the connection string from configuration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Register the repository
builder.Services.AddScoped<IConfigurationDAO, ConfigurationDAO>();
builder.Services.AddScoped<IMachineDAO>(provider => new MachineDAO(connectionString));

// Enregistrer d'autres services (ex. DAO, services business)
builder.Services.AddScoped<IConfigurationService, ConfigurationService>();
builder.Services.AddScoped<IMachineService, MachineService>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
