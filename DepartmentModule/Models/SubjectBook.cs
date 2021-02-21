using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DepartmentModule.Models
{
    public class SubjectBook
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SubjectBookID { get; set; }
       [Key]
        public int SubjectId { get; set; }
        [ForeignKey("SubjectId")]
        public Subject Subject { get; set; }
        [Key]
        public int BookId { get; set; }
        [ForeignKey("BookId")]
        public Book Book { get; set; }
    }
}
