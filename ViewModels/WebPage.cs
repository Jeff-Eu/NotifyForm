using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSearcher.Models;
using WindowsFormsApplication1.Models;

namespace WindowsFormsApplication1.ViewModels
{
    enum SiteStates { Detected, NotDetected, SiteCorrupted, Standby }

    class WebPage : INotifyPropertyChanged
    {
        string _siteUrl;
        string _title = "Dummy Title";
        string _detectedString;
        SiteStates _siteState;
        string _nameOfSearchAlgorithm = "Dummy Algorithm";
        DateTime _updatedDateTime;

        static HtmlToText _html2TextConverter = new HtmlToText();

        Func<PageData, bool> _searchAlgorithm;

        public string SiteUrl
        {
            get { return _siteUrl; }
        }

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged("Title");
            }
        }

        public string DetectedString
        {
            get { return _detectedString; }
            set
            {
                _detectedString = value;
                OnPropertyChanged("DetectedString");
            }
        }

        public SiteStates SiteState
        {
            get { return _siteState; }
            set
            {
                _siteState = value;
                OnPropertyChanged("SiteState");
            }
        }

        public string NameOfSearchAlgorithm
        {
            get { return _nameOfSearchAlgorithm; }
            set
            {
                _nameOfSearchAlgorithm = value;
                OnPropertyChanged("NameOfSearchAlgorithm");
            }
        }

        public DateTime UpdatedDateTime
        {
            get { return _updatedDateTime; }
            set
            {
                _updatedDateTime = value;
                OnPropertyChanged("UpdatedDateTime");
            }
        }

        // constructor
        internal WebPage(string url, string nameOfSearchAlgorithm, Func<PageData, bool> searchAlgorithm)
        {
            _siteUrl = url; 
            _siteState = SiteStates.Standby;
            _nameOfSearchAlgorithm = nameOfSearchAlgorithm;
            _searchAlgorithm = searchAlgorithm;

        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<WebPageDetectedEventArgs> WebPageDetected = delegate { };

        public async Task<DownloadResult> DownloadAndSearchAsync()
        {
            DownloadResult result = new DownloadResult();
            //result.Url = _siteUrl;

            if (WebPageFactory.HtmlDataMap.ContainsKey(_siteUrl))
            {
                // **[1] 如果這行不用Task.Run，整個程式會hang住，
                // 猜想是main thread只有跑到UI的地方(Debug看不到的地方)，
                // 才能被結束的Task thread中斷回code的地方，如果main thread跑在code的地方
                // 是沒辦法切到Task thread去做事的。另外Task threads之間是並行的(同步)，不會被彼此中斷。
                // 也就是說，main thread跑在code中時，它不能切到Task thread去做事情。
                // 因為這是async的方式，從頭到尾只有一個cpu做多重角色的扮演，不是真正的並行同步，
                // 若是真正的並行同步，main thread跑在code時，Task thread也能同時在做事情，
                // 所以這裡講的同步非同步，是指main thread跟Task thread利用同一個cpu去跑而言。
                // Task thread不會永遠跑不完，但main thread永遠不會停止，因此main thread
                // 是"非同步"去等待 Task thread 做完事情回報；而非main thread 做完向Task thread回報，
                // 所以main thread是老大。
                // 結論：
                // main thread 對 task thread 是非同步的
                // task thread 對 task thread 之間是同步的
                await Task.Run(() => { while (WebPageFactory.HtmlDataMap[_siteUrl] == null);});
                result.Data = WebPageFactory.HtmlDataMap[_siteUrl];
            }
            else
            {
                try
                {
                    WebPageFactory.HtmlDataMap.Add(_siteUrl, null);
                    result.Data = await Downloader.DownloadHtmlAsyncTask(_siteUrl);
                    WebPageFactory.HtmlDataMap[_siteUrl] = result.Data;
                }
                catch (Exception e)
                {
                    //await Task.Run(() => { while (WebPageFactory.HtmlDataMap[_siteUrl] == null);});
                    Console.WriteLine("nooooooooooooooooo0");
                    //result.Data = WebPageFactory.HtmlDataMap[_siteUrl];
                }
            }

            // **[3]
            PageData pageData = new PageData(result.Data);

            ///

            Title = SearchEngine.GetTitle(result.Data);

            if (result.Data == "")
                SiteState = SiteStates.SiteCorrupted;
            else
            {
                //string text = _html2TextConverter.ConvertHtml(result.Data);

                if (_searchAlgorithm(pageData))
                {
                    if (SiteState != SiteStates.Detected)
                    {
                        WebPageDetected(this, new WebPageDetectedEventArgs(this));
                        SiteState = SiteStates.Detected;
                    }
                }
                else
                    SiteState = SiteStates.NotDetected;

                DetectedString = pageData.DetectedString;
            }

            UpdatedDateTime = DateTime.Now;

            return result;
        }

        private void OnPropertyChanged(string s)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(s));
        }
    }
}
