using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApplication1.Models;

namespace WindowsFormsApplication1.ViewModels
{
    class WebPageFactory
    {
        ObservableCollection<WebPage> _webPages;

        internal static Dictionary<string, string> HtmlDataMap = new Dictionary<string, string>();
        
        public ObservableCollection<WebPage> WebPages
        {
            get { return _webPages; }
        }

        public WebPageFactory()
        {
            _webPages = new ObservableCollection<WebPage>();
        }

        public WebPage AddNew(string url, string nameOfSearchAlgorithm, Func<PageData, bool> searchAlgorithm)
        {
            WebPage webpage = new WebPage(url, nameOfSearchAlgorithm, searchAlgorithm);
            
            _webPages.Add(webpage);

            return webpage;
        }

        public bool Remove(WebPage webPage)
        {
            return _webPages.Remove(webPage);
        }
    }
}
