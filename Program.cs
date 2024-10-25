using Bogus;
using EFCore.Relationships.Youtube.Context;
using EFCore.Relationships.Youtube.Dtos;
using EFCore.Relationships.Youtube.Models;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
//service registration

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    string connectionString = builder.Configuration.GetConnectionString("SqlServer")!;
    options.UseSqlServer(connectionString);
    options.LogTo(Console.WriteLine, LogLevel.Information);
});

builder.Services.AddControllers().AddOData(options =>
{
    options.EnableQueryFeatures();
});

var app = builder.Build();
//middleware


app.MapPost("user-create", (ApplicationDbContext context, UserCreateDto request) =>
{
    User user = new()
    {
        FirstName = request.FirstName,
        LastName = request.LastName,
        UserInformation = new()
        {
            IdentityNumber = request.IdentityNumber,
            FullAddress = request.FullAddress,
        }
    };


    context.Add(user);

    context.SaveChanges();

    return Results.Ok(user);
});

app.MapGet("user-getall", (ApplicationDbContext context) =>
{
    var users = context.Users
    .Include(p => p.UserInformation)
    .Select(p => new
    {
        Id = p.Id,
        FullName = p.FullName,
        IdentityNumber = p.UserInformation!.IdentityNumber,
        FullAddress = p.UserInformation.FullAddress,
    })
    .AsQueryable();

    //var users = context.Users
    //.Join(context.UsersInformations,
    //user => user.UserInformationId,
    //userInformation => userInformation.Id,
    //(user, userInformation) => new { user, userInformation })
    //.Select(p => new
    //{
    //    Id = p.user.Id,
    //    FullName = p.user.FullName,
    //    IdentityNumber = p.userInformation.IdentityNumber,
    //    FullAddress = p.userInformation.FullAddress,
    //})
    //.ToList();

    //var users = (from u in context.Users
    //             join i in context.UsersInformations on u.UserInformationId equals i.Id
    //             select new
    //             {
    //                 Id = u.Id,
    //                 FullName = u.FullName,
    //                 IdentityNumber = i.IdentityNumber,
    //                 FullAddress = i.FullAddress,
    //                 Note = "This created by from"
    //             }).ToList();

    return users;
});

app.MapGet("user-seed-data", (ApplicationDbContext context) =>
{
    for (int i = 0; i < 100; i++)
    {
        Faker faker = new();
        string identityNumber = new Random().Next(1, 999).ToString();
        User user = new()
        {
            FirstName = faker.Person.FirstName,
            LastName = faker.Person.LastName,
            UserInformation = new()
            {
                IdentityNumber = identityNumber,
                FullAddress = faker.Person.Address.City
            }
        };

        context.Add(user);
    }

    context.SaveChanges();

    return Results.NoContent();
});

app.MapControllers();

app.Run();
