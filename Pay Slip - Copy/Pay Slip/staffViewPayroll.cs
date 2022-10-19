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
    public partial class staffViewPayroll : Form
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
        public staffViewPayroll()
        {
            InitializeComponent();
        }

        private void staffViewPayroll_Load(object sender, EventArgs e)
        {
            prevload();

            count();

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
        public string FPERIOD;
        public string TPERIOD;
        private void prevload()
        {
            using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
            {
                dataGridView2.Rows.Clear();
                DataTable distinct = new DataTable();
                tblOTS.Open();
                string list = "SELECT * FROM tblStaffPayroll WHERE FROMPERIOD = '"+FPERIOD+ "' AND TOPERIOD = '" + TPERIOD + "' ORDER BY id ASC";
                SqlDataAdapter command = new SqlDataAdapter(list, tblOTS);

                command.Fill(distinct);
                tblOTS.Close();
                foreach (DataRow item in distinct.Rows)
                {
                    int a = dataGridView2.Rows.Add();
                    dataGridView2.Rows[a].Cells["EMPCODE"].Value = item["EMPCODE"].ToString();
                    dataGridView2.Rows[a].Cells["NAME"].Value = item["EMPNAME"].ToString();
                    dataGridView2.Rows[a].Cells["BASICSALARY"].Value = string.Format("{0:#,##0.00}", Convert.ToDecimal(item["BASICSALARY"].ToString()));
                    dataGridView2.Rows[a].Cells["ALLOWANCE"].Value = string.Format("{0:#,##0.00}", Convert.ToDecimal(item["ALLOWANCE"].ToString()));
                    dataGridView2.Rows[a].Cells["OVERLEAVE"].Value = item["OVERLEAVE"].ToString();
                    dataGridView2.Rows[a].Cells["TOTALAMOUNT"].Value = string.Format("{0:#,##0.00}", Convert.ToDecimal(item["TOTALAMOUNT"].ToString()));
                    dataGridView2.Rows[a].Cells["SSS"].Value = item["SSS"].ToString();
                    dataGridView2.Rows[a].Cells["PHILHEALTH"].Value = item["PHILHEALTH"].ToString();
                    dataGridView2.Rows[a].Cells["HDMF"].Value = item["HDMF"].ToString();
                    dataGridView2.Rows[a].Cells["CA"].Value = item["CA"].ToString();
                    dataGridView2.Rows[a].Cells["SC"].Value = item["SC"].ToString();
                    dataGridView2.Rows[a].Cells["SSSLOANS"].Value = item["SSSLOANS"].ToString();
                    dataGridView2.Rows[a].Cells["HDMFLOANS"].Value = item["HDMFLOANS"].ToString();
                    dataGridView2.Rows[a].Cells["COOP"].Value = item["COOP"].ToString();
                    dataGridView2.Rows[a].Cells["FINANCING"].Value = item["FINANCING"].ToString();
                    dataGridView2.Rows[a].Cells["RSSSLOANS"].Value = item["RSSSLOANS"].ToString();
                    dataGridView2.Rows[a].Cells["SALESO"].Value = item["SALESO"].ToString();
                    dataGridView2.Rows[a].Cells["TMF"].Value = item["TMF"].ToString();
                    dataGridView2.Rows[a].Cells["OTHERS"].Value = item["OTHERS"].ToString();
                    dataGridView2.Rows[a].Cells["NET"].Value = item["NET"].ToString();
                }
            }
            dataGridView2.ClearSelection();
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
        private void count()
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
    }
}
