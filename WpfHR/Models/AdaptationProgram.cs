using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace WpfHR.Models
{
    public class AdaptationProgram
    {
        [Key]
        public int Id { get; set; }

        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string Department { get; set; }
        public string Position { get; set; }
        public DateTime StartDate { get; set; }
        public string FilePath { get; set; }
        public string SelectedModulesString { get; set; }
        public string MentorsString { get; set; }

        public int ErrorsCount { get; set; }
        public int CompletionPercentage { get; set; }
        public bool IsEmployed { get; set; }

        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; }

        [NotMapped]
        public List<Module> SelectedModules
        {
            get => string.IsNullOrEmpty(SelectedModulesString)
                ? new List<Module>()
                : SelectedModulesString.Split(';').Select(m => new Module { Name = m }).ToList();
            set => SelectedModulesString = value != null ? string.Join(";", value.Select(m => m.Name)) : null;
        }

        [NotMapped]
        public List<string> Mentors
        {
            get => string.IsNullOrEmpty(MentorsString)
                ? new List<string>()
                : MentorsString.Split(';').ToList();
            set => MentorsString = value != null ? string.Join(";", value) : null;
        }
    }
}
