using JulMar.Windows.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using WebSearcher.Models;
using WindowsFormsApplication1.Models;

namespace WindowsFormsApplication1.ViewModels
{
    class MainViewModel : INotifyPropertyChanged
    {
        DispatcherTimer _dispatcherTimer;

        public ObservableCollection<Task<DownloadResult>> Tasks { get; set; }

        public ICommand VisitSelectedSiteCommand { get; set; }
        public ICommand PollClickCommand { get; set; }
        public ICommand StopClickCommand { get; set; }
        public ICommand FetchClickCommand { get; set; }
        public ICommand ExchangeRateClickCommand { get; set; }

        public WebPageFactory WebPageFactory { get; set; }

        // Don't need to implement INotifyPropertyChanged because here View affects ViewModel (by "set").
        public WebPage SelectedPage { get; set; }

        public MainViewModel()
        {
            WebPageFactory = new WebPageFactory();

            Tasks = new ObservableCollection<Task<DownloadResult>>();

            //  DispatcherTimer setup
            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Tick += dispatcherTimer_Tick;
#if DEBUG
            _dispatcherTimer.Interval = new TimeSpan(0, 0, 4);
#endif  
#if !DEBUG
            _dispatcherTimer.Interval = new TimeSpan(0, 6, 0);
#endif

            VisitSelectedSiteCommand = new DelegatingCommand(OnVisit, () => true);
            PollClickCommand = new DelegatingCommand(OnSearch, () => !_dispatcherTimer.IsEnabled);
            StopClickCommand = new DelegatingCommand(OnStop, () => _dispatcherTimer.IsEnabled);
            FetchClickCommand = new DelegatingCommand(fetch, () => true /* 用右邊寫法需focus特定的地方才會update UI: Tasks.Count == 0 */ );
            ExchangeRateClickCommand = new DelegatingCommand(OnExchangeRate, () => true);

            loadData();

            foreach (var webPage in WebPageFactory.WebPages)
                webPage.WebPageDetected += webPage_WebPageDetected;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<WebPageDetectedEventArgs> WebPageDetected = delegate { };

        private void OnPropertyChanged(string s)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(s));
        }

        private void loadData()
        {
            // The original sample code
            //foreach (var url in TheEntireInternet.Urls)
            //    WebPageFactory.AddNew(url, "Fox", (s) => s.Contains("Fox"));

            WebPageFactory.AddNew(
                "http://rate.bot.com.tw/Pages/Static/UIP003.zh-TW.htm",
                "USD: " + DollarRule.USD.Buy.ToString() + "/" + DollarRule.USD.Sell.ToString(), SearchAlgorithms.BuySellUSD);

#if !DEBUG

            WebPageFactory.AddNew(
                "http://rate.bot.com.tw/Pages/Static/UIP003.zh-TW.htm",
                "AUD: " + DollarRule.AUD.Buy.ToString() + " / " + DollarRule.AUD.Sell.ToString(),
            SearchAlgorithms.BuySellAUD);

            WebPageFactory.AddNew(
                "http://rate.bot.com.tw/Pages/Static/UIP003.zh-TW.htm",
                "NZD: " + DollarRule.NZD.Sell.ToString(), SearchAlgorithms.SellNZD);

            WebPageFactory.AddNew(
                "http://rate.bot.com.tw/Pages/Static/UIP003.zh-TW.htm",
                "EURO: " + DollarRule.EURO.Buy.ToString() + " / " + DollarRule.EURO.Sell.ToString(),
                SearchAlgorithms.BuySellEURO);

            WebPageFactory.AddNew(
                "http://rate.bot.com.tw/Pages/Static/UIP003.zh-TW.htm",
                "JPY: " + DollarRule.JPY.Buy.ToString() + " / " + DollarRule.JPY.Sell.ToString(),
                SearchAlgorithms.BuySellJPY);

            WebPageFactory.AddNew(
                "http://finance.yahoo.com/echarts?s=cia#chart4:symbol=cia;range=ytd;indicator=volume;charttype=line;crosshair=on;ohlcvalues=0;logscale=off;source=undefined",
                "CICA: " + DollarRule.CIA.Buy.ToString(), SearchAlgorithms.CicaStock);

            WebPageFactory.AddNew(
                "http://rate.bot.com.tw/Pages/Static/UIP005.zh-TW.htm",
                "Gold: " + DollarRule.Gold.Buy.ToString() + " / " + DollarRule.Gold.Sell.ToString(), SearchAlgorithms.GoldPrice);

            WebPageFactory.AddNew(
                "http://www.stockq.org/index/TWSE.php",
                "TAIEX: 6900", SearchAlgorithms.TAIEX);

            WebPageFactory.AddNew(
                "http://finance.yahoo.com/q;_ylt=AwrTWf0xGM9ToWAADVmTmYlQ?s=^DJI",
                "Dow Jones: " + DollarRule.DowJones.Buy.ToString() + " / " + DollarRule.DowJones.Sell.ToString(), SearchAlgorithms.AmericaStock);

#endif
        }

        private void webPage_WebPageDetected(object sender, WebPageDetectedEventArgs e)
        {
            this.WebPageDetected(sender, e);
        }

        // when double-clicking the url, open the url by web browser
        private void OnVisit()
        {
            Process.Start(
                //@"C:\Users\euleramon\Desktop\Jeff進度\cvas.png" // test
                SelectedPage.SiteUrl
                );
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            fetch();
        }

        private async Task search()
        {
            // 這個foreach的操作在執行await Task.WhenAny之前，
            // 會真的呼叫到webPage.DownloadAndSearchAsync()裡面的內容，
            // 但何時會執行完呢？ 依據當時用debug trace的結果，會在函式裡遇到第一個await做完就先跳出來，
            // 但是[1]有提過，如果main thread還是執行在code的階段，cpu就不會切到task thread去執行，
            // 所以還不會真正的下載，除非等到main thread從code的執行期切到UI的，下載才會真正開始。
            // 所以[2]中提到的兩種作法，若我們將若break point是設在[3]，
            // 若是採第一種(在第一行)在main thread code執行的作法，則break point永遠到不了；
            // 但若用第二種(在第二行)此時main thread在await那個跑很多次的for loop之後就會切到UI了，
            // 所以程式之後會跑到break point的地方。
            foreach (var webPage in WebPageFactory.WebPages)
                Tasks.Add(webPage.DownloadAndSearchAsync());

            // **[2]
            //Thread.Sleep(50000); // in main thread code
            //await Task.Run(() => { for (int i = 0; i < 10000000000; i++) { } }); // in task thread code

            while (Tasks.Count > 0)
            {
                Task<DownloadResult> finishedTask = await Task.WhenAny<DownloadResult>(Tasks);

                Tasks.Remove(finishedTask);
            }

            WebPageFactory.HtmlDataMap.Clear();
        }

        void fetch()
        {
            if (Tasks.Count == 0)
                search();
        }

        public async void OnSearch()
        {
            _dispatcherTimer.Start();
        }

        void OnStop()
        {
            _dispatcherTimer.Stop();
        }

        void OnExchangeRate()
        {
            Process.Start(new ProcessStartInfo(@"https://www.google.com.tw"));
        }

    }

}
