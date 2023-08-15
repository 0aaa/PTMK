using Microsoft.EntityFrameworkCore;
using PTMK.Data;
using PTMK.Models;

namespace PTMK.Controllers
{
    internal class PersonsRepository
    {
        private readonly PTMKDbContext _context;

        public PersonsRepository(PTMKDbContext context)
            => _context = context;

        public IEnumerable<Person> GetAll()
            => _context.Persons.ToList();

        public async void Add(string[] personView)
        {
            await _context.AddAsync(new Person
            {
                Surname = personView[0]
                , Name = personView[1]
                , Patronymic = personView[2]
                , DoB = Convert.ToDateTime(personView[3])
                , Sex = personView[4] == "Male"
            });
            _context.SaveChanges();
        }

        public IEnumerable<Person> GetByLinqQuery()
            => _context.Persons.FromSql($"SELECT * FROM dbo.Person WHERE sex = 'true' AND LEFT(surname, 1) = 'F'").ToList();
    }
}