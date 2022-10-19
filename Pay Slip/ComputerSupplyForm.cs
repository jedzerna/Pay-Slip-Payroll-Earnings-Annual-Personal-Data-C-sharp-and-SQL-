using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pay_Slip
{
    public partial class ComputerSupplyForm : Form
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
        public ComputerSupplyForm()
        {
            InitializeComponent();
        }

        public DataTable items = new DataTable();
        public DataTable charging = new DataTable();


        bool msexist = false;
        bool poexist = false;

        int rowscount;

        Thread thread;
        private void ComputerSupplyForm_Load(object sender, EventArgs e)
        {
            timer1.Start();
            loadstocks();
            charging.Columns.Add("CHARGING");
            charging.Columns.Add("CODE");
            charging.Columns.Add("PROJECT");
            charging.Columns.Add("WHERE");
            charging.Columns.Add("REMARKS");
            charging.Columns.Add("REF");
            foreach (System.Data.DataColumn col in charging.Columns) col.ReadOnly = false;
            Auto();

            dataGridView1.ClearSelection();
        }
        AutoCompleteStringCollection coll = new AutoCompleteStringCollection();
        public void Auto()

        {
            using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
            {
                tblOTS.Open();
                SqlDataAdapter da = new SqlDataAdapter("select historyname from tblOtherInfos", tblOTS);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        coll.Add(dt.Rows[i]["historyname"].ToString());
                    }
                }
                tblOTS.Close();
                guna2TextBox3.AutoCompleteMode = AutoCompleteMode.Suggest;
                guna2TextBox3.AutoCompleteSource = AutoCompleteSource.CustomSource;
                guna2TextBox3.AutoCompleteCustomSource = coll;
            }
        }
        public void loadstocks()
        {
            items.Rows.Clear();
            using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
            {
                tblOTS.Open();
                string list = "Select id,name,supplier,cost,serialno from tblComputerItemList order by id desc";
                SqlCommand command = new SqlCommand(list, tblOTS);
                SqlDataReader reader = command.ExecuteReader();
                items.Load(reader);
                tblOTS.Close();
            }
            dataGridView1.DataSource = items;
        }
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (msexist == true)
            {
                MessageBox.Show("MS no. aleady added.");
                guna2TextBox1.Focus();
                return;
            }
            if (poexist == true)
            {
                MessageBox.Show("PO no. aleady added.");
                guna2TextBox2.Focus();
                return;
            }
            if (guna2TextBox1.Text.Trim() == "" && guna2TextBox2.Text.Trim() == "")
            {
                MessageBox.Show("Please enter MS no. or PO no.");
                guna2TextBox1.Focus();
                return;
            }
            string inputString = maskedTextBox1.Text;
            DateTime dDate;

            if (DateTime.TryParse(inputString, out dDate))
            {
                String.Format("{0:d/MM/yyyy}", dDate);
            }
            else
            {
                MessageBox.Show("Invalid");
                maskedTextBox1.Focus();
                return;
            }
            Int32 max;
            using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
            {
                tblOTS.Open();
                string insStmt = "insert into tblComputerTransactions ([po], [ms], [takenby], [date], [remarks]) values" +
                    " (@po,@ms,@takenby,@date,@remarks)";
                SqlCommand insCmd = new SqlCommand(insStmt, tblOTS);
                insCmd.Parameters.Clear();
                insCmd.Parameters.AddWithValue("@po", guna2TextBox2.Text.Trim());
                insCmd.Parameters.AddWithValue("@ms", guna2TextBox1.Text.Trim());
                insCmd.Parameters.AddWithValue("@takenby", guna2TextBox3.Text.Trim());
                insCmd.Parameters.AddWithValue("@date", maskedTextBox1.Text.Trim());
                insCmd.Parameters.AddWithValue("@remarks", guna2TextBox4.Text.Trim());
                int affectedRows = insCmd.ExecuteNonQuery();
                tblOTS.Close();

                var empcon = new SqlCommand("SELECT max(id) FROM [tblComputerTransactions]", tblOTS);
                tblOTS.Open();
                max = (Int32)empcon.ExecuteScalar();
                tblOTS.Close();


            }
            using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
            {
                foreach (DataGridViewRow row in dataGridView2.Rows)
                {

                    decimal qty = 0.00M;
                    tblOTS.Open();
                    String query1 = "SELECT stocks FROM tblComputerItemList where id like '" + row.Cells["id"].Value.ToString().Trim() + "'";
                    SqlCommand cmd2 = new SqlCommand(query1, tblOTS);
                    SqlDataReader dr1 = cmd2.ExecuteReader();

                    if (dr1.Read())
                    {
                        qty = Convert.ToDecimal(dr1["stocks"].ToString());
                    }
                    else
                    {
                        qty = 0.00M;
                    }
                    qty = qty + Convert.ToDecimal(row.Cells["qty"].Value.ToString().Trim());

                    tblOTS.Close();

                    SqlCommand cmd = new SqlCommand("update tblComputerItemList set stocks=@stocks where id=@id", tblOTS);
                    tblOTS.Open();
                    cmd.Parameters.AddWithValue("@id", row.Cells["id"].Value.ToString().Trim());
                    cmd.Parameters.AddWithValue("@stocks", string.Format("{0:#,##0.00}", qty));
                    cmd.ExecuteNonQuery();
                    tblOTS.Close();


                    tblOTS.Open();
                    string insStmt = "insert into tblComputerTransactionItems ([itemid], [name], [qty], [cost], [total], [ref], [ms], [po], [transactionid]) values" +
                        " (@itemid,@name,@qty,@cost,@total,@ref,@ms,@po,@transactionid)";
                    SqlCommand insCmd = new SqlCommand(insStmt, tblOTS);
                    insCmd.Parameters.Clear();
                    insCmd.Parameters.AddWithValue("@itemid", row.Cells["id"].Value.ToString().Trim());
                    insCmd.Parameters.AddWithValue("@name", row.Cells["name"].Value.ToString().Trim());
                    insCmd.Parameters.AddWithValue("@qty", row.Cells["qty"].Value.ToString().Trim());
                    insCmd.Parameters.AddWithValue("@cost", row.Cells["cost"].Value.ToString().Trim());
                    insCmd.Parameters.AddWithValue("@total", row.Cells["total"].Value.ToString().Trim());
                    insCmd.Parameters.AddWithValue("@ref", row.Cells["reference"].Value.ToString().Trim());
                    insCmd.Parameters.AddWithValue("@ms", guna2TextBox1.Text.Trim());
                    insCmd.Parameters.AddWithValue("@po", guna2TextBox2.Text.Trim());
                    insCmd.Parameters.AddWithValue("@transactionid", max.ToString().Trim());
                    int affectedRows = insCmd.ExecuteNonQuery();
                    tblOTS.Close();
                }
            }
            using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
            {

                //charging.Columns.Add("CHARGING");
                //charging.Columns.Add("CODE");
                //charging.Columns.Add("PROJECT");
                //charging.Columns.Add("WHERE");
                //charging.Columns.Add("REMARKS");
                //charging.Columns.Add("REF");
                foreach (DataRow row in charging.Rows)
                {
                    tblOTS.Open();
                    string insStmt = "insert into tblComputerTransactionInfo ([charging], [projectno], [projectdesc], [whereused], [remarks], [ms], [po], [ref], [transacid]) values" +
                        " (@charging,@projectno,@projectdesc,@whereused,@remarks,@ms,@po,@ref,@transacid)";
                    SqlCommand insCmd = new SqlCommand(insStmt, tblOTS);
                    insCmd.Parameters.Clear();
                    insCmd.Parameters.AddWithValue("@charging", row["CHARGING"].ToString().Trim());
                    insCmd.Parameters.AddWithValue("@projectno", row["CODE"].ToString().Trim());
                    insCmd.Parameters.AddWithValue("@projectdesc", row["PROJECT"].ToString().Trim());
                    insCmd.Parameters.AddWithValue("@whereused", row["WHERE"].ToString().Trim());
                    insCmd.Parameters.AddWithValue("@remarks", row["REMARKS"].ToString().Trim());
                    insCmd.Parameters.AddWithValue("@ms", guna2TextBox2.Text.Trim());
                    insCmd.Parameters.AddWithValue("@po", guna2TextBox1.Text.Trim());
                    insCmd.Parameters.AddWithValue("@ref", row["REF"].ToString().Trim());
                    insCmd.Parameters.AddWithValue("@transacid", max.ToString().Trim());
                    int affectedRows = insCmd.ExecuteNonQuery();
                    tblOTS.Close();
                }
            }

            using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
            {
                tblOTS.Open();
                SqlCommand check_User_Name = new SqlCommand("SELECT COUNT(*) FROM [tblOtherInfos] WHERE historyname = @historyname", tblOTS);
                check_User_Name.Parameters.AddWithValue("@historyname", guna2TextBox3.Text);
                int UserExist = (int)check_User_Name.ExecuteScalar();

                if (UserExist == 0)
                {
                    tblOTS.Close();

                    tblOTS.Open();
                    string insStmt = "insert into tblOtherInfos ([historyname]) values" +
                        " (@historyname)";
                    SqlCommand insCmd = new SqlCommand(insStmt, tblOTS);
                    insCmd.Parameters.Clear();
                    insCmd.Parameters.AddWithValue("@historyname", guna2TextBox3.Text.Trim());
                    int affectedRows = insCmd.ExecuteNonQuery();
                    tblOTS.Close();
                }
            }
            clear();
            MessageBox.Show("Saved");
        }
        private void clear()
        {
            Auto();
            loadstocks();
            dataGridView2.Rows.Clear();
            guna2TextBox1.Text = "";
            guna2TextBox2.Text = "";
            guna2TextBox3.Text = "";
            maskedTextBox1.Text = "";
            guna2TextBox4.Text = "";
            charging.Rows.Clear();
            msexist = false;
            poexist = false;
            guna2TextBox1.Focus();

        }
        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {
            if (guna2TextBox1.Text != "")
            {
                using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
                {
                    tblOTS.Open();
                    SqlCommand check_User_Name = new SqlCommand("SELECT COUNT(*) FROM [tblComputerTransactions] WHERE ms = @ms", tblOTS);
                    check_User_Name.Parameters.AddWithValue("@ms", guna2TextBox1.Text);
                    int UserExist = (int)check_User_Name.ExecuteScalar();

                    if (UserExist > 0)
                    {
                        msexist = true;
                        guna2TextBox1.BorderColor = Color.Maroon;
                    }
                    else
                    {
                        msexist = false;
                        guna2TextBox1.BorderColor = Color.FromArgb(213, 218, 223);
                    }
                    tblOTS.Close();
                }
            }
        }

        private void guna2TextBox2_TextChanged(object sender, EventArgs e)
        {
            if (guna2TextBox2.Text != "")
            {
                using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
                {
                    tblOTS.Open();
                    SqlCommand check_User_Name = new SqlCommand("SELECT COUNT(*) FROM [tblComputerTransactions] WHERE po = @po", tblOTS);
                    check_User_Name.Parameters.AddWithValue("@po", guna2TextBox2.Text);
                    int UserExist = (int)check_User_Name.ExecuteScalar();

                    if (UserExist > 0)
                    {
                        poexist = true;
                        guna2TextBox2.BorderColor = Color.Maroon;
                    }
                    else
                    {
                        poexist = false;
                        guna2TextBox2.BorderColor = Color.FromArgb(213, 218, 223);
                    }
                    tblOTS.Close();
                }
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            ComputerItemsADD a = new ComputerItemsADD(this);
            a.ShowDialog();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (thread == null)
            {
                thread =
              new Thread(new ThreadStart(checker));
                thread.Start();
            }
        }
        public void checker()
        {
            rowscount = items.Rows.Count;
            using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
            {
                tblOTS.Open();

                string list = "SELECT COUNT(*) FROM tblComputerItemList";
                SqlCommand command = new SqlCommand(list, tblOTS);
                Int32 count = (Int32)command.ExecuteScalar();

                tblOTS.Close();

                if (count > rowscount)
                {
                    MessageBox.Show("not equal");
                    addition();

                }
                else if (count < rowscount)
                {
                    deletion();
                }
                thread = null;

            }
        }
        public void addition()
        {
            using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
            {
                //MessageBox.Show("not equal");

                DataRow row = items.Rows[0];
                string idindt = row["id"].ToString();

                tblOTS.Open();
                //MessageBox.Show(idindt);

                string listb = "SELECT MAX(id) FROM tblComputerItemList";
                SqlCommand commandv = new SqlCommand(listb, tblOTS);
                int maxId = Convert.ToInt32(commandv.ExecuteScalar());
                tblOTS.Close();

                DataTable dat = new DataTable();
                tblOTS.Open();
                string listA = "SELECT id,name,supplier,cost FROM tblComputerItemList WHERE id BETWEEN '" + idindt + "' AND '" + maxId + "'";
                SqlCommand commandA = new SqlCommand(listA, tblOTS);
                SqlDataReader readerA = commandA.ExecuteReader();
                dat.Load(readerA);
                tblOTS.Close();
                int check = 0;
                foreach (DataRow rows in dat.Rows)
                {
                    check++;
                    if (check != 1)
                    {
                        DataRow newRow = items.NewRow();
                        newRow[0] = rows["id"].ToString();
                        newRow[1] = rows["name"].ToString();
                        newRow[2] = rows["supplier"].ToString();
                        newRow[3] = rows["cost"].ToString();
                        items.Rows.InsertAt(newRow, 0);
                        items.AcceptChanges();
                    }
                }
            }
        }
        public void deletion()
        {
            foreach (DataRow rows in items.Rows)
            {
                using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
                {
                    tblOTS.Open();
                    SqlCommand check_User_Name = new SqlCommand("SELECT COUNT(*) FROM [tblComputerItemList] WHERE ([id] = @id)", tblOTS);
                    check_User_Name.Parameters.AddWithValue("@id", rows["ID"].ToString());
                    int UserExist = (int)check_User_Name.ExecuteScalar();

                    if (UserExist > 0)
                    {

                    }
                    else
                    {
                        rows.Delete();
                    }

                    tblOTS.Close();
                }
            }
            items.AcceptChanges();
            thread = null;
        }

        private void ComputerSupplyForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (thread != null)
            {
                thread.Abort();
            }
            timer1.Stop();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            select();
        }
        private void select()
        {
            int b = dataGridView2.Rows.Count;
            int a = dataGridView2.Rows.Add();
            dataGridView2.Rows[a].Cells["id"].Value = dataGridView1.CurrentRow.Cells["itemid"].Value.ToString();
            dataGridView2.Rows[a].Cells["name"].Value = dataGridView1.CurrentRow.Cells["itemname"].Value.ToString();
            dataGridView2.Rows[a].Cells["qty"].Value = "0.00";
            dataGridView2.Rows[a].Cells["cost"].Value = dataGridView1.CurrentRow.Cells["itemcost"].Value.ToString();
            dataGridView2.Rows[a].Cells["total"].Value = "0.00";
            dataGridView2.Rows[a].Cells["reference"].Value = b + 1;
            dataGridView2.Rows[a].Cells["serialno"].Value = dataGridView1.CurrentRow.Cells["serial"].Value.ToString();

            count();
        }
        public void count()
        {
            decimal total = 0.00M;
            foreach (DataGridViewRow dgvrows in dataGridView2.Rows)
            {
                if (dgvrows.Cells["qty"].Value == null || dgvrows.Cells["qty"].Value == DBNull.Value || String.IsNullOrWhiteSpace(dgvrows.Cells["qty"].Value.ToString()))
                {
                    // here is your message box...
                }
                else if(dgvrows.Cells["cost"].Value == null || dgvrows.Cells["cost"].Value == DBNull.Value || String.IsNullOrWhiteSpace(dgvrows.Cells["cost"].Value.ToString()))
                {
                }
                else
                {
                    decimal totalrow = 0.00M;
                    decimal qty = Convert.ToDecimal(dgvrows.Cells["qty"].Value.ToString());
                    decimal cost = Convert.ToDecimal(dgvrows.Cells["cost"].Value.ToString());

                    totalrow = qty * cost;
                    total += totalrow;

                    dgvrows.Cells["total"].Value = totalrow.ToString();
                }
            }
            label6.Text = total.ToString();
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView2.Columns["more"].Index && e.RowIndex >= 0)
            {
                ComputerItemMoreInfo a = new ComputerItemMoreInfo(this);
                a.reference = dataGridView2.CurrentRow.Cells["reference"].Value.ToString();
                a.rowtotal = dataGridView2.CurrentRow.Cells["total"].Value.ToString();
                a.ShowDialog();
            }
            else if (e.ColumnIndex == dataGridView2.Columns["remove"].Index && e.RowIndex >= 0)
            {
                bool has = false;
                foreach (DataRow row in charging.Rows)
                {
                    if (row["REF"].ToString() == dataGridView2.CurrentRow.Cells["reference"].Value.ToString())
                    {
                        has = true;
                        break;
                    }
                }
                if (has == true)
                {
                    for (int i = charging.Rows.Count - 1; i >= 0; i--)
                    {
                        DataRow dr = charging.Rows[i];
                        if (dr["REF"].ToString() == dataGridView2.CurrentRow.Cells["reference"].Value.ToString())
                            dr.Delete();
                    }
                    charging.AcceptChanges();
                }
                DataGridViewRow dgvDelRow = dataGridView2.CurrentRow;
                dataGridView2.Rows.Remove(dgvDelRow);
            }
        }

        private void dataGridView2_CellLeave(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView2_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            count();
        }

        private void dataGridView2_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress -= new KeyPressEventHandler(Control_KeyPress);
            if (dataGridView2.CurrentCell.ColumnIndex == 2)
            {
                TextBox tb = e.Control as TextBox;
                if (tb != null)
                {
                    //tb.KeyPress += new KeyPressEventHandler(Column1_KeyPress);
                    tb.KeyPress += new KeyPressEventHandler(Control_KeyPress);
                }
            }
            else if (dataGridView2.CurrentCell.ColumnIndex == 3)
            {
                TextBox tb = e.Control as TextBox;
                if (tb != null)
                {
                    //tb.KeyPress += new KeyPressEventHandler(Column1_KeyPress);
                    tb.KeyPress += new KeyPressEventHandler(Control_KeyPress);
                }
            }
        }
        private void Control_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (!char.IsControl(e.KeyChar)
     && !char.IsDigit(e.KeyChar)
     && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if (e.KeyChar == '.'
                && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
            }
        }

        private void guna2TextBox6_TextChanged(object sender, EventArgs e)
        {
           
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = string.Format("name LIKE '%{0}%'", guna2TextBox6.Text.Replace("'", "''"));
            
        }

        int rowIndex;
        int col;
        private void guna2TextBox6_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                e.Handled = true;
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    rowIndex = dataGridView1.SelectedCells[0].OwningRow.Index;
                    col = dataGridView1.CurrentCell.ColumnIndex;

                    dataGridView1.FirstDisplayedScrollingRowIndex = rowIndex;

                    if (rowIndex < dataGridView1.Rows.Count - 1)
                    {
                        dataGridView1.ClearSelection();
                        this.dataGridView1.Rows[rowIndex + 1].Cells[col].Selected = true;
                    }
                }
            }
            else if (e.KeyCode == Keys.Up)
            {
                e.Handled = true;
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    rowIndex = dataGridView1.SelectedCells[0].OwningRow.Index;
                    col = dataGridView1.CurrentCell.ColumnIndex;
                    if (rowIndex >= 1)
                    {
                        dataGridView1.FirstDisplayedScrollingRowIndex = rowIndex - 1;
                    }
                    if (rowIndex > 0)
                    {
                        dataGridView1.ClearSelection();
                        this.dataGridView1.Rows[rowIndex - 1].Cells[col].Selected = true;
                    }
                }
            }
        }
        //public void ScrollToRow(int theRow)
        //{
        //    //
        //    // Expose the protected GridVScrolled method allowing you
        //    // to programmatically scroll the grid to a particular row.
        //    //
        //    if (dataGridView1.DataSource != null)
        //    {
                
        //        GridVScrolled(this, new ScrollEventArgs(ScrollEventType.LargeIncrement, theRow));
        //    }
        //}

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                select();
                this.dataGridView1.CurrentRow.Selected = true;

                e.Handled = true;
            }
         
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            bool nonNumberEntered = true;

            //if ((e.KeyChar >= 48 && e.KeyChar <= 57) || e.KeyChar == 8)
            //{
            //    nonNumberEntered = false;
            //}
            if (e.KeyChar == (char)13)
            {

                // Enter key pressed
            }else if (e.KeyChar == (char)9)
            {
                dataGridView2.Focus();
            }
            else
            {
                guna2TextBox6.Focus();
                guna2TextBox6.Text += (char)e.KeyChar;
                e.Handled = true;
            }

            //if (nonNumberEntered)
            //{
            //}
            //else
            //{
            //    guna2TextBox6.Focus();
            //    e.Handled = false;
            //}
        }

        private void dataGridView1_Leave(object sender, EventArgs e)
        {
            //dataGridView1.ClearSelection();
        }

        private void guna2TextBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                select();
            }
        }
        public string userid;
        private void ComputerSupplyForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Hide();
            ComputerList frm = new ComputerList();
            frm.id = userid;

            frm.ShowDialog();
            this.Close();
        }
       
        private void dataGridView1_Scroll(object sender, ScrollEventArgs e)
        {
           
        }
    }
}
