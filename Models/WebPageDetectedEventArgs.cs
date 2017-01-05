using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApplication1.ViewModels;

namespace WindowsFormsApplication1.Models
{
    class WebPageDetectedEventArgs : EventArgs
    {
        public WebPage WebPage { get; set; }
        public WebPageDetectedEventArgs(WebPage webPage)
        {
            WebPage = webPage;
        }
    }
}
