using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Examen3.Data;

var builder = WebApplication.CreateBuilder(args);

// DbContext (opcional, solo si vas a usarlo)
// No se aplica migraciones automáticas para que no rompa en Railway si no hay DB
builder.Services.AddDbContext<Examen3Context>(options =>
{
    var dbUrl = Environment.GetEnvironmentVariable("DATABASE")
                ?? builder.Configuration.GetConnectionString("Examen3Context");

    if (!string.IsNullOrEmpty(dbUrl))
    {
        options.UseNpgsql(dbUrl);
    }
});

// Puerto dinámico que asigna Railway
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("Examen3",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

// Controllers y Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// HttpClients
builder.Services.AddHttpClient("ApiCliente");
builder.Services.AddHttpClient("Api1", c =>
{
    c.BaseAddress = new Uri("https://programacionweb2examen3-production.up.railway.app/");
});

var app = builder.Build();

// Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("Examen3");
app.UseAuthorization();

app.MapControllers();
app.Run();
