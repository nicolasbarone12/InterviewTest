using Infraestructure;
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


builder.Services.AddDataProtection()
       .UseCryptographicAlgorithms(new AuthenticatedEncryptorConfiguration()
       {
           EncryptionAlgorithm = EncryptionAlgorithm.AES_256_GCM,
           ValidationAlgorithm = ValidationAlgorithm.HMACSHA256
       });

builder.Services.AddSingleton<Encriptation>();

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

app.UseAuthorization();

app.MapControllers();

app.Run();
