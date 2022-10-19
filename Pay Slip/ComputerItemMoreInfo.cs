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
    public partial class ComputerItemMoreInfo : Form
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
        public ComputerItemMoreInfo()
        {
            InitializeComponent();
        }
        public string reference;
        public string rowtotal;
        private ComputerSupplyForm insupp = null;
        public ComputerItemMoreInfo(Form callingForm)
        {
            insupp = callingForm as ComputerSupplyForm;

            InitializeComponent();
        }

        private void ComputerItemMoreInfo_Load(object sender, EventArgs e)
        {
            label8.Text = string.Format("{0:#,##0.00}", Convert.ToDecimal(rowtotal));
            load();
        }
        private void load()
        {

            dataGridView1.Rows.Clear();
            if (insupp.charging.Rows.Count != 0)
            {
                foreach (DataRow row in insupp.charging.Rows)
                {
                    if (row["REF"].ToString() == reference)
                    {
                        int a = dataGridView1.Rows.Add();
                        dataGridView1.Rows[a].Cells["code"].Value = row["CODE"].ToString();
                        dataGridView1.Rows[a].Cells["name"].Value = row["PROJECT"].ToString();
                        dataGridView1.Rows[a].Cells["percent"].Value = row["CHARGING"].ToString()+"%";
                    }
                }
                insupp.charging.AcceptChanges();
            }

            decimal finaltotalrow = 0.00M;

            decimal strtodc = 0.00M;
            decimal percent = 0.00M;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
    //            if (rw.Cells[i].Value == null || rw.Cells[i].Value == DBNull.Value || String.IsNullOrWhiteSpace(rw.Cells[i].Value.ToString())
    //{
    //                // here is your message box...
    //            }
                string chrgng = row.Cells["percent"].Value.ToString().Replace("%", "");

                    strtodc += Convert.ToDecimal(chrgng) / 100;
                percent += Convert.ToDecimal(chrgng)+0;


            }
            //MessageBox.Show(strtodc.ToString());
            finaltotalrow = Convert.ToDecimal(label8.Text) * strtodc;
            label7.Text = string.Format("{0:#,##0.00}", finaltotalrow);
            label12.Text = string.Format("{0:#,##0.00}", percent)+"%";
          
            percen = percent.ToString();
            if (label12.Text == "100.00%" || label12.Text == "100%")
            {
                label12.ForeColor = Color.Black;
            }
            else
            {
                label12.ForeColor = Color.Maroon;
            }
        }
        private string percen;
        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
       
               
            
            if (guna2TextBox1.Text == "")
            {
                MessageBox.Show("Blank Code");
                return;
            }
            if (guna2TextBox2.Text == "")
            {
                MessageBox.Show("Blank Project");
                return;
            }
            if (guna2TextBox4.Text == "")
            {
                MessageBox.Show("Blank Percent");
                return;
            }
            if (guna2TextBox3.Text == "")
            {
                MessageBox.Show("Blank Where Used");
                return;
            }
            bool aded = false;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["code"].Value.ToString() == guna2TextBox1.Text)
                {
                    aded = true;
                }
            }
            if (aded == true)
            {
                MessageBox.Show("Project is already added.");
                return;
            }
                //charging.Columns.Add("CHARGING");
                //charging.Columns.Add("CODE");
                //charging.Columns.Add("PROJECT");
                //charging.Columns.Add("WHERE");
                //charging.Columns.Add("REMARKS");
                //charging.Columns.Add("REF")


                object[] o = { guna2TextBox4.Text, guna2TextBox1.Text.Trim(), guna2TextBox2.Text.Trim(), guna2TextBox3.Text.Trim(), guna2TextBox5.Text.Trim(), reference };
            insupp.charging.Rows.Add(o);

            insupp.charging.AcceptChanges();

            //insupp.count();
            //MessageBox.Show("Saved");
            load();
            guna2TextBox1.Text = "";
            guna2TextBox2.Text = "";
            guna2TextBox3.Text = "";
            guna2TextBox4.Text = "";
            guna2TextBox5.Text = "";

            //int a = dataGridView1.Rows.Add();
            //dataGridView1.Rows[a].Cells["code"].Value = guna2TextBox1.Text;
            //dataGridView1.Rows[a].Cells["name"].Value = guna2TextBox2.Text;
            //dataGridView1.Rows[a].Cells["percent"].Value = guna2TextBox4.Text;
            //MessageBox.Show("Saved");
        }

        bool message = false;
        private void ComputerItemMoreInfo_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;
            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0)
            {
                //MessageBox.Show("Saved");
                //foreach (DataRow row in insupp.charging.Rows)
                //{
                //    if (row["CODE"].ToString() == dataGridView1.CurrentRow.Cells["code"].Value.ToString())
                //    {
                //        //row.Delete();
                //        insupp.charging.Rows.Remove(row);
                //    }
                //}
                //if (Convert.ToString(dr["CODE"].ToString()) == value)

                for (int i = insupp.charging.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = insupp.charging.Rows[i];
                    if (dr["CODE"].ToString() == dataGridView1.CurrentRow.Cells["code"].Value.ToString())
                        dr.Delete();
                }
                insupp.charging.AcceptChanges();

                load();
                //for (int i = 0; i < insupp.charging.Rows.Count; i++)
                //{
                //    DataRow dr = insupp.charging.Rows[i];
                //    if (Convert.ToString(dr["CODE"].ToString()) == dataGridView1.CurrentRow.Cells["code"].Value.ToString())
                //    {
                //        dr.Delete();
                //        i -= 1;
                //    }
                //}
                //insupp.charging.AcceptChanges();
            }
        }

        private void guna2TextBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            decimal x;
            if (ch == (char)Keys.Back)
            {
                e.Handled = false;
            }
            else if (!char.IsDigit(ch) && ch != '.' || !Decimal.TryParse(guna2TextBox4.Text + ch, out x))
            {
                e.Handled = true;
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            projects frm = new projects();
            frm.ShowDialog();
        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {
            if (guna2TextBox1.Text != "")
            {
                getprojectcode();
            }
        }
        private void getprojectcode()
        {
            using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
            {
                tblOTS.Open();
                DataTable dt = new DataTable();
                String query = "SELECT ACCTDESC FROM GLU4 WHERE ACCTCODE = @ACCTCODE";
                SqlCommand cmd = new SqlCommand(query, tblOTS);
                cmd.Parameters.AddWithValue("@ACCTCODE", guna2TextBox1.Text);
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    guna2TextBox2.Text = (rdr["ACCTDESC"].ToString());
                }
                else
                {
                    guna2TextBox2.Text = "";
                }
                tblOTS.Close();
            }
        }
    }
}
