using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDbContext.Entity
{
    [Table("schedule_info")]
    public class ScheduleInfo
    {
        [Key]
        public Guid Id { get; set; }

        public string scheduleCode { get; set; }

        public string scheduleQty { get; set; }

        public string startTime { get; set; }

        public string endTime { get; set; }

        public string scheduleStatusCode { get; set; }
    }
}
