using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DepartmentModule.Models
{
    public class SubjectAdditionalLiterature
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SubjectAdditionalLiteratureID { get; set; }
        public int? SubjectId { get; set; }
        [ForeignKey("SubjectId")]
        public virtual Subject Subject { get; set; }
        public int? AdditionalLiteratureId { get; set; }
        [ForeignKey("AdditionalLiteratureId")]
        public virtual Book AdditionalLiterature { get; set; }
    }
}
