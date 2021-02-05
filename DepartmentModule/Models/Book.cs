using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DepartmentModule.Models
{
    public class Book
    {
        public int Id { get; set; }
        public IdentityUser User { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }
}
