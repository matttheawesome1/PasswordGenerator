using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PassGen
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                Upper = true;
            }
            else
            {
                Upper = false;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                Lower = true;
            }

            else
            {
                Lower = false;
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                Special = true;
            }

            else
            {
                Special = false;
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked)
            {
                Numbers = true;
            }

            else
            {
                Numbers = false;
            }
        }

        //Here's what I want to do...
        //If the minimum is less than 1, delete the text in textbox, and have them re-enter the value until it is valid.
        private void MinLength_TextChanged(object sender, EventArgs e)
        {
            int min;
            if (int.TryParse(MinLength.Text, out min))
            {
                if (min >= 1)
                    Min = min;
            }
        }

        private void MaxLength_TextChanged(object sender, EventArgs e)
        {
            int max;
            if (int.TryParse(MaxLength.Text, out max))
            {
                if (max >= 1)
                {
                    Max = max;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(Min < 1) { MessageBox.Show("Invalid minimum range."); }
            if(Max < Min) { MessageBox.Show("Invalid maximum range."); }
            textBox1.Text = GeneratePass(Min, Max, Upper, Lower, Special, Numbers);
        }

        public static int randNum(int min, int max)
        {
            //Locks the random method to a single thread.
            lock(syncLock)
            {
                return random.Next(min, max);
            }
        }

        public static string GeneratePass(int min, int max, bool upper, bool lower, bool special, bool numbers)
        {

            string init = " ";

            if(upper)
            {
                init += "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            }

            if(lower)
            {
                init += "abcdefghijklmnopqrstuvwxyz";
            }

            if(special)
            {
                init += " {}!@#$%^&*()_-\\\'\"+=,./;:<=>?[]{}|`~";
            }

            if(numbers)
            {
                init += "0123456789";
            }

            char[] generate = init.ToCharArray();

            int rand = randNum(min, max);

            byte[] data = new byte[1];
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetNonZeroBytes(data);
                data = new byte[rand];
                crypto.GetNonZeroBytes(data);
            }

            StringBuilder result = new StringBuilder(rand);
            foreach(byte b in data)
            {
                result.Append(generate[b % (generate.Length)]);
            }

            return result.ToString();
        }

        //Data members.
        public int Min { get; set; } //Minimum length of string
        public int Max { get; set; } //Maximum length of string
        public bool Upper { get; set; } //Uppercase?
        public bool Lower { get; set; } //Lowercase?
        public bool Special { get; set; } //Special?
        public bool Numbers { get; set; } //Numbers allowed?

        //Random number generator sequence.
        private static readonly Random random = new Random(); //The readonly cast means that it cannot be set, only get.
        private static readonly object syncLock = new object();

    }
}
