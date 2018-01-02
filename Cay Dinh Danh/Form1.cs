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


namespace Cay_Dinh_Danh
{
    public partial class Form1 : Form
    {
        private List<List<string>> Rules;
        private List<string[]> data;
        public Form1()
        {
            InitializeComponent();


        }
        private void Form1_Load(object sender, EventArgs e)
        {
            label4.Text = "Do an mon hoc :v";
        }

        public void loadData(string filename="data.csv")
        {
            string[] lines = System.IO.File.ReadAllLines(filename);

            //List<string[]> data = new List<string[]>();
            data = new List<string[]>();
            foreach (string line in lines)
            {
                string[] words;
                words = line.Split(',').Select(p => p.Trim()).ToArray();

                data.Add(words);

            }

            // Hien thi dataTable vua tao bang DataGridView
            dataGridView1.DataSource = createDatatable();

            MessageBox.Show("Cột cuối cùng đại diện cho thuộc tính đích.\nCột đầu tiên là giá trị Order.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }
        private int getIndexofName(string nameTT)
        {
            // tim index cua ten thuoc tinh trong table
            for (int i = 0; i < data[0].Length; i++)
                if (data[0][i] == nameTT)
                    return i;
            return -1; // neu khong tim thay tra ve -1
        }

        private string doanTTDich(string[] line, List<List<string>> rules)
        {
           
            foreach(var rule in rules)
            {
               bool found = true;
               for(int i = 0; i < rule.Count-1; i++)
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

        private DataTable createDatatable()
        {
            /*
             * Tao DataTable tu data doc tu file
             */
            DataTable table = new DataTable();
            
            for(int i = 0; i < data[0].Length; i++)
            {
                table.Columns.Add(data[0][i]);

                while (table.Rows.Count < data.Count-1)
                    table.Rows.Add();

                for (int j = 0; j < data.Count-1; j++)
                    table.Rows[j][i] = data[j+1][i];
                
            }
            return table;
        }

    
        private void button1_Click(object sender, EventArgs e)
        {

            try
            {

                //Hien thi thong tin them
                

                loadData();
                int posMax = 0;
                List<giatriTT> results = hamtimTT(data, ref posMax);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Không lấy được file!\n", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private void button2_Click(object sender, EventArgs e)
        {
            //string[] line = { "Thach", "Vang", "Trung binh", "Trung binh", "Co", "" };

            //string results = doanTTDich(line, Rules);

            //label3.Text = results;

            try
            {
                Form2 f2 = new Form2(Rules, data[0], data[1]);
                f2.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Vui lòng lấy dữ liệu và tạo tập luật trước!\n", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            

        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }


        /* Tao text box va button cho user add them thuoc tinh. Roi in lai dataGridView */
        /* Dung tinh nang tren de cho user nhap du lieu ban dau */


        private class giatriTT
        {
            public string value;
            public int flag=1;
            public string check;

        }


        private List<giatriTT> hamtimTT(List<string[]> Tab, ref int posMax)
        {
            /* 
             * Ham tim thuoc tinh phan loai
             * Input: Table, posMax (Index cua thuoc tinh phan loai trong Table)
             * Output: List<giatriTT> list gia tri thuoc tinh 
             */
            int m = Tab.Count;
            int n = Tab[0].Length;
            
            List<List<giatriTT>> T = new List<List<giatriTT>>();


            int[][] Count = new int[n-2][];
            for(int i = 0; i < n-2; i++)
            {
                Count[i] = new int[2];
                Count[i][0] = 0;
            }

            int t = 0; // Bien chay cho T giatri TT
                       // moi thuoc tinh co mot vector<giatriTT> T;
            int max=0, smin=0;
            for (int j = 1; j <= n - 2; j++)
            {
                // Duyet qua cac thuoc tinh, toc, chieucao,....
                List<giatriTT> tempTT = new List<giatriTT>();
                // lay gia tri thuoc tinh o dong dau tien
                giatriTT temp = new giatriTT();
                temp.value = Tab[1][j];
                temp.check = Tab[1][n - 1]; // gia tri thuoc tinh dich
                tempTT.Add(temp);

                // lay gia tri cua thuoc tinh o 2->m-1 dong ke tiep
                for (int i = 2; i <= m - 1; i++)
                {

                    //T[t].size = so luong gia tri cua thuoc tinh
                    int f = 1;
                    for (int z = 0; z < tempTT.Count; z++)
                    {
                        if (Tab[i][j] == tempTT[z].value)
                        {// neu da co gia tri thuoc tinh
                            f = 0;
                            //if (T[t][z].flag == 0) {
                            //b = 1; break;
                            //} // khong la vector donvi
                            //if (b == 1) break;
                            if (Tab[i][n - 1] != tempTT[z].check)
                                tempTT[z].flag = 0;

                            break;
                        }
                        //if (b == 1) break;
                    }
                    if (f == 0) continue;
                    // neu cua co gtri trong T[t]
                    temp = new giatriTT();
                    temp.value = Tab[i][j];
                    temp.check = Tab[i][n - 1];
                    tempTT.Add(temp);
                }

                for (int z = 0; z < tempTT.Count; z++)
                    Count[t][0] += tempTT[z].flag; // dem o vector donvi
                                                 // So luong gia tri cua thuoc tinh dang xet
                Count[t][1] = tempTT.Count;
                if (t == 0)
                {
                    max = Count[t][0];
                    smin = Count[t][1];
                    posMax = t + 1;
                }
                if (Count[t][0] > max)
                {
                    max = Count[t][0];
                    smin = Count[t][1];
                    posMax = t + 1;
                }
                else if (Count[t][0] == max)
                {
                    if (Count[t][1] < smin)
                    {
                        max = Count[t][0];
                        smin = Count[t][1];
                        posMax = t + 1;
                    }
                }
                T.Add(tempTT);
                t++; // vector T<giatriTT> tiep theo;
            }
            return T[posMax - 1];
        }

        private List<string[]> taoBangMoi(List<string[]> Tab, giatriTT T, int posMax)
        {
            List<string[]> Tab1 = new List<string[]>();

            int m = Tab.Count;
            int n = Tab[0].Length;

            string[] temps = new string[n-1];
            int k = 0;
            for(int i = 0; i < n; i++)
            {
                if (Tab[0][i] != Tab[0][posMax])
                    temps[k++] = Tab[0][i];
            }

            Tab1.Add(temps);

            for(int i = 0; i < m; i++)  // Duyet theo hang Table ban dau
            {
                // Neu hang co (gia tri thuoc tinh dang xet) bang voi
                // (gia tri can phan hoach) thi tinh
                if (Tab[i][posMax] == T.value)
                {
                    temps = new string[n - 1];
                    k = 0;
                    for (int j = 0; j < n; j++)
                    {
                        if (j != posMax)
                            temps[k++] = Tab[i][j];
                    }
                    Tab1.Add(temps);
                }
            }
            return Tab1;
        }

        private List<string> deepCopy(List<string> str)
        {
            List<string> results = new List<string>(str.Count);

            str.ForEach((item) =>
            {
                results.Add((string)item.Clone());
            });

            return results;
        }


        private void backT(List<string[]> Tab, List<string> preResults, List<List<string>> results)
        {
            /*
             * Ham duyet cay tim tap luat
             */

            // Neu Table chi con 2 cot Ten & Ketqua thi dung lai
            if (Tab[0].Length == 2)
                return;

            int posMax = 0;
           

            List<giatriTT> T = hamtimTT(Tab, ref posMax);
            int st = T.Count;

            // luu lai ten thuoc tinh
            string tempResults = Tab[0][posMax] + ":";

            for(int i = 0; i < st; i++)
            {
                //Luu lai gia tri thuoc tinh
                string temp = tempResults + T[i].value;
                preResults.Add(temp);
                List<string[]> Tab1;
                
                //Neu la node la thi luu lai ket qua
                if(T[i].flag == 1)
                {
                    List<string> newPreResuls = deepCopy(preResults);
                    newPreResuls.Add(T[i].check);
                    results.Add(newPreResuls);
                }
                else
                {
                    Tab1 = taoBangMoi(Tab, T[i], posMax);
                    backT(Tab1, preResults, results);
                    Tab1.Clear();
                }
                preResults.RemoveAt(preResults.Count - 1);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                List<List<string>> results = new List<List<string>>();
                List<string> preResults = new List<string>();

                backT(data, preResults, results);

                Rules = results;

                string temp = "";
                label3.Text = "";
                foreach (var res in results)
                {
                    foreach (var str in res)
                    {
                        temp = str;
                        if (str != res[res.Count - 1])
                            temp += " --> ";
                        label3.Text += temp;
                    }
                    label3.Text += "\n";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Vui lòng lấy dữ liệu vào trước!\n", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void button4_Click(object sender, EventArgs e)
        {

            MessageBox.Show("Lấy dữ liệu từ file csv hoặc txt. Các thuộc tính cách nhau bởi dấu phẩy.", "Notice!", MessageBoxButtons.OK, MessageBoxIcon.Information);


            Stream myStream = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "\\";
            openFileDialog1.Filter = "excel files (*.csv)|*.xlsx|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = openFileDialog1.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            // Insert code to read the stream here.
                            string filename = openFileDialog1.FileName;
                            loadData(filename);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        
    }
}
