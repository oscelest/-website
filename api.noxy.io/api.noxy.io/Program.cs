using api.noxy.io.Context;
using api.noxy.io.Hubs;
using api.noxy.io.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSignalR();
builder.Services.AddControllers();
builder.Services.AddDbContext<APIContext>(options => options.UseMySQL(builder.Configuration["ConnectionStrings:MySQL"]!));
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:Audience"],
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]!))
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

    setup.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });

    setup.AddSecurityRequirement(new OpenApiSecurityRequirement
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

WebApplication app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    APIContext dbContext = scope.ServiceProvider.GetRequiredService<APIContext>();
    dbContext.Database.EnsureDeleted();
    dbContext.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(cors);
app.UseAuthentication();
app.UseAuthorization();
app.UseWebSockets();
app.MapHub<GameHub>("/ws/game");
app.MapControllers();
app.Run();
