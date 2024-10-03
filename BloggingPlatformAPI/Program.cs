using BloggingPlatformAPI.Automappers;
using BloggingPlatformAPI.DTOs;
using BloggingPlatformAPI.Models;
using BloggingPlatformAPI.Repository;
using BloggingPlatformAPI.Services;
using BloggingPlatformAPI.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Repository
builder.Services.AddScoped<IRepository<Blog>, BlogRepository>();

// Entity Framework

builder.Services.AddDbContext<BlogContext>(options => 
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("BlogConnection"));
});

// Validators
builder.Services.AddScoped<IValidator<BlogInsertDTO>, BlogInsertValidator>();
builder.Services.AddScoped<IValidator<BlogUpdateDTO>, BlogUpdateValidator>();

// Automappers
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Services
builder.Services.AddKeyedScoped<ICommonService<BlogDTO, BlogInsertDTO, BlogUpdateDTO>, BlogService>("blogService");

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
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
