using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace WpfHR.Models
{

    public class Module
    {
        [Key]
        public int Id { get; set; }

        public string CodeName { get; set; }

        public string Name { get; set; }

        public string? DevelopersString { get; set; }

        public string? ApproversString { get; set; }

        public string? MainApprover { get; set; }

        public string? Position { get; set; }

        public string? Status { get; set; }

        public DateTime? Deadline { get; set; }

        public string? CustomMessage { get; set; }


        [NotMapped]
        public List<string> Developers
        {
            get => string.IsNullOrEmpty(DevelopersString)
                ? new List<string>()
                : DevelopersString.Split(',').Select(d => d.Trim()).ToList();
            set => DevelopersString = value != null ? string.Join(",", value) : null;
        }

        [NotMapped]
        public List<string> Approvers
        {
            get => string.IsNullOrEmpty(ApproversString)
                ? new List<string>()
                : ApproversString.Split(',').Select(a => a.Trim()).ToList();
            set => ApproversString = value != null ? string.Join(",", value) : null;
        }
        [NotMapped]
        public bool IsChecked { get; set; }
    }


}
