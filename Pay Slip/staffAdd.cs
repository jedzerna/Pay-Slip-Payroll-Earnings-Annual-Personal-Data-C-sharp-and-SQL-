using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pay_Slip
{
    public partial class staffAdd : Form
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
        public staffAdd()
        {
            InitializeComponent();
        }
        public string name;
        public string id;
        private void staffAdd_Load(object sender, EventArgs e)
        {
            load();
        }
        public void load()
        {
            using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
            {
                dataGridView2.Rows.Clear();
                DataTable distinct = new DataTable();
                tblOTS.Open();
                string list = "SELECT EMPCODE,id FROM tblStaffData ORDER BY id DESC";
                SqlDataAdapter command = new SqlDataAdapter(list, tblOTS);

                command.Fill(distinct);
                tblOTS.Close();
                foreach (DataRow item in distinct.Rows)
                {
                    int a = dataGridView2.Rows.Add();
                    dataGridView2.Rows[a].Cells["dataGridViewTextBoxColumn1"].Value = item["EMPCODE"].ToString();
                }
            }
            dataGridView2.ClearSelection();
        }
        staffDashboard staffload = (staffDashboard)Application.OpenForms["staffDashboard"];
        private void guna2Button4_Click(object sender, EventArgs e)
        {

            if (EMPCODE.Text != "")
            {

                using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
                {
                    tblOTS.Open();
                    SqlCommand check_User_Name = new SqlCommand("SELECT COUNT(*) FROM [tblStaffData] WHERE ([EMPCODE] = @EMPCODE)", tblOTS);
                    check_User_Name.Parameters.AddWithValue("@EMPCODE", EMPCODE.Text);
                    int UserExist = (int)check_User_Name.ExecuteScalar();

                    if (UserExist > 0)
                    {

                        label23.ForeColor = Color.FromArgb(255, 128, 128);
                        label23.Text = "EMPCODE already exist";
                        label23.Visible = true;
                        EMPCODE.Focus();
                        EMPCODE.BorderColor = Color.FromArgb(255, 128, 128);
                        EMPCODE.FocusedState.BorderColor = Color.FromArgb(255, 128, 128);
                        EMPCODE.HoverState.BorderColor = Color.FromArgb(255, 128, 128);
                        return;

                    }
                    else
                    {

                        label23.Visible = false;
                        label23.Text = "";
                        label23.ForeColor = Color.LimeGreen;
                        EMPCODE.BorderColor = Color.FromArgb(213, 218, 223);
                        EMPCODE.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
                        EMPCODE.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
                    }
                    tblOTS.Close();
                }
            }

            if (EMPLOYEENAME.Text == "No EMPCODE Found")
            {
                EMPCODE.Focus();
                EMPLOYEENAME.BorderColor = Color.FromArgb(255, 128, 128);
                EMPLOYEENAME.FocusedState.BorderColor = Color.FromArgb(255, 128, 128);
                EMPLOYEENAME.HoverState.BorderColor = Color.FromArgb(255, 128, 128);
                return;

            }
            else
            {

                EMPLOYEENAME.BorderColor = Color.FromArgb(213, 218, 223);
                EMPLOYEENAME.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
                EMPLOYEENAME.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            }
            if (EMPLOYEENAME.Text == "")
            {
                EMPLOYEENAME.Focus();
                EMPLOYEENAME.BorderColor = Color.FromArgb(255, 128, 128);
                EMPLOYEENAME.FocusedState.BorderColor = Color.FromArgb(255, 128, 128);
                EMPLOYEENAME.HoverState.BorderColor = Color.FromArgb(255, 128, 128);
                MessageBox.Show("Please don't leave empty in every label.");
                return;

            }
            else
            {

                EMPLOYEENAME.BorderColor = Color.FromArgb(213, 218, 223);
                EMPLOYEENAME.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
                EMPLOYEENAME.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            }
            if (BASICSALARY.Text == "")
            {

                BASICSALARY.Focus();
                BASICSALARY.BorderColor = Color.FromArgb(255, 128, 128);
                BASICSALARY.FocusedState.BorderColor = Color.FromArgb(255, 128, 128);
                BASICSALARY.HoverState.BorderColor = Color.FromArgb(255, 128, 128);
                MessageBox.Show("Please don't leave empty in every label.");
                return;

            }
            else
            {

                BASICSALARY.BorderColor = Color.FromArgb(213, 218, 223);
                BASICSALARY.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
                BASICSALARY.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            }
            if (ALLOWANCE.Text == "")
            {

                ALLOWANCE.Focus();
                ALLOWANCE.BorderColor = Color.FromArgb(255, 128, 128);
                ALLOWANCE.FocusedState.BorderColor = Color.FromArgb(255, 128, 128);
                ALLOWANCE.HoverState.BorderColor = Color.FromArgb(255, 128, 128);
                MessageBox.Show("Please don't leave empty in every label.");
                return;

            }
            else
            {

                ALLOWANCE.BorderColor = Color.FromArgb(213, 218, 223);
                ALLOWANCE.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
                ALLOWANCE.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            }
            if (SSS.Text == "")
            {

                SSS.Focus();
                SSS.BorderColor = Color.FromArgb(255, 128, 128);
                SSS.FocusedState.BorderColor = Color.FromArgb(255, 128, 128);
                SSS.HoverState.BorderColor = Color.FromArgb(255, 128, 128);
                MessageBox.Show("Please don't leave empty in every label.");
                return;

            }
            else
            {

                SSS.BorderColor = Color.FromArgb(213, 218, 223);
                SSS.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
                SSS.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            }
            if (PHIL.Text == "")
            {

                PHIL.Focus();
                PHIL.BorderColor = Color.FromArgb(255, 128, 128);
                PHIL.FocusedState.BorderColor = Color.FromArgb(255, 128, 128);
                PHIL.HoverState.BorderColor = Color.FromArgb(255, 128, 128);
                MessageBox.Show("Please don't leave empty in every label.");
                return;

            }
            else
            {

                PHIL.BorderColor = Color.FromArgb(213, 218, 223);
                PHIL.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
                PHIL.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            }
            if (HDMF.Text == "")
            {

                HDMF.Focus();
                HDMF.BorderColor = Color.FromArgb(255, 128, 128);
                HDMF.FocusedState.BorderColor = Color.FromArgb(255, 128, 128);
                HDMF.HoverState.BorderColor = Color.FromArgb(255, 128, 128);
                MessageBox.Show("Please don't leave empty in every label.");
                return;

            }
            else
            {

                HDMF.BorderColor = Color.FromArgb(213, 218, 223);
                HDMF.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
                HDMF.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            }
            if (OTHERS.Text == "")
            {

                OTHERS.Focus();
                OTHERS.BorderColor = Color.FromArgb(255, 128, 128);
                OTHERS.FocusedState.BorderColor = Color.FromArgb(255, 128, 128);
                OTHERS.HoverState.BorderColor = Color.FromArgb(255, 128, 128);
                MessageBox.Show("Please don't leave empty in every label.");
                return;

            }
            else
            {

                OTHERS.BorderColor = Color.FromArgb(213, 218, 223);
                OTHERS.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
                OTHERS.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            }
            if (BASICSALARY.Text == "")
            {
                BASICSALARY.Text = "0.00";
            }
            if (ALLOWANCE.Text == "")
            {
                ALLOWANCE.Text = "0.00";
            }
            if (SSS.Text == "")
            {
                SSS.Text = "0.00";
            }

            if (PHIL.Text == "")
            {
                PHIL.Text = "0.00";
            }
            if (HDMF.Text == "")
            {
                HDMF.Text = "0.00";
            }
            if (OTHERS.Text == "")
            {
                OTHERS.Text = "0.00";
            }
            savedata();
        }
        private void savedata()
        {
            using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
            {
                tblOTS.Open();
                string insStmt = "insert into tblStaffData ([EMPCODE], [EMPLOYEENAME], [BASICSALARY], [ALLOWANCE], [SSS], [PHIL], [HDMF], [OTHERS], [CREATEDBY], [STATUS]) values" +
                    " (@EMPCODE,@EMPLOYEENAME,@BASICSALARY,@ALLOWANCE,@SSS,@PHIL,@HDMF,@OTHERS,@CREATEDBY,@STATUS)";
                SqlCommand insCmd = new SqlCommand(insStmt, tblOTS);
                insCmd.Parameters.Clear();
                insCmd.Parameters.AddWithValue("@EMPCODE", EMPCODE.Text.Trim());
                insCmd.Parameters.AddWithValue("@EMPLOYEENAME", EMPLOYEENAME.Text.Trim());
                insCmd.Parameters.AddWithValue("@BASICSALARY", BASICSALARY.Text.Trim());
                insCmd.Parameters.AddWithValue("@ALLOWANCE", ALLOWANCE.Text.Trim());
                insCmd.Parameters.AddWithValue("@SSS", SSS.Text.Trim());
                insCmd.Parameters.AddWithValue("@PHIL", PHIL.Text.Trim());
                insCmd.Parameters.AddWithValue("@HDMF", HDMF.Text.Trim());
                insCmd.Parameters.AddWithValue("@OTHERS", OTHERS.Text.Trim());
                insCmd.Parameters.AddWithValue("@CREATEDBY", name);
                insCmd.Parameters.AddWithValue("@STATUS", "ACTIVE");
                int affectedRows = insCmd.ExecuteNonQuery();
                tblOTS.Close();
                MessageBox.Show("Successfully Added!");
                staffload.load();
            }

            EMPCODE.Text = "";
            BASICSALARY.Text = "";
            ALLOWANCE.Text = "";
            SSS.Text = "";
            PHIL.Text = "";
            HDMF.Text = "";
            OTHERS.Text = "";
            EMPCODE.Focus();
            load();

        }

        private void EMPCODE_TextChanged(object sender, EventArgs e)
        {
            if (EMPCODE.Text != "")
            {
                getprojectcode();
            }
            else
            {
                EMPLOYEENAME.Text = "";
                EMPLOYEENAME.BorderColor = Color.FromArgb(213, 218, 223);
                EMPLOYEENAME.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
                EMPLOYEENAME.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            }
        }
        
        private void getprojectcode()
        {
            using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
            {
                tblOTS.Open();
                DataTable dt = new DataTable();
                String query = "SELECT FIRST,MIDDLE,FAMILY FROM tblPERID WHERE EMPCODE = @EMPCODE";
                SqlCommand cmd = new SqlCommand(query, tblOTS);
                cmd.Parameters.AddWithValue("@EMPCODE", EMPCODE.Text);
                SqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    EMPLOYEENAME.Text = rdr["FAMILY"].ToString() + ", "+ rdr["FIRST"].ToString()+" "+ rdr["MIDDLE"].ToString();
                    EMPLOYEENAME.BorderColor = Color.FromArgb(213, 218, 223);
                    EMPLOYEENAME.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
                    EMPLOYEENAME.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
                }
                else
                {
                    EMPLOYEENAME.Text = "No EMPCODE Found";
                    EMPLOYEENAME.BorderColor = Color.FromArgb(255, 128, 128);
                    EMPLOYEENAME.FocusedState.BorderColor = Color.FromArgb(255, 128, 128);
                    EMPLOYEENAME.HoverState.BorderColor = Color.FromArgb(255, 128, 128);
                }
                tblOTS.Close();
            }
        }

        private void ALLOWANCE_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!isNumber(e.KeyChar, ALLOWANCE.Text))
                e.Handled = true;
        }

        private void BASICSALARY_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!isNumber(e.KeyChar, BASICSALARY.Text))
                e.Handled = true;
        }
        public bool isNumber(char ch, string text)
        {
            bool res = true;
            char decimalChar = Convert.ToChar(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);

            if (ch == decimalChar && text.IndexOf(decimalChar) != -1)
            {
                res = false;
                return res;
            }

            //check if it´s a digit, decimal separator and backspace
            if (!Char.IsDigit(ch) && ch != decimalChar && ch != (char)Keys.Back)
                res = false;

            return res;
        }

        private void SSS_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!isNumber(e.KeyChar, SSS.Text))
                e.Handled = true;
        }

        private void PHIL_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (!isNumber(e.KeyChar, PHIL.Text))
                e.Handled = true;
        }

        private void HDMF_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!isNumber(e.KeyChar, HDMF.Text))
                e.Handled = true;
        }

        private void OTHERS_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!isNumber(e.KeyChar, OTHERS.Text))
                e.Handled = true;
        }

        private void SSS_TextChanged(object sender, EventArgs e)
        {

        }
        public string type;
        private void guna2Button7_Click(object sender, EventArgs e)
        {
            employees frm = new employees();
            frm.id = id;
            frm.type = type;
            frm.ShowDialog();
        }

        private void EMPCODE_KeyUp(object sender, KeyEventArgs e)
        {
            Guna.UI2.WinForms.Guna2TextBox currentContainer = ((Guna.UI2.WinForms.Guna2TextBox)sender);
            int caretPosition = currentContainer.SelectionStart;
            currentContainer.Text = currentContainer.Text.ToUpper();
            currentContainer.SelectionStart = caretPosition++;
        }
    }
}
