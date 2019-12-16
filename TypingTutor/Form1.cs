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

namespace TypingTutor
{
    public partial class Form1 : Form
    {
        private string[] panagrams;
        public int panagramCounter = 0;
        private int currentLetter = 0;
        private int[] mistakes;

        public Form1()
        {
            InitializeComponent();
            panagrams = new String[3];
            mistakes = new int[panagrams.Count()];
        }

        private void quit_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Goodbye!", "Thank You");
            Application.Exit();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            txtEnterPhrase.Enabled = true;
            txtDisplayPhrase.Enabled = true;
            lstStatistics.Items.Clear();
            var reader = new StreamReader("panagrams.txt");

            int counter = 0;
            while(!reader.EndOfStream)
            {
                panagrams[counter] = reader.ReadLine();
                counter++;
            }
            txtEnterPhrase.Focus();
            startTyping();
        }

        private void startTyping()
        {
            if (panagramCounter < panagrams.Count())
            {
                txtDisplayPhrase.Text = panagrams[panagramCounter];
            }
            else
            {
                MessageBox.Show("End of the panagrams");
                resetForm();
                displayStats();
            }
        }

        private void formOnKeyPressed(object sender, KeyEventArgs e)
        {
            //Supress Keys
            if (e.KeyCode == Keys.Back || e.KeyCode == Keys.Delete || e.KeyCode == Keys.Left || e.KeyCode == Keys.Up)
            {
                e.SuppressKeyPress = true;
            }

            if(e.KeyCode != Keys.Enter)
                highlightKeyPressed(e.KeyCode.ToString());
            else
            {
                checkAccuracy();
                txtEnterPhrase.Clear();
                panagramCounter++;
                startTyping();
            }
        }

        private void displayStats()
        {
            lstStatistics.Items.Add("Your Results : ");
            for( int i=0; i < mistakes.Count(); i++)
            {
                lstStatistics.Items.Add("In the panagram #" + (i + 1) + " you made " + mistakes[i] + " mistakes.");
            }
        }

        private void checkAccuracy()
        {
            mistakes[panagramCounter] = Math.Abs(txtEnterPhrase.TextLength - txtDisplayPhrase.TextLength);
            int loopCounter = txtDisplayPhrase.TextLength > txtEnterPhrase.TextLength ? txtEnterPhrase.TextLength : txtDisplayPhrase.TextLength;

            for(int i=0; i<loopCounter; i++)
            {
                if (txtDisplayPhrase.Text.Substring(i, 1) != txtEnterPhrase.Text.Substring(i, 1))
                    mistakes[panagramCounter] += 1;
            }
        }

        private void resetForm()
        {
            panagramCounter = 0;
            txtEnterPhrase.Clear();
            txtDisplayPhrase.Enabled = false;
            txtDisplayPhrase.Clear();
            txtEnterPhrase.Enabled = false;
        }

        private void highlightKeyPressed(string v)
        {
            resetColors();
            foreach(var c in this.Controls)
            {
                if(c is Label)
                {
                    Label label = (Label)c;
                    if((label.Tag != null) && (label.Tag.ToString().ToUpper() == v.ToUpper()))
                    {
                        label.BackColor = Color.BlueViolet;
                        return;
                    }
                }
            }
            lblInvalidKey.BackColor = Color.BlueViolet;
        }

        private void resetColors()
        {
            foreach (var c in this.Controls)
            {
                if (c is Label)
                {
                    Label label = (Label)c;
                    label.BackColor = Color.Empty;
                }
            }

        }

        private void txtWriteOnMouseClick(object sender, EventArgs e)
        {
            //move cursor to the end of written text inside the text box
            txtEnterPhrase.SelectionStart = txtEnterPhrase.TextLength;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            resetForm();
        }
    }
}
