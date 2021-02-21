using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DepartmentModule.Models
{
    public class Subject
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public  int SubjectID { get; set; }
        public  string UserID { get; set; }
        public  string Name { get; set; }
        public int? ProgramID { get; set; }
        [ForeignKey("ProgramID")]
        public virtual Book Program { get; set; }
        public int? ThemesID { get; set; }
        [ForeignKey("ThemesID")]
        public virtual Book Themes { get; set; }
        public ICollection<SubjectBook> Literatures { get; set; }
        public  ICollection<SubjectBook> AdditionalLiteratures { get; set; }
    }
}
