using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
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
        public  string Name { get; set; }
        public  string Url { get; set; }
        public  ICollection<Subject> Programs { get; set; }
        public  ICollection<Subject> Themess { get; set; }
        public ICollection<SubjectBook> Literatures { get; set; }
        public ICollection<SubjectBook> AdditionalLiteratures { get; set; }


    }
}
