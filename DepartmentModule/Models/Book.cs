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
    public class Book
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public  int BookID { get; set; }
        public string UserID { get; set; }
        [DisplayName("Nomi")]
        public  string Name { get; set; }
        [DisplayName("Url")]
        public string Url { get; set; }
        [DisplayName("O'quv dasturlari")]
        public ICollection<Subject> Programs { get; set; }
        [DisplayName("Mavzular")]
        public ICollection<Subject> Themess { get; set; }
        public ICollection<SubjectLiterature> Literatures { get; set; }
        public ICollection<SubjectAdditionalLiterature> AdditionalLiteratures { get; set; }


    }
}
