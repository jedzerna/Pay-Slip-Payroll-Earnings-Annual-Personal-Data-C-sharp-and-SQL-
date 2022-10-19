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
    public partial class staffEarnings : Form
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
        public staffEarnings()
        {
            InitializeComponent();
        }

        private void staffEarnings_Load(object sender, EventArgs e)
        {
            ChangeControlStyles(dataGridView2, ControlStyles.OptimizedDoubleBuffer, true);
            this.dataGridView2.ColumnHeadersDefaultCellStyle.SelectionBackColor = this.dataGridView2.ColumnHeadersDefaultCellStyle.BackColor;
            dataGridView2.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
        }
        private void ChangeControlStyles(Control ctrl, ControlStyles flag, bool value)
        {
            MethodInfo method = ctrl.GetType().GetMethod("SetStyle", BindingFlags.Instance | BindingFlags.NonPublic);
            if (method != null)
                method.Invoke(ctrl, new object[] { flag, value });
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            genarate();
        }
        private void genarate()
        {
            DateTime from = DateTime.Parse(maskedTextBox1.Text);
            DateTime to = DateTime.Parse(maskedTextBox2.Text);
            using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
            {
                dataGridView1.Rows.Clear();
                DataTable distinct = new DataTable();
                tblOTS.Open();
                string list = "SELECT EMPCODE,EMPNAME,TOPERIOD FROM tblStaffPayroll WHERE TOPERIOD BETWEEN @date1 AND @date2 ORDER BY TOPERIOD ASC";
                SqlDataAdapter command = new SqlDataAdapter(list, tblOTS);
                command.SelectCommand.Parameters.AddWithValue("@date1", from);
                command.SelectCommand.Parameters.AddWithValue("@date2", to);
                command.Fill(distinct);
                tblOTS.Close();
                foreach (DataRow item in distinct.Rows)
                {
                    bool checking = false;
                    if (dataGridView1.Rows.Count > 0)
                    {
                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            if (Convert.ToString(row.Cells["EMPCODE"].Value) != string.Empty)
                            {
                                if (row.Cells["EMPCODE"].Value.ToString() == item["EMPCODE"].ToString())
                                {
                                    checking = true;
                                    break;
                                }
                            }
                        }
                    }
                    if (checking == false)
                    {
                        int a = dataGridView1.Rows.Add();
                        dataGridView1.Rows[a].Cells["EMPCODE"].Value = item["EMPCODE"].ToString();
                        dataGridView1.Rows[a].Cells["NAME"].Value = item["EMPNAME"].ToString();
                    }
                }
                dataGridView1.Sort(dataGridView1.Columns["EMPCODE"], ListSortDirection.Ascending);
            }
            getdata();
        }
        private void getdata()
        {
            int numberofmonths = 0;
            DataTable listofmonths = new DataTable();
            DataTable totalmonth = new DataTable();
            totalmonth.Columns.Add("date", typeof(String));
            listofmonths.Columns.Add("date1", typeof(String));
            listofmonths.Columns.Add("date2", typeof(String));
            DateTime from = DateTime.Parse(maskedTextBox1.Text);
            DateTime to = DateTime.Parse(maskedTextBox2.Text);

            using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
            {
                dataGridView2.Rows.Clear();
                dataGridView2.Columns.Clear();
                dataGridView2.Columns.Add("dataGridViewTextBoxColumn1", "EMPCODE");
                dataGridView2.Columns.Add("dataGridViewTextBoxColumn2", "NAME");
                dataGridView3.Rows.Clear();
                dataGridView3.Columns.Clear();

                DataTable distinct = new DataTable();
                tblOTS.Open();
                string list = "SELECT DISTINCT TOPERIOD FROM tblStaffPayroll WHERE TOPERIOD BETWEEN @date1 AND @date2 ORDER BY TOPERIOD ASC";
                SqlDataAdapter command = new SqlDataAdapter(list, tblOTS);
                command.SelectCommand.Parameters.AddWithValue("@date1", from);
                command.SelectCommand.Parameters.AddWithValue("@date2", to);

                command.Fill(distinct);
                tblOTS.Close();
                foreach (DataRow item in distinct.Rows)
                {
                    bool check = false;
                    foreach (DataGridViewColumn col in dataGridView2.Columns)
                    {
                        if (DateTime.Parse(item["TOPERIOD"].ToString()).ToString("MM/yyyy") == col.Name)
                        {
                            check = true;
                        }
                    }
                    if (check == false)
                    {
                        dataGridView2.Columns.Add(DateTime.Parse(item["TOPERIOD"].ToString()).ToString("MM/yyyy"), DateTime.Parse(item["TOPERIOD"].ToString()).ToString("MM/yyyy"));
                        dataGridView3.Columns.Add(DateTime.Parse(item["TOPERIOD"].ToString()).ToString("MM/yyyy"), DateTime.Parse(item["TOPERIOD"].ToString()).ToString("MM/yyyy"));
                        int year = Convert.ToInt32(DateTime.Parse(item["TOPERIOD"].ToString()).ToString("yyyy"));
                        int month = Convert.ToInt32(DateTime.Parse(item["TOPERIOD"].ToString()).ToString("MM"));
                        int days = DateTime.DaysInMonth(year, month);
                        string fdate2 = month + "/" + days + "/" + year;
                        string fdate1 = month + "/01" + "/" + year;
                        object[] o = { DateTime.Parse(fdate1).ToString("MM/dd/yyyy"), DateTime.Parse(fdate2).ToString("MM/dd/yyyy") };
                        listofmonths.Rows.Add(o);
                        object[] w = { DateTime.Parse(item["TOPERIOD"].ToString()).ToString("MM/yyyy") };
                        totalmonth.Rows.Add(w);
                        numberofmonths++;
                    }
                }
                dataGridView2.Columns.Add("TOTALEARNINGS", "TOTALEARNINGS");
                dataGridView2.Columns.Add("AVERAGEEARNINGS", "AVERAGEEARNINGS");
                dataGridView2.Columns.Add("HALFEARNINGS", "1/2 OF A.E.");
            }

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                decimal totalearn = 0.00M;
                decimal AVERAGEearn = 0.00M;
                using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
                {

                    int a = dataGridView2.Rows.Add();
                    dataGridView2.Rows[a].Cells["dataGridViewTextBoxColumn1"].Value = row.Cells["EMPCODE"].Value.ToString();
                    dataGridView2.Rows[a].Cells["dataGridViewTextBoxColumn2"].Value = row.Cells["NAME"].Value.ToString();

                    foreach (DataRow rows in listofmonths.Rows)
                    {
                        decimal sumpermonths = 0.00M;
                        DataTable data = new DataTable();
                        tblOTS.Open();
                        string list = "SELECT EMPCODE,TOPERIOD,BASICSALARY,ALLOWANCE,OVERLEAVE FROM tblStaffPayroll WHERE EMPCODE = '" + row.Cells["EMPCODE"].Value.ToString() + "' AND TOPERIOD BETWEEN @date1 AND @date2";
                        SqlDataAdapter command = new SqlDataAdapter(list, tblOTS);
                        command.SelectCommand.Parameters.AddWithValue("@date1", DateTime.Parse(rows["date1"].ToString()).ToString("MM/dd/yyyy"));
                        command.SelectCommand.Parameters.AddWithValue("@date2", DateTime.Parse(rows["date2"].ToString()).ToString("MM/dd/yyyy"));
                        command.Fill(data);
                        tblOTS.Close();

                        foreach (DataRow datarow in data.Rows)
                        {
                            decimal sum = 0.00M;
                            if (datarow["OVERLEAVE"].ToString() != "")
                            {
                                if (Convert.ToDecimal(datarow["OVERLEAVE"].ToString()) >= 0.00M)
                                {
                                    sum += Convert.ToDecimal(datarow["BASICSALARY"].ToString()) + Convert.ToDecimal(datarow["ALLOWANCE"].ToString());
                                    sumpermonths += sum;
                                }
                                else
                                {
                                    sum += Convert.ToDecimal(datarow["BASICSALARY"].ToString()) + Convert.ToDecimal(datarow["ALLOWANCE"].ToString());
                                    decimal overleave = 0.00M;
                                    overleave = sum - Convert.ToDecimal(datarow["OVERLEAVE"].ToString().Replace("-", ""));
                                    sumpermonths += overleave;
                                }
                            }
                            else
                            {
                                sum += Convert.ToDecimal(datarow["BASICSALARY"].ToString()) + Convert.ToDecimal(datarow["ALLOWANCE"].ToString());
                                sumpermonths += sum;
                            }
                        }
                        dataGridView2.Rows[a].Cells[DateTime.Parse(rows["date1"].ToString()).ToString("MM/yyyy")].Value = string.Format("{0:#,##0.00}", sumpermonths);
                        totalearn += sumpermonths;
                    }
                    dataGridView2.Rows[a].Cells["TOTALEARNINGS"].Value = string.Format("{0:#,##0.00}", totalearn);

                    AVERAGEearn += totalearn / numberofmonths;
                    dataGridView2.Rows[a].Cells["AVERAGEEARNINGS"].Value = string.Format("{0:#,##0.00}", AVERAGEearn);
                    decimal half = 0.00M;
                    half += AVERAGEearn / 2;
                    dataGridView2.Rows[a].Cells["HALFEARNINGS"].Value = string.Format("{0:#,##0.00}", half);
                }
            }
            dataGridView3.Rows.Add();
            foreach (DataGridViewColumn row in dataGridView3.Columns)
            {
                decimal sum = 0.00M;
                foreach (DataGridViewRow dgvrow2 in dataGridView2.Rows)
                {
                    sum += Convert.ToDecimal(dgvrow2.Cells[row.Name].Value.ToString());
                }
                dataGridView3.Rows[0].Cells[row.Name].Value = string.Format("{0:#,##0.00}", sum);
            }
            decimal tearn = 0.00M;
            decimal aearn = 0.00M;
            decimal hearn = 0.00M;
            foreach (DataGridViewRow dgvrow2 in dataGridView2.Rows)
            {
                tearn += Convert.ToDecimal(dgvrow2.Cells["TOTALEARNINGS"].Value.ToString());
                aearn += Convert.ToDecimal(dgvrow2.Cells["AVERAGEEARNINGS"].Value.ToString());
                hearn += Convert.ToDecimal(dgvrow2.Cells["HALFEARNINGS"].Value.ToString());
            }
            label5.Text = string.Format("{0:#,##0.00}", tearn);
            label6.Text = string.Format("{0:#,##0.00}", aearn);
            label8.Text = string.Format("{0:#,##0.00}", hearn);

            this.dataGridView2.Columns["dataGridViewTextBoxColumn2"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridView2.Columns["TOTALEARNINGS"].Width = 120;
            this.dataGridView2.Columns["AVERAGEEARNINGS"].Width = 130;
            this.dataGridView2.Columns["HALFEARNINGS"].Width = 120;
        }
        public string id;
        private void staffEarnings_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Hide();
            payrollDashboard frm = new payrollDashboard();
            frm.id = id;
            frm.ShowDialog();
            this.Dispose();
        }
    }
}
