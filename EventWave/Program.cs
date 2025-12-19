using EventWave.Data;
using EventWave.Models;
using EventWave.Repositories;
using EventWave.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDBContext>(options=>
{
    var connectionString = builder.Configuration.GetConnectionString("cnx");
    options.UseSqlServer(connectionString);
});


builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireUppercase = false;
    options.User.RequireUniqueEmail = true;
}).AddEntityFrameworkStores<ApplicationDBContext>()
                .AddDefaultTokenProviders();
builder.Services.AddAuthentication(options =>
  {
      options.DefaultAuthenticateScheme =
     JwtBearerDefaults.AuthenticationScheme;
      options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
  })
            .AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = builder.Configuration["JWT:issuer"],
                    ValidAudience = builder.Configuration["JWT:audience"],

                    IssuerSigningKey = new SymmetricSecurityKey(
           System.Text.Encoding.UTF8.GetBytes(
               builder.Configuration["JWT:SecretKey"]
           )
       ),

                    ClockSkew = TimeSpan.Zero
                };
            }) ;
builder.Services.AddCors(options =>
          options.AddPolicy("AllowAll",
          policy => policy.AllowAnyOrigin()
          .AllowAnyHeader().AllowAnyMethod()
          ));




builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<ISpeakerRepository, SpeakerRepository>();
builder.Services.AddScoped<ISpeakerService, SpeakerService>();
builder.Services.AddScoped<IProfileRepository, ProfileRepository>();
builder.Services.AddScoped<IProfileService, ProfileService>();
builder.Services.AddScoped<IRegistrationService, RegistrationService>();
builder.Services.AddScoped<IRegistrationRepository, RegistrationRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRegistrationRepository, RegistrationRepository>();
builder.Services.AddScoped<IWaitListRepository,WaitListRepository>();
builder.Services.AddScoped<IWaitListService, WaitListService>();
builder.Services.AddScoped<IVenueRepository, VenueRepository>();
builder.Services.AddScoped<IVenueService, VenueService>();



builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });


var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    await RoleSeeder.SeedAsync(roleManager);
}
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    AdminSeeder.SeedAdmin(services).GetAwaiter().GetResult();
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("AllowAll");
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
