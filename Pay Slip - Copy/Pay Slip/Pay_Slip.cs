using System;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace Pay_Slip
{
    public partial class Pay_Slip : Form
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










        //     [System.Runtime.InteropServices.DllImport("gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        //     private static extern IntPtr CreateRoundRectRgn
        //(
        //    int nLeftRect, // X-coordinate of upper-left corner or padding at start
        //    int nTopRect,// Y-coordinate of upper-left corner or padding at the top of the textbox
        //    int nRightRect, // X-coordinate of lower-right corner or Width of the object
        //    int nBottomRect,// Y-coordinate of lower-right corner or Height of the object
        //                    //RADIUS, how round do you want it to be?
        //    int nheightRect, //height of ellipse 
        //    int nweightRect //width of ellipse
        //);
        private void ChangeControlStyles(Control ctrl, ControlStyles flag, bool value)
        {
            MethodInfo method = ctrl.GetType().GetMethod("SetStyle", BindingFlags.Instance | BindingFlags.NonPublic);
            if (method != null)
                method.Invoke(ctrl, new object[] { flag, value });
        }
        public Pay_Slip()
        {
            InitializeComponent();
            ApplyShadows(this);
            this.FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 15, 15));
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
        private void Form1_Load(object sender, EventArgs e)
        {
            SuspendLayout();


            ChangeControlStyles(dataGridView1, ControlStyles.OptimizedDoubleBuffer, true);
            this.dataGridView1.ColumnHeadersDefaultCellStyle.SelectionBackColor = this.dataGridView1.ColumnHeadersDefaultCellStyle.BackColor;
            dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.Visible = false;
            //load();
            ResumeLayout();
        }

        private void bunifuFlatButton1_MouseEnter(object sender, EventArgs e)
        {

        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            pictureBox1.Image = Properties.Resources.ddd;
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            pictureBox1.Image = Properties.Resources.ddddd;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void bunifuFlatButton1_Click(object sender, EventArgs e)
        {
           
        }

        private void bunifuFlatButton2_Click(object sender, EventArgs e)
        {

        }


        public static class ThreadHelperClass
        {
            delegate void SetTextCallback(Form f, Control ctrl, string text);
            /// <summary>
            /// Set text property of various controls
            /// </summary>
            /// <param name="form">The calling form</param>
            /// <param name="ctrl"></param>
            /// <param name="text"></param>
            public static void SetText(Form form, Control ctrl, string text)
            {
                // InvokeRequired required compares the thread ID of the 
                // calling thread to the thread ID of the creating thread. 
                // If these threads are different, it returns true. 
                if (ctrl.InvokeRequired)
                {
                    SetTextCallback d = new SetTextCallback(SetText);
                    form.Invoke(d, new object[] { form, ctrl, text });

                }
                else
                {
                    ctrl.Text = text;
                    ctrl.ForeColor = Color.LimeGreen;
                }
            }
        }
        internal delegate void SetDataSourceDelegate(DataTable table);
        private void setDataSource(DataTable table)
        {
            if (this.InvokeRequired)
            {
                Thread demoThread =
                new Thread(new ThreadStart(this.ThreadProcSafe));
                demoThread.Start();
                this.Invoke(new SetDataSourceDelegate(setDataSource), table);
            }
            else
            {
                dataGridView1.DataSource = table;
            }
        }


        private void ThreadProcSafe()
        {
            ThreadHelperClass2.SetText(this, label4, "Loading: Gathering Information please wait...");
        }
        public static class ThreadHelperClass2
        {
            delegate void SetTextCallback(Form f, Control ctrl, string text);
            /// <summary>
            /// Set text property of various controls
            /// </summary>
            /// <param name="form">The calling form</param>
            /// <param name="ctrl"></param>
            /// <param name="text"></param>
            public static void SetText(Form form, Control ctrl, string text)
            {
                // InvokeRequired required compares the thread ID of the 
                // calling thread to the thread ID of the creating thread. 
                // If these threads are different, it returns true. 
                if (ctrl.InvokeRequired)
                {
                    SetTextCallback d = new SetTextCallback(SetText);
                    form.Invoke(d, new object[] { form, ctrl, text });
                }
                else
                {
                    ctrl.Text = text;
                    ctrl.ForeColor = Color.Black;
                }
            }
        }

        private void gather()
        {
            var path = guna2TextBox1.Text + "\\OTSPAY.DBF";
            var path2 = guna2TextBox1.Text + "\\PERID.DBF";
            var path3 = guna2TextBox1.Text + "\\OTSDEDUC.DBF";
            if (File.Exists(path) && File.Exists(path2) && File.Exists(path3))
            {
                label4.BeginInvoke((Action)delegate ()
                {
                    label4.Visible = true;
                });
                guna2ProgressBar1.BeginInvoke((Action)delegate ()
                {
                    guna2ProgressBar1.Visible = true;
                });
                label4.BeginInvoke((Action)delegate ()
                {
                    label4.Text = "Loading: Cleaning and importing data to database...";
                });
                guna2ProgressBar1.BeginInvoke((Action)delegate ()
                {
                    guna2ProgressBar1.Value = 3;
                });
                using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
                {

                    string sqlTrunc = "TRUNCATE TABLE tblOTS";
                    SqlCommand cmd = new SqlCommand(sqlTrunc, tblOTS);

                    tblOTS.Open();
                    cmd.ExecuteNonQuery();
                    tblOTS.Close();

                    guna2ProgressBar1.BeginInvoke((Action)delegate ()
                    {
                        guna2ProgressBar1.Value = 6;
                    });

                    OleDbConnection oConn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + guna2TextBox1.Text + "';Extended Properties=dBase IV;");
                    OleDbCommand command = new OleDbCommand("select * from OTSPAY.DBF", oConn);
                    DataTable dt = new DataTable();
                    oConn.Open();
                    //OleDbDataReader read = command.ExecuteReader();
                    //dt.Load(read);
                    //dataGridView1.DataSource = dt;

                    dt.Load(command.ExecuteReader());
                    oConn.Close();

                    guna2ProgressBar1.BeginInvoke((Action)delegate ()
                    {
                        guna2ProgressBar1.Value = 12;
                    });

                    DataTableReader reader = dt.CreateDataReader();
                    tblOTS.Open();  ///this is my connection to the sql server
                    SqlBulkCopy sqlcpy = new SqlBulkCopy(tblOTS);
                    sqlcpy.DestinationTableName = "tblOTS";  //copy the datatable to the sql table
                    sqlcpy.WriteToServer(dt);
                    tblOTS.Close();
                    reader.Close();

                    guna2ProgressBar1.BeginInvoke((Action)delegate ()
                    {
                        guna2ProgressBar1.Value = 15;
                    });
                }
                using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
                {

                    string sqlTrunc = "TRUNCATE TABLE tblFinalOTS";
                    SqlCommand cmd = new SqlCommand(sqlTrunc, tblOTS);

                    tblOTS.Open();
                    cmd.ExecuteNonQuery();
                    tblOTS.Close();

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
                        string sqlTrunc = "TRUNCATE TABLE tblOTSDEDUC";
                        SqlCommand cmd = new SqlCommand(sqlTrunc, tblOTS);
                        tblOTS.Open();
                        cmd.ExecuteNonQuery();
                        tblOTS.Close();

                    guna2ProgressBar1.BeginInvoke((Action)delegate ()
                    {
                        guna2ProgressBar1.Value = 33;
                    });

                    OleDbConnection oConn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + guna2TextBox1.Text + "';Extended Properties=dBase IV;");
                    OleDbCommand command = new OleDbCommand("select * from OTSDEDUC.DBF", oConn);
                    DataTable dt = new DataTable();
                    oConn.Open();
                    dt.Load(command.ExecuteReader());
                    oConn.Close();

                    guna2ProgressBar1.BeginInvoke((Action)delegate ()
                    {
                        guna2ProgressBar1.Value = 36;
                    });
                    DataTableReader reader = dt.CreateDataReader();
                    tblOTS.Open();  ///this is my connection to the sql server
                    SqlBulkCopy sqlcpy = new SqlBulkCopy(tblOTS);
                    sqlcpy.DestinationTableName = "tblOTSDEDUC";  //copy the datatable to the sql table
                    sqlcpy.WriteToServer(dt);
                    tblOTS.Close();
                    reader.Close();

                    guna2ProgressBar1.BeginInvoke((Action)delegate ()
                    {
                        guna2ProgressBar1.Value = 39;
                    });
                }


                label4.BeginInvoke((Action)delegate ()
                {
                    label4.Text = "Loading: Cleaning and importing data to database completed...";
                });

                label4.BeginInvoke((Action)delegate ()
                {
                    label4.Text = "Loading: Getting data from database...";
                });
                using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
                {

                    tblOTS.Open();
                    DataTable dt = new DataTable();
                    using (SqlDataAdapter a2 = new SqlDataAdapter("SELECT * FROM tblOTS ORDER BY INT DESC", tblOTS))
                        a2.Fill(dt);
                    BindingSource bs = new BindingSource();
                    bs.DataSource = dt;
                    setDataSource(dt);

                    tblOTS.Close();
                    guna2ProgressBar1.BeginInvoke((Action)delegate ()
                    {
                        guna2ProgressBar1.Value = 42;
                    });
                }

                label4.BeginInvoke((Action)delegate ()
                {
                    label4.Text = "Loading: Getting data from database completed...";
                });
                label4.BeginInvoke((Action)delegate ()
                {
                    label4.Text = "Loading: Collecting data...";
                });
                decimal result = 0.00M;
                decimal add2 = 0.00M;
                int rows;

                using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
                {


                    SqlCommand cmd9 = new SqlCommand("SELECT COUNT(*) FROM tblOTS", tblOTS);
                    tblOTS.Open();
                    Int32 count = (Int32)cmd9.ExecuteScalar();
                    tblOTS.Close();
                    rows = count;

                    result = 50 / (decimal)count;
                    add2 += 0 + 42;

                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        label4.BeginInvoke((Action)delegate ()
                        {
                            label4.Text = "Loading: Collecting data...";
                        });

                        row.Cells[9].Value = row.Cells[9].Value.ToString();

                        tblOTS.Open();
                        DataTable dt = new DataTable();
                        String query = "SELECT FIRST,MIDDLE,FAMILY FROM tblPERID WHERE EMPCODE = '" + row.Cells[9].Value.ToString() + "'";
                        SqlCommand cmd = new SqlCommand(query, tblOTS);
                        SqlDataReader rdr = cmd.ExecuteReader();

                        if (rdr.Read())
                        {
                            row.Cells[0].Value = rdr["FAMILY"].ToString().Trim() + ", " + rdr["FIRST"].ToString().Trim() + " " + rdr["MIDDLE"].ToString().Trim();

                        }
                        else
                        {
                            row.Cells[0].Value = "";
                            row.Cells[0].Style.BackColor = Color.Red;
                        }
                        tblOTS.Close();



                        tblOTS.Open();
                        DataTable dt2 = new DataTable();
                        String query2 = "SELECT PERIODD,ADV,COOP,SSSLOAN,OTHERS,HDMFCONT FROM tblOTSDEDUC WHERE EMPCODE = '" + row.Cells[9].Value + "' AND PERIODD = '" + row.Cells[11].Value + "'";
                        SqlCommand cmd2 = new SqlCommand(query2, tblOTS);
                        SqlDataReader rdr2 = cmd2.ExecuteReader();

                        if (rdr2.Read())
                        {
                            row.Cells[2].Value = rdr2["PERIODD"].ToString();
                            row.Cells[3].Value = rdr2["ADV"].ToString();
                            row.Cells[4].Value = rdr2["COOP"].ToString();
                            row.Cells[5].Value = rdr2["SSSLOAN"].ToString();
                            row.Cells[6].Value = rdr2["OTHERS"].ToString();
                            row.Cells[7].Value = rdr2["HDMFCONT"].ToString();

                        }
                        tblOTS.Close();



                        if (row.Cells[3].Value == null || row.Cells[3].Value.ToString().Trim() == "")
                        {
                            row.Cells[3].Value = "0.00";
                        }
                        if (row.Cells[4].Value == null || row.Cells[4].Value.ToString().Trim() == "")
                        {
                            row.Cells[4].Value = "0.00";
                        }
                        if (row.Cells[5].Value == null || row.Cells[5].Value.ToString().Trim() == "")
                        {
                            row.Cells[5].Value = "0.00";
                        }
                        if (row.Cells[6].Value == null || row.Cells[6].Value.ToString().Trim() == "")
                        {
                            row.Cells[6].Value = "0.00";
                        }
                        if (row.Cells[18].Value == null || row.Cells[18].Value.ToString().Trim() == "")
                        {
                            row.Cells[18].Value = "0.00";
                        }
                        if (row.Cells[7].Value == null || row.Cells[7].Value.ToString().Trim() == "")
                        {
                            row.Cells[7].Value = "";
                        }
                        if (row.Cells[14].Value == null || row.Cells[14].Value.ToString().Trim() == "")
                        {
                            row.Cells[14].Value = "0.00";
                        }
                        if (row.Cells[15].Value == null || row.Cells[15].Value.ToString().Trim() == "")
                        {
                            row.Cells[15].Value = "0.00";
                        }
                        if (row.Cells[16].Value == null || row.Cells[16].Value.ToString().Trim() == "")
                        {
                            row.Cells[16].Value = "0.00";
                        }
                        if (row.Cells[17].Value == null || row.Cells[17].Value.ToString().Trim() == "")
                        {
                            row.Cells[17].Value = "0.00";
                        }

                        decimal dailyrate = 0.00M;
                        dailyrate += Convert.ToDecimal(row.Cells[14].Value.ToString()) * Convert.ToDecimal(row.Cells[16].Value.ToString());

                        decimal otrate = 0.00M;
                        otrate += Convert.ToDecimal(row.Cells[15].Value.ToString()) * Convert.ToDecimal(row.Cells[17].Value.ToString());

                        decimal totalgross = 0.00M;
                        totalgross += Convert.ToDecimal(otrate) + Convert.ToDecimal(dailyrate);
                        row.Cells[1].Value = Math.Round(totalgross, 2).ToString();


                        string totaldeduc;
                        decimal deduc = 0.00M;
                        deduc += Convert.ToDecimal(row.Cells[3].Value.ToString()) + Convert.ToDecimal(row.Cells[4].Value.ToString()) + Convert.ToDecimal(row.Cells[5].Value.ToString()) + Convert.ToDecimal(row.Cells[6].Value.ToString()) + Convert.ToDecimal(row.Cells[18].Value.ToString());
                        totaldeduc = deduc.ToString();

                        decimal sum4 = 0.00M;
                        sum4 += Convert.ToDecimal(row.Cells[1].Value.ToString()) - Convert.ToDecimal(totaldeduc);
                        row.Cells[8].Value = Math.Round(sum4, 2).ToString();



                        tblOTS.Open();

                        string insStmt2 = "insert into tblFinalOTS ([EMPCODE],[EMPLOYEENAME],[PERIOD],[COY],[PROJCODE],[DAYRATE],[OTRATE],[NUMDAY],[NUMHOUR],[SSS],[TITLE],[GROSS],[PERIODD],[ADV],[COOP],[SSSLOANS],[OTHERS],[HDMFCONT],[TOTALNET]) values" +
                                      " (@EMPCODE,@EMPLOYEENAME,@PERIOD,@COY,@PROJCODE,@DAYRATE,@OTRATE,@NUMDAY,@NUMHOUR,@SSS,@TITLE,@GROSS,@PERIODD,@ADV,@COOP,@SSSLOANS,@OTHERS,@HDMFCONT,@TOTALNET)";

                        SqlCommand insCmd2 = new SqlCommand(insStmt2, tblOTS);

                        insCmd2.Parameters.AddWithValue("@EMPCODE", row.Cells[9].Value.ToString().Replace("¥", "Ñ").Trim());
                        insCmd2.Parameters.AddWithValue("@EMPLOYEENAME", row.Cells[0].Value.ToString().Replace("¥", "Ñ").Trim());
                        insCmd2.Parameters.AddWithValue("@PERIOD", row.Cells[11].Value.ToString());
                        insCmd2.Parameters.AddWithValue("@COY", row.Cells[12].Value.ToString());
                        insCmd2.Parameters.AddWithValue("@PROJCODE", row.Cells[13].Value.ToString());
                        insCmd2.Parameters.AddWithValue("@DAYRATE", row.Cells[14].Value.ToString());
                        insCmd2.Parameters.AddWithValue("@OTRATE", row.Cells[15].Value.ToString());
                        insCmd2.Parameters.AddWithValue("@NUMDAY", row.Cells[16].Value.ToString());
                        insCmd2.Parameters.AddWithValue("@NUMHOUR", row.Cells[17].Value.ToString());
                        insCmd2.Parameters.AddWithValue("@SSS", row.Cells[18].Value.ToString());
                        insCmd2.Parameters.AddWithValue("@TITLE", row.Cells[19].Value.ToString());
                        insCmd2.Parameters.AddWithValue("@GROSS", row.Cells[1].Value.ToString());
                        if (row.Cells[2].Value != null && row.Cells[2].Value.ToString().Trim() != "")
                        {
                            insCmd2.Parameters.AddWithValue("@PERIODD", row.Cells[2].Value.ToString());
                        }
                        else
                        {
                            insCmd2.Parameters.AddWithValue("@PERIODD", "");
                        }
                        insCmd2.Parameters.AddWithValue("@ADV", row.Cells[3].Value.ToString());
                        insCmd2.Parameters.AddWithValue("@COOP", row.Cells[4].Value.ToString());
                        insCmd2.Parameters.AddWithValue("@SSSLOANS", row.Cells[5].Value.ToString());
                        insCmd2.Parameters.AddWithValue("@OTHERS", row.Cells[6].Value.ToString());
                        if (row.Cells[7].Value != null && row.Cells[7].Value.ToString().Trim() != "")
                        {
                            insCmd2.Parameters.AddWithValue("@HDMFCONT", row.Cells[7].Value.ToString());
                        }
                        else
                        {
                            insCmd2.Parameters.AddWithValue("@HDMFCONT", "");
                        }
                        insCmd2.Parameters.AddWithValue("@TOTALNET", row.Cells[8].Value.ToString());
                        insCmd2.ExecuteNonQuery();

                        tblOTS.Close();
                        //tblOTS.Dispose();

                        add2 = add2 + result;
                        guna2ProgressBar1.BeginInvoke((Action)delegate ()
                        {
                            guna2ProgressBar1.Value = Decimal.ToInt32(add2);
                        });

                    }

                }

                dataGridView1.BeginInvoke((Action)delegate ()
                {
                    dataGridView1.DataSource = null;
                    DataTable td = new DataTable();
                    td.Rows.Clear();
                });
                label4.BeginInvoke((Action)delegate ()
                {
                    label4.Text = "Database updated successfully...";
                });
                obj.dataGridView1.BeginInvoke((Action)delegate ()
                {
                    //obj.dataGridView1.DataSource = null;
                    //obj.dt.Rows.Clear();
                    //obj.dataGridView1.Rows.Clear();
                    obj.load2();
                });
                guna2ProgressBar1.BeginInvoke((Action)delegate ()
                {
                    guna2ProgressBar1.Value = 100;
                });
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Done");
            }
            else
            {
                MessageBox.Show("The OTSPAY.DBF/PERID.DBF doesn't exist make sure to locate the file with the specified folder.");
            }

        }
        payslipform obj = (payslipform)Application.OpenForms["payslipform"];
        private void load2()
        {
            using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
            {
                //bunifuProgressBar1.BeginInvoke((Action)delegate ()
                //{
                //    bunifuProgressBar1.Value = 67;
                //});
                //tblOTS.Open();
                //DataTable dt = new DataTable();
                //using (SqlDataAdapter a2 = new SqlDataAdapter("SELECT * FROM tblFinalOTS ORDER BY INT DESC", tblOTS))
                //    a2.Fill(dt);
                //BindingSource bs = new BindingSource();
                //bs.DataSource = dt;
                //setDataSource(dt);

                //tblOTS.Close();

                tblOTS.Open();
                DataTable dt = new DataTable();
                using (SqlDataAdapter a2 = new SqlDataAdapter("SELECT * FROM tblFinalOTS ORDER BY INT ASC", tblOTS))
                    a2.Fill(dt);
                BindingSource bs = new BindingSource();
                bs.DataSource = dt;
                foreach (DataRow item in dt.Rows)
                {

                    int n = dataGridView1.Rows.Add();
                    //dataGridView1.Rows[n].Cells[20].Value = item["INT"];
                    dataGridView1.Rows[n].Cells[0].Value = item["EMPCODE"].ToString();
                    dataGridView1.Rows[n].Cells[1].Value = item["EMPLOYEENAME"];
                    dataGridView1.Rows[n].Cells[3].Value = item["PERIOD"];
                    dataGridView1.Rows[n].Cells[4].Value = item["COY"];
                    dataGridView1.Rows[n].Cells[5].Value = item["PROJCODE"];
                    dataGridView1.Rows[n].Cells[6].Value = item["DAYRATE"];
                    dataGridView1.Rows[n].Cells[7].Value = item["OTRATE"];
                    dataGridView1.Rows[n].Cells[8].Value = item["NUMDAY"];
                    dataGridView1.Rows[n].Cells[9].Value = item["NUMHOUR"];
                    dataGridView1.Rows[n].Cells[10].Value = item["SSS"];
                    dataGridView1.Rows[n].Cells[11].Value = item["TITLE"];
                    dataGridView1.Rows[n].Cells[12].Value = item["INT"];
                    dataGridView1.Rows[n].Cells[13].Value = item["GROSS"];
                    dataGridView1.Rows[n].Cells[14].Value = item["PERIODD"];
                    dataGridView1.Rows[n].Cells[15].Value = item["ADV"];
                    dataGridView1.Rows[n].Cells[16].Value = item["COOP"];
                    dataGridView1.Rows[n].Cells[17].Value = item["SSSLOANS"];
                    dataGridView1.Rows[n].Cells[18].Value = item["OTHERS"];
                    dataGridView1.Rows[n].Cells[19].Value = item["HDMFCONT"];
                    dataGridView1.Rows[n].Cells[20].Value = item["TOTALNET"];
                }
                tblOTS.Close();

                tblOTS.Dispose();

            }

        }
        private void bunifuCustomLabel1_Click(object sender, EventArgs e)
        {

        }

        private void bunifuDatepicker3_onValueChanged(object sender, EventArgs e)
        {

        }

        private void bunifuDatepicker4_onValueChanged(object sender, EventArgs e)
        {

        }


        private void bunifuFlatButton3_Click(object sender, EventArgs e)
        {

        }

        private void bunifuFlatButton4_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }
        private void BackgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
        }

        private void BackgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
        }
        private void backgroundWorker1_DoWork_1(object sender, DoWorkEventArgs e)
        {


        }
        private void timer1_Tick_1(object sender, EventArgs e)
        {
        }

        private void backgroundWorker1_RunWorkerCompleted_1(object sender, RunWorkerCompletedEventArgs e)
        {
        }

        private void bunifuFlatButton4_Click_1(object sender, EventArgs e)
        {
            //dataGridView1.Rows[0].Cells[1].Value = "1";
            //dataGridView1.Rows[0].Cells[8].Value = "2";


            //if (dataGridView1.Rows[0].Cells[0].Value.ToString() != "")
            //{
            //    MessageBox.Show("0 " + dataGridView1.Rows[0].Cells[0].Value.ToString());
            //}
            //if (dataGridView1.Rows[0].Cells[2].Value.ToString() != "")
            //{
            //    MessageBox.Show("2 " + dataGridView1.Rows[0].Cells[2].Value.ToString());
            //}
            //if (dataGridView1.Rows[0].Cells[3].Value.ToString() != "")
            //{
            //    MessageBox.Show("3 " + dataGridView1.Rows[0].Cells[3].Value.ToString());
            //}
            //if (dataGridView1.Rows[0].Cells[4].Value.ToString() != "")
            //{
            //    MessageBox.Show("4 " + dataGridView1.Rows[0].Cells[4].Value.ToString());
            //}
            //if (dataGridView1.Rows[0].Cells[5].Value.ToString() != "")
            //{
            //    MessageBox.Show("5 " + dataGridView1.Rows[0].Cells[5].Value.ToString());
            //}
            //if (dataGridView1.Rows[0].Cells[6].Value.ToString() != "")
            //{
            //    MessageBox.Show("6 " + dataGridView1.Rows[0].Cells[6].Value.ToString());
            //}
            //if (dataGridView1.Rows[0].Cells[7].Value.ToString() != "")
            //{
            //    MessageBox.Show("7 " + dataGridView1.Rows[0].Cells[7].Value.ToString());
            //}
            ////if (dataGridView1.Rows[0].Cells[8].Value.ToString() != "")
            ////{
            ////    MessageBox.Show("8 " + dataGridView1.Rows[0].Cells[8].Value.ToString());
            ////}
            //if (dataGridView1.Rows[0].Cells[9].Value.ToString() != "")
            //{
            //    MessageBox.Show("9 " + dataGridView1.Rows[0].Cells[9].Value.ToString());
            //}
            //if (dataGridView1.Rows[0].Cells[10].Value.ToString() != "")
            //{
            //    MessageBox.Show("10 " + dataGridView1.Rows[0].Cells[10].Value.ToString());
            //}
            //if (dataGridView1.Rows[0].Cells[11].Value.ToString() != "")
            //{
            //    MessageBox.Show("11 " + dataGridView1.Rows[0].Cells[11].Value.ToString());
            //}
            //if (dataGridView1.Rows[0].Cells[12].Value.ToString() != "")
            //{
            //    MessageBox.Show("12 " + dataGridView1.Rows[0].Cells[12].Value.ToString());
            //}
            //if (dataGridView1.Rows[0].Cells[13].Value.ToString() != "")
            //{
            //    MessageBox.Show("13 " + dataGridView1.Rows[0].Cells[13].Value.ToString());
            //}
            //if (dataGridView1.Rows[0].Cells[14].Value.ToString() != "")
            //{
            //    MessageBox.Show("14 " + dataGridView1.Rows[0].Cells[14].Value.ToString());
            //}
            //if (dataGridView1.Rows[0].Cells[15].Value.ToString() != "")
            //{
            //    MessageBox.Show("15 " + dataGridView1.Rows[0].Cells[15].Value.ToString());
            //}
            //if (dataGridView1.Rows[0].Cells[16].Value.ToString() != "")
            //{
            //    MessageBox.Show("16 " + dataGridView1.Rows[0].Cells[16].Value.ToString());
            //}
            //if (dataGridView1.Rows[0].Cells[17].Value.ToString() != "")
            //{
            //    MessageBox.Show("17 " + dataGridView1.Rows[0].Cells[17].Value.ToString());
            //}
            //if (dataGridView1.Rows[0].Cells[18].Value.ToString() != "")
            //{
            //    MessageBox.Show("18 " + dataGridView1.Rows[0].Cells[18].Value.ToString());
            //}
            //if (dataGridView1.Rows[0].Cells[19].Value.ToString() != "")
            //{
            //    MessageBox.Show("19 " + dataGridView1.Rows[0].Cells[19].Value.ToString());
            //}
            //if (dataGridView1.Rows[0].Cells[20].Value.ToString() != "")
            //{
            //    MessageBox.Show("20 " + dataGridView1.Rows[0].Cells[20].Value.ToString());
            //}

            load2();
        }

        private void bunifuFlatButton5_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (Convert.ToDecimal(row.Cells[16].Value.ToString()) >= 48.01M)
                {
                    //MessageBox.Show(dataGridView1.Rows[6].Cells[16].Value.ToString());
                    //MessageBox.Show(dataGridView1.Rows[6].Cells[14].Value.ToString());
                    string otnum;
                    string totalotnum;
                    decimal sum = 0.00M;
                    sum += Convert.ToDecimal(row.Cells[16].Value.ToString()) - 48;
                    otnum = sum.ToString();

                    decimal sum1 = 0.00M;
                    sum1 += Convert.ToDecimal(row.Cells[15].Value.ToString()) * Convert.ToDecimal(otnum);
                    totalotnum = sum1.ToString();

                    string normalrate;
                    decimal sum2 = 0.00M;
                    sum2 += Convert.ToDecimal(row.Cells[14].Value.ToString()) * 48;
                    normalrate = sum2.ToString();

                    decimal sum3 = 0.00M;
                    sum3 += Convert.ToDecimal(normalrate) + Convert.ToDecimal(totalotnum);
                    row.Cells[1].Value = sum3.ToString();

                    if (row.Cells[2].Value != null && row.Cells[2].Value.ToString().Trim() != "")
                    {
                        string totaldeduc;
                        decimal deduc = 0.00M;
                        deduc += Convert.ToDecimal(row.Cells[3].Value.ToString()) + Convert.ToDecimal(row.Cells[4].Value.ToString()) + Convert.ToDecimal(row.Cells[5].Value.ToString()) + Convert.ToDecimal(row.Cells[6].Value.ToString()) + Convert.ToDecimal(row.Cells[18].Value.ToString());
                        totaldeduc = deduc.ToString();

                        decimal sum4 = 0.00M;
                        sum4 += Convert.ToDecimal(row.Cells[1].Value.ToString()) - Convert.ToDecimal(totaldeduc);
                        row.Cells[8].Value = sum4.ToString("N2");
                    }

                }
                else
                {
                    decimal sum2 = 0.00M;
                    sum2 += Convert.ToDecimal(row.Cells[14].Value.ToString()) * Convert.ToDecimal(row.Cells[16].Value.ToString());
                    row.Cells[1].Value = sum2.ToString();

                    if (row.Cells[2].Value != null && row.Cells[2].Value.ToString().Trim() != "")
                    {

                        string ADV;
                        string COOP;
                        string SSSLOAN;
                        string OTHERS;
                        string SSS;
                        if (row.Cells[3].Value != null && row.Cells[3].Value.ToString().Trim() != "")
                        {
                            ADV = row.Cells[3].Value.ToString();
                        }
                        else
                        {
                            ADV = "0";
                        }
                        if (row.Cells[4].Value != null && row.Cells[4].Value.ToString().Trim() != "")
                        {
                            COOP = row.Cells[4].Value.ToString();
                        }
                        else
                        {
                            COOP = "0";
                        }
                        if (row.Cells[5].Value != null && row.Cells[5].Value.ToString().Trim() != "")
                        {
                            SSSLOAN = row.Cells[5].Value.ToString();
                        }
                        else
                        {
                            SSSLOAN = "0";
                        }
                        if (row.Cells[6].Value != null && row.Cells[6].Value.ToString().Trim() != "")
                        {
                            OTHERS = row.Cells[6].Value.ToString();
                        }
                        else
                        {
                            OTHERS = "0";
                        }
                        if (row.Cells[18].Value != null && row.Cells[18].Value.ToString().Trim() != "")
                        {
                            SSS = row.Cells[18].Value.ToString();
                        }
                        else
                        {
                            SSS = "0";
                        }
                        string totaldeduc;
                        decimal deduc = 0.00M;
                        deduc += Convert.ToDecimal(ADV) + Convert.ToDecimal(COOP) + Convert.ToDecimal(SSSLOAN) + Convert.ToDecimal(OTHERS) + Convert.ToDecimal(SSS);
                        totaldeduc = deduc.ToString();

                        decimal sum4 = 0.00M;
                        sum4 += Convert.ToDecimal(row.Cells[1].Value.ToString()) - Convert.ToDecimal(totaldeduc);
                        row.Cells[8].Value = sum4.ToString("N2");
                    }

                }
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {

            dataGridView1.BeginInvoke((Action)delegate ()
            {
                dataGridView1.Visible = false;
            });
            //backgroundWorker1.RunWorkerAsync();
            label4.Visible = true;
            System.Threading.Thread thread =
              new System.Threading.Thread(new System.Threading.ThreadStart(gather));
            thread.Start();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    string[] files = Directory.GetFiles(fbd.SelectedPath);

                    var path = fbd.SelectedPath.ToString() + "\\OTSPAY.DBF";
                    var path2 = fbd.SelectedPath.ToString() + "\\PERID.DBF";
                    var path3 = fbd.SelectedPath.ToString() + "\\OTSDEDUC.DBF";

                    if (File.Exists(path) && File.Exists(path2) && File.Exists(path3))
                    {
                        guna2TextBox1.Text = fbd.SelectedPath.ToString();
                        guna2Button2.Visible = true;
                    }
                    else
                    {
                        guna2TextBox1.Text = "";
                        MessageBox.Show("The OTSPAY.DBF/PERID.DBF/OTSDEDUC.DBF doesn't exist make sure to locate the file with the specified folder.");
                    }
                }
            }
        }

        private void bunifuCustomTextbox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
