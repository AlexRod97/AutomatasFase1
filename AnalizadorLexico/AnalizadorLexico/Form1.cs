using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AnalizadorLexico.Classes;

namespace AnalizadorLexico
{
    public partial class Form1 : Form
    {
        List<string> sets = new List<string>();
        List<string> tokens = new List<string>();
        List<string> actions = new List<string>();
        string error = "";
        InputAnalizer inputAnalizer = new InputAnalizer();
        InfixToPosfix infixToPosfix = new InfixToPosfix(); 
        BinaryExpressionTree<string> ExpressionTree = new BinaryExpressionTree<string>(); 

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string line;
            bool flag = true; 

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.IO.StreamReader file = new System.IO.StreamReader(openFileDialog1.FileName);
                while ((line = file.ReadLine()) != null)
                {
                    line = line.ToLower();                    
                    switch(line)
                    {
                        case "sets":
                            line = file.ReadLine();
                            while (flag)
                            {                                
                                if (line == "tokens")
                                {
                                    flag = false;
                                }
                                else
                                {
                                    sets.Add(line);
                                    line = file.ReadLine().ToLower();
                                }                                
                            }
                        break;

                        case "tokens":
                            line = file.ReadLine();
                            while (flag)
                            {
                                if (line == "actions")
                                {
                                    flag = false;
                                }
                                else
                                {
                                    tokens.Add(line);
                                    line = file.ReadLine().ToLower();
                                }                             
                            }
                            break;

                        case "actions":
                            line = file.ReadLine();
                            while (flag)
                            {
                                if (line.Contains("error"))
                                {
                                    flag = false;
                                }
                                else
                                {
                                    actions.Add(line);
                                    line = file.ReadLine().ToLower();
                                }                               
                            }

                            break;

                        case "error":
                            var completeLine = line.Split('=');
                            error = completeLine[1];
                        break;

                    }
                }
                file.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {          
            string expression = textBox1.Text;
            expression = inputAnalizer.Analize(expression);
            MessageBox.Show(infixToPosfix.ConvertToPosfix(expression));
        }
    }
}
