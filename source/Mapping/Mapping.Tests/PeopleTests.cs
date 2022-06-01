using Mapping.Data;
using Mapster;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Mapping.Tests;

public class PeopleTests
{
    [Fact]
    public async Task People_Insert_Test_Expect_Data_To_Be_Present()
    {
        //arrange
        var builder = new DbContextOptionsBuilder<PeopleContext>()
            .UseSqlite("Data Source=people.db;");

        var context = new PeopleContext(builder.Options);

        var json = await File.ReadAllTextAsync("people.json");
        var peopleList = JsonSerializer.Deserialize<List<Person>>(json);

        foreach (var person in peopleList)
        {
            person.Id = Guid.NewGuid();
            context.People.Add(person);
        }

        //act
        await context.SaveChangesAsync();

        //assert
        var personActual = context.People.First();
        Assert.NotNull(personActual);
    }

    [Fact]
    public async Task GetPeople_Manual_Mapping()
    {
        //arrange
        var builder = new DbContextOptionsBuilder<PeopleContext>()
            .UseSqlite("Data Source=people.db;");

        var context = new PeopleContext(builder.Options);

        //act
        var peopleList = await context.People.Select(p => new PersonDto { 
            EmailAddress = p.EmailAddress, 
            FirstName = p.FirstName, 
            Id = p.Id, 
            LastName = p.LastName }).ToListAsync();

        //assert
        Assert.NotNull(peopleList);
    }

    [Fact]
    public async Task GetPeopleList_Mapster_Mapping()
    {
        //arrange
        var builder = new DbContextOptionsBuilder<PeopleContext>()
            .UseSqlite("Data Source=people.db;");

        var context = new PeopleContext(builder.Options);

        //act
        var peopleList = await context.People.ProjectToType<PersonDto>().ToListAsync();

        //assert
        Assert.NotNull(peopleList);
    }
}