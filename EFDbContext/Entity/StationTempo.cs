using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDbContext.Entity
{
    [Table("station_tempo")]
    public class StationTempo
    {
        [Key]
        public string StationCode { get; set; }

        /// <summary>
        /// 理论节拍
        /// </summary>
        public int TheoreticalTempo { get; set; }
        /// <summary>
        /// 平均节拍
        /// </summary>
        public float AverageTempo { get; set; }
        /// <summary>
        /// 节拍数
        /// </summary>
        public int TempoCount { get; set; }
        /// <summary>
        /// 机器节拍
        /// </summary>
        public int MachineTempo { get; set; }
    }
}
