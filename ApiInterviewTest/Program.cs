using Infraestructure;
using Infraestructure.Auth;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Services;
using Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<JwtTokenService>();
// builder.Services.AddScoped<ClientValidationService>();


Infraestructure.Auth.JwtAuthentication.ConfigureJwtAuthentication(builder.Services, builder.Configuration);   //Auth.JwtAuthentication.ConfigureJwtAuthentication(builder.Services, builder.Configuration);



builder.Services.AddScoped<IService, UserService>(s =>
{
    return new UserService();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
