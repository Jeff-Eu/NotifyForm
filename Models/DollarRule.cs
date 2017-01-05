using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1.Models
{
    class Dollar
    {
        public double Buy { get; set; }
        public double Sell { get; set; }

        public Dollar(double buy, double sell)
        {
            Buy = buy;
            Sell = sell;
        }
    }

    static class DollarRule
    {
        static readonly string szUsd = ConfigurationManager.AppSettings["usd"];
        static readonly string szEuro = ConfigurationManager.AppSettings["euro"];
        static readonly string szNzd = ConfigurationManager.AppSettings["nzd"];
        static readonly string szAud = ConfigurationManager.AppSettings["aud"];
        static readonly string szJpy = ConfigurationManager.AppSettings["jpy"];
        static readonly string szGold = ConfigurationManager.AppSettings["gold"];
        static readonly string szTaiex = ConfigurationManager.AppSettings["taiex"];
        static readonly string szDowJones = ConfigurationManager.AppSettings["dowJones"];
        static readonly string szCia = ConfigurationManager.AppSettings["cia"];

        public static Dollar USD = GetDollar(szUsd);
        public static Dollar EURO = GetDollar(szEuro);
        public static Dollar NZD = GetDollar(szNzd);
        public static Dollar AUD = GetDollar(szAud);
        public static Dollar JPY = GetDollar(szJpy);
        public static Dollar Gold = GetDollar(szGold);
        public static Dollar TAIEX = GetDollar(szTaiex);
        public static Dollar DowJones = GetDollar(szDowJones);
        public static Dollar CIA = GetDollar(szCia);

        public static Dollar GetDollar(string s)
        {
            string[] splitStrings = s.Split(',');

            Dollar dollar = new Dollar(double.Parse(splitStrings[0]), double.Parse(splitStrings[1]));

            return dollar;
        }
    }
}
