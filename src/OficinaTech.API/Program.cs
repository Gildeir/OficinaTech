using Microsoft.OpenApi.Models;
using OficinaTech.Application.Services;
using OficinaTech.Infrastructure.Repositories.Interfaces;
using OficinaTech.Infrastructure.Repositories;
using OficinaTech.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using OficinaTech.Infrastructure.Data.Context;
using OficinaTech.Domain.Interfaces;
using OficinaTech.Infrastructure.ExternalServices;
using OficinaTech.API.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Prioriza variáveis de ambiente antes do appsettings.json
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables(); 

// Ajuste da ConnectionString para carregar de variável de ambiente primeiro
var connectionString = builder.Configuration["DATABASE_URL"]
    ?? builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<OficinaTechDbContext>(options =>
    options.UseNpgsql(connectionString));


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "OficinaTech API",
        Version = "v1",
        Description = "API para gerenciamento de orçamentos e estoque de oficina",
        Contact = new OpenApiContact
        {
            Name = "Gildeir L. Rodrigues",
            Email = "gildeirlopes@gmail.com",
            Url = new Uri("https://github.com/gildeir")
        }
    });
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    });

builder.Services.AddHttpClient<ViaCepService>();
builder.Services.AddScoped<IViaCepService, ViaCepService>();

builder.Services.AddScoped<IOrcamentoService, OrcamentoService>();
builder.Services.AddScoped<IOrcamentoRepository, OrcamentoRepository>();

builder.Services.AddScoped<IPecaRepository, PecaRepository>();
builder.Services.AddScoped<IPecaService, PecaService>();

builder.Services.AddScoped<IOrcamentoPecaService, OrcamentoPecaService>();
builder.Services.AddScoped<IOrcamentoPecaRepository, OrcamentoPecaRepository>();

builder.Services.AddScoped<IMovimentacaoEstoqueRepository, MovimentacaoEstoqueRepository>();
builder.Services.AddScoped<IMovimentacaoEstoqueService, MovimentacaoEstoqueService>();
builder.Services.AddScoped<IServiceOrderRepository, ServiceOrderRepository>();
builder.Services.AddScoped<IServiceOrderService, ServiceOrderService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "OficinaTech API v1");
        c.RoutePrefix = ""; // Corrigiu swagger incompleto no navegador.
    });
}

app.UseAuthorization();

//0.0.0.0 é uma forma de permitir que a aplicação aceite conexões de qualquer IP - por exemplo: Docker, não apenas do localhost, 
//ou seja, você pode acessar o aplicativo de outras máquinas na mesma rede ou até de fora.
app.Urls.Add("http://0.0.0.0:80");

app.MapControllers();
app.Run();
