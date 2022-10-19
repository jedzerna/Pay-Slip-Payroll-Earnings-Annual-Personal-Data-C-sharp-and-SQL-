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
    public partial class AnnualRowEditing : Form
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
        public string id;
        public AnnualRowEditing()
        {
            InitializeComponent();
        }

        private DashAnnual dash = null;
        public AnnualRowEditing(Form callingForm)
        {
            dash = callingForm as DashAnnual;
            InitializeComponent();
        }
        private void AnnualRowEditing_Load(object sender, EventArgs e)
        {
            load();

        }
        public void load()
        {
            using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
            {
                tblOTS.Open();
                DataTable dt = new DataTable();
                String query = "SELECT * FROM tblGLU2N WHERE ID = '" + id + "'";
                SqlCommand cmd = new SqlCommand(query, tblOTS);
                SqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    label3.Text = (rdr["VOUCHER"].ToString()+" "+ rdr["NUM"].ToString());
                    maskedTextBox1.Text = DateTime.Parse(rdr["DATE"].ToString()).ToString("MM/dd/yyyy");
                    label5.Text = string.Format("{0:#,##0.00}", Convert.ToDecimal(rdr["AMOUNT"].ToString()));
                    label6.Text = (rdr["TYPE"].ToString());
                    guna2TextBox1.Text = (rdr["DESCRIPTION"].ToString());
                    guna2TextBox2.Text = (rdr["ACCTCODE"].ToString());
                }
                tblOTS.Close();

            }
        }

        private void guna2TextBox2_TextChanged(object sender, EventArgs e)
        {
            using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
            {
                tblOTS.Open();
                DataTable dt = new DataTable();
                String query = "SELECT ACCTDESC FROM GLU4 WHERE ACCTCODE = @ACCTCODE";
                SqlCommand cmd = new SqlCommand(query, tblOTS);
                cmd.Parameters.AddWithValue("@ACCTCODE", guna2TextBox2.Text);
               SqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    guna2TextBox1.Text = (rdr["ACCTDESC"].ToString());

                }
                else
                {
                    guna2TextBox1.Text = "";
                }
                tblOTS.Close();
            }
        }

        private void maskedTextBox1_Leave(object sender, EventArgs e)
        {
            maskedTextBox1.Text = DateTime.Parse(maskedTextBox1.Text).ToString("MM/dd/yyyy");
            DateTime temp;
            if (DateTime.TryParse(maskedTextBox1.Text, out temp))
            {
                // Yay :)
            }
            else
            {
                MessageBox.Show("Date is invalid");
                maskedTextBox1.Text = "";
            }

        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            if (guna2TextBox2.Text == "")
            {
                MessageBox.Show("ACCTCODE is invalid");
                return;
            }
            maskedTextBox1.Text = DateTime.Parse(maskedTextBox1.Text).ToString("MM/dd/yyyy");
            DateTime temp;
            if (DateTime.TryParse(maskedTextBox1.Text, out temp))
            {
                // Yay :)
            }
            else
            {
                MessageBox.Show("Date is invalid");
                return;
            }
            using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
            {
                SqlCommand cmd3 = new SqlCommand("update tblGLU2N set DATE=@DATE,ACCTCODE=@ACCTCODE,DESCRIPTION=@DESCRIPTION,FIRST3=@FIRST3 where ID=@ID", tblOTS);
                tblOTS.Open();
                cmd3.Parameters.AddWithValue("@DATE", DateTime.Parse(maskedTextBox1.Text).ToString("MM/dd/yyyy"));
                cmd3.Parameters.AddWithValue("@ACCTCODE", guna2TextBox2.Text.Trim());
                cmd3.Parameters.AddWithValue("@DESCRIPTION", guna2TextBox1.Text.Trim());

                if (guna2TextBox2.Text.Length < 3)
                {
                    cmd3.Parameters.AddWithValue("@FIRST3", guna2TextBox2.Text.Trim());
                }
                else
                {
                    cmd3.Parameters.AddWithValue("@FIRST3", guna2TextBox2.Text.Substring(0, 3));
                }
                cmd3.ExecuteNonQuery();
                tblOTS.Close();
            }
            dash.dataGridView1.CurrentRow.Cells["Column3"].Value = DateTime.Parse(maskedTextBox1.Text).ToString("MM/dd/yyyy");
            dash.dataGridView1.CurrentRow.Cells["Column4"].Value = guna2TextBox2.Text;
            dash.dataGridView1.CurrentRow.Cells["Column7"].Value = guna2TextBox1.Text;
            if (guna2TextBox2.Text.Length < 3)
            {
                dash.dataGridView1.CurrentRow.Cells["Column10"].Value = guna2TextBox2.Text;
            }
            else
            {
                dash.dataGridView1.CurrentRow.Cells["Column10"].Value = guna2TextBox2.Text.Substring(0, 3);
            }
            dash.distinct.DefaultView.Sort = "FIRST3,DATE,ACCTCODE ASC";
            dash.distinct.AcceptChanges();

            dash.guna2Button5.Visible = false;
            dash.guna2Button2.Visible = true;
            dash.dataGridView1.Columns["Column12"].Visible = false;
            MessageBox.Show("Saved");
            this.Close();
        }
    }
}
