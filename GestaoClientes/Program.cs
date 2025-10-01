using FluentValidation;
using GestaoClientes.Application.CasosDeUso.Clientes;
using GestaoClientes.Application.Interfaces;
using GestaoClientes.Application.Validations;
using GestaoClientes.Infra.Db;
using GestaoClientes.Infra.Repositorios;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddSingleton<IDbContext>(_ =>
    new SqlServerDbContext(builder.Configuration.GetConnectionString("Default")!));


builder.Services.AddScoped<ICriarCliente, CriarCliente>();
builder.Services.AddScoped<IObterScoreCliente, ObterScoreCliente>();
builder.Services.AddScoped<IClienteRepositorio, ClienteRepositorio>();


builder.Services.AddValidatorsFromAssemblyContaining<ClienteCriarRequestValidator>();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Gestão de Clientes API",
        Version = "v1",
        Description = "API para cadastro de clientes e cálculo de Score de Confiança."
    });

    c.EnableAnnotations();

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
        c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Gestão de Clientes v1");
        c.RoutePrefix = "swagger"; 
    });

}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
