using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;




namespace Cay_Dinh_Danh
{
    public partial class Form2 : Form
    {
        private List<List<string>> Rules;
        private string[] nameTTs;
        DataTable table;
        public Form2(List<List<string>> rules, string[] nameTT, string[] sample)
        {
            InitializeComponent();
            Rules = rules;


            string temp = "";
            label1.Text = "";
            foreach (var res in Rules)
            {
                foreach (var str in res)
                {
                    temp = str;
                    if (str != res[res.Count - 1])
                        temp += " --> ";
                    label1.Text += temp;
                }
                label1.Text += "\n";
            }

            //ghi huong dan len lable 3
            label3.Text = "Hướng dẫn:\n\n"+
                "B1. Nhập các trường hợp test vào bên dưới (không cần nhập giá trị thuộc tính đích.\n\n"
                + "B2. Ấn button \"Đoán thuộc tính đich\" và xem kết quả tại cột thuộc tính đích (cột cuối).";



            //set ten cac thuoc tinh
            nameTTs = nameTT;

            //tao dataGridView de nhap 
            table = createDatatable(sample);
            dataGridView1.DataSource = table;

            //Dat read only o thuoc tinh dich

            dataGridView1.Columns[nameTTs[nameTTs.Length - 1]].ReadOnly = true;
   

        }

        private DataTable createDatatable(string[] sample)
        {
            /*
             * Tao DataTable tu data doc tu file
             */
            DataTable table = new DataTable();

            for (int i = 0; i < nameTTs.Length; i++)
            {
                table.Columns.Add(nameTTs[i]);
            }

            table.Rows.Add();
            for (int j = 0; j < sample.Length-1; j++)
                table.Rows[0][j] = sample[j];

            


            return table;
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

        }
        private int getIndexofName(string nameTT)
        {
            // tim index cua ten thuoc tinh trong table
            for (int i = 0; i < nameTTs.Length; i++)
                if (nameTTs[i] == nameTT)
                    return i;
            return -1; // neu khong tim thay tra ve -1
        }

        private string doanTTDich(string[] line, List<List<string>> rules)
        {

            foreach (var rule in rules)
            {
                bool found = true;
                for (int i = 0; i < rule.Count - 1; i++)
                {
                    string[] thuocGT = rule[i].Split(':');
                    int indexOfTT = getIndexofName(thuocGT[0]);
                    if (indexOfTT == -1 || line[indexOfTT] != thuocGT[1])
                    {
                        found = false;
                        break;
                    }
                }
                if (found)
                    return rule[rule.Count - 1];
            }

            return "N/A";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.EndEdit())
                {
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        string[] line = new string[nameTTs.Length];
                        for (int j = 0; j < nameTTs.Length-1; j++)
                            line[j] = table.Rows[i][j].ToString();

                        string TTDich = doanTTDich(line, Rules);

                        table.Rows[i][nameTTs.Length - 1] = TTDich;
                    }
                }

            }
            catch(Exception ex)
            {
                MessageBox.Show("Vui long nhap day du du lieu!\n" + ex.Message,"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }


        }

        
    }
}
