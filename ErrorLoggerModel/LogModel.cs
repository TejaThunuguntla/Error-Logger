using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErrorLoggerModel
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public class Log
    {
        [Key]
        public int LogId { get; set; }

        public int AppId { get; set; }

        [ForeignKey("AppId")]
        public virtual Application App { get; set; }

        [Required]
        public Type type { get; set; }
        
        [Required]
        public DateTime timestamp { get; set; }

        public string exception { get; set; }

        public string description { get; set; }

        public enum Type { DEBUG, WARNING, ERROR };
    }
}
