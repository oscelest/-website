using api.noxy.io.Context;
using api.noxy.io.Hubs;
using api.noxy.io.Interface;
using api.noxy.io.Utility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSignalR(options => options.EnableDetailedErrors = true);
builder.Services.AddControllers();
builder.Services.AddDbContext<APIContext>(options => options.UseMySQL(builder.Configuration["ConnectionStrings:MySQL"]!));
builder.Services.AddTransient<IJWT, JWT>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IGameRepository, GameRepository>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:Audience"],
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]!))
    };
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            if (context.HttpContext.Request.Path.StartsWithSegments("/ws/game"))
            {
                context.Token = context.Request.Query["access_token"];
            }
            return Task.CompletedTask;
        }
    };
});

string cors = "CORS Origin Policy";
string[] origins = builder.Configuration.GetSection("CORS:Origins").Get<string[]>()!;
string methods = builder.Configuration["CORS:Methods"]!;
string headers = builder.Configuration["CORS:Headers"]!;
builder.Services.AddCors(options => options.AddPolicy(cors, policy => policy.WithOrigins(origins).WithMethods(methods).WithHeaders(headers).AllowCredentials()));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(setup =>
{
    setup.SwaggerDoc("v1", new OpenApiInfo { Title = "api.noxy.io", Version = "v1" });
    setup.CustomSchemaIds(x => x.FullName?[((x.Namespace?.Length ?? 0) + 1)..].Replace("+", "") ?? x.Name);

    OpenApiReference reference = new() { Type = ReferenceType.SecurityScheme, Id = "Bearer" };
    OpenApiSecurityRequirement requirement = new() { { new OpenApiSecurityScheme { Reference = reference }, Array.Empty<string>() } };
    setup.AddSecurityRequirement(requirement);
    setup.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });
});

WebApplication app = builder.Build();

using (IServiceScope scope = app.Services.CreateScope())
{
    APIContext dbContext = scope.ServiceProvider.GetRequiredService<APIContext>();
    dbContext.Database.EnsureDeleted();
    dbContext.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.ConfigObject.AdditionalItems.Add("persistAuthorization", "true");
    });
}

app.UseCors(cors);
app.UseAuthentication();
app.UseAuthorization();
app.UseWebSockets();
app.MapHub<GameHub>("/ws/game");
app.MapControllers();
app.Run();
