using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ide
{
    public class ProjectProperties
    {
        public string projectPath { get; set; }
        public string compiler { get; set; }
        public string extension { get; set; }
        public string projectName { get; set; }
        public string currentFile { get; set; }
    }
}
