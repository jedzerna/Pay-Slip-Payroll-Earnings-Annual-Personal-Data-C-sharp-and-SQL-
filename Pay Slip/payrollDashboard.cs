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
using System.Threading;
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
            timer2.Enabled = true;
            timer2.Start();
            loadversion();
            load();
            log();
            calculatepayroll();
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
                        if (label5.Text == "User")
                        {
                            guna2Button9.Visible = false;
                        }
                        if (label5.Text == "Developer")
                        {
                            label11.Visible = true;
                        }
                        if (dr1["image"] != DBNull.Value)
                        {
                            byte[] img = (byte[])(dr1["image"]);
                            MemoryStream mstream = new MemoryStream(img);
                            guna2CirclePictureBox1.Image = System.Drawing.Image.FromStream(mstream);
                        }
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
        SqlCommand cmd;
        string version = "";
        public void loadversion()
        {
            using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
            {
                try
                {
                    string fileName1 = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"\systemVER.Vsn";

                    if (File.Exists(fileName1))
                    {
                        // Read entire text file content in one string    
                        string text = File.ReadAllText(fileName1);
                        //MessageBox.Show(text.ToString().Trim());
                        version = text.ToString().Trim();
                    }

                    //tblOTS.Open();
                    //String query = "SELECT * FROM tblUpdatesVer where id = 1";
                    //cmd = new SqlCommand(query, tblOTS);
                    //SqlDataReader dr = cmd.ExecuteReader();

                    //if (dr.Read())
                    //{

                    //    if (dr["version"].ToString() == version)
                    //    {

                    //    }
                    //    else
                    //    {
                    //        if (dr["type"].ToString() == "IMPORTANT")
                    //        {
                    //            timer2.Stop();
                    //            MessageBox.Show("Important updates available!!!");
                    //            updateView u = new updateView();
                    //            u.type = dr["type"].ToString();
                    //            u.ShowDialog();
                    //        }
                    //        else
                    //        {
                    //            DialogResult dialogResult1 = MessageBox.Show("Updates Available, Do you want to update now?", "Update?", MessageBoxButtons.YesNo);
                    //            if (dialogResult1 == DialogResult.Yes)
                    //            {
                    //                updateView u = new updateView();
                    //                u.type = dr["type"].ToString();
                    //                u.ShowDialog();
                    //            }
                    //            else
                    //            {
                    //                timer1.Start();
                    //                label12.Visible = true;
                    //            }
                    //        }
                    //    }

                    //}
                    //dr.Close();
                    //tblOTS.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }


        }
        private void calculatepayroll()
        {
            using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
            {
                DataTable distinct = new DataTable();
                tblOTS.Open();
                string list = "SELECT NET FROM tblStaffPayroll WHERE FROMPERIOD = '" + dataGridView2.Rows[0].Cells["FPERIOD"].Value.ToString() + "' AND TOPERIOD = '" + dataGridView2.Rows[0].Cells["TPERIOD"].Value.ToString() + "'";
                SqlDataAdapter command = new SqlDataAdapter(list, tblOTS);
                command.Fill(distinct);
                tblOTS.Close();
                decimal sum = 0.00M;
                foreach (DataRow item in distinct.Rows)
                {
                    sum += Convert.ToDecimal(item["NET"].ToString());
                }
                label9.Text = string.Format("{0:#,##0.00}", sum);
            }
            int wsum = label9.Width + label9.Location.X;
            label10.SetBounds(wsum, 54, 41, 21);
            using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
            {
                DataTable distinct = new DataTable();
                tblOTS.Open();
                string list = "SELECT NET FROM tblStaffPayroll WHERE FROMPERIOD = '" + dataGridView2.Rows[1].Cells["FPERIOD"].Value.ToString() + "' AND TOPERIOD = '" + dataGridView2.Rows[1].Cells["TPERIOD"].Value.ToString() + "'";
                SqlDataAdapter command = new SqlDataAdapter(list, tblOTS);
                command.Fill(distinct);
                tblOTS.Close();
                decimal sum = 0.00M;
                foreach (DataRow item in distinct.Rows)
                {
                    sum += Convert.ToDecimal(item["NET"].ToString());
                }
                label10.Text = string.Format("{0:#,##0.00}", sum);
            }
            if (Convert.ToDecimal(label9.Text) < Convert.ToDecimal(label10.Text))
            {
                label10.ForeColor = Color.Green;
            }
            else if (Convert.ToDecimal(label9.Text) > Convert.ToDecimal(label10.Text))
            {
                label10.ForeColor = Color.Maroon;
            }
            else
            {
                label10.ForeColor = Color.Black;
            }
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
            if (label5.Text == "Developer")
            {
                access = true;
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
                MessageBox.Show("You don't have permission to access this. Please contact your administrator. Thank you.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            if (label5.Text == "Developer")
            {
                access = true;
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
                MessageBox.Show("You don't have permission to access this. Please contact your administrator. Thank you.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            if (label5.Text == "Developer")
            {
                access = true;
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
                MessageBox.Show("You don't have permission to access this. Please contact your administrator. Thank you.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            if (label5.Text == "Developer")
            {
                access = true;
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
                MessageBox.Show("You don't have permission to access this. Please contact your administrator. Thank you.","Error", MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
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
            if (label5.Text == "Developer")
            {
                access = true;
            }
            if (access == true)
            {
                staffViewPayroll S = new staffViewPayroll();
                S.FPERIOD = dataGridView2.CurrentRow.Cells["FPERIOD"].Value.ToString();
                S.TPERIOD = dataGridView2.CurrentRow.Cells["TPERIOD"].Value.ToString();
                S.ShowDialog();
            }
            else
            {
                MessageBox.Show("You don't have permission to access this. Please contact your administrator. Thank you.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
            if (label5.Text == "Developer")
            {
                access = true;
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
                MessageBox.Show("You don't have permission to access this. Please contact your administrator. Thank you.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            this.Hide();
            employees frm = new employees();
            frm.id = id;
            frm.type = label5.Text;
            frm.ShowDialog();
            this.Dispose();
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
            if (label5.Text == "Developer")
            {
                access = true;
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
                MessageBox.Show("You don't have permission to access this. Please contact your administrator. Thank you.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void label35_Click(object sender, EventArgs e)
        {
            timer2.Enabled = false;
            timer2.Stop();
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
            frm.typelog = label5.Text;
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

        private void guna2Panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (label12.Visible == true)
            {
                if (pictureBox2.Visible == true)
                {
                    pictureBox2.Visible = false;
                }
                else if (pictureBox2.Visible == false)
                {
                    pictureBox2.Visible = true;
                }
            }
        }
        int a = 0;
        int b = 0;
        private void timer2_Tick(object sender, EventArgs e)
        {
            using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
            {
                try
                {
                    tblOTS.Open();
                    String query = "SELECT * FROM tblUpdatesVer where id = 1";
                    cmd = new SqlCommand(query, tblOTS);
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        if (dr["version"].ToString() != version)
                        {
                            if (dr["type"].ToString() == "IMPORTANT")
                            {
                                timer2.Stop();
                                MessageBox.Show("Important updates available!!!");
                                updateView u = new updateView();
                                u.type = dr["type"].ToString();
                                u.ShowDialog();
                            }
                            else
                            {
                                timer1.Start();
                                label12.Visible = true;
                            }
                        }
                    }
                    dr.Close();
                    tblOTS.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void label12_Click(object sender, EventArgs e)
        {
            updateView u = new updateView();
            u.ShowDialog();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            updateView u = new updateView();
            u.ShowDialog();
        }

        private void label11_Click(object sender, EventArgs e)
        {
            developer d = new developer();
            d.ShowDialog();
        }

        private void guna2Button10_Click(object sender, EventArgs e)
        {
            bool access = false;
            using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
            {
                tblOTS.Open();
                SqlCommand cmd2 = new SqlCommand("SELECT grantaccess FROM systemAccountsPermissions where accountid like '" + id + "' AND detail = 'ANNUAL'", tblOTS);
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
            if (label5.Text == "Developer")
            {
                access = true;
            }
            if (access == true)
            {
                this.Hide();//Hide the 'current' form, i.e frm_form1 
                            //show another form ( frm_form2 )   
                DashAnnual frm = new DashAnnual();
                frm.id = id;
                frm.ShowDialog();
                //Close the form.(frm_form1)
                this.Dispose();
            }
            else
            {
                MessageBox.Show("You don't have permission to access this. Please contact your administrator. Thank you.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void guna2Button11_Click(object sender, EventArgs e)
        {
            bool access = false;
            using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
            {
                tblOTS.Open();
                SqlCommand cmd2 = new SqlCommand("SELECT grantaccess FROM systemAccountsPermissions where accountid like '" + id + "' AND detail = 'COMP'", tblOTS);
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
            if (label5.Text == "Developer")
            {
                access = true;
            }
            if (access == true)
            {
                this.Hide();//Hide the 'current' form, i.e frm_form1 
                            //show another form ( frm_form2 )   
                ComputerList frm = new ComputerList();
                frm.id = id;
                frm.ShowDialog();
                //Close the form.(frm_form1)
                this.Dispose();
            }
            else
            {
                MessageBox.Show("You don't have permission to access this. Please contact your administrator. Thank you.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void payrollDashboard_FormClosing(object sender, FormClosingEventArgs e)
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
}
