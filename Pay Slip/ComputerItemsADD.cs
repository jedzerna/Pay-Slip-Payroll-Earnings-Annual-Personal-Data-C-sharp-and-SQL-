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
    public partial class ComputerItemsADD : Form
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
        public ComputerItemsADD()
        {
            InitializeComponent();
        }

        private ComputerSupplyForm insupp = null;
        public ComputerItemsADD(Form callingForm)
        {
            insupp = callingForm as ComputerSupplyForm;

            InitializeComponent();
        }


        private void ComputerItemsADD_Load(object sender, EventArgs e)
        {
            Auto();
            dictionar();
            loadstocks();

        }
        DataTable items = new DataTable();
        public void loadstocks()
        {
            items.Rows.Clear();
            using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
            {
                tblOTS.Open();
                string list = "Select id,name,supplier,serialno from tblComputerItemList order by id desc";
                SqlCommand command = new SqlCommand(list, tblOTS);
                SqlDataReader reader = command.ExecuteReader();
                items.Load(reader);
                tblOTS.Close();
            }
            dataGridView1.DataSource = items;
        }
        AutoCompleteStringCollection coll = new AutoCompleteStringCollection();
        public void Auto()
        {
            using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
            {
                tblOTS.Open();
                SqlDataAdapter da = new SqlDataAdapter("select suppliername from tblSupplier", tblOTS);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        coll.Add(dt.Rows[i]["suppliername"].ToString());
                    }
                }
                tblOTS.Close();
                guna2TextBox3.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                guna2TextBox3.AutoCompleteSource = AutoCompleteSource.CustomSource;
                guna2TextBox3.AutoCompleteCustomSource = coll;
            }
        }

        AutoCompleteStringCollection dictionary = new AutoCompleteStringCollection();
        public void dictionar()

        {
            using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
            {
                tblOTS.Open();
                SqlDataAdapter da = new SqlDataAdapter("select description from tblDictionary", tblOTS);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dictionary.Add(dt.Rows[i]["description"].ToString());
                    }
                }
                tblOTS.Close();
                guna2TextBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                guna2TextBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;
                guna2TextBox1.AutoCompleteCustomSource = dictionary;
            }
        }
        public string name = "Jed Zerna";
        private void guna2Button1_Click(object sender, EventArgs e)
        {
           

            if (guna2TextBox1.Text == "")
            {
                MessageBox.Show("Please don't leave description blank.");
                return;
            }
            if (guna2TextBox2.Text == "")
            {
                MessageBox.Show("Please don't leave description blank.");
                return;
            }
            if (guna2TextBox3.Text == "")
            {
                MessageBox.Show("Please don't leave description blank.");
                return;
            }
            if (guna2TextBox4.Text == "")
            {
                MessageBox.Show("Please don't leave description blank.");
                return;
            }
            using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
            {
                tblOTS.Open();
                string insStmt = "insert into tblComputerItemList ([name], [stocks], [supplier], [remarks], [dateadded], [addedby], [cost], [serialno]) values" +
                    " (@name,@stocks,@supplier,@remarks,@dateadded,@addedby,@cost,@serialno)";
                SqlCommand insCmd = new SqlCommand(insStmt, tblOTS);
                insCmd.Parameters.Clear();
                insCmd.Parameters.AddWithValue("@name", guna2TextBox1.Text.Trim());
                insCmd.Parameters.AddWithValue("@stocks", guna2TextBox2.Text.Trim());
                insCmd.Parameters.AddWithValue("@supplier", guna2TextBox3.Text.Trim());
                insCmd.Parameters.AddWithValue("@remarks", guna2TextBox5.Text.Trim());
                insCmd.Parameters.AddWithValue("@dateadded", DateTime.Now.ToString("MM/dd/yyyy"));
                insCmd.Parameters.AddWithValue("@addedby", name);
                insCmd.Parameters.AddWithValue("@cost", string.Format("{0:#,##0.00}", Convert.ToDecimal(guna2TextBox4.Text.Trim())));
                insCmd.Parameters.AddWithValue("@serialno", guna2TextBox6.Text.Trim());
                int affectedRows = insCmd.ExecuteNonQuery();
                tblOTS.Close();


                var empcon = new SqlCommand("SELECT max(id) FROM [tblComputerItemList]", tblOTS);
                tblOTS.Open();
                Int32 max = (Int32)empcon.ExecuteScalar();
                tblOTS.Close();

                //insupp.loadstocks();
                object[] o = { max, guna2TextBox1.Text.Trim(), guna2TextBox3.Text.Trim(), guna2TextBox4.Text.Trim(), guna2TextBox6.Text.Trim() };
                insupp.items.Rows.Add(o);
                insupp.items.DefaultView.Sort = "id DESC";


                object[] b = { max, guna2TextBox1.Text.Trim(), guna2TextBox3.Text.Trim(), guna2TextBox6.Text.Trim() };
                items.Rows.Add(b);
                items.DefaultView.Sort = "id DESC";
            }
            using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
            {
                tblOTS.Open();
                SqlCommand check_User_Name = new SqlCommand("SELECT COUNT(*) FROM [tblSupplier] WHERE suppliername = @suppliername", tblOTS);
                check_User_Name.Parameters.AddWithValue("@suppliername", guna2TextBox3.Text);
                int UserExist = (int)check_User_Name.ExecuteScalar();

                if (UserExist == 0)
                {
                    tblOTS.Close();

                    tblOTS.Open();
                    string insStmt = "insert into tblSupplier ([suppliername]) values" +
                        " (@suppliername)";
                    SqlCommand insCmd = new SqlCommand(insStmt, tblOTS);
                    insCmd.Parameters.Clear();
                    insCmd.Parameters.AddWithValue("@suppliername", guna2TextBox3.Text.Trim());
                    int affectedRows = insCmd.ExecuteNonQuery();
                    tblOTS.Close();
                }
            }
            string[] separators = new string[] { " " };
            string text = guna2TextBox1.Text;

            foreach (string word in text.Split(separators, StringSplitOptions.RemoveEmptyEntries))
            {
                using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
                {
                    tblOTS.Open();
                    SqlCommand check_User_Name = new SqlCommand("SELECT COUNT(*) FROM [tblDictionary] WHERE description = @description", tblOTS);
                    check_User_Name.Parameters.AddWithValue("@description", word.Trim());
                    int UserExist = (int)check_User_Name.ExecuteScalar();

                    if (UserExist == 0)
                    {
                        tblOTS.Close();

                        tblOTS.Open();
                        //MessageBox.Show(word);

                        string insStmt = "insert into tblDictionary ([description]) values" +
                            " (@description)";
                        SqlCommand insCmd = new SqlCommand(insStmt, tblOTS);
                        insCmd.Parameters.Clear();
                        insCmd.Parameters.AddWithValue("@description", word.Trim());
                        int affectedRows = insCmd.ExecuteNonQuery();
                        tblOTS.Close();
                        dictionary.Add(word.Trim());
                    }
                }



            }
            Auto();
            MessageBox.Show("Saved");
            guna2TextBox1.Text = "";
            guna2TextBox2.Text = "0";
            guna2TextBox4.Text = "";
            guna2TextBox6.Text = "";
            guna2TextBox5.Text = "";
            guna2TextBox1.Focus();
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

        private void guna2TextBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            decimal x;
            if (ch == (char)Keys.Back)
            {
                e.Handled = false;
            }
            else if (!char.IsDigit(ch) && ch != '.' || !Decimal.TryParse(guna2TextBox2.Text + ch, out x))
            {
                e.Handled = true;
            }
        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {
            (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = string.Format("name LIKE '%{0}%'", guna2TextBox1.Text.Replace("'", "''"));

        }

        private void guna2TextBox6_TextChanged(object sender, EventArgs e)
        {
            (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = string.Format("serialno LIKE '%{0}%'", guna2TextBox6.Text.Replace("'", "''"));
        }

        private void ComputerItemsADD_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                guna2Button1_Click(sender,e);
            }
        }

        private void guna2Button1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                guna2Button1_Click(sender, e);
            }
        }

        private void guna2TextBox5_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                guna2Button1_Click(sender, e);
            }
        }

        private void guna2TextBox3_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
