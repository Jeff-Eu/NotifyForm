using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1.Models
{
    class PageData
    {
        public string Source { get; set; }
        public string DetectedString { get; set; }

        public PageData(string source)
        {
            Source = source;
        }
    }
}
