using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDbContext.Entity
{
    [Table("exception_binding_information")]
    public class ExceptionBindingInformation
    {
        [Key]
        public Guid ID { get; set; } = Guid.NewGuid();
        public string snNumber { get; set; }
        public string exceptionType { get; set; }
        public string exceptionCode { get; set; }
        public string time { get; set; }
    }
}
