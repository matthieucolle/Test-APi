

using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using MinimalAPI.Data;
using Microsoft.OpenApi.Models;
using AutoMapper;
using MinimalAPI.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using MinimalAPI.Models;
using System.Data.SqlClient;
using MySqlConnector;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Partageons API",
        Description = "Partagez vos outils",
        Version = "v1"
    });
});



builder.Services.AddDbContext<AppDbContext>(options =>
{
    var sqlConBuillder = new MySqlConnectionStringBuilder();
    sqlConBuillder.ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    sqlConBuillder.UserID = builder.Configuration["User"];
    sqlConBuillder.Password = builder.Configuration["Password"];

    options.UseMySql(sqlConBuillder.ConnectionString + $"user={sqlConBuillder.UserID};Pwd={sqlConBuillder.Password};",
        ServerVersion.AutoDetect(sqlConBuillder.ConnectionString));
});

builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Partageons API V1");
    });
}

app.UseHttpsRedirection();

app.MapGet("api/v1/users", async (IUserRepo repo, IMapper mapper) =>
{
    var users = await repo.GetAllUsers();
    return Results.Ok(mapper.Map<IEnumerable<UserReadDto>>(users));
});

app.MapGet("api/v1/users/{id}", async (IUserRepo repo, IMapper mapper, int id) =>
{
    var user = await repo.GetUserById(id);
    if (user != null)
    {
        return Results.Ok(mapper.Map<UserReadDto>(user));
    }
    return Results.NotFound();
});


app.MapPost("api/v1/users", async (IUserRepo repo, IMapper mapper, UserCreateDto userCreateDto) =>
            {
                var userModel = mapper.Map<User>(userCreateDto);

                await repo.CreateUser(userModel);
                await repo.SaveChange();

                var userReadDto = mapper.Map<UserReadDto>(userModel);

                return Results.Created($"api/v1/users/{userReadDto.Id}", userReadDto);
            });


app.MapPut("api/v1/users/{id}", async (IUserRepo repo, IMapper mapper, int id, UserUpdateDto userUpdateDto) =>
{
    var user = await repo.GetUserById(id);
    if (user == null)
    {
        return Results.NotFound();
    }
    mapper.Map(userUpdateDto, user);
    await repo.SaveChange();
    return Results.NoContent();
});

app.MapDelete("api/v1/users/{id}", async (IUserRepo repo, IMapper mapper, int id) =>
{
    var user = await repo.GetUserById(id);
    if (user == null)
    {
        Results.NotFound();
    }

    repo.DeleteUser(user);

    await repo.SaveChange();

    return Results.NoContent();
});

app.Run();
