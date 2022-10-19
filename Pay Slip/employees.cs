using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pay_Slip
{
    public partial class employees : Form
    {
      

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams handleparam = base.CreateParams;
                handleparam.ExStyle |= 0x02000000;
                return handleparam;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            this.DoubleBuffered = true;
        }
        public static void SetDoubleBuffered(System.Windows.Forms.Control c)
        {
            //Taxes: Remote Desktop Connection and painting
            //http://blogs.msdn.com/oldnewthing/archive/2006/01/03/508694.aspx
            if (System.Windows.Forms.SystemInformation.TerminalServerSession)
                return;

            System.Reflection.PropertyInfo aProp =
                  typeof(System.Windows.Forms.Control).GetProperty(
                        "DoubleBuffered",
                        System.Reflection.BindingFlags.NonPublic |
                        System.Reflection.BindingFlags.Instance);

            aProp.SetValue(c, true, null);
        }
        public employees()
        {
            InitializeComponent();
        }
        public string id;
        private void employees_Load(object sender, EventArgs e)
        {
            SuspendLayout();
            try
            {
                load();
            }
            catch
            {
                MessageBox.Show("Connection Failed. Please change the server settings to continue");
            }

            ChangeControlStyles(dataGridView1, ControlStyles.OptimizedDoubleBuffer, true);
            this.dataGridView1.ColumnHeadersDefaultCellStyle.SelectionBackColor = this.dataGridView1.ColumnHeadersDefaultCellStyle.BackColor;
            dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
            //load();
            ResumeLayout();

        }
        private void ChangeControlStyles(Control ctrl, ControlStyles flag, bool value)
        {
            MethodInfo method = ctrl.GetType().GetMethod("SetStyle", BindingFlags.Instance | BindingFlags.NonPublic);
            if (method != null)
                method.Invoke(ctrl, new object[] { flag, value });
        }
        DataTable dt = new DataTable();
        public void load()
        {
            //dataGridView1.BeginInvoke((Action)delegate ()
            //{
            using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
            {


                DataTable ddt = new DataTable();

                tblOTS.Open();

                string list = "SELECT EMPCODE,FIRST,MIDDLE,FAMILY FROM tblPERID ORDER BY FAMILY ASC";
                SqlCommand command = new SqlCommand(list, tblOTS);
                SqlDataReader reader = command.ExecuteReader();
                ddt.Load(reader);

                dt.Clear();
                dt.Columns.Add("EMPCODE");
                dt.Columns.Add("NAME");
                foreach (DataRow row in ddt.Rows)
                {
                    object[] o = { row["EMPCODE"].ToString().Trim(), row["FAMILY"].ToString().Trim() + ", " + row["FIRST"].ToString().Trim() + " " + row["MIDDLE"].ToString().Trim() };
                    dt.Rows.Add(o);
                }
                dataGridView1.DataSource = dt;
                dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
                dataGridView1.RowHeadersVisible = false;

                tblOTS.Close();




            }

            //});
        }

        string lb = "{";
        string rb = "}";
        private void pictureBox1_Click(object sender, EventArgs e)
        {
         
        }

        private void bunifuCustomTextbox1_TextChanged(object sender, EventArgs e)
        {
           

        }

        private void bunifuCustomTextbox2_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void bunifuCustomTextbox2_TextChanged_1(object sender, EventArgs e)
        {
            

        }

        private void bunifuCustomTextbox3_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void bunifuCustomLabel15_Click(object sender, EventArgs e)
        {
       
        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {
            (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = string.Format("EMPCODE LIKE '%{0}%'", guna2TextBox1.Text.Replace("[", lb).Replace("]", rb).Replace("​*", "[*​]").Replace("%", "[%]").Replace("'", "''"));
        }

        private void guna2TextBox2_TextChanged(object sender, EventArgs e)
        {
            (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = string.Format("NAME LIKE '%" + guna2TextBox2.Text.Replace("[", lb).Replace("]", rb).Replace("​*", "[*​]").Replace("%", "[%]").Replace("'", "''") + "%'");
        }
        public string type;
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            bool access = false;
            using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
            {
                tblOTS.Open();
                SqlCommand cmd2 = new SqlCommand("SELECT grantaccess FROM systemAccountsPermissions where accountid like '" + id + "' AND detail = 'EMPINFO'", tblOTS);
                SqlDataReader dr1 = cmd2.ExecuteReader();
                payrollDashboard rw = new payrollDashboard();
                if (dr1.Read())
                {
                    if (type == "Developer")
                    {
                        access = true;
                    }
                    else
                    {
                        access = Convert.ToBoolean(dr1["grantaccess"].ToString());
                    }
                }
                dr1.Close();
                tblOTS.Close();
            }
            if (access == true)
            {
                this.Hide();
                payslipEmpDetail P = new payslipEmpDetail();
                P.id = id;
                P.EMPC = dataGridView1.CurrentRow.Cells["Column1"].Value.ToString().Trim();
                P.EMPN = dataGridView1.CurrentRow.Cells["Column2"].Value.ToString().Trim();
                P.type = type;
                P.ShowDialog();
                this.Dispose();
            }
            else
            {
                MessageBox.Show("You don't have permission to access this. Please contact your administrator. Thank you.");
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void employees_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Hide();
            payrollDashboard frm = new payrollDashboard();
            frm.id = id;
            frm.ShowDialog();
            this.Dispose();
        }
    }
}
