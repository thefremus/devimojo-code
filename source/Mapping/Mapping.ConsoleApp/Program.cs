// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Mapping.Data;
using Mapster;
using Microsoft.EntityFrameworkCore;

var summary = BenchmarkRunner.Run(typeof(Program).Assembly);

public class PeopleService
{
    private readonly PeopleContext _context;
    public PeopleService()
    {
        var builder = new DbContextOptionsBuilder<PeopleContext>()
                            .UseSqlite("Data Source=people.db;");

        _context = new PeopleContext(builder.Options);
    }

    [Benchmark]
    public IList<PersonDto> GetPeopleListFromManualMapping() 
    {
        return _context.People.Select(p => new PersonDto
        {
            EmailAddress = p.EmailAddress,
            FirstName = p.FirstName,
            Id = p.Id,
            LastName = p.LastName
        }).ToList();
    }

    [Benchmark]
    public IList<PersonDto> GetPeopleListFromMapster()
    {
        return _context.People.ProjectToType<PersonDto>().ToList();
    }
}


