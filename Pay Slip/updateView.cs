using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace Pay_Slip
{
    public partial class updateView : Form
    {
        public updateView()
        {
            InitializeComponent();
        }
        public string type;
        private void updateView_Load(object sender, EventArgs e)
        {
            if (type == "IMPORTANT")
            {
                label1.Text = "Important Updates!";
            }
            load();
            dataGridView1.ClearSelection();
           
        }
        public void load()
        {
            dataGridView1.Rows.Clear();
            using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
            {
                DataTable dt = new DataTable();
                tblOTS.Open();
                string list = "";
                if (guna2CheckBox1.Checked)
                {
                    list = "SELECT type,description,date FROM tblUpdatesList ORDER BY id DESC";
                }
                else
                {
                    list = "SELECT TOP 50 type,description,date FROM tblUpdatesList ORDER BY id DESC";
                }
                SqlCommand command = new SqlCommand(list, tblOTS);
                SqlDataReader reader = command.ExecuteReader();
                dt.Load(reader);
                tblOTS.Close();
                int rows = 0;
                foreach (DataRow row in dt.Rows)
                {
                    rows++;
                    int a = dataGridView1.Rows.Add();
                    dataGridView1.Rows[a].Cells["Column1"].Value = row["type"].ToString();
                    var nl = Environment.NewLine;

                    dataGridView1.Rows[a].Cells["Column2"].Value = row["description"].ToString().Replace("<:newline:>", nl);

                    if (row["date"].ToString() != "" || row["date"] != DBNull.Value)
                    {
                        dataGridView1.Rows[a].Cells["Column3"].Value = DateTime.Parse(row["date"].ToString()).ToString("MM/dd/yyyy hh:mm tt");
                    }
                    if (rows != 1 && row["type"].ToString() == "3")
                    {
                        dataGridView1.Rows.Add();
                    }
                }
            }
        }
        private void guna2Button2_Click(object sender, EventArgs e)
        {
            try
            {
                string path = Path.GetDirectoryName(Application.ExecutablePath) + @"\AppUpdator";
                Process.Start(path + "\\SystemAppUpdator.exe");

                Process[] workers = Process.GetProcessesByName("Pay Slip");
                foreach (Process worker in workers)
                {
                    worker.Kill();
                    worker.WaitForExit();
                    worker.Dispose();
                }
            }
            catch
            {
                MessageBox.Show("Please contact service provider");
            }
        }

        private void updateView_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (type == "IMPORTANT")
            {
                Process[] workers = Process.GetProcessesByName("Pay Slip");
                foreach (Process worker in workers)
                {
                    worker.Kill();
                    worker.WaitForExit();
                    worker.Dispose();
                }
            }
        }

        private void listBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["Column1"].Value == null || row.Cells["Column1"].Value == DBNull.Value || String.IsNullOrWhiteSpace(row.Cells["Column1"].Value.ToString()))
                {
                }
                else
                {
                    if (row.Cells["Column1"].Value.ToString() == "1")
                    {
                        row.Cells["Column2"].Style.ForeColor = Color.Blue;
                        row.Cells["Column2"].Style.Font = new Font("Segoe UI Semibold", 11);
                    }
                    else if (row.Cells["Column1"].Value.ToString() == "2")
                    {
                        row.Cells["Column2"].Style.ForeColor = Color.Black;
                        row.Cells["Column2"].Style.Font = new Font("Segoe UI Semibold", 10);
                    }
                    else if (row.Cells["Column1"].Value.ToString() == "3")
                    {
                        row.Cells["Column2"].Style.ForeColor = Color.DimGray;
                        row.Cells["Column2"].Style.Font = new Font("Segoe UI Semibold", 10, FontStyle.Italic);
                    }
                    row.Cells["Column3"].Style.Font = new Font("Segoe UI Semibold", 8);
                }

            }
        }

        private void guna2CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            load();
        }
    }
}
