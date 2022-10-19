using System;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using ExcelApp = Microsoft.Office.Interop.Excel;

namespace Pay_Slip
{
    public partial class workerspayslip : Form
    {
        private const int WM_NCHITTEST = 0x84;
        private const int WS_MINIMIZEBOX = 0x20000;
        private const int HTCLIENT = 0x1;
        private const int HTCAPTION = 0x2;
        private const int CS_DBLCLKS = 0x8;
        private const int CS_DROPSHADOW = 0x00020000;
        private const int WM_NCPAINT = 0x0085;
        private const int WM_ACTIVATEAPP = 0x001C;



        [EditorBrowsable(EditorBrowsableState.Never)]
        public struct MARGINS
        {
            public int leftWidth;
            public int rightWidth;
            public int topHeight;
            public int bottomHeight;
        }



        [DllImport("dwmapi.dll")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS pMarInset);

        [DllImport("dwmapi.dll")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        [DllImport("dwmapi.dll")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static extern int DwmIsCompositionEnabled(ref int pfEnabled);

        [EditorBrowsable(EditorBrowsableState.Never)]
        public static bool IsCompositionEnabled()
        {
            if (Environment.OSVersion.Version.Major < 6) return false;

            bool enabled;
            DwmIsCompositionEnabled(out enabled);

            return enabled;
        }



        [DllImport("dwmapi.dll")]
        private static extern int DwmIsCompositionEnabled(out bool enabled);

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect,
            int nTopRect,
            int nRightRect,
            int nBottomRect,
            int nWidthEllipse,
            int nHeightEllipse
         );

        private bool CheckIfAeroIsEnabled()
        {
            if (Environment.OSVersion.Version.Major >= 6)
            {
                int enabled = 0;
                DwmIsCompositionEnabled(ref enabled);

                return (enabled == 1) ? true : false;
            }
            return false;
        }


        public void ApplyShadows(Form form)
        {
            var v = 2;

            DwmSetWindowAttribute(form.Handle, 2, ref v, 4);

            MARGINS margins = new MARGINS()
            {
                bottomHeight = 1,
                leftWidth = 0,
                rightWidth = 0,
                topHeight = 0
            };

            DwmExtendFrameIntoClientArea(form.Handle, ref margins);
        }
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
        public workerspayslip()
        {
            InitializeComponent();
            ApplyShadows(this);
            this.FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 15, 15));
        }
        private void ChangeControlStyles(Control ctrl, ControlStyles flag, bool value)
        {
            MethodInfo method = ctrl.GetType().GetMethod("SetStyle", BindingFlags.Instance | BindingFlags.NonPublic);
            if (method != null)
                method.Invoke(ctrl, new object[] { flag, value });
        }
        private void workerspayslip_Load(object sender, EventArgs e)
        {
            SuspendLayout();
            ChangeControlStyles(dataGridView1, ControlStyles.OptimizedDoubleBuffer, true);
            this.dataGridView1.ColumnHeadersDefaultCellStyle.SelectionBackColor = this.dataGridView1.ColumnHeadersDefaultCellStyle.BackColor;
            dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
            dataGridView1.RowHeadersVisible = false;
            //dataGridView1.Visible = false;
            //load();
            guna2DateTimePicker1.Value = DateTime.Now;
            perglu.Columns.Add("SOURCE");
            perglu.Columns.Add("DATE");
            perglu.Columns.Add("EMPCODE");
            perglu.Columns.Add("AMOUNT");
            perglu.Columns.Add("KIND");
            ResumeLayout();
        }

        public DataTable ReadExcel(string fileName, string fileExt)
        {
            string conn = string.Empty;
            DataTable dtexcel = new DataTable();
            //if (fileExt.CompareTo(".DBF") == 0)
                conn = @"provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileName + ";Extended Properties=dBase IV;"; //for below excel 2007  
            //else
            //    conn = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileName + ";Extended Properties=dBase IV;"; //for above excel 2007  
            using (OleDbConnection con = new OleDbConnection(conn))
            {
                try
                {
                    OleDbDataAdapter oleAdpt = new OleDbDataAdapter("select * from GLU4.DBF", con); //here we read data from sheet1  
                    oleAdpt.Fill(dtexcel); //fill excel data into dataTable  
                }
                catch { }
            }
            return dtexcel;
        }
        private void glu()
        {
            DataTable YourResultSet = new DataTable();

            OleDbConnection yourConnectionHandler = new OleDbConnection(
                @"Provider=VFPOLEDB.1;Data Source=C:\Users\PC1\Documents\Visual FoxPro Projects\");

            // if including the full dbc (database container) reference, just tack that on
            //      OleDbConnection yourConnectionHandler = new OleDbConnection(
            //          "Provider=VFPOLEDB.1;Data Source=C:\\SomePath\\NameOfYour.dbc;" );


            // Open the connection, and if open successfully, you can try to query it
            yourConnectionHandler.Open();

            if (yourConnectionHandler.State == ConnectionState.Open)
            {
                string mySQL = "select * from GLU4";  // dbf table name
                OleDbCommand MyQuery = new OleDbCommand(mySQL, yourConnectionHandler);
                OleDbDataAdapter DA = new OleDbDataAdapter(MyQuery);
                DA.Fill(YourResultSet);
                yourConnectionHandler.Close();
            }

        }
        public DataTable perglu = new DataTable();
        public DataTable perpay = new DataTable();
        public void submit()
        {
            DateTime datee = DateTime.Parse(guna2DateTimePicker1.Value.ToString());
            string fdate = datee.ToString("M/d/yyyy");
            label1.BeginInvoke((Action)delegate ()
            {
                label1.Visible = true;
            });
            guna2ProgressBar1.BeginInvoke((Action)delegate ()
            {
                guna2ProgressBar1.Visible = true;
            });
            guna2ProgressBar1.BeginInvoke((Action)delegate ()
            {
                guna2ProgressBar1.Value = 3;
            });
            using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
            {

                //string sqlTrunc = "TRUNCATE TABLE tblFinalWorkers";
                //SqlCommand cmd = new SqlCommand(sqlTrunc, tblOTS);
                //tblOTS.Open();
                //cmd.ExecuteNonQuery();
                //tblOTS.Close();

            }
            guna2ProgressBar1.BeginInvoke((Action)delegate ()
            {
                guna2ProgressBar1.Value = 6;
            });
            using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
            {
                string sqlTrunc = "TRUNCATE TABLE GLU4";
                SqlCommand cmd = new SqlCommand(sqlTrunc, tblOTS);
                tblOTS.Open();
                cmd.ExecuteNonQuery();
                tblOTS.Close();
                //try
                //{

                //    DataTable dt = new DataTable();
                //    OleDbConnection oConn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + guna2TextBox1.Text + "';Extended Properties=dBase IV;");
                //    OleDbCommand command = new OleDbCommand("select * from GLU4.DBF", oConn);

                //    oConn.Open();
                //    dt.Load(command.ExecuteReader());
                //    oConn.Close();

                //    DataTableReader reader = dt.CreateDataReader();
                //    tblOTS.Open();  ///this is my connection to the sql server
                //    SqlBulkCopy sqlcpy = new SqlBulkCopy(tblOTS);
                //    sqlcpy.DestinationTableName = "GLU4";  //copy the datatable to the sql table
                //    sqlcpy.WriteToServer(dt);
                //    tblOTS.Close();
                //    reader.Close();
                //}
                //catch
                //{
                //    DataTable dtExcel = new DataTable();
                //    //OleDbDataAdapter oleAdpt = new OleDbDataAdapter("select * from [GLU4$]", con);
                //    string filePath = string.Empty;
                //    string fileExt = string.Empty;

                //    filePath = guna2TextBox1.Text + "\\GLU4.DBF"; //get the path of the file  
                //    fileExt = Path.GetExtension(filePath); //get the file extension  
                //    if (fileExt.CompareTo(".DBF") == 0 || fileExt.CompareTo(".DBF") == 0)
                //    {
                //        try
                //        {
                //            dtExcel = ReadExcel(filePath, fileExt); //read excel file  
                //            //dataGridView1.Visible = true;
                //            //dataGridView1.DataSource = dtExcel;
                //        }
                //        catch (Exception ex)
                //        {
                //            MessageBox.Show(ex.Message.ToString());
                //        }
                      
                //    }
                //    else
                //    {
                //        MessageBox.Show("Please choose .DBF or .DBF file only.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error); //custom messageBox to show error  
                //    }
                ////}



                DataTable YourResultSet = new DataTable();

                OleDbConnection yourConnectionHandler = new OleDbConnection(
                    @"Provider=VFPOLEDB.1;Data Source='" + guna2TextBox1.Text + "'");

                yourConnectionHandler.Open();

                if (yourConnectionHandler.State == ConnectionState.Open)
                {
                    string mySQL = "select * from GLU4";  // dbf table name

                    OleDbCommand MyQuery = new OleDbCommand(mySQL, yourConnectionHandler);
                    OleDbDataAdapter DA = new OleDbDataAdapter(MyQuery);

                    DA.Fill(YourResultSet);

                    yourConnectionHandler.Close();
                }

                foreach (DataRow row in YourResultSet.Rows)
                {
                    row["ACCTCODE"] = row["ACCTCODE"].ToString().Trim();
                }
                if (YourResultSet.Rows.Count == 0)
                {
                    MessageBox.Show("No GLU4 data found.");
                    thread.Abort();
                }
                DataTableReader reader = YourResultSet.CreateDataReader();
                tblOTS.Open();  ///this is my connection to the sql server
                SqlBulkCopy sqlcpy = new SqlBulkCopy(tblOTS);
                sqlcpy.DestinationTableName = "GLU4";  //copy the datatable to the sql table
                sqlcpy.WriteToServer(YourResultSet);
                tblOTS.Close();
                reader.Close();



            }
            guna2ProgressBar1.BeginInvoke((Action)delegate ()
            {
                guna2ProgressBar1.Value = 12;
            });
            using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
            {
                string sqlTrunc = "TRUNCATE TABLE LEOGLU";
                SqlCommand cmd = new SqlCommand(sqlTrunc, tblOTS);
                tblOTS.Open();
                cmd.ExecuteNonQuery();
                tblOTS.Close();

                guna2ProgressBar1.BeginInvoke((Action)delegate ()
                {
                    guna2ProgressBar1.Value = 15;
                });
                OleDbConnection oConn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + guna2TextBox1.Text + "';Extended Properties=dBase IV;");
                OleDbCommand command = new OleDbCommand("select * from PERGLU.DBF", oConn);
                DataTable dt = new DataTable();
                oConn.Open();
                dt.Load(command.ExecuteReader());
                oConn.Close();

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("No PERGLU data found.");
                    thread.Abort();
                }

                DataTableReader reader = dt.CreateDataReader();
                tblOTS.Open();  ///this is my connection to the sql server
                SqlBulkCopy sqlcpy = new SqlBulkCopy(tblOTS);
                sqlcpy.DestinationTableName = "LEOGLU";  //copy the datatable to the sql table
                sqlcpy.WriteToServer(dt);
                tblOTS.Close();
                reader.Close();

                

                tblOTS.Open();
                DataTable dts = new DataTable();
                string list = "Select * from LEOGLU WHERE DATE = '" + fdate + "'";
                SqlCommand acommand = new SqlCommand(list, tblOTS);
                SqlDataReader areader = acommand.ExecuteReader();
                dts.Load(areader);
                tblOTS.Close();

                perglu.Rows.Clear();
                foreach (DataRow row in dts.Rows)
                {
                    DateTime aatee = DateTime.Parse(guna2DateTimePicker1.Value.ToString());
                    string adate = aatee.ToString("MM/dd/yyyy");

                    if (adate == DateTime.Parse(row["DATE"].ToString()).ToString("MM/dd/yyyy"))
                    {
                        //MessageBox.Show(row["SOURCE"].ToString().Trim());
                        tblOTS.Open();
                        SqlCommand check_User_Name = new SqlCommand("SELECT COUNT(*) FROM [GLU4] WHERE ([ACCTCODE] = @ACCTCODE)", tblOTS);
                        check_User_Name.Parameters.AddWithValue("@ACCTCODE", row["SOURCE"].ToString().Trim());
                        int UserExist = (int)check_User_Name.ExecuteScalar();

                        if (UserExist > 0)
                        {

                        }
                        else
                        {
                            //perglu.ImportRow(row);
                            object[] o = { row["SOURCE"].ToString(), row["DATE"].ToString(), row["EMPCODE"].ToString(), row["AMOUNT"].ToString(), row["KIND"].ToString() };
                            perglu.Rows.Add(o);
                        }
                        perglu.AcceptChanges();
                        tblOTS.Close();

                    }

                }



                guna2ProgressBar1.BeginInvoke((Action)delegate ()
                {
                    guna2ProgressBar1.Value = 18;
                });
            }
            using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
            {
                string sqlTrunc = "TRUNCATE TABLE tblPERID";
                SqlCommand cmd = new SqlCommand(sqlTrunc, tblOTS);
                tblOTS.Open();
                cmd.ExecuteNonQuery();
                tblOTS.Close();

                guna2ProgressBar1.BeginInvoke((Action)delegate ()
                {
                    guna2ProgressBar1.Value = 21;
                });

                OleDbConnection oConn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + guna2TextBox1.Text + "';Extended Properties=dBase IV;");
                OleDbCommand command = new OleDbCommand("select * from PERID.DBF", oConn);
                DataTable dt = new DataTable();
                oConn.Open();
                dt.Load(command.ExecuteReader());
                oConn.Close();

                guna2ProgressBar1.BeginInvoke((Action)delegate ()
                {
                    guna2ProgressBar1.Value = 24;
                });


                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("No PERID data found.");
                    thread.Abort();
                }


                DataTableReader reader = dt.CreateDataReader();
                tblOTS.Open();  ///this is my connection to the sql server
                SqlBulkCopy sqlcpy = new SqlBulkCopy(tblOTS);
                sqlcpy.DestinationTableName = "tblPERID";  //copy the datatable to the sql table
                sqlcpy.WriteToServer(dt);
                tblOTS.Close();
                reader.Close();

                guna2ProgressBar1.BeginInvoke((Action)delegate ()
                {
                    guna2ProgressBar1.Value = 27;
                });
            }
            using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
            {
                guna2ProgressBar1.BeginInvoke((Action)delegate ()
                {
                    guna2ProgressBar1.Value = 30;
                });
                string sqlTrunc = "TRUNCATE TABLE LEOPAY";
                SqlCommand cmd = new SqlCommand(sqlTrunc, tblOTS);
                tblOTS.Open();
                cmd.ExecuteNonQuery();
                tblOTS.Close();

                guna2ProgressBar1.BeginInvoke((Action)delegate ()
                {
                    guna2ProgressBar1.Value = 33;
                });


                OleDbConnection oConn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + guna2TextBox1.Text + "';Extended Properties=dBase IV;");
                OleDbCommand command = new OleDbCommand("select * from PERPAY.DBF", oConn);
                DataTable dt = new DataTable();
                oConn.Open();
                dt.Load(command.ExecuteReader());
                oConn.Close();


                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("No PERPAY data found.");
                    thread.Abort();
                }

                DataTableReader reader = dt.CreateDataReader();
                tblOTS.Open();  ///this is my connection to the sql server
                SqlBulkCopy sqlcpy = new SqlBulkCopy(tblOTS);
                sqlcpy.DestinationTableName = "LEOPAY";  //copy the datatable to the sql table
                sqlcpy.WriteToServer(dt);
                tblOTS.Close();
                reader.Close();
            }
            using (SqlConnection tblOTS1 = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
            {
                tblOTS1.Open();
                SqlCommand check_User_Name = new SqlCommand("SELECT COUNT(*) FROM [LEOPAY] WHERE ([PERIOD] = @PERIOD)", tblOTS1);
                check_User_Name.Parameters.AddWithValue("@PERIOD", fdate);
                int UserExist = (int)check_User_Name.ExecuteScalar();

                if (UserExist > 0)
                {
                    tblOTS1.Close();
                    using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
                    {
                        guna2ProgressBar1.BeginInvoke((Action)delegate ()
                        {
                            guna2ProgressBar1.Value = 36;
                        });
                        tblOTS.Open();
                        DataTable dt = new DataTable();
                        using (SqlDataAdapter a2 = new SqlDataAdapter("SELECT * FROM LEOPAY WHERE PERIOD = '" + fdate + "' ORDER BY EMPCODE ASC", tblOTS))
                            a2.Fill(dt);
                        BindingSource bs = new BindingSource();
                        bs.DataSource = dt;
                        setDataSource(dt);

                        tblOTS.Close();
                    }
                    using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
                    {
                        guna2ProgressBar1.BeginInvoke((Action)delegate ()
                        {
                            guna2ProgressBar1.Value = 36;
                        });
                        tblOTS.Open();
                        DataTable dt = new DataTable();
                        using (SqlDataAdapter a2 = new SqlDataAdapter("SELECT * FROM LEOGLU WHERE DATE = '" + fdate + "'", tblOTS))
                            a2.Fill(dt);
                        BindingSource bs = new BindingSource();
                        bs.DataSource = dt;
                        setDataSource1(dt);

                        tblOTS.Close();

                        if (dt.Rows.Count == 0)
                        {
                            MessageBox.Show("No PERGLU data found.");
                            thread.Abort();
                        }
                    }
                    foreach (DataGridViewRow dg2 in dataGridView2.Rows)
                    {
                        dg2.Cells[0].Value = Regex.Replace(dg2.Cells[0].Value.ToString(), "[^0-9.]", "");
                    }
                    decimal result = 0.00M;
                    decimal add2 = 0.00M;

                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
                    {
                        SqlCommand cmd9 = new SqlCommand("SELECT COUNT(*) FROM LEOPAY WHERE PERIOD = '" + fdate + "'", con);
                        con.Open();
                        Int32 count = (Int32)cmd9.ExecuteScalar();
                        con.Close();

                        result = 64 / (decimal)count;
                        add2 += result + 36;
                    }
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
                        {
                            string val = row.Cells[9].Value.ToString() + "  ";

                            tblOTS.Open();
                            String query2 = "SELECT FIRST,MIDDLE,FAMILY FROM tblPERID WHERE EMPCODE = '" + val + "'";
                            SqlCommand cmd2 = new SqlCommand(query2, tblOTS);
                            SqlDataReader rdr2 = cmd2.ExecuteReader();

                            if (rdr2.Read())
                            {
                                row.Cells[0].Value = rdr2["FAMILY"].ToString().Replace("¥", "Ñ").Trim() + ", " + rdr2["FIRST"].ToString().Replace("¥", "Ñ").Trim() + " " + rdr2["MIDDLE"].ToString().Replace("¥", "Ñ").Trim();

                            }
                            else
                            {
                                row.Cells[0].Value = "";
                                row.Cells[0].Style.BackColor = Color.Red;
                            }
                            tblOTS.Close();

                            tblOTS.Open();
                            String query3 = "SELECT ACCTDESC FROM GLU4 WHERE ACCTCODE = '" + row.Cells[13].Value.ToString().Trim() + "'";
                            SqlCommand cmd3 = new SqlCommand(query3, tblOTS);
                            SqlDataReader rdr3 = cmd3.ExecuteReader();

                            if (rdr3.Read())
                            {
                                row.Cells[1].Value = rdr3["ACCTDESC"].ToString();
                            }
                            tblOTS.Close();

                            foreach (DataGridViewRow dg2 in dataGridView2.Rows)
                            {
                                if (row.Cells[9].Value.ToString() == dg2.Cells[2].Value.ToString() && Regex.Replace(row.Cells[13].Value.ToString(), "[^0-9.]", "") == Regex.Replace(dg2.Cells[0].Value.ToString(), "[^0-9.]", ""))
                                {
                                    if (dg2.Cells[4].Value.ToString() == "K")
                                    {
                                        row.Cells[6].Value = dg2.Cells[3].Value.ToString();

                                    }
                                    if (dg2.Cells[4].Value.ToString() == "A")
                                    {
                                        row.Cells[4].Value = dg2.Cells[3].Value.ToString();

                                    }
                                    if (dg2.Cells[4].Value.ToString() == "C")
                                    {
                                        row.Cells[3].Value = dg2.Cells[3].Value.ToString();

                                    }
                                    if (dg2.Cells[4].Value.ToString() == "L")
                                    {
                                        row.Cells[5].Value = dg2.Cells[3].Value.ToString();

                                    }
                                    if (dg2.Cells[4].Value.ToString() == "V")
                                    {
                                        row.Cells[7].Value = dg2.Cells[3].Value.ToString();

                                    }
                                }
                            }
                            if (row.Cells[6].Value == null || row.Cells[6].Value.ToString() == "")
                            {
                                row.Cells[6].Value = "0.00";
                            }
                            if (row.Cells[4].Value == null || row.Cells[4].Value.ToString() == "")
                            {
                                row.Cells[4].Value = "0.00";
                            }
                            if (row.Cells[3].Value == null || row.Cells[3].Value.ToString() == "")
                            {
                                row.Cells[3].Value = "0.00";
                            }
                            if (row.Cells[5].Value == null || row.Cells[5].Value.ToString() == "")
                            {
                                row.Cells[5].Value = "0.00";
                            }
                            if (row.Cells[7].Value == null || row.Cells[7].Value.ToString() == "")
                            {
                                row.Cells[7].Value = "0.00";
                            }

                            decimal dailyrate = 0.00M;
                            dailyrate += Convert.ToDecimal(row.Cells[14].Value.ToString()) * Convert.ToDecimal(row.Cells[16].Value.ToString());

                            decimal otrate = 0.00M;
                            if (row.Cells[15].Value.ToString() == "" || row.Cells[15].Value == null || row.Cells[15].Value.ToString() == string.Empty)
                            {
                                otrate = 0.00M;
                            }
                            else if (row.Cells[17].Value.ToString() == "" || row.Cells[17].Value == null || row.Cells[17].Value.ToString() == string.Empty)
                            {
                                otrate = 0.00M;
                            }
                            else
                            {
                                otrate += Convert.ToDecimal(row.Cells[15].Value.ToString()) * Convert.ToDecimal(row.Cells[17].Value.ToString());
                            }

                            decimal totalgross = 0.00M;
                            totalgross += Convert.ToDecimal(otrate) + Convert.ToDecimal(dailyrate);
                            row.Cells[2].Value = totalgross.ToString();

                            string totaldeduc;
                            decimal deduc = 0.00M;
                            deduc += Convert.ToDecimal(row.Cells[3].Value.ToString()) + Convert.ToDecimal(row.Cells[4].Value.ToString()) + Convert.ToDecimal(row.Cells[5].Value.ToString()) + Convert.ToDecimal(row.Cells[6].Value.ToString()) + Convert.ToDecimal(row.Cells[7].Value.ToString()) + Convert.ToDecimal(row.Cells[18].Value.ToString());
                            totaldeduc = deduc.ToString();

                            decimal sum4 = 0.00M;
                            sum4 += Convert.ToDecimal(row.Cells[2].Value.ToString()) - Convert.ToDecimal(totaldeduc);
                            row.Cells[8].Value = sum4.ToString();

                            tblOTS.Open();

                            string insStmt2 = "insert into tblFinalWorkers ([EMPCODE],[EMPLOYEENAME],[PERIOD],[COY],[ACCTCODE],[ACCTDESC],[DAYRATE],[OTRATE],[NUMDAY],[NUMHOUR],[SSS],[TITLE],[GROSS],[COOP],[CA],[SSSLOANS],[OTHERS],[SC],[NET]) values" +
                                          " (@EMPCODE,@EMPLOYEENAME,@PERIOD,@COY,@ACCTCODE,@ACCTDESC,@DAYRATE,@OTRATE,@NUMDAY,@NUMHOUR,@SSS,@TITLE,@GROSS,@COOP,@CA,@SSSLOANS,@OTHERS,@SC,@NET)";

                            SqlCommand insCmd2 = new SqlCommand(insStmt2, tblOTS);

                            insCmd2.Parameters.AddWithValue("@EMPCODE", row.Cells[9].Value.ToString().Replace("¥", "Ñ").Trim());
                            insCmd2.Parameters.AddWithValue("@EMPLOYEENAME", row.Cells[0].Value.ToString().Replace("¥", "Ñ").Trim());
                            insCmd2.Parameters.AddWithValue("@PERIOD", row.Cells[11].Value.ToString());
                            insCmd2.Parameters.AddWithValue("@COY", row.Cells[12].Value.ToString());
                            if (row.Cells[13].Value == null || row.Cells[13].Value == DBNull.Value || String.IsNullOrWhiteSpace(row.Cells[13].Value.ToString()))
                            {
                                insCmd2.Parameters.AddWithValue("@ACCTCODE", DBNull.Value);
                            }
                            else
                            {
                                insCmd2.Parameters.AddWithValue("@ACCTCODE", row.Cells[13].Value.ToString().Trim());
                            }
                            if (row.Cells[1].Value == null || row.Cells[1].Value == DBNull.Value || String.IsNullOrWhiteSpace(row.Cells[1].Value.ToString()))
                            {
                                insCmd2.Parameters.AddWithValue("@ACCTDESC", DBNull.Value);
                            }
                            else
                            {
                                insCmd2.Parameters.AddWithValue("@ACCTDESC", row.Cells[1].Value.ToString());
                            }
                            insCmd2.Parameters.AddWithValue("@DAYRATE", row.Cells[14].Value.ToString());
                            insCmd2.Parameters.AddWithValue("@OTRATE", row.Cells[15].Value.ToString());
                            insCmd2.Parameters.AddWithValue("@NUMDAY", row.Cells[16].Value.ToString());
                            insCmd2.Parameters.AddWithValue("@NUMHOUR", row.Cells[17].Value.ToString());
                            insCmd2.Parameters.AddWithValue("@SSS", row.Cells[18].Value.ToString());
                            insCmd2.Parameters.AddWithValue("@TITLE", row.Cells[19].Value.ToString());
                            insCmd2.Parameters.AddWithValue("@GROSS", row.Cells[2].Value.ToString());
                            insCmd2.Parameters.AddWithValue("@COOP", row.Cells[3].Value.ToString());
                            insCmd2.Parameters.AddWithValue("@CA", row.Cells[4].Value.ToString());
                            insCmd2.Parameters.AddWithValue("@SSSLOANS", row.Cells[5].Value.ToString());
                            insCmd2.Parameters.AddWithValue("@OTHERS", row.Cells[6].Value.ToString());
                            insCmd2.Parameters.AddWithValue("@SC", row.Cells[7].Value.ToString());
                            insCmd2.Parameters.AddWithValue("@NET", row.Cells[8].Value.ToString());
                            insCmd2.ExecuteNonQuery();

                            tblOTS.Close();
                            add2 = add2 + result;

                            guna2ProgressBar1.BeginInvoke((Action)delegate ()
                            {
                                guna2ProgressBar1.Value = Decimal.ToInt32(add2);
                            });


                            label1.BeginInvoke((Action)delegate ()
                            {
                                label1.Text = "IMPORTING DATA OF " + row.Cells[0].Value.ToString();
                            });
                        }
                    }
                    guna2ProgressBar1.BeginInvoke((Action)delegate ()
                    {
                        guna2ProgressBar1.Value = 100;
                    });
                    label1.BeginInvoke((Action)delegate ()
                    {
                        label1.Text = "DATABASE UPDATED...";
                        label1.ForeColor = Color.LimeGreen;
                    });
                    MessageBox.Show("Done");
                    obj.dataGridView1.BeginInvoke((Action)delegate ()
                    {
                        //obj.dataGridView1.DataSource = null;
                        //obj.dt.Rows.Clear();
                        //obj.dataGridView1.Rows.Clear();
                        obj.load();
                    });
                }
                else
                {
                    MessageBox.Show("No Records found. Please select another date!");
                }
            }



            
        }
        workerspage obj = (workerspage)Application.OpenForms["workerspage"];
        internal delegate void SetDataSourceDelegate(DataTable table);
        private void setDataSource(DataTable table)
        {
            if (this.InvokeRequired)
            {

                this.Invoke(new SetDataSourceDelegate(setDataSource), table);
            }
            else
            {
                dataGridView1.DataSource = table;
            }
        }
        private void setDataSource1(DataTable table)
        {
            if (this.InvokeRequired)
            {

                this.Invoke(new SetDataSourceDelegate(setDataSource1), table);
            }
            else
            {
                dataGridView2.DataSource = table;
            }
        }
        private void bunifuFlatButton2_Click(object sender, EventArgs e)
        {


        }

        private void bunifuFlatButton1_Click(object sender, EventArgs e)
        {

        }

        private void bunifuFlatButton3_Click(object sender, EventArgs e)
        {
            ////MessageBox.Show(dataGridView1.Rows[0].Cells[0].Value.ToString());
            ////dataGridView1.Rows[0].Cells[1].Value = "0";
            //dataGridView1.Rows[0].Cells[1].Value = "1";
            //dataGridView1.Rows[0].Cells[2].Value = "2";
            //dataGridView1.Rows[0].Cells[3].Value = "3";
            //dataGridView1.Rows[0].Cells[4].Value = "4";
            //dataGridView1.Rows[0].Cells[5].Value = "5";
            //dataGridView1.Rows[0].Cells[6].Value = "6";
            //dataGridView1.Rows[0].Cells[7].Value = "7";
            //dataGridView1.Rows[0].Cells[8].Value = "8";
            //dataGridView1.Rows[0].Cells[9].Value = "9";
            ////dataGridView1.Rows[0].Cells[10].Value = "10";
            ////dataGridView1.Rows[0].Cells[11].Value = "11";
            //dataGridView1.Rows[0].Cells[12].Value = "12";
            //MessageBox.Show(dataGridView1.Rows[0].Cells[10].Value.ToString());
            //MessageBox.Show(dataGridView1.Rows[0].Cells[11].Value.ToString());
            ////MessageBox.Show(dataGridView1.Rows[0].Cells[12].Value.ToString());
            //dataGridView1.Rows[0].Cells[13].Value = "13";
            //dataGridView1.Rows[0].Cells[14].Value = "14";
            //dataGridView1.Rows[0].Cells[15].Value = "15";
            //dataGridView1.Rows[0].Cells[16].Value = "16";
            //dataGridView1.Rows[0].Cells[17].Value = "17";
            //dataGridView1.Rows[0].Cells[18].Value = "18";
            //dataGridView1.Rows[0].Cells[19].Value = "19";
            ////dataGridView1.Rows[0].Cells[20].Value = "20";
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            pictureBox1.Image = Properties.Resources.ddd;
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            pictureBox1.Image = Properties.Resources.ddddd;
        }
        //FolderBrowserDialog fbd = new FolderBrowserDialog();
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            try
            {
                using (var fbd = new FolderBrowserDialog())
                {
                    DialogResult result = fbd.ShowDialog();

                    if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    {
                        string[] files = Directory.GetFiles(fbd.SelectedPath);

                        var path = fbd.SelectedPath.ToString() + "\\PERPAY.DBF";
                        var path2 = fbd.SelectedPath.ToString() + "\\PERGLU.DBF";
                        var path3 = fbd.SelectedPath.ToString() + "\\PERID.DBF";
                        var path4 = fbd.SelectedPath.ToString() + "\\GLU4.DBF";

                        if (File.Exists(path) && File.Exists(path2) && File.Exists(path3) && File.Exists(path4))
                        {
                            guna2TextBox1.Text = fbd.SelectedPath.ToString();
                            guna2Button2.Visible = true;
                            guna2Button3.Visible = true;
                        }
                        else
                        {
                            guna2TextBox1.Text = "";
                            MessageBox.Show("The OTSPAY.DBF/PERID.DBF/OTSDEDUC.DBF doesn't exist make sure to locate the file with the specified folder.");
                        }


                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void guna2Button2_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime datee = DateTime.Parse(guna2DateTimePicker1.Value.ToString());
                string fdate = datee.ToString("M/d/yyyy");
                using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
                {
                    tblOTS.Open();
                    SqlCommand check_User_Name = new SqlCommand("SELECT COUNT(*) FROM [tblFinalWorkers] WHERE ([PERIOD] = @PERIOD)", tblOTS);
                    check_User_Name.Parameters.AddWithValue("@PERIOD", fdate);
                    int UserExist = (int)check_User_Name.ExecuteScalar();

                    if (UserExist > 0)
                    {

                        MessageBox.Show("Records already exist...");
                    }
                    else
                    {

                        thread =
                        new Thread(new ThreadStart(submit));
                        thread.Start();
                    }
                    tblOTS.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        Thread thread;
        private void label3_Click(object sender, EventArgs e)
        {
 
        }

        private void label2_Click(object sender, EventArgs e)
        {
            workerspaysliperrors w = new workerspaysliperrors(this);
            w.ShowDialog();
        }
        Thread syncthread;
        private void guna2Button3_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    DateTime datee = DateTime.Parse(guna2DateTimePicker1.Value.ToString());
            //    string fdate = datee.ToString("M/d/yyyy");
            //    using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
            //    {
            //        tblOTS.Open();
            //        SqlCommand check_User_Name = new SqlCommand("SELECT COUNT(*) FROM [tblFinalWorkers] WHERE ([PERIOD] = @PERIOD)", tblOTS);
            //        check_User_Name.Parameters.AddWithValue("@PERIOD", fdate);
            //        int UserExist = (int)check_User_Name.ExecuteScalar();

            //        if (UserExist > 0)
            //        {

            //            MessageBox.Show("Records already exist...");
            //        }
            //        else
            //        {

                        syncthread =
                        new Thread(new ThreadStart(sync));
                        syncthread.Start();
                //    }
                //    tblOTS.Close();
                //}
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }
        private void sync()
        {
            label1.BeginInvoke((Action)delegate ()
            {
                label1.Visible = true;
            });
            guna2ProgressBar1.BeginInvoke((Action)delegate ()
            {
                guna2ProgressBar1.Visible = true;
            });
            DateTime datee = DateTime.Parse(guna2DateTimePicker1.Value.ToString());
            string fdate = datee.ToString("M/d/yyyy");
            using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
            {
                string sqlTrunc = "TRUNCATE TABLE GLU4";
                SqlCommand cmd = new SqlCommand(sqlTrunc, tblOTS);
                tblOTS.Open();
                cmd.ExecuteNonQuery();
                tblOTS.Close();

                DataTable YourResultSet = new DataTable();

                OleDbConnection yourConnectionHandler = new OleDbConnection(
                    @"Provider=VFPOLEDB.1;Data Source='" + guna2TextBox1.Text + "'");

                yourConnectionHandler.Open();

                if (yourConnectionHandler.State == ConnectionState.Open)
                {
                    string mySQL = "select * from GLU4";  // dbf table name

                    OleDbCommand MyQuery = new OleDbCommand(mySQL, yourConnectionHandler);
                    OleDbDataAdapter DA = new OleDbDataAdapter(MyQuery);

                    DA.Fill(YourResultSet);

                    yourConnectionHandler.Close();
                }

                foreach (DataRow row in YourResultSet.Rows)
                {
                    row["ACCTCODE"] = row["ACCTCODE"].ToString().Replace("¥", "Ñ").Trim().Trim();
                    row["ACCTDESC"] = row["ACCTDESC"].ToString().Replace("¥", "Ñ").Trim().Trim();
                }
                if (YourResultSet.Rows.Count == 0)
                {
                    MessageBox.Show("No GLU4 data found.");
                    thread.Abort();
                }
                DataTableReader reader = YourResultSet.CreateDataReader();
                tblOTS.Open();  ///this is my connection to the sql server
                SqlBulkCopy sqlcpy = new SqlBulkCopy(tblOTS);
                sqlcpy.DestinationTableName = "GLU4";  //copy the datatable to the sql table
                sqlcpy.WriteToServer(YourResultSet);
                tblOTS.Close();
                reader.Close();

            }
            using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
            {
                string sqlTrunc = "TRUNCATE TABLE PERDEDUC";
                SqlCommand cmd = new SqlCommand(sqlTrunc, tblOTS);
                tblOTS.Open();
                cmd.ExecuteNonQuery();
                tblOTS.Close();

                OleDbConnection oConn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + guna2TextBox1.Text + "';Extended Properties=dBase IV;");
                OleDbCommand command = new OleDbCommand("select * from PERDEDUC.DBF", oConn);
                DataTable dt = new DataTable();
                oConn.Open();
                dt.Load(command.ExecuteReader());
                oConn.Close();

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("No PERDEDUC data found.");
                    thread.Abort();
                }

                DataTableReader reader = dt.CreateDataReader();
                tblOTS.Open();  ///this is my connection to the sql server
                SqlBulkCopy sqlcpy = new SqlBulkCopy(tblOTS);
                sqlcpy.DestinationTableName = "PERDEDUC";  //copy the datatable to the sql table
                sqlcpy.WriteToServer(dt);
                tblOTS.Close();
                reader.Close();

            }
            using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
            {
                string sqlTrunc = "TRUNCATE TABLE tblPERID";
                SqlCommand cmd = new SqlCommand(sqlTrunc, tblOTS);
                tblOTS.Open();
                cmd.ExecuteNonQuery();
                tblOTS.Close();

                OleDbConnection oConn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + guna2TextBox1.Text + "';Extended Properties=dBase IV;");
                OleDbCommand command = new OleDbCommand("select * from PERID.DBF", oConn);
                DataTable dt = new DataTable();
                oConn.Open();
                dt.Load(command.ExecuteReader());
                oConn.Close();

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("No PERID data found.");
                    thread.Abort();
                }

                DataTableReader reader = dt.CreateDataReader();
                tblOTS.Open();  ///this is my connection to the sql server
                SqlBulkCopy sqlcpy = new SqlBulkCopy(tblOTS);
                sqlcpy.DestinationTableName = "tblPERID";  //copy the datatable to the sql table
                sqlcpy.WriteToServer(dt);
                tblOTS.Close();
                reader.Close();
            }
            using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
            {
                string sqlTrunc = "TRUNCATE TABLE LEOPAY";
                SqlCommand cmd = new SqlCommand(sqlTrunc, tblOTS);
                tblOTS.Open();
                cmd.ExecuteNonQuery();
                tblOTS.Close();

                OleDbConnection oConn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + guna2TextBox1.Text + "';Extended Properties=dBase IV;");
                OleDbCommand command = new OleDbCommand("select * from PERPAY.DBF", oConn);
                DataTable dt = new DataTable();
                oConn.Open();
                dt.Load(command.ExecuteReader());
                oConn.Close();

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("No PERPAY data found.");
                    thread.Abort();
                }

                DataTableReader reader = dt.CreateDataReader();
                tblOTS.Open();  ///this is my connection to the sql server
                SqlBulkCopy sqlcpy = new SqlBulkCopy(tblOTS);
                sqlcpy.DestinationTableName = "LEOPAY";  //copy the datatable to the sql table
                sqlcpy.WriteToServer(dt);
                tblOTS.Close();
                reader.Close();
            }
            DataTable sqlrow = new DataTable();
            using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
            {
                tblOTS.Open();
                string list = "SELECT EMPCODE,SOURCE,PERIOD,COY,ACCTCODE,DAYRATE,OTRATE,NUMDAY,NUMHOUR,SSS,TITLE FROM LEOPAY WHERE PERIOD = '" + fdate + "'";
                SqlCommand command = new SqlCommand(list, tblOTS);
                SqlDataReader reader = command.ExecuteReader();
                sqlrow.Load(reader);
                tblOTS.Close();
            }
            sqlrow.Columns.Add("EMPLOYEENAME");
            sqlrow.Columns.Add("ACCTDESC");
            sqlrow.Columns.Add("GROSS");
            sqlrow.Columns.Add("ADV");
            sqlrow.Columns.Add("COOP");
            sqlrow.Columns.Add("SSSLOAN");
            sqlrow.Columns.Add("OTHER");
            sqlrow.Columns.Add("SC");
            sqlrow.Columns.Add("NET");
            guna2ProgressBar1.BeginInvoke((Action)delegate ()
            {
                guna2ProgressBar1.Value = 1;
            });

            decimal result = 0.00M;
            decimal add2 = 0.00M;
            int rows;
            Int32 count = (Int32)sqlrow.Rows.Count;
            rows = count;

            result = 99 / (decimal)count;
            add2 += result + 1;

            foreach (DataRow row in sqlrow.Rows)
            {
                using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
                {
                    tblOTS.Open();
                    String query2 = "SELECT FIRST,MIDDLE,FAMILY FROM tblPERID WHERE EMPCODE = '" + row["EMPCODE"].ToString() + "'";
                    SqlCommand cmd2 = new SqlCommand(query2, tblOTS);
                    SqlDataReader rdr2 = cmd2.ExecuteReader();

                    if (rdr2.Read())
                    {
                        row["EMPLOYEENAME"] = rdr2["FAMILY"].ToString().Replace("¥", "Ñ").Trim() + ", " + rdr2["FIRST"].ToString().Replace("¥", "Ñ").Trim() + " " + rdr2["MIDDLE"].ToString().Replace("¥", "Ñ").Trim();
                    }
                    else
                    {
                        row["EMPLOYEENAME"] = "";
                    }
                    tblOTS.Close();

                    tblOTS.Open();
                    String query3 = "SELECT ACCTDESC FROM GLU4 WHERE ACCTCODE = '" + row["ACCTCODE"].ToString().Trim() + "'";
                    SqlCommand cmd3 = new SqlCommand(query3, tblOTS);
                    SqlDataReader rdr3 = cmd3.ExecuteReader();

                    if (rdr3.Read())
                    {
                        row["ACCTDESC"] = rdr3["ACCTDESC"].ToString();
                    }
                    tblOTS.Close();
                }
                decimal numday = Convert.ToDecimal(row["DAYRATE"].ToString()) * Convert.ToDecimal(row["NUMDAY"].ToString());
                decimal othrs = Convert.ToDecimal(row["OTRATE"].ToString()) * Convert.ToDecimal(row["NUMHOUR"].ToString());
                decimal gross = numday + othrs;

                row["GROSS"] = string.Format("{0:#,##0.00}", gross);

                using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
                {
                    tblOTS.Open();
                    String query2 = "SELECT * FROM PERDEDUC WHERE EMPCODE = '" + row["EMPCODE"].ToString() + "' AND SOURCE = '" + row["SOURCE"].ToString() + "' AND PERIOD = '" + row["PERIOD"].ToString() + "'";
                    SqlCommand cmd2 = new SqlCommand(query2, tblOTS);
                    SqlDataReader rdr2 = cmd2.ExecuteReader();

                    if (rdr2.Read())
                    {
                        if (rdr2["COUNT"].ToString() == "" || rdr2["COUNT"] == DBNull.Value)
                        {
                            row["ADV"] = rdr2["ADV"].ToString();
                            row["COOP"] = rdr2["COOP"].ToString();
                            row["SSSLOAN"] = rdr2["SSSLOAN"].ToString();
                            row["OTHER"] = rdr2["OTHER"].ToString();
                            row["SC"] = "0.00";
                            string empcode = row["EMPCODE"].ToString();
                            string source = row["SOURCE"].ToString();
                            string period = row["PERIOD"].ToString();
                            tblOTS.Close();

                            SqlCommand cmd3 = new SqlCommand("update PERDEDUC set COUNT=@COUNT where EMPCODE = '" + row["EMPCODE"].ToString() + "' AND SOURCE = '" + row["SOURCE"].ToString() + "' AND PERIOD = '" + row["PERIOD"].ToString() + "'", tblOTS);
                            tblOTS.Open();
                            cmd3.Parameters.AddWithValue("@EMPCODE", empcode);
                            cmd3.Parameters.AddWithValue("@SOURCE", source);
                            cmd3.Parameters.AddWithValue("@PERIOD", period);
                            cmd3.Parameters.AddWithValue("@COUNT", "1");
                            cmd3.ExecuteNonQuery();
                            tblOTS.Close();
                        }
                        else
                        {
                            row["ADV"] = "0.00";
                            row["COOP"] = "0.00";
                            row["SSSLOAN"] = "0.00";
                            row["OTHER"] = "0.00";
                            row["SC"] = "0.00";
                            tblOTS.Close();
                        }
                    }
                    else
                    {
                        row["ADV"] = "0.00";
                        row["COOP"] = "0.00";
                        row["SSSLOAN"] = "0.00";
                        row["OTHER"] = "0.00";
                        row["SC"] = "0.00";
                        tblOTS.Close();
                    }
                }
                decimal deduc = Convert.ToDecimal(row["ADV"].ToString()) + Convert.ToDecimal(row["COOP"].ToString()) + Convert.ToDecimal(row["SSSLOAN"].ToString()) + Convert.ToDecimal(row["OTHER"].ToString()) + Convert.ToDecimal(row["SSS"].ToString());
                decimal net = gross - deduc;

                row["NET"] = string.Format("{0:#,##0.00}", net);

                add2 = add2 + result;

                guna2ProgressBar1.BeginInvoke((Action)delegate ()
                {
                    guna2ProgressBar1.Value = Decimal.ToInt32(add2);
                });

                label1.BeginInvoke((Action)delegate ()
                {
                    label1.Text = "IMPORTING DATA OF " + row["EMPLOYEENAME"].ToString();
                });
                using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
                {
                    tblOTS.Open();

                    string insStmt2 = "insert into tblFinalWorkers ([EMPCODE],[EMPLOYEENAME],[PERIOD],[COY],[ACCTCODE],[ACCTDESC],[DAYRATE],[OTRATE],[NUMDAY],[NUMHOUR],[SSS],[TITLE],[GROSS],[COOP],[CA],[SSSLOANS],[OTHERS],[SC],[NET]) values" +
                                  " (@EMPCODE,@EMPLOYEENAME,@PERIOD,@COY,@ACCTCODE,@ACCTDESC,@DAYRATE,@OTRATE,@NUMDAY,@NUMHOUR,@SSS,@TITLE,@GROSS,@COOP,@CA,@SSSLOANS,@OTHERS,@SC,@NET)";

                    SqlCommand insCmd2 = new SqlCommand(insStmt2, tblOTS);

                    insCmd2.Parameters.AddWithValue("@EMPCODE", row["EMPCODE"].ToString().Trim());
                    insCmd2.Parameters.AddWithValue("@EMPLOYEENAME", row["EMPLOYEENAME"].ToString().Trim());
                    insCmd2.Parameters.AddWithValue("@PERIOD", row["PERIOD"].ToString().Trim());
                    insCmd2.Parameters.AddWithValue("@COY", row["COY"].ToString().Trim());
                    insCmd2.Parameters.AddWithValue("@ACCTCODE", row["ACCTCODE"].ToString().Trim());
                    insCmd2.Parameters.AddWithValue("@ACCTDESC", row["ACCTDESC"].ToString().Trim());
                    insCmd2.Parameters.AddWithValue("@DAYRATE", row["DAYRATE"].ToString().Trim());
                    insCmd2.Parameters.AddWithValue("@OTRATE", row["OTRATE"].ToString().Trim());
                    insCmd2.Parameters.AddWithValue("@NUMDAY", row["NUMDAY"].ToString().Trim());
                    insCmd2.Parameters.AddWithValue("@NUMHOUR", row["NUMHOUR"].ToString().Trim());
                    insCmd2.Parameters.AddWithValue("@SSS", row["SSS"].ToString().Trim());
                    insCmd2.Parameters.AddWithValue("@TITLE", row["TITLE"].ToString().Trim());
                    insCmd2.Parameters.AddWithValue("@GROSS", row["GROSS"].ToString().Trim());
                    insCmd2.Parameters.AddWithValue("@COOP", row["COOP"].ToString().Trim());
                    insCmd2.Parameters.AddWithValue("@CA", row["ADV"].ToString().Trim());
                    insCmd2.Parameters.AddWithValue("@SSSLOANS", row["SSSLOAN"].ToString().Trim());
                    insCmd2.Parameters.AddWithValue("@OTHERS", row["OTHER"].ToString().Trim());
                    insCmd2.Parameters.AddWithValue("@SC", row["SC"].ToString().Trim());
                    insCmd2.Parameters.AddWithValue("@NET", row["NET"].ToString().Trim());
                    insCmd2.ExecuteNonQuery();
                    tblOTS.Close();
                }
            }
            guna2ProgressBar1.BeginInvoke((Action)delegate ()
            {
                guna2ProgressBar1.Value = 100;
            });
            label1.BeginInvoke((Action)delegate ()
            {
                label1.Text = "DATABASE UPDATED...";
                label1.ForeColor = Color.LimeGreen;
            });
            MessageBox.Show("Done");
            obj.dataGridView1.BeginInvoke((Action)delegate ()
            {
                //obj.dataGridView1.DataSource = null;
                //obj.dt.Rows.Clear();
                //obj.dataGridView1.Rows.Clear();
                obj.load();
            });
        }
    }
}
