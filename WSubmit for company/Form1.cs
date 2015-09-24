using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;


namespace WSubmit_for_company
{
    public partial class Form1 : Form
    {
        ManualResetEvent wait_handle = new ManualResetEvent(true);

        static int ch = 0, Symbol = 0, alpNumaric = 0, AlNu = 0, spases = 0,counter = 0;
        static int checkthread = 0;
        static int brakethread = 0;
        static long fileLength = 0;
        

        
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread File_Read = new Thread(new ThreadStart(file_read));
            File_Read.Start();
            dispaly_Value();
        }

        

        private void dispaly_Value()
        {
            progressBar1.Value = 0;

            while (checkthread == 0)
            {
                
                    progressBar1.Value = 100;

                label2.Text = counter.ToString();
                label4.Text = ch.ToString();
                label6.Text = spases.ToString();
                label8.Text = AlNu.ToString();
                label10.Text = Symbol.ToString();
                label12.Text = alpNumaric.ToString();
            }

            label2.Text = counter.ToString();
            label4.Text = ch.ToString();
            label6.Text = spases.ToString();
            label8.Text = AlNu.ToString();
            label10.Text = Symbol.ToString();
            label12.Text = alpNumaric.ToString();

            if (checkthread == 1)
            {
                resetGloblevariable();
            }   
        }

        private void resetGloblevariable() 
        {
            ch = 0; Symbol = 0; alpNumaric = 0; AlNu = 0; spases = 0; counter = 0;
        }

        public void file_read()
        {

            var txtFiles = Directory.EnumerateFiles(@"D:\\test/", "*.txt");
            foreach (string currentFile in txtFiles)
            {

                //FileInfo f = new FileInfo(currentFile);
                //fileLength = f.Length;

                StreamReader sr = new StreamReader(currentFile);

                string delim = " "; //maybe some more delimiters like ?! and so on
                string[] fields = null;
                string line = null;
                int i = 0;

                 while (!sr.EndOfStream)
                {
                    wait_handle.WaitOne();
                    if (brakethread == 1)
                    {
                        break;
                    }
                    
                    line = sr.ReadLine();
                    line.Trim();
                    fields = line.Split(delim.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    counter += fields.Length;
                    ch += line.Length+1;
                     //ch += line.Count(char.IsLetter);
                    spases += line.Count(Char.IsWhiteSpace);
                    AlNu += line.Count(Char.IsNumber);
                    Symbol += line.Count(Char.IsSymbol);
                    alpNumaric += line.Count(char.IsLetterOrDigit);

                    

                    i++;
                   
                }
                 ch--;
            }

             checkthread = 1;
             brakethread = 0;
        }

        private void Pause_Click(object sender, EventArgs e)
        {
            wait_handle.Reset();
        }

        private void Resume_Click(object sender, EventArgs e)
        {
            wait_handle.Set();
        }

        private void Stop_Click(object sender, EventArgs e)
        {
            brakethread = 1;
        }

       
    }

}
