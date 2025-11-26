using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Examen3.Data;

var url = Environment.GetEnvironmentVariable("DATABASE");
Console.WriteLine($"La conexión a la DB es: {url}");

var builder = WebApplication.CreateBuilder(args);

// DbContext usando variable de entorno
builder.Services.AddDbContext<Examen3Context>(options =>
{
    if (!string.IsNullOrEmpty(url))
    {
        options.UseNpgsql(url);
    }
    else
    {
        // Fallback local
        options.UseNpgsql(builder.Configuration.GetConnectionString("Examen3Context")
            ?? throw new InvalidOperationException("Connection string 'Examen3Context' not found."));
    }
});

// Exponer puerto 8080 para contenedores
builder.WebHost.UseUrls("http://0.0.0.0:8080");

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("Examen3",
        builder =>
        {
            builder.AllowAnyOrigin()
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

// Aplicar migraciones automáticamente
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<Examen3Context>();
    db.Database.Migrate();
}

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
