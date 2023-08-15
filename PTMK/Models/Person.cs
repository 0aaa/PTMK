using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PTMK.Models
{
    [Table("Person")]
    internal class Person
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required(ErrorMessage = "Surname required")]
        public string Surname { get; set; } = "";

        [Required(ErrorMessage = "Name required")]
        public string Name { get; set; } = "";

        [Required(ErrorMessage = "Patronymic required")]
        public string Patronymic { get; set; } = "";

        [Required(ErrorMessage = "DoB required")]
        public DateTime DoB { get; set; }

        [Required(ErrorMessage = "Sex required")]
        public bool Sex { get; set; }// "1" for Male and "0" for Female.
    }
}
