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
    public partial class staffEditSAC : Form
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
        public string empcode;
        public string empname;
        public string basicsalary;
        public string allowance;
        public string sss;
        public string philhealth;
        public string hdmf;
        public string others;
        public string form;
        public staffEditSAC()
        {
            InitializeComponent();
        }

        private void staffEditSAC_Load(object sender, EventArgs e)
        {
            label11.Text = empcode;
            label2.Text = empname;
            BASICSALARY.Text = basicsalary;
            ALLOWANCE.Text = allowance;
            SSS.Text = sss;
            PHIL.Text = philhealth;
            HDMF.Text = hdmf;
            OTHERS.Text = others;
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
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

            using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
            {
                SqlCommand cmd3 = new SqlCommand("update tblStaffData set BASICSALARY=@BASICSALARY,ALLOWANCE=@ALLOWANCE,SSS=@SSS,PHIL=@PHIL,HDMF=@HDMF,OTHERS=@OTHERS where EMPCODE=@EMPCODE", tblOTS);
                tblOTS.Open();
                cmd3.Parameters.AddWithValue("@EMPCODE", label11.Text);
                cmd3.Parameters.AddWithValue("@BASICSALARY", BASICSALARY.Text);
                cmd3.Parameters.AddWithValue("@ALLOWANCE", ALLOWANCE.Text);
                cmd3.Parameters.AddWithValue("@SSS", SSS.Text);
                cmd3.Parameters.AddWithValue("@PHIL", PHIL.Text);
                cmd3.Parameters.AddWithValue("@HDMF", HDMF.Text);
                cmd3.Parameters.AddWithValue("@OTHERS", OTHERS.Text);
                cmd3.ExecuteNonQuery();
                tblOTS.Close();
            }
            staffInformations staffInformations = (staffInformations)Application.OpenForms["staffInformations"];
            staffInformations.load();
            if (form == "A")
            {
                staffPayroll staffPayroll = (staffPayroll)Application.OpenForms["staffPayroll"];
                staffPayroll.dataGridView2.CurrentRow.Cells["BASICSALARY"].Value = string.Format("{0:#,##0.00}", Convert.ToDecimal(BASICSALARY.Text));
                staffPayroll.dataGridView2.CurrentRow.Cells["ALLOWANCE"].Value = string.Format("{0:#,##0.00}", Convert.ToDecimal(ALLOWANCE.Text));
                staffPayroll.dataGridView2.CurrentRow.Cells["SSS"].Value = string.Format("{0:#,##0.00}", Convert.ToDecimal(SSS.Text));
                staffPayroll.dataGridView2.CurrentRow.Cells["PHILHEALTH"].Value = string.Format("{0:#,##0.00}", Convert.ToDecimal(PHIL.Text));
                staffPayroll.dataGridView2.CurrentRow.Cells["HDMF"].Value = string.Format("{0:#,##0.00}", Convert.ToDecimal(HDMF.Text));
                staffPayroll.dataGridView2.CurrentRow.Cells["OTHERS"].Value = string.Format("{0:#,##0.00}", Convert.ToDecimal(OTHERS.Text));
            }
            MessageBox.Show("Saved.");
            this.Close();

        }
        public bool isNumber(char ch, string text)
        {
            bool res = true;
            char decimalChar = Convert.ToChar(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);

            //check if it´s a decimal separator and if doesn´t already have one in the text string
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

        private void BASICSALARY_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!isNumber(e.KeyChar, BASICSALARY.Text))
                e.Handled = true;
        }

        private void ALLOWANCE_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!isNumber(e.KeyChar, ALLOWANCE.Text))
                e.Handled = true;
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
    }
}
