using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DepartmentModule.Models
{
    public class SubjectLiterature
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SubjectLiteratureID { get; set; }
        public int? SubjectId { get; set; }
        [ForeignKey("SubjectId")]
        public virtual Subject Subject { get; set; }
        public int? LiteratureId { get; set; }
        [ForeignKey("LiteratureId")]
        public virtual Book Literature { get; set; }
    }
}
