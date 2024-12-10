using System.ComponentModel.DataAnnotations;

namespace WpfHR.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        public string? FullName { get; set; }

        public string? Department { get; set; }

        public string? Position { get; set; }
    }

}