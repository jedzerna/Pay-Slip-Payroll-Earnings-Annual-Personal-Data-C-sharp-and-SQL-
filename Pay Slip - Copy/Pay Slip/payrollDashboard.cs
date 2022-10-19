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
    public partial class payrollDashboard : Form
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
        public payrollDashboard()
        {
            InitializeComponent();
        }
        public string id;
        private void payrollDashboard_Load(object sender, EventArgs e)
        {
          
            load();
            log();
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
        public void log()
        {
            try
            {
                using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
                {

                    tblOTS.Open();
                    String query1 = "SELECT * FROM systemAccounts where id like '" + id + "'";
                    SqlCommand cmd2 = new SqlCommand(query1, tblOTS);
                    SqlDataReader dr1 = cmd2.ExecuteReader();

                    payrollDashboard f = new payrollDashboard();
                    if (dr1.Read())
                    {
                        label4.Text = (dr1["username"].ToString());
                        label3.Text = (dr1["fullname"].ToString());
                        label5.Text = (dr1["type"].ToString());
                    }
                    else
                    {
                        dr1.Close();
                        MessageBox.Show("Somethings wrong. Please re-login!");
                        this.Hide();//Hide the 'current' form, i.e frm_form1 
                                    //show another form ( frm_form2 )   
                        systemLogin frm = new systemLogin();
                        frm.ShowDialog();
                        //Close the form.(frm_form1)
                        this.Dispose();
                    }
                    dr1.Close();

                }
            }
            catch
            {
                MessageBox.Show("Somethings wrong. Please re-login!");
                this.Hide();//Hide the 'current' form, i.e frm_form1 
                            //show another form ( frm_form2 )   
                systemLogin frm = new systemLogin();
                frm.ShowDialog();
                //Close the form.(frm_form1)
                this.Dispose();
            }
        }
        private void load()
        {
            using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
            {
                DataTable distinct = new DataTable();
                tblOTS.Open();
                string list = "SELECT DISTINCT FROMPERIOD,TOPERIOD,CREATEDBY FROM tblStaffPayroll ORDER BY TOPERIOD DESC";
                SqlDataAdapter command = new SqlDataAdapter(list, tblOTS);
                command.Fill(distinct);
                tblOTS.Close();
                dataGridView2.DataSource = distinct;
            }
            dataGridView2.ClearSelection();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            bool access = false;
            using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
            {
                tblOTS.Open();
                SqlCommand cmd2 = new SqlCommand("SELECT grantaccess FROM systemAccountsPermissions where accountid like '" + id + "' AND detail = 'OTSE'", tblOTS);
                SqlDataReader dr1 = cmd2.ExecuteReader();
                payrollDashboard rw = new payrollDashboard();
                if (dr1.Read())
                {
                    if (label5.Text == "Developer")
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
                this.Hide();//Hide the 'current' form, i.e frm_form1 
                            //show another form ( frm_form2 )   
                EarningsView frm = new EarningsView();
                frm.id = id;
                frm.ShowDialog();
                //Close the form.(frm_form1)
                this.Dispose();
            }
            else
            {
                MessageBox.Show("You don't have permission to access this. Please contact your administrator. Thank you.");
            }
        }

        private void payrollDashboard_FormClosed(object sender, FormClosedEventArgs e)
        {
            //this.Hide();
            //selection frm = new selection();
            //frm.ShowDialog();
            //this.Close();
        }

        private void guna2Button7_Click(object sender, EventArgs e)
        {
            bool access = false;
            using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
            {
                tblOTS.Open();
                SqlCommand cmd2 = new SqlCommand("SELECT grantaccess FROM systemAccountsPermissions where accountid like '" + id + "' AND detail = 'PSLIP'", tblOTS);
                SqlDataReader dr1 = cmd2.ExecuteReader();
                payrollDashboard rw = new payrollDashboard();
                if (dr1.Read())
                {
                    if (label5.Text == "Developer")
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
                this.Hide();//Hide the 'current' form, i.e frm_form1 
                            //show another form ( frm_form2 )   
                selection frm = new selection();
                frm.id = id;
                frm.ShowDialog();
                //Close the form.(frm_form1)
                this.Dispose();
            }
            else
            {
                MessageBox.Show("You don't have permission to access this. Please contact your administrator. Thank you.");
            }
        }

        private void guna2Button8_Click(object sender, EventArgs e)
        {
            bool access = false;
            using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
            {
                tblOTS.Open();
                SqlCommand cmd2 = new SqlCommand("SELECT grantaccess FROM systemAccountsPermissions where accountid like '" + id + "' AND detail = 'STAFFINFO'", tblOTS);
                SqlDataReader dr1 = cmd2.ExecuteReader();
                payrollDashboard rw = new payrollDashboard();
                if (dr1.Read())
                {
                    if (label5.Text == "Developer")
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
                this.Hide();//Hide the 'current' form, i.e frm_form1 
                            //show another form ( frm_form2 )   
                staffDashboard frm = new staffDashboard();
                frm.id = id;
                frm.type = label5.Text;
                frm.ShowDialog();
                //Close the form.(frm_form1)
                this.Dispose();
            }
            else
            {
                MessageBox.Show("You don't have permission to access this. Please contact your administrator. Thank you.");
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            bool access = false;
            using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
            {
                tblOTS.Open();
                SqlCommand cmd2 = new SqlCommand("SELECT grantaccess FROM systemAccountsPermissions where accountid like '" + id + "' AND detail = 'PAYROLL'", tblOTS);
                SqlDataReader dr1 = cmd2.ExecuteReader();
                payrollDashboard rw = new payrollDashboard();
                if (dr1.Read())
                {
                    if (label5.Text == "Developer")
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
                staffPayroll frm = new staffPayroll();
                frm.id = id;
                frm.name = label3.Text;
                frm.ShowDialog();
                this.Dispose();
            }
            else
            {
                MessageBox.Show("You don't have permission to access this. Please contact your administrator. Thank you.");
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            staffViewPayroll S = new staffViewPayroll();
            S.FPERIOD = dataGridView2.CurrentRow.Cells["FPERIOD"].Value.ToString();
            S.TPERIOD = dataGridView2.CurrentRow.Cells["TPERIOD"].Value.ToString();
            S.ShowDialog();
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            bool access = false;
            using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
            {
                tblOTS.Open();
                SqlCommand cmd2 = new SqlCommand("SELECT grantaccess FROM systemAccountsPermissions where accountid like '" + id + "' AND detail = 'STAFFE'", tblOTS);
                SqlDataReader dr1 = cmd2.ExecuteReader();
                payrollDashboard rw = new payrollDashboard();
                if (dr1.Read())
                {
                    if (label5.Text == "Developer")
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
                this.Hide();//Hide the 'current' form, i.e frm_form1 
                            //show another form ( frm_form2 )   
                staffEarnings frm = new staffEarnings();
                frm.id = id;
                frm.ShowDialog();
                //Close the form.(frm_form1)
                this.Dispose();
            }
            else
            {
                MessageBox.Show("You don't have permission to access this. Please contact your administrator. Thank you.");
            }
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {

            employees frm = new employees();
            frm.id = id;
            frm.type = label5.Text;
            frm.ShowDialog();
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            projects frm = new projects();
            frm.ShowDialog();
        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {
            bool access = false;
            using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
            {
                tblOTS.Open();
                SqlCommand cmd2 = new SqlCommand("SELECT grantaccess FROM systemAccountsPermissions where accountid like '" + id + "' AND detail = 'DATA'", tblOTS);
                SqlDataReader dr1 = cmd2.ExecuteReader();
                payrollDashboard rw = new payrollDashboard();
                if (dr1.Read())
                {
                    if (label5.Text == "Developer")
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
                this.Hide();//Hide the 'current' form, i.e frm_form1 
                            //show another form ( frm_form2 )   
                payrollSettings frm = new payrollSettings();
                frm.id = id;
                frm.ShowDialog();
                //Close the form.(frm_form1)
                this.Dispose();
            }
            else
            {
                MessageBox.Show("You don't have permission to access this. Please contact your administrator. Thank you.");
            }
        }

        private void label35_Click(object sender, EventArgs e)
        {
            this.Hide();//Hide the 'current' form, i.e frm_form1 
                        //show another form ( frm_form2 )   
            systemLogin frm = new systemLogin();
            frm.ShowDialog();
            //Close the form.(frm_form1)
            this.Dispose();
        }

        private void guna2Button9_Click(object sender, EventArgs e)
        {
            this.Hide();//Hide the 'current' form, i.e frm_form1 
                        //show another form ( frm_form2 )   
            systemAccounts frm = new systemAccounts();
            frm.id = id;
            frm.name = label3.Text;
            frm.ShowDialog();
            //Close the form.(frm_form1)
            this.Dispose();
        }

        private void label6_Click(object sender, EventArgs e)
        {
            systemAccountsInfoandUpdate frm = new systemAccountsInfoandUpdate();
            frm.id = id;
            frm.ShowDialog();
        }
    }
}
