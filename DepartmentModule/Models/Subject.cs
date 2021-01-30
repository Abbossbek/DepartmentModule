using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DepartmentModule.Models
{
    public class Subject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Book Program { get; set; }
        public Book Themes { get; set; }
        public List<Book> Literatures { get; set; }
        public List<Book> AdditionalLiteratures { get; set; }
    }
}
