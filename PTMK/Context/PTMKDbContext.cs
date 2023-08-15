using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PTMK.Models;

namespace PTMK.Data
{
    internal class PTMKDbContext : DbContext
    {
        public DbSet<Person> Persons { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlServer("Server=.\\;Initial catalog=PTMKDb;Integrated Security=true;MultipleActiveResultSets=true;App=PTMK;TrustServerCertificate=true");

        public IEnumerable<Person> GetBySqlDataReader()
        {
            using var connection = new SqlConnection("Server=.\\;Initial catalog=PTMKDb;Integrated Security=true;MultipleActiveResultSets=true;App=PTMK;TrustServerCertificate=true");
            connection.Open();
            var res = new List<Person>();
            var reader = new SqlCommand("SELECT * FROM dbo.Person WHERE sex = 'true' AND LEFT(surname, 1) = 'F'", connection).ExecuteReader();
            while (reader.Read())
            {
                res.Add(new()
                {
                    Surname = reader["Surname"].ToString()
                    , Name = reader["Name"].ToString()
                    , Patronymic = reader["Patronymic"].ToString()
                    , DoB = (DateTime)reader["DoB"]
                    , Sex = (bool)reader["Sex"]
                });
            }
            return res;
        }
    }
}