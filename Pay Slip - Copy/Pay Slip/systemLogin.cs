using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pay_Slip
{
    public partial class systemLogin : Form
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
        public systemLogin()
        {
            InitializeComponent();
        }

        private void systemLogin_Load(object sender, EventArgs e)
        {

        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            login();
        }
        private void login()
        {
            try
            {
                using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("select username,password from systemAccounts where username = '" + guna2TextBox3.Text + "' COLLATE SQL_Latin1_General_CP1_CS_AS  and password = '" + guna2TextBox2.Text + "' COLLATE SQL_Latin1_General_CP1_CS_AS", tblOTS);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        try
                        {
                            tblOTS.Open();
                            String query1 = "SELECT * FROM systemAccounts where username like '" + guna2TextBox3.Text.Trim() + "'";
                            SqlCommand cmd2 = new SqlCommand(query1, tblOTS);
                            SqlDataReader dr1 = cmd2.ExecuteReader();
                            bool active = true;
                            payrollDashboard f = new payrollDashboard();
                            if (dr1.Read())
                            {
                                f.id = (dr1["id"].ToString());
                                if (dr1["status"].ToString() == "DEACTIVATE")
                                {
                                    active = false;
                                }
                            }
                            dr1.Close();
                            tblOTS.Close();
                            if (active == false) 
                            {
                                MessageBox.Show("Your account has been deactivate, please contact your administrator. Thank you.");
                            }
                            else
                            {
                                this.Hide();
                                f.ShowDialog();
                                this.Close();
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        finally
                        {
                            if (tblOTS != null)
                            {
                                tblOTS.Dispose();
                            }
                            if (cmd != null)
                            {
                                cmd.Dispose();
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid Login please check username and password");
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void guna2TextBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                login();
                e.Handled = true;
            }
        }
        private void guna2TextBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                login();
                e.Handled = true;
            }
        }
    }
}
