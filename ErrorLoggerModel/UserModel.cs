using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErrorLoggerModel
{
    using System.ComponentModel.DataAnnotations;

    public class User
    {
        public User()
        {
            apps = new HashSet<Application>();
        }

        [Key]
        public int UserId { get; set; }

        [Required]
        public string mailID { get; set; }

        [Required]
        public string firstname { get; set; }

        [Required]
        public string lastname { get; set; }

        public string phone { get; set; }

        [Required]
        public ROLE access { get; set; }

        [Required]
        public bool Status { get; set; }

        [Required]
        public DateTime LastLogin { get; set; }
        public enum ROLE { ADMIN, USER, NONE };
        public virtual ICollection<Application> apps { get; set; }        
    }
}
