using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pay_Slip
{
    public partial class staffInformations : Form
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
        public staffInformations()
        {
            InitializeComponent();
        }
        public string empcode;
        public string form;
        private void staffInformations_Load(object sender, EventArgs e)
        {
            load();
            loadDGV();
            loadDGVbal();
            calculate();
            ChangeControlStyles(dataGridView2, ControlStyles.OptimizedDoubleBuffer, true);
            this.dataGridView2.ColumnHeadersDefaultCellStyle.SelectionBackColor = this.dataGridView2.ColumnHeadersDefaultCellStyle.BackColor;
            dataGridView2.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
        }
        private void ChangeControlStyles(Control ctrl, ControlStyles flag, bool value)
        {
            MethodInfo method = ctrl.GetType().GetMethod("SetStyle", BindingFlags.Instance | BindingFlags.NonPublic);
            if (method != null)
                method.Invoke(ctrl, new object[] { flag, value });
        }
        private void calculate()
        {
            try
            {
                try
                {
                    using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
                    {
                        tblOTS.Open();
                        string list = "SELECT ROUND(SUM(AMOUNT), 2) FROM tblLOANS WHERE EMPCODE = '" + empcode + "'";
                        SqlCommand command = new SqlCommand(list, tblOTS);
                        decimal count = (decimal)command.ExecuteScalar();
                        label32.Text = string.Format("{0:#,##0.00}", count);
                        tblOTS.Close();
                    }
                }
                catch
                {
                    label32.Text = "0.00";
                }
                try
                {
                    using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
                    {
                        tblOTS.Open();
                        string list = "SELECT ROUND(SUM(PAYMENT), 2) FROM tblLOANS WHERE EMPCODE = '" + empcode + "'";
                        SqlCommand command = new SqlCommand(list, tblOTS);
                        decimal count = (decimal)command.ExecuteScalar();
                        label30.Text = string.Format("{0:#,##0.00}", count);
                        tblOTS.Close();
                    }
                }
                catch
                {
                    label30.Text = "0.00";
                }
                try
                {
                    using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
                    {
                        tblOTS.Open();
                        string list = "SELECT ROUND(SUM(BALANCE), 2) FROM tblLOANS WHERE EMPCODE = '" + empcode + "'";
                        SqlCommand command = new SqlCommand(list, tblOTS);
                        decimal count = (decimal)command.ExecuteScalar();
                        label36.Text = string.Format("{0:#,##0.00}", count);
                        tblOTS.Close();
                    }
                }
                catch
                {
                    label36.Text = "0.00";
                }
                try
                {
                    using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
                    {
                        tblOTS.Open();
                        string list = "SELECT ROUND(SUM(SURCHARGE), 2) FROM tblLOANS WHERE EMPCODE = '" + empcode + "'";
                        SqlCommand command = new SqlCommand(list, tblOTS);
                        decimal count = (decimal)command.ExecuteScalar();
                        label27.Text = string.Format("{0:#,##0.00}", count);
                        tblOTS.Close();
                    }
                }
                catch
                {
                    label27.Text = "0.00";
                }

                decimal all = 0.00M;
                all = Convert.ToDecimal(label36.Text) + Convert.ToDecimal(label32.Text);
                decimal bal = 0.00M;
                bal = all - Convert.ToDecimal(label30.Text);
                label26.Text = string.Format("{0:#,##0.00}", bal);
            }
            catch
            {
                label25.Visible = true;
                label25.Text = "No Loans Found";
            }
            using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
            {
                tblOTS.Open();
                String query = "SELECT dateupdated FROM payrollSettings WHERE details = 'LOANDBF'";
                SqlCommand cmd = new SqlCommand(query, tblOTS);
                SqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    label24.Text = "PAYMENT IS UPDATED ON " + (rdr["dateupdated"].ToString());
                }
                tblOTS.Close();

            }
        }
        public void load()
        {
            using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
            {
                tblOTS.Open();
                DataTable dt = new DataTable();
                String query = "SELECT * FROM tblStaffData WHERE EMPCODE = '" + empcode + "'";
                SqlCommand cmd = new SqlCommand(query, tblOTS);
                SqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    label2.Text = (rdr["EMPCODE"].ToString());
                    label3.Text = (rdr["EMPLOYEENAME"].ToString());
                    label5.Text = string.Format("{0:#,##0.00}", Convert.ToDecimal(rdr["BASICSALARY"].ToString()));
                    label7.Text = string.Format("{0:#,##0.00}", Convert.ToDecimal(rdr["ALLOWANCE"].ToString()));
                    label9.Text = string.Format("{0:#,##0.00}", Convert.ToDecimal(rdr["SSS"].ToString()));
                    label11.Text = string.Format("{0:#,##0.00}", Convert.ToDecimal(rdr["PHIL"].ToString()));
                    label13.Text = string.Format("{0:#,##0.00}", Convert.ToDecimal(rdr["HDMF"].ToString()));
                    label15.Text = string.Format("{0:#,##0.00}", Convert.ToDecimal(rdr["OTHERS"].ToString()));
                    label19.Text = (rdr["CREATEDBY"].ToString());
                    if (rdr["STATUS"].ToString() == "ACTIVE")
                    {
                        label20.Text = (rdr["STATUS"].ToString());
                        label20.ForeColor = Color.Green;
                    }
                    else
                    {
                        label20.Text = (rdr["STATUS"].ToString());
                        label20.ForeColor = Color.Maroon;
                    }
                }
                tblOTS.Close();

            }
        }
        public void loadDGV()
        {
            using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
            {
                dataGridView2.Rows.Clear();
                DataTable distinct = new DataTable();
                tblOTS.Open();
                string list = "SELECT * FROM tblStaffPayroll WHERE EMPCODE = '"+empcode+"' ORDER BY id DESC";
                SqlDataAdapter command = new SqlDataAdapter(list, tblOTS);

                command.Fill(distinct);
                tblOTS.Close();
                foreach (DataRow item in distinct.Rows)
                {
                    int a = dataGridView2.Rows.Add();
                    dataGridView2.Rows[a].Cells["FPERIOD"].Value = item["FROMPERIOD"].ToString();
                    dataGridView2.Rows[a].Cells["TPERIOD"].Value = item["TOPERIOD"].ToString();
                    dataGridView2.Rows[a].Cells["BASICSALARY"].Value = item["BASICSALARY"].ToString();
                    dataGridView2.Rows[a].Cells["ALLOWANCE"].Value = item["ALLOWANCE"].ToString();
                    dataGridView2.Rows[a].Cells["OVERLEAVE"].Value = item["OVERLEAVE"].ToString();
                    dataGridView2.Rows[a].Cells["TOTALAMOUNT"].Value = item["TOTALAMOUNT"].ToString();
                    dataGridView2.Rows[a].Cells["SSS"].Value = item["SSS"].ToString();
                    dataGridView2.Rows[a].Cells["PHILHEALTH"].Value = item["PHILHEALTH"].ToString();
                    dataGridView2.Rows[a].Cells["HDMF"].Value = item["HDMF"].ToString();
                    dataGridView2.Rows[a].Cells["CA"].Value = item["CA"].ToString();
                    dataGridView2.Rows[a].Cells["SC"].Value = item["SC"].ToString();
                    dataGridView2.Rows[a].Cells["SSSLOANS"].Value = item["SSSLOANS"].ToString();
                    dataGridView2.Rows[a].Cells["HDMFLOANS"].Value = item["HDMFLOANS"].ToString();
                    dataGridView2.Rows[a].Cells["COOP"].Value = item["COOP"].ToString();
                    dataGridView2.Rows[a].Cells["FINANCING"].Value = item["FINANCING"].ToString();
                    dataGridView2.Rows[a].Cells["RSSSLOANS"].Value = item["RSSSLOANS"].ToString();
                    dataGridView2.Rows[a].Cells["SALESO"].Value = item["SALESO"].ToString();
                    dataGridView2.Rows[a].Cells["TMF"].Value = item["TMF"].ToString();
                    dataGridView2.Rows[a].Cells["OTHERS"].Value = item["OTHERS"].ToString();
                    dataGridView2.Rows[a].Cells["NET"].Value = item["NET"].ToString();
                }
            }
            dataGridView2.ClearSelection();
        }
        public void loadDGVbal()
        {
            using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
            {
                dataGridView1.Rows.Clear();
                DataTable distinct = new DataTable();
                tblOTS.Open();
                string list = "SELECT TYPE,BALANCE FROM tblStaffBalances WHERE EMPCODE = '" + empcode + "' ORDER BY id DESC";
                SqlDataAdapter command = new SqlDataAdapter(list, tblOTS);

                command.Fill(distinct);
                tblOTS.Close();
                foreach (DataRow item in distinct.Rows)
                {
                    int a = dataGridView1.Rows.Add();
                    dataGridView1.Rows[a].Cells["TYPE"].Value = item["TYPE"].ToString();
                    dataGridView1.Rows[a].Cells["BALANCE"].Value = item["BALANCE"].ToString();
                }

            }
            dataGridView1.ClearSelection();
        }

        private void dataGridView1_Leave(object sender, EventArgs e)
        {
            dataGridView1.ClearSelection();
        }

        private void dataGridView2_Leave(object sender, EventArgs e)
        {
            dataGridView2.ClearSelection();
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            staffEditStatus aa = new staffEditStatus();
            aa.empcode = label2.Text;
            aa.empname = label3.Text;
            aa.status = label20.Text;
            aa.form = form;
            aa.ShowDialog();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            staffEditSAC aa = new staffEditSAC();
            aa.empcode = label2.Text;
            aa.empname = label3.Text;
            aa.basicsalary = label5.Text;
            aa.allowance = label7.Text;
            aa.sss = label9.Text;
            aa.philhealth = label11.Text;
            aa.hdmf = label13.Text;
            aa.others = label15.Text;
            aa.form = form;
            aa.ShowDialog();
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {

        }

        private void label35_Click(object sender, EventArgs e)
        {
            staffCA s = new staffCA();
            s.EMPC = label2.Text;
            s.EMPN = label3.Text;
            s.ShowDialog();
        }
    }
}
