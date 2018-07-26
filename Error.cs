using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ide
{
   public class Error
    {
        public int line { get; set; }
        public string path { get; set; }
        public string error { get; set; }

        public Error()
        {

        }

        public Error(int line, string path, string error)
        {
            this.line = line;
            this.path = path;
            this.error = error;
        }
    }
}
