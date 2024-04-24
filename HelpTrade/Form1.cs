using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HelpTrade
{
    public partial class Form1 : Form
    {
        private BindingList<Label> currenciesLabel;

        private BindingList<TextBox> inputLongPreview;
        private BindingList<TextBox> inputShortPreview;
        private BindingList<TextBox> inputLongActual;
        private BindingList<TextBox> inputShortActual;
        private string[] result;

        private int topOrigin;
        private int leftOrigin;
        private int margLeft;
        private int margTop;
        private string[] currenciesName;
        private int nbCurrencies;

        private Lastdata ld;
        public Form1()
        {
            InitializeComponent();

            nbCurrencies = 8;

            topOrigin = 80;
            leftOrigin = 20;
            margLeft = 150;
            margTop = 30;
            currenciesLabel = new BindingList<Label>();
            currenciesName = new string[nbCurrencies];
            result = new string[nbCurrencies];

            currenciesName[0] = "AUD"; currenciesName[1] = "GBP"; currenciesName[2] = "CAD";
            currenciesName[3] = "EUR"; currenciesName[4] = "JPY"; currenciesName[5] = "CHF";
            currenciesName[6] = "USD"; currenciesName[7] = "NZD";

            createCurrencies();

            inputLongPreview = new BindingList<TextBox>();
            leftOrigin += 50;
            createInput(inputLongPreview, leftOrigin);

            inputShortPreview = new BindingList<TextBox>();
            leftOrigin += margLeft;
            createInput(inputShortPreview, leftOrigin);

            inputLongActual = new BindingList<TextBox>();
            leftOrigin += margLeft;
            createInput(inputLongActual, leftOrigin);

            inputShortActual = new BindingList<TextBox>();
            leftOrigin += margLeft;
            createInput(inputShortActual, leftOrigin);

            ld = new Lastdata(nbCurrencies);

            FormClosing += onFormClosing;
            Load += OnLoad;
            Shown += OnShown;

        }

        private void OnShown(object sender, EventArgs e)
        {
            for (int i = 0; i < nbCurrencies; i++)
            {
                inputLongPreview[i].Text = ld.InputLong[i];
                inputShortPreview[i].Text = ld.InputShort[i];
            }
        }

        private void OnLoad(object sender, EventArgs e)
        {
            ld = FileManager.loadFile(nbCurrencies);
        }

        private void onFormClosing(object sender, FormClosingEventArgs e)
        {
            FileManager.saveFile(ld);
        }

        public void createCurrencies()
        {
            int CurTopOrigin = topOrigin;
            int CurLeftOrigin = leftOrigin;
            int CurMargTop = margTop;
            for(int i=0; i< nbCurrencies; i++)
            {
                Label curTemp = new Label() { Width = 30, Name = currenciesName[i], Location = new Point(CurLeftOrigin, CurTopOrigin), Text = currenciesName[i] };
                this.Controls.Add(curTemp);
                currenciesLabel.Add(curTemp);
                CurTopOrigin += CurMargTop;
            }  
        }

        public void createInput(BindingList<TextBox> input, int InLeftOrigin)
        {
            int InTopOrigin = topOrigin;
            int InMargTop = margTop;
            for (int i = 0; i < nbCurrencies; i++)
            {
                TextBox inTemp = new TextBox() { Width = 100, Height = 25, Name = currenciesName[i]+"in", Location = new Point(InLeftOrigin, InTopOrigin)};
                this.Controls.Add(inTemp);
                input.Add(inTemp);
                InTopOrigin += InMargTop;
            }
        }

        private void tester(object sender, EventArgs e)
        {
            bool check = true;
            for(int i=0; i<nbCurrencies; i++)
            {
                if(inputLongPreview[i].Text=="" || inputShortPreview[i].Text == "" ||
                    inputLongActual[i].Text == "" || inputLongActual[i].Text == "")
                {
                    check = false;
                }
            }
            if(check)
            {
                Generate();
            }
            else
            {
                MessageBox.Show("Veuillez remplir tous les champs ! ", "TradeHelp", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void Generate()
        {
            for(int i=0; i< nbCurrencies; i++)
            {
                int resultPrevius = int.Parse(""+inputLongPreview[i].Text) - int.Parse(""+inputShortPreview[i].Text);
                int resultActual = int.Parse(""+inputLongActual[i].Text) - int.Parse(""+inputShortActual[i].Text);

                if(resultPrevius>resultActual)
                {
                    result[i] = "Baissiere";
                }
                else  if(resultPrevius<resultActual)
                {
                    result[i] = "Haussiere";
                }
                else
                {
                    result[i] = "Egal";
                }
            }
            createFile();
        }
        public void createFile()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = "PaireTrade.txt";
            sfd.Filter = "Fichier Txt | *.txt ";

            if(sfd.ShowDialog()==DialogResult.OK)
            {
                var pathFile = sfd.FileName;
                StreamWriter sw = new StreamWriter(pathFile, false, Encoding.ASCII);
                for (int i = 0; i< nbCurrencies; i++)
                {
                    for (int j = i+1; j < nbCurrencies; j++)
                    {
                        if(result[i] != result[j])
                        {
                            string paire1 = currenciesName[i]+currenciesName[j]+" : "+result[i];
                            string paire2 = currenciesName[j] + currenciesName[i] + " : " + result[j];
                            sw.WriteLine(paire1 + " | " + paire2);
                        }
                    }
                }
                sw.Close();
                saveActualData();
            }

        }

        private void saveActualData()
        {
            for(int i=0; i<nbCurrencies; i++)
            {
                ld.InputLong[i] = inputLongActual[i].Text;
                ld.InputShort[i] = inputShortActual[i].Text;
            }
        }

        private void quit(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Reset(object sender, EventArgs e)
        {
            for(int i=0; i<nbCurrencies; i++)
            {
                inputLongActual[i].Text = "";
                inputShortActual[i].Text = "";
                inputLongPreview[i].Text = "";
                inputShortPreview[i].Text = "";
            }
        }

        private void site(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.tradingster.com");
        }
    }
}
