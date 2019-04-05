using System;
using System.Collections;
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
        string abc = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        InputAnalizer inputAnalizer = new InputAnalizer();
        InfixToPosfix infixToPosfix = new InfixToPosfix(); 
        Dictionary<int, string> regularExpressionMap = new Dictionary<int,string>();
        BinaryExpressionTree<TreeDictionary> ExpressionTree = new BinaryExpressionTree<TreeDictionary>();
        SetsValidations setsValue = new SetsValidations();
        TokensValidations tokensValue = new TokensValidations();
        String[] archivo = new string[1];
        List<string> exreg = new List<string>();

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string line;
            bool flag = true;
            int cantLineas = 0;
            int position = 0;             

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.IO.StreamReader file = new System.IO.StreamReader(openFileDialog1.FileName);
                while ((line = file.ReadLine()) != null)
                {
                    cantLineas++;
                }
                file.Close();
                archivo = new string[cantLineas];
                System.IO.StreamReader file2 = new System.IO.StreamReader(openFileDialog1.FileName);

                while ((line = file2.ReadLine()) != null)
                {
                    if(line != "")
                    {
                        archivo[position] = line;
                        position++;
                    }
                }
                file2.Close();
                archivo = archivo.Where(c => c != null).ToArray();

                if (setsValue.Read(archivo) == false)
                {
                    MessageBox.Show(setsValue.error + "   en line " + setsValue.line, "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    tokensValue.Get(archivo);
                    int posicion = 0;
                    if (tokensValue.Read(archivo) == false)
                    {
                        for (int i = 0; i < archivo.Count(); i++)
                        {
                            if (archivo[i].ToUpper().Contains("TOKENS"))
                            {
                                posicion = i;
                                break;
                            }
                        }
                        MessageBox.Show(setsValue.error + "   en la line " + (tokensValue.line + posicion), "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        exreg = tokensValue.result;
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string trial = textBox1.Text;
            string expression = toChain(exreg);
            Dictionary<int, string> map = new Dictionary<int, string>();
            List<string> stringExreg = new List<string>();
            List<CharDictionary> charExreg = new List<CharDictionary>();
            List<TreeDictionary> final = new List<TreeDictionary>();

            stringExreg = inputAnalizer.Analize(exreg);
            charExreg = inputAnalizer.convertToChar(stringExreg);
            charExreg = infixToPosfix.ConvertToPosfix(charExreg);
            final = inputAnalizer.convertToTreeDictionary(charExreg, stringExreg);

            for (int i = 0; i < stringExreg.Count; i++)
            {
                textBox2.Text += stringExreg.ElementAt(i).ToString();
            }
            
            for (int i = final.Count-1; i >= 0; i--)
            {               
                ExpressionTree.Insert(final.ElementAt(i));
            }
            
            ExpressionTree.cabeza = ExpressionTree.Nullable(ExpressionTree.cabeza);
            ExpressionTree.cabeza = ExpressionTree.FirstPos(ExpressionTree.cabeza);
            ExpressionTree.cabeza = ExpressionTree.LastPos(ExpressionTree.cabeza);
            ExpressionTree.cabeza = ExpressionTree.FollowPos(ExpressionTree.cabeza);
            ExpressionTree.cabeza = ExpressionTree.makeAutomaton(ExpressionTree.cabeza);

            makeDGV(ExpressionTree.transiciones, ExpressionTree.table, ExpressionTree.setsList);             
        }

        public List<string> convertFollow (BinaryExpressionTree<TreeDictionary> expression)
        {
            List<FollowDictionary> follow = expression.Follow;
            List<string> list = new List<string>(); 

            foreach (FollowDictionary item in follow)
            {
                StringBuilder line = new StringBuilder();
                for (int i = 0; i < item.getValues().Count; i++)
                {
                    if (i + 1 != item.getValues().Count)
                    {
                        line.Append(item.getValues().ElementAt(i));
                        line.Append(",");
                    }
                    else
                    {
                        line.Append(item.getValues().ElementAt(i));
                    }
                }
                list.Add(line.ToString());
            }
            return list;
        }

        public void makeDGV(List<List<int>> transiciones, List<List<string>> tabla, List<string> sets)
        {
            //dataGridView1.ColumnCount = sets.Count;
            List<string> conjuntos = new List<string>();
            List<string> automata = new List<string>();
            List<string> follows = convertFollow(ExpressionTree);
            dataGridView1.ColumnCount = 2;
            dataGridView1.RowCount = follows.Count;
            dataGridView1.Columns[0].Name = "Nodo";
            dataGridView1.Columns[1].Name = "Follow";

            for (int i = 0; i < follows.Count; i++)
            {
                dataGridView1.Rows[i].Cells[0].Value = i + 1;
                dataGridView1.Rows[i].Cells[1].Value = follows.ElementAt(i);
            }

            dataGridView2.ColumnCount = sets.Count;
            dataGridView2.RowCount = transiciones.Count;
            dataGridView2.Columns[0].Name = "Conjunto";

            for (int i = 0; i < sets.Count - 1; i++)
            {
                dataGridView2.Columns[i + 1].HeaderCell.Value = sets.ElementAt(i);
            }         

            for (int i = 0; i < transiciones.Count; i++)
            {
                StringBuilder element = new StringBuilder();
                element.Append(abc.ElementAt(i).ToString());
                element.Append(": ");
                for (int j = 0; j < transiciones.ElementAt(i).Count; j++)
                {
                    if(j + 1 < transiciones.ElementAt(i).Count)
                    {
                        element.Append(transiciones.ElementAt(i).ElementAt(j));
                        element.Append(",");
                    }
                    else
                    {
                        element.Append(transiciones.ElementAt(i).ElementAt(j));                        
                    }
                }
                dataGridView2.Rows[i].Cells[0].Value = element.ToString();
            }

            for (int i = 0; i < tabla.Count; i++)
            {
                for (int j = 0; j < tabla.ElementAt(i).Count - 1; j++)
                {
                    char number = Convert.ToChar(tabla.ElementAt(i).ElementAt(j)); 

                    if(Char.IsDigit(number))
                    {
                        int value = Convert.ToInt32(number.ToString());
                        string literal = abc.ElementAt(value - 1).ToString();
                        dataGridView2.Rows[i].Cells[j + 1].Value = literal;
                    }
                    else
                    {
                        dataGridView2.Rows[i].Cells[j + 1].Value = tabla.ElementAt(i).ElementAt(j);
                    }
                    
                }
            }
        }

        private string toChain (List<string> cadena)
        {
            string result = "";

            for (int i = 0; i < exreg.Count; i++)
            {
                result += exreg.ElementAt(i);
            }
            return result;  
        }
    }
}
