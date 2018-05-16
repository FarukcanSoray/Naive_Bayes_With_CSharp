using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NaiveBayes;

namespace NaiveBayesGUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            List<string> classes = new List<string>() { "ekonomi", "magazin", "saglik", "siyasi", "spor" };
            List<string> valueNames = new List<string>() { "F-Measure", "Recall", "Precision" };
            List<TextBox> classesBox = new List<TextBox>();
            List<TextBox> valueNamesBox = new List<TextBox>();
            foreach (var i in classes)
            {
                classesBox.Add(new TextBox());
                classesBox.Last().ReadOnly = true;
                classesBox.Last().Text = i;
            }

            foreach (var i in valueNames)
            {
                valueNamesBox.Add(new TextBox());
                valueNamesBox.Last().ReadOnly = true;
                valueNamesBox.Last().Text = i;
            }
            TextBox a = new TextBox();
            foreach (var i in classesBox)
                tableLayoutPanel1.Controls.Add(i, classesBox.IndexOf(i) + 1, 0);

            foreach (var i in valueNamesBox)
                tableLayoutPanel1.Controls.Add(i, 0, valueNamesBox.IndexOf(i) + 1);
        }

        private void calculateButton_Click(object sender, EventArgs e)
        {
         
            var valuesForEachClass = startProgram.start();

            for(int i = 0; i < 5; i++)
            {
                TextBox t = new TextBox();
                t.ReadOnly = true;
                t.Text = valuesForEachClass.ElementAt(i).Value.Item1.ToString();
                tableLayoutPanel1.Controls.Add(t, i + 1, 1);
            }

            for (int i = 0; i < 5; i++)
            {
                TextBox t = new TextBox();
                t.ReadOnly = true;
                t.Text = valuesForEachClass.ElementAt(i).Value.Item2.ToString();
                tableLayoutPanel1.Controls.Add(t, i + 1, 2);
            }

            for (int i = 0; i < 5; i++)
            {
                TextBox t = new TextBox();
                t.ReadOnly = true;
                t.Text = valuesForEachClass.ElementAt(i).Value.Item3.ToString();
                tableLayoutPanel1.Controls.Add(t, i + 1, 3);
            }

        }
    }
}
