using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DepartmentModule.Models
{
    public class Subject
    {
        public int Id { get; set; }
        public IdentityUser User { get; set; }
        public string Name { get; set; }
        public Book Program { get; set; }
        public Book Themes { get; set; }
        public ICollection<Book> Literatures { get; set; }
        public ICollection<Book> AdditionalLiteratures { get; set; }
    }
}
