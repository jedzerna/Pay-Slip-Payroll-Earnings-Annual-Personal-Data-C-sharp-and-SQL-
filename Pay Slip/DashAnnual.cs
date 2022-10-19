using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;

namespace Pay_Slip
{
    public partial class DashAnnual : Form
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
        public DashAnnual()
        {
            InitializeComponent();
        }
        private payrollSettings data = null;
        public DashAnnual(Form callingForm)
        {
            data = callingForm as payrollSettings;
            InitializeComponent();
        }
        public string form;
        int populuation;
        private void DashAnnual_Load(object sender, EventArgs e)
        {
           
            SuspendLayout();
            Cursor.Current = Cursors.WaitCursor;
            annualprint.Columns.Add("ROW");
            annualprint.Columns.Add("DEBIT");
            annualprint.Columns.Add("CREDIT");

            annualgrandtotalprint.Columns.Add("ROW");
            annualgrandtotalprint.Columns.Add("DEBIT");
            annualgrandtotalprint.Columns.Add("CREDIT");

            annualtitleprint.Columns.Add("Title");

            //distinct.Columns.Add("DEBIT");
            //distinct.Columns.Add("CREDIT");
            //distinct.Columns.Add("FIRST3");
            //distinct.Columns.Add("DESCRIPTION");


          

            if (form == "DATA")
            {
                guna2Button2.Visible = false;
                data.dt.Columns.Add("DESCRIPTION");
                data.dt.Columns.Add("DEBIT", typeof(Decimal));
                data.dt.Columns.Add("CREDIT", typeof(Decimal));
                data.dt.Columns.Add("FIRST3");
                data.dt.Columns.Add("ID");
                data.dt.Columns.Add("YEAR", typeof(int));
                red = "";

                int a = 0;
                //List<string> colorsList = new List<string>();
                //guna2ComboBox1.Items.Add("Show All");
                foreach (DataRow row in data.dt.Rows)
                {
                    row["DATE"] = DateTime.Parse(row["DATE"].ToString()).ToString("MM/dd/yyyy");
                    if (DateTime.Parse(row["DATE"].ToString()).ToString("yyyy") != red)
                    {
                        a++;
                        guna2ComboBox1.Items.Add(DateTime.Parse(row["DATE"].ToString()).ToString("yyyy"));
                        red = DateTime.Parse(row["DATE"].ToString()).ToString("yyyy");

                    }
                    //colorsList.Add(DateTime.Parse(row["DATE"].ToString()).ToString("yyyy"));
                    //foreach (string item in colorsList)
                    //{
                    //    if (DateTime.Parse(row["DATE"].ToString()).ToString("yyyy") == item)
                    //    {
                    //        break;
                    //    }
                    //    else
                    //    {
                    //        colorsList.Add(DateTime.Parse(row["DATE"].ToString()).ToString("yyyy"));
                    //        guna2ComboBox1.Items.Add(DateTime.Parse(row["DATE"].ToString()).ToString("yyyy"));
                    //    }
                    //}
                }
                if (a != 1)
                {
                    label8.Visible = true;
                }
                else
                {
                    label8.Visible = false;
                }
                guna2ComboBox1.Text = red;
                //guna2ComboBox1.Text = DateTime.Now.Year.ToString();
                //loadfrmdate();
                if (a != 1)
                {
                    MessageBox.Show("DATA FOUND MORE THAN ONE OF A YEAR. PLEASE SELECT ONE TO CONTINUE...");
                    guna2Button6.Visible = true;
                }
                else
                {
                    thread =
                      new Thread(new ThreadStart(loadfrmdate));
                    thread.Start();
                    //guna2Button3.Visible = true;
                }
            }
            else
            {
                guna2Button2.Visible = true;
                using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
                {
                    DataTable distinct = new DataTable();
                    tblOTS.Open();
                    string list = "SELECT DISTINCT YEAR FROM tblGLU2N ORDER BY YEAR DESC";
                    SqlCommand command = new SqlCommand(list, tblOTS);
                    SqlDataReader reader = command.ExecuteReader();
                    distinct.Load(reader);
                    red = "";

                    guna2ComboBox1.Items.Add("Show All");

                    foreach (DataRow row in distinct.Rows)
                    {
                        //row["DATE"] = DateTime.Parse(row["DATE"].ToString()).ToString("MM/dd/yyyy");
                        //if (DateTime.Parse(row["DATE"].ToString()).ToString("yyyy") != red)
                        //{

                            guna2ComboBox1.Items.Add(row["YEAR"].ToString());

                            red = row["YEAR"].ToString();
                        //}
                    }
                    tblOTS.Close();
                }
                guna2ComboBox1.Text = DateTime.Now.Year.ToString();
                //load();
                //if (guna2ComboBox1.Text != "")
                //{
                load();
                //}
            }
            SetDoubleBuffer(dataGridView1, DoubleBuffered = true);
            ChangeControlStyles(dataGridView1, ControlStyles.OptimizedDoubleBuffer, true);
            this.dataGridView1.ColumnHeadersDefaultCellStyle.SelectionBackColor = this.dataGridView1.ColumnHeadersDefaultCellStyle.BackColor;
            dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;

            Cursor.Current = Cursors.Default;
            ResumeLayout();
        }
        string red;
        static void SetDoubleBuffer(Control ctl, bool DoubleBuffered)
        {
            typeof(Control).InvokeMember("DoubleBuffered",
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
                null, ctl, new object[] { DoubleBuffered });
        }
        Thread thread;
        private void ChangeControlStyles(Control ctrl, ControlStyles flag, bool value)
        {
            MethodInfo method = ctrl.GetType().GetMethod("SetStyle", BindingFlags.Instance | BindingFlags.NonPublic);
            if (method != null)
                method.Invoke(ctrl, new object[] { flag, value });
        }

        //SqlCommand command;
        public void loadfrmdate()
        {
          

            guna2Button3.BeginInvoke((Action)delegate ()
            {
                guna2Button6.Visible = false;
                guna2ComboBox1.Enabled = false;
                label6.Visible = true;
                guna2ProgressIndicator1.Visible = true;
                guna2ProgressIndicator1.Start();
            });

            label6.BeginInvoke((Action)delegate ()
            {
                label6.Text = "Loading... 0%";
            });

            label6.BeginInvoke((Action)delegate ()
            {
                label6.Text = "Loading... 5%";
            });


            decimal result = 0.00M;
            decimal add2 = 0.00M;
            int rows;

            label6.BeginInvoke((Action)delegate ()
            {
                label6.Text = "Loading... 8%";
            });


            int count = data.dt.Rows.Count;

            rows = count;

            result = 90 / (decimal)count;
            add2 += 0 + 10;


            decimal debit = 0.00M;
            decimal credit = 0.00M;

            label6.BeginInvoke((Action)delegate ()
            {
                label6.Text = "Loading... 8%";
            });

            int id = 0;
            string year = "";
            foreach (DataRow row in data.dt.Rows)
            {
              
                    row["YEAR"] = red;
              
                id++;
                row["ID"] = id.ToString();
                using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
                {
                    tblOTS.Open();
                    DataTable dt = new DataTable();
                    String query = "SELECT ACCTDESC FROM GLU4 WHERE ACCTCODE = '" + row["ACCTCODE"].ToString().Trim() + "'";
                    SqlCommand cmd = new SqlCommand(query, tblOTS);
                    SqlDataReader rdr = cmd.ExecuteReader();

                    if (rdr.Read())
                    {
                        row["DESCRIPTION"] = rdr["ACCTDESC"].ToString();
                    }
                    tblOTS.Close();
                }
                row["DATE"] = DateTime.Parse(row["DATE"].ToString()).ToString("MM/dd/yyyy");
                if (row["TYPE"].ToString() == "C")
                {
                    row["CREDIT"] = Convert.ToDecimal(row["AMOUNT"].ToString());
                    row["DEBIT"] = 0.00M;
                    credit += Convert.ToDecimal(row["AMOUNT"].ToString());

                }
                else
                {
                    row["CREDIT"] = 0.00M;
                    row["DEBIT"] = Convert.ToDecimal(row["AMOUNT"].ToString());
                    debit += Convert.ToDecimal(row["AMOUNT"].ToString());
                }
                if (row["ACCTCODE"].ToString().Length <=3 && row["ACCTCODE"].ToString() != "")
                {
                    row["FIRST3"] = row["ACCTCODE"].ToString();
                }else if (row["ACCTCODE"].ToString() == "")
                {
                    row["FIRST3"] = "";
                }
                else
                {
                    row["FIRST3"] = row["ACCTCODE"].ToString().Substring(0, 3);
                }

                add2 = add2 + result;
                label6.BeginInvoke((Action)delegate ()
                {
                    label6.Text = "Loading... " + Decimal.ToInt32(add2).ToString() + "%";
                });
            }
            guna2ProgressIndicator1.BeginInvoke((Action)delegate ()
            {
                label6.Text = "Please wait... 99%";
            });

            data.dt.DefaultView.Sort = "FIRST3 ASC";

            //foreach (DataRow row in data.dt.Rows)
            //{
               
            //}

            guna2ProgressIndicator1.BeginInvoke((Action)delegate ()
        {
            label4.Text = string.Format("{0:#,##0.00}", debit);
            label5.Text = string.Format("{0:#,##0.00}", credit);
            guna2ProgressIndicator1.Stop();

            label6.Visible = false;
            guna2ProgressIndicator1.Visible = false;

            label1.Text = "ROWS FOUND: " + dataGridView1.Rows.Count.ToString();
            dataGridView1.DataSource = data.dt;
            dataGridView1.ClearSelection();
            Cursor.Current = Cursors.Default;
            label6.Text = "Success... 100%"; 
            guna2Button3.Visible = true;
        });
            thread.Abort();

        }
        public DataTable distinct = new DataTable();

        public void load()
        {
            Cursor.Current = Cursors.WaitCursor;

            if (guna2ComboBox1.Text == "")
            {
                return;
            }

            using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
            {
              
                if (guna2ComboBox1.Text == "Show All")
                {
                    distinct.Rows.Clear();

                    tblOTS.Open();
                    string list = "SELECT * FROM tblGLU2N ORDER BY FIRST3,DATE,ACCTCODE ASC";
                    SqlCommand commands = new SqlCommand(list, tblOTS);
                    SqlDataReader reader = commands.ExecuteReader();
                    distinct.Load(reader);
                    tblOTS.Close();

                }
                else
                {
                    distinct.Rows.Clear();
                    tblOTS.Open();
                    string list = "SELECT * FROM tblGLU2N WHERE YEAR = '"+guna2ComboBox1.Text+ "' ORDER BY FIRST3,DATE,ACCTCODE ASC";

                    SqlCommand commands = new SqlCommand(list, tblOTS);
                    SqlDataReader reader = commands.ExecuteReader();
                    distinct.Load(reader);
                    tblOTS.Close();

                }


                //distinct.DefaultView.Sort = "FIRST3 ASC";
                dataGridView1.DataSource = distinct;
                //distinct.AcceptChanges();

                //decimal debit = 0.00M;
                //decimal credit = 0.00M;
                //foreach (DataGridViewRow row in dataGridView1.Rows)
                //{
                //    if (row.Cells["Column8"].Value == null || row.Cells["Column8"].Value == DBNull.Value || String.IsNullOrWhiteSpace(row.Cells["Column8"].Value.ToString()))
                //    {

                //    }
                //    else
                //    {
                //        if (row.Cells["Column8"].Value.ToString() != "-")
                //        {
                //            debit += Convert.ToDecimal(row.Cells["Column8"].Value.ToString());
                //        }
                //    }
                //    if (row.Cells["Column9"].Value == null || row.Cells["Column9"].Value == DBNull.Value || String.IsNullOrWhiteSpace(row.Cells["Column9"].Value.ToString()))
                //    {

                //    }
                //    else
                //    {
                //        if (row.Cells["Column9"].Value.ToString() != "-")
                //        {
                //            credit += Convert.ToDecimal(row.Cells["Column9"].Value.ToString());
                //        }
                //    }
                //}

                Decimal debit = Convert.ToDecimal(distinct.Compute("SUM(DEBIT)", string.Empty));
                Decimal credit = Convert.ToDecimal(distinct.Compute("SUM(CREDIT)", string.Empty));
                label4.Text = string.Format("{0:#,##0.00}", debit);
                label5.Text = string.Format("{0:#,##0.00}", credit);

                dataGridView1.ClearSelection();
                label1.Text = "ROWS FOUND: " + dataGridView1.Rows.Count.ToString();


                Cursor.Current = Cursors.Default;
            }
        }

        private void DashAnnual_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (thread != null)
            {
                thread.Abort();
            }
            if (form == "DATA")
            {

                this.Hide();
                payrollSettings frm = new payrollSettings();
                frm.id = id;
                frm.ShowDialog();
                this.Close();
            }
            else
            {
                this.Hide();
                payrollDashboard frm = new payrollDashboard();
                frm.id = id;
                frm.ShowDialog();
                this.Close();
            }
        }

        private void guna2ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (guna2ComboBox1.Text != "")
            {

                if (form == "DATA")
                {
                    red = guna2ComboBox1.Text;
                        //loadfrmdate();
                    
                }
                else
                {
                    if (guna2ComboBox1.Text == "Show All")
                    {
                        guna2Button1.Visible = false;
                    }
                    else
                    {
                        guna2Button1.Visible = true;
                    }
                    load();
                }

            }
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("No Data Found.");
                return;
            }
            if (guna2ComboBox1.Text == "")
            {
                MessageBox.Show("No Date Found");
                return;
            }
            //if (guna2TextBox1.Text == "")
            //{
            //    MessageBox.Show("Please put what year you want to save..");
            //    return;
            //}

            using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
            {

                tblOTS.Open();
                SqlCommand check_User_Name = new SqlCommand("SELECT COUNT(*) FROM [tblGLU2N] WHERE ([YEAR] = @YEAR)", tblOTS);
                check_User_Name.Parameters.AddWithValue("@YEAR", guna2ComboBox1.Text);
                int UserExist = (int)check_User_Name.ExecuteScalar();

                if (UserExist > 0)
                {
                    MessageBox.Show("Data already Added");
                    return;
                }

                tblOTS.Close();

            }
            Cursor.Current = Cursors.WaitCursor;
            data.dt.Columns.Remove("ID");
            //foreach (DataRow row in data.dt.Rows)
            //{
            //    row["YEAR"] = guna2TextBox1.Text;
            //}
            //data.dt.Columns.RemoveAt(columnIndex);
            using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
            {
                DataTableReader reader = data.dt.CreateDataReader();
                tblOTS.Open();  ///this is my connection to the sql server
                SqlBulkCopy sbc = new SqlBulkCopy(tblOTS);
                sbc.DestinationTableName = "tblGLU2N";  //copy the datatable to the sql table
                sbc.WriteToServer(data.dt);
                tblOTS.Close();
                reader.Close();
                MessageBox.Show("GLU2N IMPORTED SUCCESSFULLY");
            }
            Cursor.Current = Cursors.Default;
            this.Hide();
            payrollSettings frm = new payrollSettings();
            frm.id = id;
            frm.ShowDialog();
            this.Close();
        }

        public DataTable RemoveDuplicateRows(DataTable dTable, string colName)
        {
            Hashtable hTable = new Hashtable();
            ArrayList duplicateList = new ArrayList();

            //Add list of all the unique item value to hashtable, which stores combination of key, value pair.
            //And add duplicate item value in arraylist.
            foreach (DataRow drow in dTable.Rows)
            {
                if (hTable.Contains(drow[colName]))
                    duplicateList.Add(drow);
                else
                    hTable.Add(drow[colName], string.Empty);
            }

            //Removing a list of duplicate items from datatable.
            foreach (DataRow dRow in duplicateList)
                dTable.Rows.Remove(dRow);

            //Datatable which contains unique records will be return as output.
            return dTable;
        }
        public DataTable annualprint = new DataTable();
        public DataTable annualgrandtotalprint = new DataTable();
        public DataTable annualtitleprint = new DataTable();
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("No Data Found.");
                return;
            }
            if (guna2ComboBox1.Text == "")
            {
                MessageBox.Show("No Date Found");
                return;
            }
            Cursor.Current = Cursors.WaitCursor;
            annualprint.Rows.Clear();
            annualgrandtotalprint.Rows.Clear();
            annualtitleprint.Rows.Clear();
            foreach (System.Data.DataColumn col in annualprint.Columns) col.ReadOnly = false;
            string first3 = "";
            //decimal debit = 0.00M;
            //decimal credit = 0.00M;


            //MessageBox.Show(TotalPrice.ToString());
            decimal debit = 0.00M;
            decimal credit = 0.00M;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["Column10"].Value.ToString() != first3 || first3 == "")
                {
                    if (form == "DATA")
                    {
                        debit = Convert.ToDecimal(data.dt.Compute("SUM(DEBIT)", "FIRST3 = " + row.Cells["Column10"].Value.ToString()));
                        credit = Convert.ToDecimal(data.dt.Compute("SUM(CREDIT)", "FIRST3 = " + row.Cells["Column10"].Value.ToString()));
                    }
                    else
                    {
                        using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
                        {
                            tblOTS.Open();
                            SqlCommand sumdebit = new SqlCommand("SELECT SUM (DEBIT) FROM tblGLU2N WHERE FIRST3 = '"+ row.Cells["Column10"].Value.ToString() + "' AND YEAR='"+guna2ComboBox1.Text+"'", tblOTS);
                            object result = sumdebit.ExecuteScalar();
                            debit = Convert.ToDecimal(result);
                            tblOTS.Close();

                        }
                        using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
                        {
                            tblOTS.Open();
                            SqlCommand sumdebit = new SqlCommand("SELECT SUM (CREDIT) FROM tblGLU2N WHERE FIRST3 = '" + row.Cells["Column10"].Value.ToString() + "' AND YEAR='" + guna2ComboBox1.Text + "'", tblOTS);
                            object result = sumdebit.ExecuteScalar();
                            credit = Convert.ToDecimal(result);
                            tblOTS.Close();

                        }

                        //debit = Convert.ToDecimal(distinct.Compute("SUM(DEBIT)", "FIRST3 = " + row.Cells["Column10"].Value.ToString()));
                        //credit = Convert.ToDecimal(distinct.Compute("SUM(CREDIT)", "FIRST3 = " + row.Cells["Column10"].Value.ToString()));
                    }


                    string sdebit = string.Format("{0:#,##0.00}", debit);
                    string scredit = string.Format("{0:#,##0.00}", credit);
                    if (sdebit == "0.00" || sdebit == "")
                    {
                        sdebit = "           -";
                    }
                    if (scredit == "0.00" || scredit == "")
                    {
                        scredit = "           -";
                    }

                    object[] o = { row.Cells["Column10"].Value.ToString(), sdebit, scredit }; 
                    annualprint.Rows.Add(o);
                }
                //if (row.Cells["Column10"].Value.ToString() != first3)
                //{
                //    object[] o = { first3, debit, credit };
                //    annualprint.Rows.Add(o);
                //    //debit = 0.00M;
                //    //credit = 0.00M;
                //    debit = Convert.ToDecimal(row.Cells["Column8"].Value.ToString());
                //    credit = Convert.ToDecimal(row.Cells["Column9"].Value.ToString());

                //    if (debit == 0.00M)
                //    {
                //        debit = "-";
                //    }
                //    else
                //    {
                //        debit = Convert.ToDecimal(row.Cells["Column8"].Value.ToString());
                //    }
                //    if (credit == 0.00M)
                //    {
                //        credit = "-";
                //    }
                //    else
                //    {
                //        credit = Convert.ToDecimal(row.Cells["Column9"].Value.ToString());
                //    }
                //}
                //if (row.Cells["Column10"].Value.ToString() == first3)
                //{
                //    if (row.Cells["Column8"].Value.ToString() != "")
                //    {
                //        debit += Convert.ToDecimal(row.Cells["Column8"].Value.ToString());
                //    }
                //    if (row.Cells["Column9"].Value.ToString() != "")
                //    {
                //        credit += Convert.ToDecimal(row.Cells["Column9"].Value.ToString());
                //    }

                //}

                first3 = row.Cells["Column10"].Value.ToString();


            }

            object[] b = { "Grand Total", label4.Text, label5.Text };
            annualgrandtotalprint.Rows.Add(b);
            if (guna2ComboBox1.Text == "Show All")
            {
                object[] c = { "ANNUAL SUMMARY" };
                annualtitleprint.Rows.Add(c);
            }
            else
            {
                object[] c = { "SUMMARY OF ANNUAL " + guna2ComboBox1.Text };
                annualtitleprint.Rows.Add(c);
            }
            AnnualPrint A = new AnnualPrint(this);
            A.ShowDialog();
            Cursor.Current = Cursors.Default;
        }
        private void start1()
        {

            //reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);
            

            //string exeFolder = Path.GetDirectoryName(Application.ExecutablePath) + @"\DTRVIEW.rdlc";
            //ReportDataSource dr = new ReportDataSource("DataSet1", WORKERSDATA());
            //this.reportViewer1.LocalReport.ReportPath = exeFolder;
            //this.reportViewer1.LocalReport.DataSources.Add(dr);
          
            //var deviceInfo = @"<DeviceInfo>
            //        <EmbedFonts>None</EmbedFonts>
            //       </DeviceInfo>";

            ////byte[] bytes = rdlc.Render("PDF", deviceInfo);

            //byte[] Bytes = reportViewer1.LocalReport.Render(format: "PDF", deviceInfo);
            //DateTime fadate = DateTime.Now;
            //string ffdate = fadate.ToString("MM-dd-yyyy");
            //using (FileStream stream = new FileStream(tb1 + "\\Biometric " + ffdate.ToString() + ".pdf", FileMode.Create))
            //{
            //    stream.Write(Bytes, 0, Bytes.Length);
            //}
            //filename = "Biometric " + ffdate.ToString() + ".pdf";
            Cursor.Current = Cursors.Default;
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            guna2Button5.Visible = true;
            guna2Button2.Visible = false;
            dataGridView1.Columns["Column12"].Visible = true;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            var senderGrid = (DataGridView)sender;
            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0)
            {
                if (form == "DATA")
                {

                    //string find = "ID = '" + dataGridView1.CurrentRow.Cells["Column11"].Value.ToString() + "'";
                    ////find out id  
                    //DataRow[] row = data.dt.Select(find);
                    ////update row  
                    ////row[0]["SALARY"] = 25000;
                    ////Accept Changes  

                    ////DataRow row = data.dt.Select(dataGridView1.CurrentRow.Cells["Column11"].Value.ToString() + " =ID").FirstOrDefault();
                    ////row["Product_name"] = "cde";
                    //row[0]["VOUCHER"] = dataGridView1.CurrentRow.Cells["Column1"].Value.ToString();
                    //row[0]["NUM"] = dataGridView1.CurrentRow.Cells["Column2"].Value.ToString();
                    //row[0]["DATE"] = dataGridView1.CurrentRow.Cells["Column3"].Value.ToString();
                    //row[0]["ACCTCODE"] = dataGridView1.CurrentRow.Cells["Column4"].Value.ToString();
                    //row[0]["AMOUNT"] = dataGridView1.CurrentRow.Cells["Column5"].Value.ToString();
                    //row[0]["TYPE"] = dataGridView1.CurrentRow.Cells["Column6"].Value.ToString();
                    //row[0]["DESCRIPTION"] = dataGridView1.CurrentRow.Cells["Column7"].Value.ToString();
                    //row[0]["DEBIT"] = dataGridView1.CurrentRow.Cells["Column8"].Value.ToString();
                    //row[0]["CREDIT"] = dataGridView1.CurrentRow.Cells["Column9"].Value.ToString();
                    ////row["FIRST3"] = dataGridView1.CurrentRow.Cells["Column10"].Value.ToString();
                    //if (dataGridView1.CurrentRow.Cells["Column10"].Value.ToString().Length <= 3 && dataGridView1.CurrentRow.Cells["Column10"].Value.ToString() != "")
                    //{
                    //    row[0]["FIRST3"] = dataGridView1.CurrentRow.Cells["Column10"].Value.ToString();
                    //}
                    //else if (dataGridView1.CurrentRow.Cells["Column10"].Value.ToString() == "")
                    //{
                    //    row[0]["FIRST3"] = "";
                    //}
                    //else
                    //{
                    //    row[0]["FIRST3"] = dataGridView1.CurrentRow.Cells["Column10"].Value.ToString().Substring(0, 3);
                    //}
                    //data.dt.AcceptChanges();
                    //MessageBox.Show(row[0]["FIRST3"].ToString());

                    //foreach (DataRow row in data.dt.Rows)
                    //{
                    //    if (dataGridView1.CurrentRow.Cells["Column11"].Value.ToString() == row["ID"].ToString())
                    //    {
                    //        row["VOUCHER"] = dataGridView1.CurrentRow.Cells["Column1"].Value.ToString();
                    //        row["NUM"] = dataGridView1.CurrentRow.Cells["Column2"].Value.ToString();
                    //        row["DATE"] = dataGridView1.CurrentRow.Cells["Column3"].Value.ToString();
                    //        row["ACCTCODE"] = dataGridView1.CurrentRow.Cells["Column4"].Value.ToString();
                    //        row["AMOUNT"] = dataGridView1.CurrentRow.Cells["Column5"].Value.ToString();
                    //        row["TYPE"] = dataGridView1.CurrentRow.Cells["Column6"].Value.ToString();
                    //        row["DESCRIPTION"] = dataGridView1.CurrentRow.Cells["Column7"].Value.ToString();
                    //        row["DEBIT"] = dataGridView1.CurrentRow.Cells["Column8"].Value.ToString();
                    //        row["CREDIT"] = dataGridView1.CurrentRow.Cells["Column9"].Value.ToString();
                    //        //row["FIRST3"] = dataGridView1.CurrentRow.Cells["Column10"].Value.ToString();
                    //        if (dataGridView1.CurrentRow.Cells["Column10"].Value.ToString().Length <= 3 && dataGridView1.CurrentRow.Cells["Column10"].Value.ToString() != "")
                    //        {
                    //            row["FIRST3"] = dataGridView1.CurrentRow.Cells["Column10"].Value.ToString();
                    //        }
                    //        else if (dataGridView1.CurrentRow.Cells["Column10"].Value.ToString() == "")
                    //        {
                    //            row["FIRST3"] = "";
                    //        }
                    //        else
                    //        {
                    //            row["FIRST3"] = dataGridView1.CurrentRow.Cells["Column10"].Value.ToString().Substring(0, 3);
                    //        }
                    //        MessageBox.Show(row["FIRST3"].ToString());
                    //        break;
                    //    }
                    //    data.dt.AcceptChanges();
                    //}
                    //MessageBox.Show("Done");

                }
                else
                {
                    AnnualRowEditing a = new AnnualRowEditing(this);
                    a.id = dataGridView1.CurrentRow.Cells["Column11"].Value.ToString();
                    a.ShowDialog();
                    //if (dataGridView1.CurrentRow.Cells["Column4"].Value.ToString() == "" || dataGridView1.CurrentRow.Cells["Column4"].Value.ToString().Length < 3)
                    //{
                    //    MessageBox.Show("Code is invalid");
                    //    return;
                    //}
                    //if (dataGridView1.CurrentRow.Cells["Column3"].Value.ToString() == "")
                    //{
                    //    MessageBox.Show("Date is invalid");
                    //    return;
                    //}
                    ////using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
                    ////{
                    ////    SqlCommand cmd3 = new SqlCommand("update tblGLU2N set DATE=@DATE,ACCTCODE=@ACCTCODE,DESCRIPTION=@DESCRIPTION where ID=@ID", tblOTS);
                    ////    tblOTS.Open();
                    ////    cmd3.Parameters.AddWithValue("@DATE", dataGridView1.CurrentRow.Cells["Column3"].Value.ToString());
                    ////    cmd3.Parameters.AddWithValue("@ACCTCODE", dataGridView1.CurrentRow.Cells["Column4"].Value.ToString());
                    ////    cmd3.Parameters.AddWithValue("@DESCRIPTION", dataGridView1.CurrentRow.Cells["Column7"].Value.ToString());
                    ////    cmd3.ExecuteNonQuery();
                    ////    tblOTS.Close();
                    ////}
                    //MessageBox.Show("Saved");
                }
            }
            Cursor.Current = Cursors.Default;
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            guna2Button5.Visible = false;
            guna2Button2.Visible = true;
            dataGridView1.Columns["Column12"].Visible = false;
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            if (form == "DATA")
            {
               
            }
            else
            {

                //foreach (DataGridViewRow dgvrow in dataGridView1.Rows)
                //{
                //    foreach (DataRow row in data.dt.Rows)
                //    {
                //        if (dgvrow.Cells["Column11"].Value.ToString() == row["ID"].ToString())
                //        {
                //            row["VOUCHER"] = dgvrow.Cells["Column1"].Value.ToString();
                //            row["NUM"] = dgvrow.Cells["Column2"].Value.ToString();
                //            row["DATE"] = dgvrow.Cells["Column3"].Value.ToString();
                //            row["ACCTCODE"] = dgvrow.Cells["Column4"].Value.ToString();
                //            row["AMOUNT"] = dgvrow.Cells["Column5"].Value.ToString();
                //            row["TYPE"] = dgvrow.Cells["Column6"].Value.ToString();
                //            row["DESCRIPTION"] = dgvrow.Cells["Column7"].Value.ToString();
                //            row["DEBIT"] = dgvrow.Cells["Column8"].Value.ToString();
                //            row["CREDIT"] = dgvrow.Cells["Column9"].Value.ToString();
                //            row["FIRST3"] = dgvrow.Cells["Column10"].Value.ToString();
                //            break;
                //        }
                //    }
                //}
            }
            guna2Button5.Visible = false;
            guna2Button2.Visible = true;
            dataGridView1.Columns["Column12"].Visible = false;
            MessageBox.Show("Done");
        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {
            if (guna2ComboBox1.Text != "")
            {
                thread =
                  new Thread(new ThreadStart(loadfrmdate));
                thread.Start();
                guna2Button3.Visible = true;
            }
        }

        private void dataGridView1_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
          
        }

        private void dataGridView1_CellParsing(object sender, DataGridViewCellParsingEventArgs e)
        {
          
        }
    }
}
