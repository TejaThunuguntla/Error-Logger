using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public class LogMV
    {
        public Type type { get; set; }
        
        public DateTime timestamp { get; set; }

        public string exception { get; set; }
        public int AppId { get; set; }

        public string description { get; set; }

        public enum Type { DEBUG, WARNING, ERROR };
        public enum REQUEST { GET, POST };
    }
}
