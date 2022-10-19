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
    public partial class staffPayroll : Form
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
        public string name;
        public staffPayroll()
        {
            InitializeComponent();
        }

        private void staffPayroll_Load(object sender, EventArgs e)
        {
            //string stmt = "SELECT COUNT(*) FROM tblStaffPayrolltemp";
            //int count = 0;

            //using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
            //{
            //    using (SqlCommand cmdCount = new SqlCommand(stmt, tblOTS))
            //    {
            //        tblOTS.Open();
            //        count = (int)cmdCount.ExecuteScalar();
            //    }
            //}
            //if (count > 0)
            //{

            //}
            //else
            //{
            load();
            count();
            if (dataGridView2.Columns[6].Visible == true)
            {
                guna2CheckBox2.Checked = true;
            }
            else
            {
                guna2CheckBox2.Checked = false;
            }
            if (dataGridView2.Columns[2].Visible == true)
            {
                guna2CheckBox3.Checked = true;
            }
            else
            {
                guna2CheckBox3.Checked = false;
            }
            ChangeControlStyles(dataGridView2, ControlStyles.OptimizedDoubleBuffer, true);
            this.dataGridView2.ColumnHeadersDefaultCellStyle.SelectionBackColor = this.dataGridView2.ColumnHeadersDefaultCellStyle.BackColor;
            dataGridView2.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
            //}
        }
        private void ChangeControlStyles(Control ctrl, ControlStyles flag, bool value)
        {
            MethodInfo method = ctrl.GetType().GetMethod("SetStyle", BindingFlags.Instance | BindingFlags.NonPublic);
            if (method != null)
                method.Invoke(ctrl, new object[] { flag, value });
        }
        private void load()
        {
            using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
            {
                dataGridView2.Rows.Clear();
                DataTable distinct = new DataTable();
                tblOTS.Open();
                string list = "SELECT * FROM tblStaffData WHERE STATUS ='ACTIVE' ORDER BY EMPLOYEENAME ASC";
                SqlDataAdapter command = new SqlDataAdapter(list, tblOTS);
                command.Fill(distinct);
                tblOTS.Close();
                foreach (DataRow item in distinct.Rows)
                {
                    int a = dataGridView2.Rows.Add();
                    dataGridView2.Rows[a].Cells["EMPCODE"].Value = item["EMPCODE"].ToString();
                    dataGridView2.Rows[a].Cells["NAME1"].Value = item["EMPLOYEENAME"].ToString();
                    dataGridView2.Rows[a].Cells["OVERLEAVE"].Value = "";
                    dataGridView2.Rows[a].Cells["BASICSALARY"].Value = string.Format("{0:#,##0.00}", Convert.ToDecimal(item["BASICSALARY"].ToString()));
                    dataGridView2.Rows[a].Cells["ALLOWANCE"].Value = string.Format("{0:#,##0.00}", Convert.ToDecimal(item["ALLOWANCE"].ToString()));
                    dataGridView2.Rows[a].Cells["SSS"].Value = string.Format("{0:#,##0.00}", Convert.ToDecimal(item["SSS"].ToString()));
                    dataGridView2.Rows[a].Cells["PHILHEALTH"].Value = string.Format("{0:#,##0.00}", Convert.ToDecimal(item["PHIL"].ToString()));
                    dataGridView2.Rows[a].Cells["HDMF"].Value = string.Format("{0:#,##0.00}", Convert.ToDecimal(item["HDMF"].ToString()));
                    dataGridView2.Rows[a].Cells["OTHERS"].Value = string.Format("{0:#,##0.00}", Convert.ToDecimal(item["OTHERS"].ToString()));
                }
            }
            dataGridView2.ClearSelection();
        }
        public string id;
        private void staffPayroll_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Hide();//Hide the 'current' form, i.e frm_form1 
                        //show another form ( frm_form2 )   
            payrollDashboard frm = new payrollDashboard();
            frm.id = id;
            frm.ShowDialog();
            //Close the form.(frm_form1)
            this.Dispose();
        }

        private void dataGridView2_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView2.Rows.Count != 0)
            {

                decimal basicsalary = 0.00M;
                decimal allowance = 0.00M;
                decimal sss = 0.00M;
                decimal phil = 0.00M;
                decimal hdmf = 0.00M;
                decimal others = 0.00M;
                decimal ca = 0.00M;
                decimal sc = 0.00M;
                decimal sssl = 0.00M;
                decimal hdmfl = 0.00M;
                decimal coop = 0.00M;
                decimal financing = 0.00M;
                decimal rsssloans = 0.00M;
                decimal salesout = 0.00M;
                decimal tmf = 0.00M;

                if (Convert.ToString(dataGridView2.CurrentRow.Cells["BASICSALARY"].Value) != string.Empty)
                {
                    basicsalary = Convert.ToDecimal(dataGridView2.CurrentRow.Cells["BASICSALARY"].Value.ToString());
                }
                else
                {
                    basicsalary = 0.00M;
                }
                if (Convert.ToString(dataGridView2.CurrentRow.Cells["ALLOWANCE"].Value) != string.Empty)
                {
                    allowance = Convert.ToDecimal(dataGridView2.CurrentRow.Cells["ALLOWANCE"].Value.ToString());
                }
                else
                {
                    allowance = 0.00M;
                }


                decimal sum = 0.00M;
                decimal overleave = 0.00M;
                if (Convert.ToString(dataGridView2.CurrentRow.Cells["OVERLEAVE"].Value) != string.Empty)
                {
                    sum += basicsalary + allowance;

                    if (Convert.ToDecimal(dataGridView2.CurrentRow.Cells["OVERLEAVE"].Value.ToString()) >= 0.00M)
                    {
                        overleave = sum + Convert.ToDecimal(dataGridView2.CurrentRow.Cells["OVERLEAVE"].Value.ToString());

                    }
                    else
                    {
                        overleave = sum - Convert.ToDecimal(dataGridView2.CurrentRow.Cells["OVERLEAVE"].Value.ToString().Replace("-", ""));
                    }
                }
                else
                {
                    if (Convert.ToString(dataGridView2.CurrentRow.Cells["BASICSALARY"].Value) != string.Empty)
                    {
                        overleave += basicsalary + allowance;
                    }
                }

                dataGridView2.CurrentRow.Cells["TOTALAMOUNT"].Value = string.Format("{0:#,##0.00}", Convert.ToDecimal(overleave));
                if (Convert.ToString(dataGridView2.CurrentRow.Cells["SSS"].Value) != string.Empty)
                {
                    sss = Convert.ToDecimal(dataGridView2.CurrentRow.Cells["SSS"].Value.ToString());
                }
                else
                {
                    sss = 0.00M;
                }
                if (Convert.ToString(dataGridView2.CurrentRow.Cells["PHILHEALTH"].Value) != string.Empty)
                {
                    phil = Convert.ToDecimal(dataGridView2.CurrentRow.Cells["PHILHEALTH"].Value.ToString());
                }
                else
                {
                    phil = 0.00M;
                }
                if (Convert.ToString(dataGridView2.CurrentRow.Cells["HDMF"].Value) != string.Empty)
                {
                    hdmf = Convert.ToDecimal(dataGridView2.CurrentRow.Cells["HDMF"].Value.ToString());
                }
                else
                {
                    hdmf = 0.00M;
                }
                if (Convert.ToString(dataGridView2.CurrentRow.Cells["OTHERS"].Value) != string.Empty)
                {
                    others = Convert.ToDecimal(dataGridView2.CurrentRow.Cells["OTHERS"].Value.ToString());
                }
                else
                {
                    others = 0.00M;
                }
                if (Convert.ToString(dataGridView2.CurrentRow.Cells["CA"].Value) != string.Empty)
                {
                    ca = Convert.ToDecimal(dataGridView2.CurrentRow.Cells["CA"].Value.ToString());
                }
                else
                {
                    ca = 0.00M;
                }
                if (Convert.ToString(dataGridView2.CurrentRow.Cells["SC"].Value) != string.Empty)
                {
                    sc = Convert.ToDecimal(dataGridView2.CurrentRow.Cells["SC"].Value.ToString());
                }
                else
                {
                    sc = 0.00M;
                }
                if (Convert.ToString(dataGridView2.CurrentRow.Cells["SSSLOANS"].Value) != string.Empty)
                {
                    sssl = Convert.ToDecimal(dataGridView2.CurrentRow.Cells["SSSLOANS"].Value.ToString());
                }
                else
                {
                    sssl = 0.00M;
                }


                if (Convert.ToString(dataGridView2.CurrentRow.Cells["HDMFLOANS"].Value) != string.Empty)
                {
                    hdmfl = Convert.ToDecimal(dataGridView2.CurrentRow.Cells["HDMFLOANS"].Value.ToString());
                }
                else
                {
                    hdmfl = 0.00M;
                }
                if (Convert.ToString(dataGridView2.CurrentRow.Cells["COOP"].Value) != string.Empty)
                {
                    coop = Convert.ToDecimal(dataGridView2.CurrentRow.Cells["COOP"].Value.ToString());
                }
                else
                {
                    coop = 0.00M;
                }
                if (Convert.ToString(dataGridView2.CurrentRow.Cells["FINANCING"].Value) != string.Empty)
                {
                    financing = Convert.ToDecimal(dataGridView2.CurrentRow.Cells["FINANCING"].Value.ToString());
                }
                else
                {
                    financing = 0.00M;
                }
                if (Convert.ToString(dataGridView2.CurrentRow.Cells["RSSSLOANS"].Value) != string.Empty)
                {
                    rsssloans = Convert.ToDecimal(dataGridView2.CurrentRow.Cells["RSSSLOANS"].Value.ToString());
                }
                else
                {
                    rsssloans = 0.00M;
                }
                if (Convert.ToString(dataGridView2.CurrentRow.Cells["SALESO"].Value) != string.Empty)
                {
                    salesout = Convert.ToDecimal(dataGridView2.CurrentRow.Cells["SALESO"].Value.ToString());
                }
                else
                {
                    salesout = 0.00M;
                }
                if (Convert.ToString(dataGridView2.CurrentRow.Cells["TMF"].Value) != string.Empty)
                {
                    tmf = Convert.ToDecimal(dataGridView2.CurrentRow.Cells["TMF"].Value.ToString());
                }
                else
                {
                    tmf = 0.00M;
                }
                decimal sumall = 0.00M;
                sumall += sss + phil + hdmf + others + ca + sc + sssl + hdmfl + coop + financing + rsssloans + salesout + tmf;

                decimal netall = 0.00M;
                netall = overleave - sumall;
                dataGridView2.CurrentRow.Cells["NET"].Value = string.Format("{0:#,##0.00}", Convert.ToDecimal(netall));


            }
        }

        private void dataGridView2_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {

        }
        public void count()
        {
            if (dataGridView2.Rows.Count != 0)
            {
                decimal tbasicsalary = 0.00M;
                decimal tallowance = 0.00M;
                decimal toverleave = 0.00M;
                decimal ttotalamount = 0.00M;
                decimal tsss = 0.00M;
                decimal tphil = 0.00M;
                decimal thdmf = 0.00M;
                decimal tothers = 0.00M;
                decimal tca = 0.00M;
                decimal tsc = 0.00M;
                decimal tsssl = 0.00M;
                decimal thdmfl = 0.00M;
                decimal tcoop = 0.00M;
                decimal tfinancing = 0.00M;
                decimal trsssloans = 0.00M;
                decimal tsalesout = 0.00M;
                decimal ttmf = 0.00M;
                decimal ttotalnet = 0.00M;
                foreach (DataGridViewRow row in dataGridView2.Rows)
                {

                    decimal basicsalary = 0.00M;
                    decimal allowance = 0.00M;
                    decimal sss = 0.00M;
                    decimal phil = 0.00M;
                    decimal hdmf = 0.00M;
                    decimal others = 0.00M;
                    decimal ca = 0.00M;
                    decimal sc = 0.00M;
                    decimal sssl = 0.00M;
                    decimal hdmfl = 0.00M;
                    decimal coop = 0.00M;
                    decimal financing = 0.00M;
                    decimal rsssloans = 0.00M;
                    decimal salesout = 0.00M;
                    decimal tmf = 0.00M;

                    if (Convert.ToString(row.Cells["BASICSALARY"].Value) != string.Empty)
                    {
                        basicsalary = Convert.ToDecimal(row.Cells["BASICSALARY"].Value.ToString());
                    }
                    else
                    {
                        basicsalary = 0.00M;
                    }
                    if (Convert.ToString(row.Cells["ALLOWANCE"].Value) != string.Empty)
                    {
                        allowance = Convert.ToDecimal(row.Cells["ALLOWANCE"].Value.ToString());
                    }
                    else
                    {
                        allowance = 0.00M;
                    }


                    decimal sum = 0.00M;
                    decimal overleave = 0.00M;
                    if (Convert.ToString(row.Cells["OVERLEAVE"].Value) != string.Empty)
                    {
                        sum += basicsalary + allowance;

                        if (Convert.ToDecimal(row.Cells["OVERLEAVE"].Value.ToString()) >= 0.00M)
                        {
                            overleave = sum + Convert.ToDecimal(row.Cells["OVERLEAVE"].Value.ToString());

                        }
                        else
                        {
                            overleave = sum - Convert.ToDecimal(row.Cells["OVERLEAVE"].Value.ToString().Replace("-", ""));
                        }
                    }
                    else
                    {
                        if (Convert.ToString(row.Cells["BASICSALARY"].Value) != string.Empty)
                        {
                            overleave += basicsalary + allowance;
                        }
                    }

                    row.Cells["TOTALAMOUNT"].Value = string.Format("{0:#,##0.00}", Convert.ToDecimal(overleave));
                    if (Convert.ToString(row.Cells["SSS"].Value) != string.Empty)
                    {
                        sss = Convert.ToDecimal(row.Cells["SSS"].Value.ToString());
                    }
                    else
                    {
                        sss = 0.00M;
                    }
                    if (Convert.ToString(row.Cells["PHILHEALTH"].Value) != string.Empty)
                    {
                        phil = Convert.ToDecimal(row.Cells["PHILHEALTH"].Value.ToString());
                    }
                    else
                    {
                        phil = 0.00M;
                    }
                    if (Convert.ToString(row.Cells["HDMF"].Value) != string.Empty)
                    {
                        hdmf = Convert.ToDecimal(row.Cells["HDMF"].Value.ToString());
                    }
                    else
                    {
                        hdmf = 0.00M;
                    }
                    if (Convert.ToString(row.Cells["OTHERS"].Value) != string.Empty)
                    {
                        others = Convert.ToDecimal(row.Cells["OTHERS"].Value.ToString());
                    }
                    else
                    {
                        others = 0.00M;
                    }
                    if (Convert.ToString(row.Cells["CA"].Value) != string.Empty)
                    {
                        ca = Convert.ToDecimal(row.Cells["CA"].Value.ToString());
                    }
                    else
                    {
                        ca = 0.00M;
                    }
                    if (Convert.ToString(row.Cells["SC"].Value) != string.Empty)
                    {
                        sc = Convert.ToDecimal(row.Cells["SC"].Value.ToString());
                    }
                    else
                    {
                        sc = 0.00M;
                    }
                    if (Convert.ToString(row.Cells["SSSLOANS"].Value) != string.Empty)
                    {
                        sssl = Convert.ToDecimal(row.Cells["SSSLOANS"].Value.ToString());
                    }
                    else
                    {
                        sssl = 0.00M;
                    }


                    if (Convert.ToString(row.Cells["HDMFLOANS"].Value) != string.Empty)
                    {
                        hdmfl = Convert.ToDecimal(row.Cells["HDMFLOANS"].Value.ToString());
                    }
                    else
                    {
                        hdmfl = 0.00M;
                    }
                    if (Convert.ToString(row.Cells["COOP"].Value) != string.Empty)
                    {
                        coop = Convert.ToDecimal(row.Cells["COOP"].Value.ToString());
                    }
                    else
                    {
                        coop = 0.00M;
                    }
                    if (Convert.ToString(row.Cells["FINANCING"].Value) != string.Empty)
                    {
                        financing = Convert.ToDecimal(row.Cells["FINANCING"].Value.ToString());
                    }
                    else
                    {
                        financing = 0.00M;
                    }
                    if (Convert.ToString(row.Cells["RSSSLOANS"].Value) != string.Empty)
                    {
                        rsssloans = Convert.ToDecimal(row.Cells["RSSSLOANS"].Value.ToString());
                    }
                    else
                    {
                        rsssloans = 0.00M;
                    }
                    if (Convert.ToString(row.Cells["SALESO"].Value) != string.Empty)
                    {
                        salesout = Convert.ToDecimal(row.Cells["SALESO"].Value.ToString());
                    }
                    else
                    {
                        salesout = 0.00M;
                    }
                    if (Convert.ToString(row.Cells["TMF"].Value) != string.Empty)
                    {
                        tmf = Convert.ToDecimal(row.Cells["TMF"].Value.ToString());
                    }
                    else
                    {
                        tmf = 0.00M;
                    }
                    decimal sumall = 0.00M;
                    sumall += sss + phil + hdmf + others + ca + sc + sssl + hdmfl + coop + financing + rsssloans + salesout + tmf;

                    decimal netall = 0.00M;
                    netall = overleave - sumall;
                    row.Cells["NET"].Value = string.Format("{0:#,##0.00}", Convert.ToDecimal(netall));


                    tbasicsalary += basicsalary;
                    tallowance += allowance;

                    if (String.IsNullOrEmpty(row.Cells["OVERLEAVE"].Value as String))
                    {
                    }
                    else
                    {

                        toverleave += Convert.ToDecimal(row.Cells["OVERLEAVE"].Value.ToString());
                    }
                    ttotalamount += overleave;
                    tsss += sss;
                    tphil += phil;
                    thdmf += hdmf;
                    tothers += others;
                    tca += ca;
                    tsc += sc;
                    tsssl += sssl;
                    thdmfl += hdmfl;
                    tcoop += coop;
                    tfinancing += financing;
                    trsssloans += rsssloans;
                    tsalesout += salesout;
                    ttmf += tmf;
                    ttotalnet += netall;

                }

                label5.Text = string.Format("{0:#,##0.00}", tbasicsalary);
                label1.Text = string.Format("{0:#,##0.00}", tallowance);
                label3.Text = string.Format("{0:#,##0.00}", toverleave);
                label7.Text = string.Format("{0:#,##0.00}", ttotalamount);
                label9.Text = string.Format("{0:#,##0.00}", tsss);
                label11.Text = string.Format("{0:#,##0.00}", tphil);
                label13.Text = string.Format("{0:#,##0.00}", thdmf);
                label15.Text = string.Format("{0:#,##0.00}", tothers);
                label38.Text = string.Format("{0:#,##0.00}", tca);
                label17.Text = string.Format("{0:#,##0.00}", tsc);
                label19.Text = string.Format("{0:#,##0.00}", tsssl);
                label21.Text = string.Format("{0:#,##0.00}", thdmfl);
                label36.Text = string.Format("{0:#,##0.00}", tcoop);
                label34.Text = string.Format("{0:#,##0.00}", tfinancing);
                label32.Text = string.Format("{0:#,##0.00}", trsssloans);
                label30.Text = string.Format("{0:#,##0.00}", tsalesout);
                label25.Text = string.Format("{0:#,##0.00}", ttmf);
                label23.Text = string.Format("{0:#,##0.00}", ttotalnet);
            }
        }
        private void Control_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (!char.IsControl(e.KeyChar)
     && !char.IsDigit(e.KeyChar)
     && e.KeyChar != '.' && e.KeyChar != '-')
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if (e.KeyChar == '.'
                && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
            }
            if (e.KeyChar == '-'
              && (sender as TextBox).Text.IndexOf('-') > -1)
            {
                e.Handled = true;
            }
        }
        private void Control_KeyPress1(object sender, KeyPressEventArgs e)
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

        private void dataGridView2_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (dataGridView2.CurrentCell.ColumnIndex == 4)
            {
                TextBox tb = e.Control as TextBox;
                if (tb != null)
                {
                    //tb.KeyPress += new KeyPressEventHandler(Column1_KeyPress);
                    tb.KeyPress += new KeyPressEventHandler(Control_KeyPress);
                }
            }
            if (dataGridView2.CurrentCell.ColumnIndex == 10 || dataGridView2.CurrentCell.ColumnIndex == 11 || dataGridView2.CurrentCell.ColumnIndex == 12 || dataGridView2.CurrentCell.ColumnIndex == 13 || dataGridView2.CurrentCell.ColumnIndex == 14 || dataGridView2.CurrentCell.ColumnIndex == 15 || dataGridView2.CurrentCell.ColumnIndex == 16 || dataGridView2.CurrentCell.ColumnIndex == 17 || dataGridView2.CurrentCell.ColumnIndex == 18)
            {
                TextBox tb = e.Control as TextBox;
                if (tb != null)
                {
                    //tb.KeyPress += new KeyPressEventHandler(Column1_KeyPress);
                    tb.KeyPress += new KeyPressEventHandler(Control_KeyPress1);
                }
            }
        }

        private void guna2CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2CheckBox1.Checked)
            {
                dataGridView2.Columns[10].Visible = true;
                dataGridView2.Columns[11].Visible = true;
                dataGridView2.Columns[12].Visible = true;
                dataGridView2.Columns[13].Visible = true;
                dataGridView2.Columns[14].Visible = true;
                dataGridView2.Columns[15].Visible = true;
                dataGridView2.Columns[16].Visible = true;
                dataGridView2.Columns[17].Visible = true;
                dataGridView2.Columns[18].Visible = true;
            }
            else
            {
                dataGridView2.Columns[10].Visible = false;
                dataGridView2.Columns[11].Visible = false;
                dataGridView2.Columns[12].Visible = false;
                dataGridView2.Columns[13].Visible = false;
                dataGridView2.Columns[14].Visible = false;
                dataGridView2.Columns[15].Visible = false;
                dataGridView2.Columns[16].Visible = false;
                dataGridView2.Columns[17].Visible = false;
                dataGridView2.Columns[18].Visible = false;
            }
        }

        private void dataGridView2_Enter(object sender, EventArgs e)
        {

        }

        private void dataGridView1_Leave(object sender, EventArgs e)
        {

        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            count();
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {

            DateTime temp;
            if (DateTime.TryParse(maskedTextBox1.Text, out temp))
            {
                // Yay :)
            }
            else
            {
                MessageBox.Show("Invalid Date");
                return;
            }
            DateTime tempt;
            if (DateTime.TryParse(maskedTextBox2.Text, out tempt))
            {
                // Yay :)
            }
            else
            {
                MessageBox.Show("Invalid Date");
                return;
            }
            if (maskedTextBox1.Text.Contains("_") || maskedTextBox2.Text.Contains("_"))
            {
                MessageBox.Show("Invalid Date");
                return;
            }

            using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
            {
                tblOTS.Open();
                SqlCommand check_User_Name = new SqlCommand("SELECT COUNT(*) FROM [tblStaffPayroll] WHERE FROMPERIOD = @FROMPERIOD AND TOPERIOD = @TOPERIOD", tblOTS);
                check_User_Name.Parameters.AddWithValue("@FROMPERIOD", maskedTextBox1.Text);
                check_User_Name.Parameters.AddWithValue("@TOPERIOD", maskedTextBox2.Text);
                int UserExist = (int)check_User_Name.ExecuteScalar();

                if (UserExist > 0)
                {
                    MessageBox.Show("Payroll Period is already exist");
                    return;
                }
                tblOTS.Close();
            }
            DialogResult dialogResult = MessageBox.Show("Are you sure to continue this?", "Continue?", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                save();
            }
        }
        private void save()
        {
            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                decimal ca = 0.00M;
                decimal sc = 0.00M;
                decimal sssl = 0.00M;
                decimal hdmfl = 0.00M;
                decimal coop = 0.00M;
                decimal financing = 0.00M;
                decimal rsssloans = 0.00M;
                decimal salesout = 0.00M;
                decimal tmf = 0.00M;

                if (Convert.ToString(row.Cells["CA"].Value) != string.Empty)
                {
                    ca = Convert.ToDecimal(row.Cells["CA"].Value.ToString());
                }
                else
                {
                    ca = 0.00M;
                }
                if (Convert.ToString(row.Cells["SC"].Value) != string.Empty)
                {
                    sc = Convert.ToDecimal(row.Cells["SC"].Value.ToString());
                }
                else
                {
                    sc = 0.00M;
                }
                if (Convert.ToString(row.Cells["SSSLOANS"].Value) != string.Empty)
                {
                    sssl = Convert.ToDecimal(row.Cells["SSSLOANS"].Value.ToString());
                }
                else
                {
                    sssl = 0.00M;
                }

                if (Convert.ToString(row.Cells["HDMFLOANS"].Value) != string.Empty)
                {
                    hdmfl = Convert.ToDecimal(row.Cells["HDMFLOANS"].Value.ToString());
                }
                else
                {
                    hdmfl = 0.00M;
                }
                if (Convert.ToString(row.Cells["COOP"].Value) != string.Empty)
                {
                    coop = Convert.ToDecimal(row.Cells["COOP"].Value.ToString());
                }
                else
                {
                    coop = 0.00M;
                }
                if (Convert.ToString(row.Cells["FINANCING"].Value) != string.Empty)
                {
                    financing = Convert.ToDecimal(row.Cells["FINANCING"].Value.ToString());
                }
                else
                {
                    financing = 0.00M;
                }
                if (Convert.ToString(row.Cells["RSSSLOANS"].Value) != string.Empty)
                {
                    rsssloans = Convert.ToDecimal(row.Cells["RSSSLOANS"].Value.ToString());
                }
                else
                {
                    rsssloans = 0.00M;
                }
                if (Convert.ToString(row.Cells["SALESO"].Value) != string.Empty)
                {
                    salesout = Convert.ToDecimal(row.Cells["SALESO"].Value.ToString());
                }
                else
                {
                    salesout = 0.00M;
                }
                if (Convert.ToString(row.Cells["TMF"].Value) != string.Empty)
                {
                    tmf = Convert.ToDecimal(row.Cells["TMF"].Value.ToString());
                }
                else
                {
                    tmf = 0.00M;
                }

                using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
                {
                    tblOTS.Open();
                    string insStmt = "insert into tblStaffPayroll ([EMPCODE], [EMPNAME], [BASICSALARY], [ALLOWANCE], [OVERLEAVE], [TOTALAMOUNT], [SSS], [PHILHEALTH], [HDMF], [CA], [SC], [SSSLOANS], [HDMFLOANS], [COOP], [FINANCING], [RSSSLOANS], [SALESO], [TMF], [OTHERS], [NET], [FROMPERIOD], [TOPERIOD], [CREATEDBY]) values" +
                        " (@EMPCODE,@EMPNAME,@BASICSALARY,@ALLOWANCE,@OVERLEAVE,@TOTALAMOUNT,@SSS,@PHILHEALTH,@HDMF,@CA,@SC,@SSSLOANS,@HDMFLOANS,@COOP,@FINANCING,@RSSSLOANS,@SALESO,@TMF,@OTHERS,@NET,@FROMPERIOD,@TOPERIOD,@CREATEDBY)";
                    SqlCommand insCmd = new SqlCommand(insStmt, tblOTS);
                    insCmd.Parameters.Clear();
                    insCmd.Parameters.AddWithValue("@EMPCODE", row.Cells["EMPCODE"].Value.ToString().Trim());
                    insCmd.Parameters.AddWithValue("@EMPNAME", row.Cells["NAME1"].Value.ToString().Trim());
                    insCmd.Parameters.AddWithValue("@BASICSALARY", row.Cells["BASICSALARY"].Value.ToString().Trim());
                    insCmd.Parameters.AddWithValue("@ALLOWANCE", row.Cells["ALLOWANCE"].Value.ToString().Trim());
                    insCmd.Parameters.AddWithValue("@OVERLEAVE", row.Cells["OVERLEAVE"].Value.ToString().Trim());
                    insCmd.Parameters.AddWithValue("@TOTALAMOUNT", row.Cells["TOTALAMOUNT"].Value.ToString().Trim());
                    insCmd.Parameters.AddWithValue("@SSS", row.Cells["SSS"].Value.ToString().Trim());
                    insCmd.Parameters.AddWithValue("@PHILHEALTH", row.Cells["PHILHEALTH"].Value.ToString().Trim());
                    insCmd.Parameters.AddWithValue("@HDMF", row.Cells["HDMF"].Value.ToString().Trim());
                    insCmd.Parameters.AddWithValue("@CA", ca.ToString());
                    insCmd.Parameters.AddWithValue("@SC", sc.ToString());
                    insCmd.Parameters.AddWithValue("@SSSLOANS", sssl.ToString());
                    insCmd.Parameters.AddWithValue("@HDMFLOANS", hdmfl.ToString());
                    insCmd.Parameters.AddWithValue("@COOP", coop.ToString());
                    insCmd.Parameters.AddWithValue("@FINANCING", financing.ToString());
                    insCmd.Parameters.AddWithValue("@RSSSLOANS", rsssloans.ToString());
                    insCmd.Parameters.AddWithValue("@SALESO", salesout.ToString());
                    insCmd.Parameters.AddWithValue("@TMF", tmf.ToString());
                    insCmd.Parameters.AddWithValue("@OTHERS", row.Cells["OTHERS"].Value.ToString().Trim());
                    insCmd.Parameters.AddWithValue("@NET", row.Cells["NET"].Value.ToString().Trim());
                    insCmd.Parameters.AddWithValue("@FROMPERIOD", maskedTextBox1.Text.Trim());
                    insCmd.Parameters.AddWithValue("@TOPERIOD", maskedTextBox2.Text.Trim());
                    insCmd.Parameters.AddWithValue("@CREATEDBY", name.Trim());
                    int affectedRows = insCmd.ExecuteNonQuery();
                    tblOTS.Close();
                }
            }
            MessageBox.Show("Successfully Added!");
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
            {
                string sqlTrunc = "SELECT COUNT(*) FROM tblStaffPayrolltemp";
                SqlCommand cmd = new SqlCommand(sqlTrunc, tblOTS);

                tblOTS.Open();
                int count = (int)cmd.ExecuteScalar();
                tblOTS.Close();
                if (count != 0)
                {
                    DialogResult dialogResult = MessageBox.Show("Found some data in temporary files, are you sure to continue?", "Continue?", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        saveTEMP();
                    }
                }
                else
                {
                    DialogResult dialogResult = MessageBox.Show("Save this as temporary?", "Continue?", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        saveTEMP();
                    }
                }
            }
        }
        private void saveTEMP()
        {
            using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
            {

                string sqlTrunc = "TRUNCATE TABLE tblStaffPayrolltemp";
                SqlCommand cmd = new SqlCommand(sqlTrunc, tblOTS);

                tblOTS.Open();
                cmd.ExecuteNonQuery();
                tblOTS.Close();

            }
            foreach (DataGridViewRow row in dataGridView2.Rows)
            {

                string ca = "";
                string sc = "";
                string sssl = "";
                string hdmfl = "";
                string coop = "";
                string financing = "";
                string rsssloans = "";
                string salesout = "";
                string tmf = "";

                if (Convert.ToString(row.Cells["CA"].Value) != string.Empty)
                {
                    ca = row.Cells["CA"].Value.ToString();
                }
                else
                {
                    ca = "";
                }
                if (Convert.ToString(row.Cells["SC"].Value) != string.Empty)
                {
                    sc = row.Cells["SC"].Value.ToString();
                }
                else
                {
                    sc = "";
                }
                if (Convert.ToString(row.Cells["SSSLOANS"].Value) != string.Empty)
                {
                    sssl = row.Cells["SSSLOANS"].Value.ToString();
                }
                else
                {
                    sssl = "";
                }


                if (Convert.ToString(row.Cells["HDMFLOANS"].Value) != string.Empty)
                {
                    hdmfl = row.Cells["HDMFLOANS"].Value.ToString();
                }
                else
                {
                    hdmfl = "";
                }
                if (Convert.ToString(row.Cells["COOP"].Value) != string.Empty)
                {
                    coop = row.Cells["COOP"].Value.ToString();
                }
                else
                {
                    coop = "";
                }
                if (Convert.ToString(row.Cells["FINANCING"].Value) != string.Empty)
                {
                    financing = row.Cells["FINANCING"].Value.ToString();
                }
                else
                {
                    financing = "";
                }
                if (Convert.ToString(row.Cells["RSSSLOANS"].Value) != string.Empty)
                {
                    rsssloans = row.Cells["RSSSLOANS"].Value.ToString();
                }
                else
                {
                    rsssloans = "";
                }
                if (Convert.ToString(row.Cells["SALESO"].Value) != string.Empty)
                {
                    salesout = row.Cells["SALESO"].Value.ToString();
                }
                else
                {
                    salesout = "";
                }
                if (Convert.ToString(row.Cells["TMF"].Value) != string.Empty)
                {
                    tmf = row.Cells["TMF"].Value.ToString();
                }
                else
                {
                    tmf = "";
                }

                using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
                {
                    tblOTS.Open();
                    string insStmt = "insert into tblStaffPayrolltemp ([EMPCODE], [EMPNAME], [OVERLEAVE], [CA], [SC], [SSSLOANS], [HDMFLOANS], [COOP], [FINANCING], [RSSSLOANS], [SALESO], [TMF]) values" +
                        " (@EMPCODE,@EMPNAME,@OVERLEAVE,@CA,@SC,@SSSLOANS,@HDMFLOANS,@COOP,@FINANCING,@RSSSLOANS,@SALESO,@TMF)";
                    SqlCommand insCmd = new SqlCommand(insStmt, tblOTS);
                    insCmd.Parameters.Clear();
                    insCmd.Parameters.AddWithValue("@EMPCODE", row.Cells["EMPCODE"].Value.ToString().Trim());
                    insCmd.Parameters.AddWithValue("@EMPNAME", row.Cells["NAME1"].Value.ToString().Trim());
                    insCmd.Parameters.AddWithValue("@OVERLEAVE", row.Cells["OVERLEAVE"].Value.ToString().Trim());
                    insCmd.Parameters.AddWithValue("@CA", ca.ToString());
                    insCmd.Parameters.AddWithValue("@SC", sc.ToString());
                    insCmd.Parameters.AddWithValue("@SSSLOANS", sssl.ToString());
                    insCmd.Parameters.AddWithValue("@HDMFLOANS", hdmfl.ToString());
                    insCmd.Parameters.AddWithValue("@COOP", coop.ToString());
                    insCmd.Parameters.AddWithValue("@FINANCING", financing.ToString());
                    insCmd.Parameters.AddWithValue("@RSSSLOANS", rsssloans.ToString());
                    insCmd.Parameters.AddWithValue("@SALESO", salesout.ToString());
                    insCmd.Parameters.AddWithValue("@TMF", tmf.ToString());
                    int affectedRows = insCmd.ExecuteNonQuery();
                    tblOTS.Close();
                }
            }
            MessageBox.Show("Successfully Save as Temp!");
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            prevload();
        }
        private void prevload()
        {
            using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
            {
                dataGridView2.Rows.Clear();
                DataTable distinct = new DataTable();
                tblOTS.Open();
                string list = "SELECT * FROM tblStaffPayrolltemp ORDER BY EMPNAME ASC";
                SqlDataAdapter command = new SqlDataAdapter(list, tblOTS);

                command.Fill(distinct);
                tblOTS.Close();
                foreach (DataRow item in distinct.Rows)
                {
                    tblOTS.Open();
                    String query1 = "SELECT STATUS FROM tblStaffData WHERE EMPCODE = '" + item["EMPCODE"].ToString() + "'";
                    SqlCommand cmd1 = new SqlCommand(query1, tblOTS);
                    SqlDataReader rdr1 = cmd1.ExecuteReader();
                    bool active = true;
                    if (rdr1.Read())
                    {
                        if (rdr1["STATUS"].ToString() == "INACTIVE")
                        {
                            active = false;
                        }
                    }
                    tblOTS.Close();

                    if (active == true)
                    {
                        int a = dataGridView2.Rows.Add();
                        dataGridView2.Rows[a].Cells["EMPCODE"].Value = item["EMPCODE"].ToString();
                        dataGridView2.Rows[a].Cells["NAME1"].Value = item["EMPNAME"].ToString();

                        tblOTS.Open();
                        DataTable dt = new DataTable();
                        String query = "SELECT * FROM tblStaffData WHERE EMPCODE = '" + item["EMPCODE"].ToString() + "'";
                        SqlCommand cmd = new SqlCommand(query, tblOTS);
                        SqlDataReader rdr = cmd.ExecuteReader();

                        if (rdr.Read())
                        {
                            dataGridView2.Rows[a].Cells["BASICSALARY"].Value = string.Format("{0:#,##0.00}", Convert.ToDecimal(rdr["BASICSALARY"].ToString()));
                            dataGridView2.Rows[a].Cells["ALLOWANCE"].Value = string.Format("{0:#,##0.00}", Convert.ToDecimal(rdr["ALLOWANCE"].ToString()));
                            dataGridView2.Rows[a].Cells["SSS"].Value = string.Format("{0:#,##0.00}", Convert.ToDecimal(rdr["SSS"].ToString()));
                            dataGridView2.Rows[a].Cells["PHILHEALTH"].Value = string.Format("{0:#,##0.00}", Convert.ToDecimal(rdr["PHIL"].ToString()));
                            dataGridView2.Rows[a].Cells["HDMF"].Value = string.Format("{0:#,##0.00}", Convert.ToDecimal(rdr["HDMF"].ToString()));
                            dataGridView2.Rows[a].Cells["OTHERS"].Value = string.Format("{0:#,##0.00}", Convert.ToDecimal(rdr["OTHERS"].ToString()));
                        }
                        tblOTS.Close();


                        dataGridView2.Rows[a].Cells["OVERLEAVE"].Value = item["OVERLEAVE"].ToString();
                        dataGridView2.Rows[a].Cells["CA"].Value = item["CA"].ToString();
                        dataGridView2.Rows[a].Cells["SC"].Value = item["SC"].ToString();
                        dataGridView2.Rows[a].Cells["SSSLOANS"].Value = item["SSSLOANS"].ToString();
                        dataGridView2.Rows[a].Cells["HDMFLOANS"].Value = item["HDMFLOANS"].ToString();
                        dataGridView2.Rows[a].Cells["COOP"].Value = item["COOP"].ToString();
                        dataGridView2.Rows[a].Cells["FINANCING"].Value = item["FINANCING"].ToString();
                        dataGridView2.Rows[a].Cells["RSSSLOANS"].Value = item["RSSSLOANS"].ToString();
                        dataGridView2.Rows[a].Cells["SALESO"].Value = item["SALESO"].ToString();
                        dataGridView2.Rows[a].Cells["TMF"].Value = item["TMF"].ToString();
                    }
                    else
                    {
                        tblOTS.Open();
                        using (SqlCommand command1 = new SqlCommand("DELETE FROM tblStaffPayrolltemp WHERE EMPCODE = '" + item["EMPCODE"].ToString() + "'", tblOTS))
                        {
                            command1.ExecuteNonQuery();
                        }
                        tblOTS.Close();
                    }

                }
            }
            dataGridView2.ClearSelection();
            count();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            load();
            count();
        }

        private void guna2CheckBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2CheckBox3.Checked)
            {
                dataGridView2.Columns[2].Visible = true;
                dataGridView2.Columns[3].Visible = true;
                dataGridView2.Columns[4].Visible = true;
                dataGridView2.Columns[5].Visible = true;
            }
            else
            {
                dataGridView2.Columns[2].Visible = false;
                dataGridView2.Columns[3].Visible = false;
                dataGridView2.Columns[4].Visible = false;
                dataGridView2.Columns[5].Visible = false;
            }
        }

        private void guna2CheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2CheckBox2.Checked)
            {
                dataGridView2.Columns[6].Visible = true;
                dataGridView2.Columns[7].Visible = true;
                dataGridView2.Columns[8].Visible = true;
                dataGridView2.Columns[9].Visible = true;
            }
            else
            {
                dataGridView2.Columns[6].Visible = false;
                dataGridView2.Columns[7].Visible = false;
                dataGridView2.Columns[8].Visible = false;
                dataGridView2.Columns[9].Visible = false;
            }
        }

        private void dataGridView2_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {

                if (e.KeyCode == Keys.F2)
                {
                    staffInformations frm = new staffInformations();
                    frm.empcode = dataGridView2.CurrentRow.Cells["EMPCODE"].Value.ToString();
                    frm.form = "A";
                    frm.ShowDialog();
                }
                if (e.KeyCode == Keys.F1)
                {
                    stafPayrollPopup s = new stafPayrollPopup();

                    s.empcode = dataGridView2.CurrentRow.Cells["EMPCODE"].Value.ToString();

                    s.empname = dataGridView2.CurrentRow.Cells["NAME1"].Value.ToString();

                    s.basicsalary = dataGridView2.CurrentRow.Cells["BASICSALARY"].Value.ToString();
                    s.allowance = dataGridView2.CurrentRow.Cells["ALLOWANCE"].Value.ToString();
                    if (Convert.ToString(dataGridView2.CurrentRow.Cells["EMPCODE"].Value) != string.Empty)
                    {
                        if (!string.IsNullOrEmpty(dataGridView2.CurrentRow.Cells["EMPCODE"].Value.ToString()))
                        {
                            //MessageBox.Show(dataGridView2.CurrentRow.Cells["OVERLEAVE"].Value.ToString());

                            s.overleave = "";
                        }
                        else
                        {
                            s.overleave = dataGridView2.CurrentRow.Cells["OVERLEAVE"].Value.ToString();
                        }
                    }
                    else
                    {
                        s.overleave = "";
                    }
                    s.sss = dataGridView2.CurrentRow.Cells["SSS"].Value.ToString();
                    s.phil = dataGridView2.CurrentRow.Cells["PHILHEALTH"].Value.ToString();
                    s.hdmf = dataGridView2.CurrentRow.Cells["HDMF"].Value.ToString();
                    if (Convert.ToString(dataGridView2.CurrentRow.Cells["OTHERS"].Value) != string.Empty)
                    {
                        s.others = dataGridView2.CurrentRow.Cells["OTHERS"].Value.ToString();
                    }
                    else
                    {
                        s.others = "";
                    }
                    if (Convert.ToString(dataGridView2.CurrentRow.Cells["CA"].Value) != string.Empty)
                    {
                        s.ca = dataGridView2.CurrentRow.Cells["CA"].Value.ToString();
                    }
                    else
                    {
                        s.ca = "";
                    }
                    if (Convert.ToString(dataGridView2.CurrentRow.Cells["SC"].Value) != string.Empty)
                    {
                        s.sc = dataGridView2.CurrentRow.Cells["SC"].Value.ToString();
                    }
                    else
                    {
                        s.sc = "";
                    }
                    if (Convert.ToString(dataGridView2.CurrentRow.Cells["SSSLOANS"].Value) != string.Empty)
                    {
                        s.sssl = dataGridView2.CurrentRow.Cells["SSSLOANS"].Value.ToString();
                    }
                    else
                    {
                        s.sssl = "";
                    }
                    if (Convert.ToString(dataGridView2.CurrentRow.Cells["HDMFLOANS"].Value) != string.Empty)
                    {
                        s.hdmfl = dataGridView2.CurrentRow.Cells["HDMFLOANS"].Value.ToString();
                    }
                    else
                    {
                        s.hdmfl = "";
                    }
                    if (Convert.ToString(dataGridView2.CurrentRow.Cells["COOP"].Value) != string.Empty)
                    {
                        s.coop = dataGridView2.CurrentRow.Cells["COOP"].Value.ToString();
                    }
                    else
                    {
                        s.coop = "";
                    }
                    if (Convert.ToString(dataGridView2.CurrentRow.Cells["FINANCING"].Value) != string.Empty)
                    {
                        s.financing = dataGridView2.CurrentRow.Cells["FINANCING"].Value.ToString();
                    }
                    else
                    {
                        s.financing = "";
                    }
                    if (Convert.ToString(dataGridView2.CurrentRow.Cells["RSSSLOANS"].Value) != string.Empty)
                    {
                        s.rsssloans = dataGridView2.CurrentRow.Cells["RSSSLOANS"].Value.ToString();
                    }
                    else
                    {
                        s.rsssloans = "";
                    }
                    if (Convert.ToString(dataGridView2.CurrentRow.Cells["SALESO"].Value) != string.Empty)
                    {
                        s.salesout = dataGridView2.CurrentRow.Cells["SALESO"].Value.ToString();
                    }
                    else
                    {
                        s.salesout = "";
                    }
                    if (Convert.ToString(dataGridView2.CurrentRow.Cells["TMF"].Value) != string.Empty)
                    {
                        s.tmf = dataGridView2.CurrentRow.Cells["TMF"].Value.ToString();
                    }
                    else
                    {
                        s.tmf = "";
                    }
                    s.amount = dataGridView2.CurrentRow.Cells["TOTALAMOUNT"].Value.ToString();
                    s.net = dataGridView2.CurrentRow.Cells["NET"].Value.ToString();
                    s.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
