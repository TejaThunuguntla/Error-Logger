using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErrorLoggerModel
{
    using System.ComponentModel.DataAnnotations;
    public class Application
    {
        public Application()
        {
            users = new HashSet<User>();
        }

        [Key]
        public int AppId { get; set; }

        [Required]
        public string AppName { get; set; }

        [Required]
        public bool Enabled { get; set; }

        public virtual ICollection<User> users { get; set; }
    }
}