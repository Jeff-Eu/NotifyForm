using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace WindowsFormsApplication1.Models
{
    class SearchAlgorithms
    {
        public static bool BuySellUSD(PageData pageData)
        {
            string siteContent = pageData.Source;

            // Define the regular expression pattern. 
            string pattern = @"\b[0-9]*\.*[0-9]+\b";

            Regex regex = new Regex("<td class=\"decimal\">" + pattern + "</td>");
            var collection = regex.Matches(siteContent);

            /* The third and fourth item contain BUY and SELL of the USD dollar respectively. */
            regex = new Regex(pattern);

            string usd_buy_string = regex.Matches(collection[2].ToString())[0].ToString();
            string usd_sell_string = regex.Matches(collection[3].ToString())[0].ToString();

            pageData.DetectedString = usd_buy_string + "/" + usd_sell_string;

            /* convert to double */
            double usd_buy = Double.Parse(usd_buy_string);
            double usd_sell = Double.Parse(usd_sell_string);

            /* defined threshold of your rule */
            if (usd_sell < DollarRule.USD.Sell || usd_buy > DollarRule.USD.Buy)
                return true;

            return false;
        }

        public static bool BuySellAUD(PageData pageData)
        {
            string siteContent = pageData.Source;

            // Define the regular expression pattern. 
            string pattern = @"\b[0-9]*\.*[0-9]+\b";

            Regex regex = new Regex("<td class=\"decimal\">" + pattern + "</td>");
            var collection = regex.Matches(siteContent);

            regex = new Regex(pattern);

            string aud_buy_string = regex.Matches(collection[14].ToString())[0].ToString();
            string aud_sell_string = regex.Matches(collection[15].ToString())[0].ToString();

            pageData.DetectedString = aud_buy_string + "/" + aud_sell_string;

            /* convert to double */
            double aud_buy = Double.Parse(aud_buy_string);
            double aud_sell = Double.Parse(aud_sell_string);

            /* defined threshold of your rule */
            if (aud_sell < DollarRule.AUD.Sell || aud_buy > DollarRule.AUD.Buy)
                return true;

            return false;
        }

        public static bool SellNZD(PageData pageData)
        {
            string siteContent = pageData.Source;

            // Define the regular expression pattern. 
            string pattern = @"\b[0-9]*\.*[0-9]+\b";

            Regex regex = new Regex("<td class=\"decimal\">" + pattern + "</td>");
            var collection = regex.Matches(siteContent);

            regex = new Regex(pattern);

            string sell_string = regex.Matches(collection[41].ToString())[0].ToString();

            pageData.DetectedString = sell_string;

            /* convert to double */
            double sell = Double.Parse(sell_string);

            /* defined threshold of your rule */
            if (sell < DollarRule.NZD.Sell)
                return true;

            return false;
        }

        public static bool BuySellEURO(PageData pageData)
        {
            string siteContent = pageData.Source;

            // Define the regular expression pattern. 
            string pattern = @"\b[0-9]*\.*[0-9]+\b";

            Regex regex = new Regex("<td class=\"decimal\">" + pattern + "</td>");
            var collection = regex.Matches(siteContent);

            regex = new Regex(pattern);

            string eur_buy_string = regex.Matches(collection[52].ToString())[0].ToString();
            string eur_sell_string = regex.Matches(collection[53].ToString())[0].ToString();

            pageData.DetectedString = eur_buy_string + "/" + eur_sell_string;

            /* convert to double */
            double buy = Double.Parse(eur_buy_string);
            double sell = Double.Parse(eur_sell_string);

            /* defined threshold of your rule */
            if (sell < DollarRule.EURO.Sell || buy > DollarRule.EURO.Buy)
                return true;

            return false;
        }

        public static bool BuySellJPY(PageData pageData)
        {
            string siteContent = pageData.Source;

            // Define the regular expression pattern. 
            string pattern = @"\b[0-9]*\.*[0-9]+\b";

            Regex regex = new Regex("<td class=\"decimal\">" + pattern + "</td>");
            var collection = regex.Matches(siteContent);

            regex = new Regex(pattern);

            string jpy_buy_string = regex.Matches(collection[30].ToString())[0].ToString();
            string jpy_sell_string = regex.Matches(collection[31].ToString())[0].ToString();

            pageData.DetectedString = jpy_buy_string + "/" + jpy_sell_string;

            /* convert to double */
            double jpy_buy = Double.Parse(jpy_buy_string);
            double jpy_sell = Double.Parse(jpy_sell_string);

            /* defined threshold of your rule */
            if (jpy_sell < DollarRule.JPY.Sell || jpy_buy > DollarRule.JPY.Buy)
                return true;

            return false;
        }

        public static bool CicaStock(PageData pageData)
        {
            string siteContent = pageData.Source;

            // Define the regular expression pattern. 
            string pattern = @"\b[0-9]*\.*[0-9]+\b";

            Regex regex = new Regex("<span id=\"yfs_l84_CIA\" data-sq=\"CIA:value\">" +
                pattern + 
                "</span>");
            var collection = regex.Matches(siteContent);

            regex = new Regex(pattern);

            string sell_string = regex.Matches(collection[0].ToString())[0].ToString();

            pageData.DetectedString = sell_string;

            /* convert to double */
            double sell = Double.Parse(sell_string);

            /* defined threshold of your rule */
            if (sell > DollarRule.CIA.Buy)
                return true;

            return false;
        }

        public static bool GoldPrice(PageData pageData)
        {
            string siteContent = pageData.Source;

            // Define the regular expression pattern. 
            string pattern = @"\b[0-9]*\.*[0-9]+\b";

            Regex regex = new Regex("<td class=\"decimal\">" + pattern + "</td>");
            var collection = regex.Matches(siteContent);

            /* The third and fourth item contain BUY and SELL of the USD dollar respectively. */
            regex = new Regex(pattern);

            string sell_string = regex.Matches(collection[0].ToString())[0].ToString();
            string buy_string = regex.Matches(collection[1].ToString())[0].ToString();

            pageData.DetectedString = buy_string + "/" + sell_string;

            /* convert to double */
            double buy = Double.Parse(buy_string);
            double sell = Double.Parse(sell_string);

            /* defined threshold of your rule */
            if (sell < DollarRule.Gold.Sell || buy > DollarRule.Gold.Buy)
                return true;

            return false;
        }

        public static bool TAIEX(PageData pageData) // 台灣加權指數
        {
            string siteContent = pageData.Source;

            // Define the regular expression pattern. 
            string pattern = @"\b[0-9]*\.*[0-9]+\b";

            Regex regex = new Regex("<td nowrap align=center>" + pattern + "</td>");
            var collection = regex.Matches(siteContent);

            /* The third and fourth item contain BUY and SELL of the USD dollar respectively. */
            regex = new Regex(pattern);

            string sell_string = regex.Matches(collection[0].ToString())[0].ToString();

            pageData.DetectedString = sell_string;

            /* convert to double */
            double sell = Double.Parse(sell_string);

            /* defined threshold of your rule */
            if (sell < DollarRule.TAIEX.Sell)
                return true;

            return false;
        }

        public static bool AmericaStock(PageData pageData) // 成交價 of Dow Jones Industrial Average
        {
            string siteContent = pageData.Source;
            
            // Define the regular expression pattern. 
            string pattern = @"\b[0-9]*(\,*[0-9]+)*\.[0-9]+\b"; // it can find the decimal string like '17,113.24'.

            // Explain here,
            // the character '^' is not an escape character in C#, but it is in ISO regular expression.
            // So here I put \\ before ^ to represent \^ in regular expression.
            Regex regex = new Regex("<span id=\"yfs_l10_\\^dji\">" +
               pattern);

            var collection = regex.Matches(siteContent);

            regex = new Regex(pattern);

            string value_string = regex.Matches(collection[0].ToString())[0].ToString();
            
            pageData.DetectedString = value_string;

            /* convert to double */
            double value = Double.Parse(value_string);

            if (value > DollarRule.DowJones.Buy ||
                value < DollarRule.DowJones.Sell)
                return true;

            return false;
        }

    }
}
