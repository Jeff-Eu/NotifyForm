using System;
using System.Net;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net.Http;
using System.Text;

namespace WebSearcher.Models
{
	class Downloader
	{
        // Often throwing exception even if the website exists.
        public async static Task<string> DownloadHtmlAsyncTaskWithHttpClient(string url)
        {
            Debug.WriteLine("Starting download for " + url);

            #region MyRegion

            //HttpResponseMessage response = await client.GetAsync("http://www.contoso.com/");
            //response.EnsureSuccessStatusCode();
            //string responseBody = await response.Content.ReadAsStringAsync();
            //// Above three lines can be replaced with new helper method in following line 
            //// string body = await client.GetStringAsync(uri);

            HttpClient client = new HttpClient();
            HttpResponseMessage response = null;
            string responseBody;
            try
            {
                //response = await client.GetAsync(url);
                responseBody = await client.GetStringAsync(url);
            }
            catch(Exception e)
            {
                int a;
            }
            response.EnsureSuccessStatusCode();
            if (response.IsSuccessStatusCode)
            {
                responseBody = await response.Content.ReadAsStringAsync();
                Debug.WriteLine("Finished download of " + url);            
            }
            else
            {
                responseBody = "";
                Debug.WriteLine("website is not found." + url);
            }

            #endregion

            return responseBody;
        }

		public async static Task<string> DownloadHtmlAsyncTask(string url)
		{
            Debug.WriteLine("Starting download for " + url);

            WebClient client = new WebClient();

            client.Encoding = Encoding.UTF8;

            var download = Task.Run<string>(
                () =>
                {
                    try { return client.DownloadString(url); }
                    catch 
                    {
                        Debug.WriteLine("website is not found." + url);
                        return ""; 
                    }
                });
            await download;

            Debug.WriteLine("Finished download of " + url);

            return download.Result;
		}

        public async static Task<string> DownloadHtmlAsyncTask2(string url)
        {
            Debug.WriteLine("Starting download for " + url);

            WebClient client = new WebClient();

            string download = await client.DownloadStringTaskAsync(url);

            Debug.WriteLine("Finished download of " + url);

            return download;
        }

		public static string DownloadHtml(string url)
		{
			Debug.WriteLine( "Starting download for " + url );

			WebClient client = new WebClient();
			var download = client.DownloadString( url );

			Debug.WriteLine( "Finished download of " + url );
			
			return download;
		}
	}
}
