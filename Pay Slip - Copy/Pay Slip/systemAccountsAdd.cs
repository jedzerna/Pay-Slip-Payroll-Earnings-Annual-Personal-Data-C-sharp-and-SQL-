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
    public partial class systemAccountsAdd : Form
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
        public systemAccountsAdd()
        {
            InitializeComponent();
        }
        public string id;
        public string form;
        private string createdby;
        public string name;
        private void systemAccountsAdd_Load(object sender, EventArgs e)
        {
            int a = dataGridView2.Rows.Add();
            dataGridView2.Rows[a].Cells["details"].Value = "OTS Earnings";
            dataGridView2.Rows[a].Cells["Column1"].Value = "OTSE";
            int b = dataGridView2.Rows.Add();
            dataGridView2.Rows[b].Cells["details"].Value = "Staff Earnings";
            dataGridView2.Rows[b].Cells["Column1"].Value = "STAFFE";
            int c = dataGridView2.Rows.Add();
            dataGridView2.Rows[c].Cells["details"].Value = "Payroll";
            dataGridView2.Rows[c].Cells["Column1"].Value = "PAYROLL";
            int d = dataGridView2.Rows.Add();
            dataGridView2.Rows[d].Cells["details"].Value = "Employees Informations";
            dataGridView2.Rows[d].Cells["Column1"].Value = "EMPINFO";
            int q = dataGridView2.Rows.Add();
            dataGridView2.Rows[q].Cells["details"].Value = "Data";
            dataGridView2.Rows[q].Cells["Column1"].Value = "DATA";
            int f = dataGridView2.Rows.Add();
            dataGridView2.Rows[f].Cells["details"].Value = "Pay Slip";
            dataGridView2.Rows[f].Cells["Column1"].Value = "PSLIP";
            int QW = dataGridView2.Rows.Add();
            dataGridView2.Rows[QW].Cells["details"].Value = "Staffs";
            dataGridView2.Rows[QW].Cells["Column1"].Value = "STAFFINFO";
            if (form == "update")
            {
                using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
                {
                    tblOTS.Open();
                    String query1 = "SELECT * FROM systemAccounts where id like '" + id + "'";
                    SqlCommand cmd2 = new SqlCommand(query1, tblOTS);
                    SqlDataReader dr1 = cmd2.ExecuteReader();

                    payrollDashboard rw = new payrollDashboard();
                    if (dr1.Read())
                    {
                        if (dr1["type"].ToString() == "Developer")
                        {
                            guna2ComboBox1.Items.Add("Developer");
                        }
                        EMPCODE.Text = (dr1["username"].ToString());
                        EMPLOYEENAME.Text = (dr1["fullname"].ToString());
                        guna2ComboBox1.Text = (dr1["type"].ToString());
                        type = (dr1["type"].ToString());
                        guna2ComboBox2.Text = (dr1["status"].ToString());
                        pass = (dr1["password"].ToString());
                    }
                    dr1.Close();
                    tblOTS.Close();
                }

                using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
                {
                    DataTable distinct = new DataTable();
                    tblOTS.Open();
                    string list = "SELECT detail,grantaccess FROM systemAccountsPermissions WHERE accountid = '" + id + "'";
                    SqlDataAdapter command = new SqlDataAdapter(list, tblOTS);
                    command.Fill(distinct);
                    foreach (DataRow dtrow in distinct.Rows)
                    {
                        if (dtrow["grantaccess"].ToString() == "True")
                        {
                            foreach (DataGridViewRow row in dataGridView2.Rows)
                            {
                                if (dtrow["detail"].ToString() == row.Cells["Column1"].Value.ToString())
                                {
                                    row.Cells["grant"].Value = true;
                                    break;
                                }
                            }
                        }
                    }
                    tblOTS.Close();
                }
            }
            else
            {
                EMPCODE.ReadOnly = false;
                EMPLOYEENAME.ReadOnly = false;
                guna2Button2.Text = "Add";
            }

        }
        private string pass;
        private string type;

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            if (form != "update")
            {
                using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
                {
                    tblOTS.Open();
                    SqlCommand check_User_Name = new SqlCommand("SELECT COUNT(*) FROM [systemAccounts] WHERE username = @username", tblOTS);
                    check_User_Name.Parameters.AddWithValue("@username", EMPCODE.Text);
                    int UserExist = (int)check_User_Name.ExecuteScalar();

                    if (UserExist > 0)
                    {
                        label23.Text = "Username is already exist";
                        label23.ForeColor = Color.Maroon;
                        label23.Visible = true;
                        return;
                    }
                    tblOTS.Close();
                }
            }
            label23.Visible = false;
            saved();
        }
        private void saved()
        {
            if (form == "update")
            {
                if (type == "Developer")
                {
                    MessageBox.Show("Sorry but you can't update a Developer user, due to for maintainance of the system. Thank you.");
                }
                else
                {
                    using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
                    {
                        tblOTS.Open();
                        using (SqlCommand command = new SqlCommand("DELETE FROM systemAccountsPermissions WHERE accountid = '" + id + "'", tblOTS))
                        {
                            command.ExecuteNonQuery();
                        }
                        tblOTS.Close();

                    }
                    foreach (DataGridViewRow row in dataGridView2.Rows)
                    {
                        using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
                        {
                            tblOTS.Open();
                            string insStmt = "insert into systemAccountsPermissions ([accountid], [detail], [grantaccess]) values" +
                                " (@accountid,@detail,@grantaccess)";
                            SqlCommand insCmd = new SqlCommand(insStmt, tblOTS);
                            insCmd.Parameters.Clear();
                            insCmd.Parameters.AddWithValue("@accountid", id);
                            insCmd.Parameters.AddWithValue("@detail", row.Cells["Column1"].Value.ToString().Trim());
                            if (Convert.ToBoolean(row.Cells["grant"].Value) == true)
                            {
                                insCmd.Parameters.AddWithValue("@grantaccess", "True");
                            }
                            else
                            {
                                insCmd.Parameters.AddWithValue("@grantaccess", "False");
                            }
                            int affectedRows = insCmd.ExecuteNonQuery();
                            tblOTS.Close();
                        }
                    }
                    using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
                    {
                        SqlCommand cmd3 = new SqlCommand("update systemAccounts set status=@status,type=@type where id=@id", tblOTS);
                        tblOTS.Open();
                        cmd3.Parameters.AddWithValue("@id", id);
                        cmd3.Parameters.AddWithValue("@status", guna2ComboBox2.Text);
                        cmd3.Parameters.AddWithValue("@type", guna2ComboBox1.Text);
                        cmd3.ExecuteNonQuery();
                        tblOTS.Close();
                    }
                    MessageBox.Show("Updated");
                }
            }
            else
            {
                using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
                {
                    SqlCommand cmd3 = new SqlCommand("insert into systemAccounts ([username], [password], [fullname],[type],[status],[createdby]) values" +
                            " (@username,@password,@fullname,@type,@status,@createdby)", tblOTS);
                    tblOTS.Open();
                    cmd3.Parameters.AddWithValue("@username", EMPCODE.Text);
                    cmd3.Parameters.AddWithValue("@password", "12345");
                    cmd3.Parameters.AddWithValue("@fullname", EMPLOYEENAME.Text);
                    cmd3.Parameters.AddWithValue("@type", guna2ComboBox1.Text);
                    cmd3.Parameters.AddWithValue("@status", guna2ComboBox2.Text);
                    cmd3.Parameters.AddWithValue("@createdby", name);
                    cmd3.ExecuteNonQuery();
                    tblOTS.Close();
                }
                int populuation;
                using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
                {
                    tblOTS.Open();
                    SqlCommand cmd = new SqlCommand("SELECT MAX(id) FROM systemAccounts", tblOTS);
                    populuation = (int)cmd.ExecuteScalar();
                    tblOTS.Close();
                }
                using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
                {
                    tblOTS.Open();
                    using (SqlCommand command = new SqlCommand("DELETE FROM systemAccountsPermissions WHERE accountid = '" + populuation + "'", tblOTS))
                    {
                        command.ExecuteNonQuery();
                    }
                    tblOTS.Close();
                }
                foreach (DataGridViewRow row in dataGridView2.Rows)
                {
                    using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
                    {
                        tblOTS.Open();
                        string insStmt = "insert into systemAccountsPermissions ([accountid], [detail], [grantaccess]) values" +
                            " (@accountid,@detail,@grantaccess)";
                        SqlCommand insCmd = new SqlCommand(insStmt, tblOTS);
                        insCmd.Parameters.Clear();
                        insCmd.Parameters.AddWithValue("@accountid", populuation);
                        insCmd.Parameters.AddWithValue("@detail", row.Cells["Column1"].Value.ToString().Trim());
                        if (Convert.ToBoolean(row.Cells["grant"].Value) == true)
                        {
                            insCmd.Parameters.AddWithValue("@grantaccess", "True");
                        }
                        else
                        {
                            insCmd.Parameters.AddWithValue("@grantaccess", "False");
                        }
                        int affectedRows = insCmd.ExecuteNonQuery();
                        tblOTS.Close();
                    }
                }
                MessageBox.Show("Saved");
            }
            systemAccounts.load();
            this.Close();
        }
        systemAccounts systemAccounts = (systemAccounts)Application.OpenForms["systemAccounts"];

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure to continue this?", "Continue?", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
                {
                    tblOTS.Open();
                    using (SqlCommand command = new SqlCommand("DELETE FROM systemAccounts WHERE id = '" + id + "'", tblOTS))
                    {
                        command.ExecuteNonQuery();
                    }
                    tblOTS.Close();

                }
                using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
                {
                    tblOTS.Open();
                    using (SqlCommand command = new SqlCommand("DELETE FROM systemAccountsPermissions WHERE accountid = '" + id + "'", tblOTS))
                    {
                        command.ExecuteNonQuery();
                    }
                    tblOTS.Close();
                }
                MessageBox.Show("Deleted");
                systemAccounts.load();
                this.Close();
            }
        }
    }
}
