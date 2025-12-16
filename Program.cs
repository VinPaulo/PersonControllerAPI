using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Person.Data;
using Person.Routes;
using System.Text;

// Cria o esqueleto da aplicação
var builder = WebApplication.CreateBuilder(args);

// Configurações Gerais
// Carrega as configurações do arquivo appsettings.json
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Define as configurações do JWT (o token de autenticação)
var jwtSettings = new JwtSettings();
builder.Configuration.Bind("JwtSettings", jwtSettings);
// Fallback para variáveis de ambiente (comum em Docker/Render)
if (string.IsNullOrEmpty(jwtSettings.Key))
    jwtSettings.Key = Environment.GetEnvironmentVariable("JwtSettings__Key") ?? "MinhaChaveSuperSecretaDe32Caracteres!";
if (string.IsNullOrEmpty(jwtSettings.Issuer))
    jwtSettings.Issuer = Environment.GetEnvironmentVariable("JwtSettings__Issuer") ?? "PersonApi";
if (string.IsNullOrEmpty(jwtSettings.Audience))
    jwtSettings.Audience = Environment.GetEnvironmentVariable("JwtSettings__Audience") ?? "PersonApiUser";


// Registra o contexto do banco
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

if (!string.IsNullOrEmpty(databaseUrl))
{
    // Parse DATABASE_URL for Render.com
    try 
    {
        var uri = new Uri(databaseUrl);
        var db = uri.AbsolutePath.Trim('/');
        var userInfo = uri.UserInfo.Split(':');
        var password = userInfo.Length > 1 ? userInfo[1] : string.Empty;
        var user = userInfo[0];
        connectionString = $"Host={uri.Host};Port={uri.Port};Database={db};Username={user};Password={password};Pooling=true;SSL Mode=Require;Trust Server Certificate=True;";
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro ao fazer parse da DATABASE_URL: {ex.Message}");
    }
}

if (!string.IsNullOrEmpty(connectionString))
{
    builder.Services.AddDbContext<PersonContext>(options =>
        options.UseNpgsql(connectionString));
    Console.WriteLine("Usando PostgreSQL.");
}
else
{
    builder.Services.AddDbContext<PersonContext>(options =>
        options.UseInMemoryDatabase("PersonDb"));
    Console.WriteLine("Usando Banco em Memória.");
}

builder.Services.AddScoped<PersonContext>();

// Configuração do Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // Configurações básicas da documentação
    c.SwaggerDoc("v1", new OpenApiInfo { 
        Title = "Controle de Pessoas", 
        Version = "v1",
        Description = "API para gestão de cadastro de pessoas"
    });
    
    // Configura como o Swagger vai lidar com autenticação JWT
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Insira o token JWT no campo abaixo."
    });
    
    // Adiciona a exigência de segurança na documentação
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Configuração de Autenticação e Autorização
// Configura a autenticação JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // Define as regras de validação do token
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,                 // Verifica quem emitiu
            ValidateAudience = true,               // Verifica para quem foi emitido
            ValidateLifetime = true,               // Verifica se ainda é válido (não expirou)
            ValidateIssuerSigningKey = true,       // Verifica a assinatura
            ValidIssuer = jwtSettings.Issuer,      // Emissor válido
            ValidAudience = jwtSettings.Audience,  // Audiência válida
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),  // Chave de assinatura
            ClockSkew = TimeSpan.Zero              // Sem tolerância de tempo (normalmente tem uns 5 min)
        };
    });

// Adiciona o serviço de autorização
builder.Services.AddAuthorization();

// Configuração de CORS (Cross-Origin Resource Sharing)
// Permite que outros sites/aplicações acessem nossa API
builder.Services.AddCors(options =>
{
    // "AllowAll" é uma política bem permissiva - para produção, seria melhor restringir mais
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()        // Qualquer site pode acessar
              .AllowAnyMethod()        // Qualquer método (GET, POST, etc.)
              .AllowAnyHeader());      // Qualquer cabeçalho HTTP
});

// Adiciona suporte a controllers tradicionais
builder.Services.AddControllers();

// Constrói a aplicação com as configurações definidas acima
var app = builder.Build();

// Ferramentas para Ambiente de Desenvolvimento e Produção (para facilitar testes)
app.UseSwagger();          
app.UseSwaggerUI();

// Configuração do Pipeline de Requisições HTTP
// app.UseHttpsRedirection();    // Redireciona HTTP para HTTPS
app.UseCors("AllowAll");      // Aplica a política de CORS que definimos
app.UseAuthentication();      // Ativa a autenticação
app.UseAuthorization();       // Ativa a autorização

// Redireciona a raiz para o Swagger
app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();

// Mapeamento de Rotas
app.MapControllers();         // Mapeia os controllers tradicionais
app.MapPersonEndpoints();     // Mapeia os endpoints específicos de Person

// Inicia a aplicação
app.Run();

// Uma classe simples pra guardar as configurações do JWT
public class JwtSettings
{
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public string Key { get; set; } = string.Empty;
}