using APICatalogo.Context;
using APICatalogo.DTO_s.Mapping;
using APICatalogo.Filters;
using APICatalogo.Models;
using APICatalogo.Repositories;
using APICatalogo.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "APICatalogo", Version = "v1" });
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
        {
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Bearer JWT"
        });

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
                new string[] {}
            }
        });
    }
);
var secretKey = builder.Configuration["JWT:SecretKey"]
    ?? throw new ArgumentException("Chave de segurança inválida!");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;//Verifica se o token informado é válido
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;//Se o token for inválido, ou não for informado, serão pedidas as credencias do usuário.
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateAudience = true, // valida o usuario
        ValidateIssuer = true, // valdia o emissor da chave de segurança
        ValidateLifetime = true, // valida o tempo de vida do token
        ValidateIssuerSigningKey = true, // valida a chave de assinatura do emissor
        ClockSkew = TimeSpan.Zero, // configura o tempo de resposta entre o servidor emissor do token e o receptor
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
}
);
builder.Services.AddIdentity<ApplicationUser, IdentityRole>().
    AddEntityFrameworkStores<AppDbContext>().
    AddDefaultTokenProviders();

builder.Services.AddAuthentication();
builder.Services.AddAuthentication("Bearer").AddJwtBearer();
var mySqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddScoped<ApiLoggingFilter>();
builder.Services.AddControllers(options => options.Filters.Add(typeof(ApiExceptionFilter)));
builder.Services.AddDbContext<AppDbContext>(options => options.UseMySql(mySqlConnection,ServerVersion.AutoDetect(mySqlConnection)));
//este comando configura o middleware de exceções seja habilitado. Ele será usado para exceções de nível controlador/action.
builder.Services.AddControllers(options => options.Filters.Add(typeof(ApiExceptionFilter))).
    AddJsonOptions(
    options =>
    options.JsonSerializerOptions.ReferenceHandler =
    ReferenceHandler.IgnoreCycles).AddNewtonsoftJson();
builder.Services.AddScoped<ICategoriaRepository, CategoriasRepository>();
builder.Services.AddScoped<IProdutosRepository, ProdutosRepository>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddAutoMapper(typeof(ProdutosDTOMappingExtension));

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
