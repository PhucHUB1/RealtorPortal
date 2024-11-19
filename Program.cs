using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using RealEstate.Models;
using RealEstate.Services;
using RealEstate.Services.Implement;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

// Configure database context
builder.Services.AddDbContext<RealEstateContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure controllers with JSON options
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Register services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IListingsService, ListingsService>();

// Configure JWT authentication
var jwtKey = builder.Configuration["Jwt:Secret"];
var key = Encoding.ASCII.GetBytes(jwtKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        RoleClaimType = ClaimTypes.Role 
    };
});

// Configure Swagger for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SchemaGeneratorOptions = new SchemaGeneratorOptions
    {
        SchemaIdSelector = type => type.FullName
    };

    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your token in the text input below.\n\nExample: \"Bearer abcdef12345\""
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// Seed default roles and admin user
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<RealEstateContext>();
    await SeedRolesAndAdminUserAsync(context);
}

// Configure middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

// Helper function to seed roles and admin user
static async Task SeedRolesAndAdminUserAsync(RealEstateContext context)
{
    await context.Database.EnsureCreatedAsync();

    // Ensure Admin role exists
    var adminRole = await context.Roles.AsNoTracking().FirstOrDefaultAsync(r => r.Name == "Admin");
    if (adminRole == null)
    {
        adminRole = new Role
        {
            Name = "Admin",
            CreatedAt = DateTime.Now,
        };
        await context.Roles.AddAsync(adminRole);
        await context.SaveChangesAsync();
    }

    // Ensure Admin user exists
    if (!await context.Users.AnyAsync(u => u.RoleId == adminRole.Id))
    {
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword("admin123");
        await context.Users.AddAsync(new User
        {
            Email = "admin@realestate.com",
            Username = "admin",
            Password = hashedPassword,
            Name = "Administrator",
            RoleId = adminRole.Id,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        });
        await context.SaveChangesAsync();
    }

    // Ensure User role exists
    var userRole = await context.Roles.AsNoTracking().FirstOrDefaultAsync(r => r.Name == "User");
    if (userRole == null)
    {
        userRole = new Role
        {
            Name = "User",
            CreatedAt = DateTime.Now,
        };
        await context.Roles.AddAsync(userRole);
        await context.SaveChangesAsync();
    }
}
