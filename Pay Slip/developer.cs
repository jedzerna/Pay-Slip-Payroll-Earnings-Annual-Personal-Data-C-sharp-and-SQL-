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
    public partial class developer : Form
    {
        public developer()
        {
            InitializeComponent();
        }

        private void developer_Load(object sender, EventArgs e)
        {
            load();
          
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
            dataGridView1.ClearSelection();
            if (dataGridView1.Rows.Count != 0)
            {
                label4.Text = dataGridView1.Rows[0].Cells["Column2"].Value.ToString();
            }
        }

        private void guna2CheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            load();
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

        private void getupdatedfiles()
        {

            string sourcePath = Path.GetDirectoryName(Application.ExecutablePath);
            string targetPath = @"\\JED-PC\Users\Public\Documents\SYSGLU";

            //MessageBox.Show(targetPath);
            string fileName = "Pay Slip.exe";
            string sourceFile = System.IO.Path.Combine(sourcePath, "Pay Slip.exe");
            string destFile = System.IO.Path.Combine(targetPath, "Pay Slip.exe");
            System.IO.Directory.CreateDirectory(targetPath);
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(sourcePath);
            int count = Directory.GetFiles(sourcePath).Length;
            System.IO.File.Copy(sourceFile, destFile, true);
            if (System.IO.Directory.Exists(sourcePath))
            {
                string[] files = System.IO.Directory.GetFiles(sourcePath);


                // Copy the files and overwrite destination files if they already exist.
                foreach (string s in files)
                {
                    // Use static Path methods to extract only the file name from the path.
                    fileName = System.IO.Path.GetFileName(s);
                    destFile = System.IO.Path.Combine(targetPath, fileName);
                    System.IO.File.Copy(s, destFile, true);

                }

            }
            else
            {
                MessageBox.Show("Source path does not exist!", "Error");
                return;
            }
        }
        private void guna2Button3_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure to add this update?", "Add", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                //updateupdator();
                getupdatedfiles();
                using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("update tblUpdatesVer set version=@version,type=@type where id = 1", tblOTS);

                    tblOTS.Open();
                    //cmd.Parameters.AddWithValue("@updatestats", guna2TextBox2.Text);
                    cmd.Parameters.AddWithValue("@version", guna2TextBox1.Text);
                    if (guna2CheckBox1.Checked)
                    {
                        cmd.Parameters.AddWithValue("@type", "IMPORTANT");
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@type", "");
                    }
                    cmd.ExecuteNonQuery();
                    tblOTS.Close();
                }
                var date = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
                var desc = "";
                int lines = 0;
                foreach (string line in guna2TextBox3.Lines)
                {
                    lines++;
                    if (lines == 1)
                    {
                        desc += line.ToString();
                    }
                    else
                    {
                        desc += "<:newline:>"+line.ToString();
                    }

                }
                using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
                {
                    tblOTS.Open();
                    string insStmt = "insert into tblUpdatesList ([type], [description], [date]) values" +
                        " (@type,@description,@date)";
                    SqlCommand insCmd = new SqlCommand(insStmt, tblOTS);
                    insCmd.Parameters.AddWithValue("@type", "3");
                    insCmd.Parameters.AddWithValue("@description", desc);
                    insCmd.Parameters.AddWithValue("@date", DBNull.Value);
                    int affectedRows = insCmd.ExecuteNonQuery();
                    tblOTS.Close();
                }

                using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
                {
                    tblOTS.Open();
                    string insStmt = "insert into tblUpdatesList ([type], [description], [date]) values" +
                        " (@type,@description,@date)";
                    SqlCommand insCmd = new SqlCommand(insStmt, tblOTS);
                    insCmd.Parameters.AddWithValue("@type", "2");
                    insCmd.Parameters.AddWithValue("@description", guna2TextBox5.Text);
                    insCmd.Parameters.AddWithValue("@date", DBNull.Value);
                    int affectedRows = insCmd.ExecuteNonQuery();
                    tblOTS.Close();
                }
                using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
                {
                    tblOTS.Open();
                    string insStmt = "insert into tblUpdatesList ([type], [description], [date]) values" +
                        " (@type,@description,@date)";
                    SqlCommand insCmd = new SqlCommand(insStmt, tblOTS);
                    insCmd.Parameters.AddWithValue("@type", "1");
                    insCmd.Parameters.AddWithValue("@description", guna2TextBox1.Text);
                    insCmd.Parameters.AddWithValue("@date", date);
                    int affectedRows = insCmd.ExecuteNonQuery();
                    tblOTS.Close();
                }
                load();
                MessageBox.Show("Update Info Added");
                guna2TextBox1.Text = "";
                guna2TextBox5.Text = "";
                guna2TextBox3.Text = "";
            }

        }
    }
}
