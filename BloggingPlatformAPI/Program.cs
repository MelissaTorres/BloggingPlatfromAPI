using Asp.Versioning;
using BloggingPlatformAPI.Automappers;
using BloggingPlatformAPI.DTOs;
using BloggingPlatformAPI.Helpers;
using BloggingPlatformAPI.Models;
using BloggingPlatformAPI.Repository;
using BloggingPlatformAPI.Services;
using BloggingPlatformAPI.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);
var _myCors = "AllowSpecificOrigins";

// Add services to the container.
// Repository
builder.Services.AddScoped<IRepository<Blog>, BlogRepository>();

// Entity Framework
builder.Services.AddDbContext<UserContext>(options => 
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("BlogConnection"));
});

builder.Services.AddDbContext<BlogContext>(options => 
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("BlogConnection"));
});

// Validators
builder.Services.AddScoped<IValidator<BlogInsertDTO>, BlogInsertValidator>();
builder.Services.AddScoped<IValidator<BlogUpdateDTO>, BlogUpdateValidator>();
builder.Services.AddScoped<IValidator<BlogUpdateDTO>, BlogPatchValidator>();

// Automappers
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Services
builder.Services.AddKeyedScoped<ICommonService<BlogDTO, BlogInsertDTO, BlogUpdateDTO>, BlogService>("blogService");

// Add versioning
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = ApiVersionReader.Combine( 
        new HeaderApiVersionReader("X-API-Version"),
        new MediaTypeApiVersionReader("ver"));
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddHsts(options =>
{
    options.MaxAge = TimeSpan.FromDays(365);
});

builder.Services.AddAuthorization();

builder.Services.AddAuthentication()
    .AddBearerToken(IdentityConstants.BearerScheme);

builder.Services.AddIdentityCore<User>()
    .AddEntityFrameworkStores<UserContext>()
    .AddApiEndpoints();

// cors
builder.Services.AddCors(options => 
{
    // this works with localhost - dev env only
    options.AddPolicy(name : _myCors,
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });

    // prod version
    //options.AddDefaultPolicy(builder =>
    //{
    //    builder.WithOrigins("www.example.com")
    //    .WithHeaders("X-API-Version")
    //    //.AllowAnyHeader()
    //    .AllowAnyMethod();
    //});
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();
}
else 
{
    app.UseHsts();
}

app.UseHttpsRedirection();

app.MapIdentityApi<User>();

app.UseCors(_myCors);

app.UseAuthorization();

app.UseAuthentication();

app.MapControllers();

app.Run();
