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
        Dictionary<int, string> regularExpressionMap = new Dictionary<int,string>();
        BinaryExpressionTree<TreeDictionary> ExpressionTree = new BinaryExpressionTree<TreeDictionary>();

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
            
            regularExpressionMap = inputAnalizer.Analize(expression);
            regularExpressionMap = infixToPosfix.ConvertToPosfix(regularExpressionMap);

            for (int i = 0; i < regularExpressionMap.Count; i++)
            {
                textBox2.Text += regularExpressionMap.Values.ElementAt(i);               
            }

            for (int i = regularExpressionMap.Count-1; i >= 0; i--)
            {
                TreeDictionary element = new TreeDictionary();
                element.setKey(regularExpressionMap.ElementAt(i).Key);
                element.setValue(regularExpressionMap.ElementAt(i).Value);
                ExpressionTree.Insert(element);
            }
            ExpressionTree.cabeza = ExpressionTree.Nullable(ExpressionTree.cabeza);
            ExpressionTree.cabeza = ExpressionTree.FirstPos(ExpressionTree.cabeza);
            ExpressionTree.cabeza = ExpressionTree.LastPos(ExpressionTree.cabeza);
            ExpressionTree.cabeza = ExpressionTree.FollowPos(ExpressionTree.cabeza);
            ExpressionTree.makeAutomaton(ExpressionTree.cabeza);
            MessageBox.Show("Done");                     
        }
    }
}
