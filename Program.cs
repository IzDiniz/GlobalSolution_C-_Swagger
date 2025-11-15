using Microsoft.EntityFrameworkCore;
using AirQualityAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// Adicionar serviços ao container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Air Quality API",
        Version = "v1",
        Description = "API RESTful para monitoramento de qualidade do ar - FIAP",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Equipe de Desenvolvimento",
            Email = "contato@airquality.com"
        }
    });

    // Incluir comentários XML na documentação Swagger
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

// Configurar o contexto do banco de dados
// Para desenvolvimento, usamos InMemory. Para produção, configure SQL Server no appsettings.json
builder.Services.AddDbContext<AirQualityContext>(options =>
{
    // Usar banco de dados em memória para facilitar testes
    options.UseInMemoryDatabase("AirQualityDB");
    
    // Para usar SQL Server, descomente as linhas abaixo e configure a connection string
    // var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    // options.UseSqlServer(connectionString);
});

// Configurar CORS para permitir requisições de diferentes origens
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Inicializar o banco de dados com dados de exemplo
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AirQualityContext>();
    context.Database.EnsureCreated();
}

// Configurar o pipeline de requisições HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Air Quality API v1");
        c.RoutePrefix = string.Empty; // Swagger na raiz da aplicação
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

app.Run();
