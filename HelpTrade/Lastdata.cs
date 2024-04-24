using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpTrade
{
    public class Lastdata
    {
        private string[] inputLong;
        public string[] InputLong
        {
            get => inputLong;
            set => inputLong = value;
        }
        private string[] inputShort;
        public string[] InputShort
        {
            get => inputShort;
            set => inputShort = value;
        }

        public Lastdata(int nbCurrencies)
        {
            InputLong = new string[nbCurrencies];
            InputShort = new string[nbCurrencies];
        }

        public Lastdata()
        {
            InputLong = new string[8];
            InputShort = new string[8];
        }
    }
}
