using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        [DisplayName("Nomi")]
        public string Name { get; set; }
        public int? ProgramID { get; set; }
        [ForeignKey("ProgramID")]
        [DisplayName("Fan dasturi")]
        public virtual Book Program { get; set; }
        public int? ThemesID { get; set; }
        [ForeignKey("ThemesID")]
        [DisplayName("Mavzular")]
        public virtual Book Themes { get; set; }
        [DisplayName("Adabiyotlar")]
        public ICollection<SubjectLiterature> Literatures { get; set; }
        [DisplayName("Qo'shimcha adabiyotlar")]
        public ICollection<SubjectAdditionalLiterature> AdditionalLiteratures { get; set; }
    }
}
