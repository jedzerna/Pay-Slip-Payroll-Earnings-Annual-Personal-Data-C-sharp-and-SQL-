using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Pay_Slip
{
    public partial class payslipform : Form
    {


        //private const int WM_NCHITTEST = 0x84;
        //private const int WS_MINIMIZEBOX = 0x20000;
        //private const int HTCLIENT = 0x1;
        //private const int HTCAPTION = 0x2;
        //private const int CS_DBLCLKS = 0x8;
        //private const int CS_DROPSHADOW = 0x00020000;
        //private const int WM_NCPAINT = 0x0085;
        //private const int WM_ACTIVATEAPP = 0x001C;

        //[EditorBrowsable(EditorBrowsableState.Never)]
        //public struct MARGINS
        //{
        //    public int leftWidth;
        //    public int rightWidth;
        //    public int topHeight;
        //    public int bottomHeight;
        //}



        //[DllImport("dwmapi.dll")]
        //[EditorBrowsable(EditorBrowsableState.Never)]
        //public static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS pMarInset);

        //[DllImport("dwmapi.dll")]
        //[EditorBrowsable(EditorBrowsableState.Never)]
        //public static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        //[DllImport("dwmapi.dll")]
        //[EditorBrowsable(EditorBrowsableState.Never)]
        //public static extern int DwmIsCompositionEnabled(ref int pfEnabled);

        //[EditorBrowsable(EditorBrowsableState.Never)]
        //public static bool IsCompositionEnabled()
        //{
        //    if (Environment.OSVersion.Version.Major < 6) return false;

        //    bool enabled;
        //    DwmIsCompositionEnabled(out enabled);

        //    return enabled;
        //}


        //[DllImport("dwmapi.dll")]
        //private static extern int DwmIsCompositionEnabled(out bool enabled);

        //[DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        //private static extern IntPtr CreateRoundRectRgn
        //(
        //    int nLeftRect,
        //    int nTopRect,
        //    int nRightRect,
        //    int nBottomRect,
        //    int nWidthEllipse,
        //    int nHeightEllipse
        // );

        //private bool CheckIfAeroIsEnabled()
        //{
        //    if (Environment.OSVersion.Version.Major >= 6)
        //    {
        //        int enabled = 0;
        //        DwmIsCompositionEnabled(ref enabled);

        //        return (enabled == 1) ? true : false;
        //    }
        //    return false;
        //}


        //public void ApplyShadows(Form form)
        //{
        //    var v = 2;

        //    DwmSetWindowAttribute(form.Handle, 2, ref v, 4);

        //    MARGINS margins = new MARGINS()
        //    {
        //        bottomHeight = 1,
        //        leftWidth = 0,
        //        rightWidth = 0,
        //        topHeight = 0
        //    };

        //    DwmExtendFrameIntoClientArea(form.Handle, ref margins);
        //}

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
        public payslipform()
        {
            InitializeComponent();
            //ApplyShadows(this);
            //this.FormBorderStyle = FormBorderStyle.None;
            //Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 15, 15));
        }
        selection obj = (selection)Application.OpenForms["selection"];
        public DataTable dt = new DataTable();
        private void payslipform_Load(object sender, EventArgs e)
        {
            //dataGridView1.ClearSelection();
            SuspendLayout();
            try
            {
                load2();
            }
            catch
            {
                MessageBox.Show("Connection Failed. Please change the server settings to continue");
            }

            ResumeLayout();
            ChangeControlStyles(dataGridView1, ControlStyles.OptimizedDoubleBuffer, true);
            this.dataGridView1.ColumnHeadersDefaultCellStyle.SelectionBackColor = this.dataGridView1.ColumnHeadersDefaultCellStyle.BackColor;
            dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
        
        }
        public int w;
        public int h;
        public void resizing()
        {
            this.Height = h;
            this.Width = w;
        }

        private void ChangeControlStyles(Control ctrl, ControlStyles flag, bool value)
        {
            MethodInfo method = ctrl.GetType().GetMethod("SetStyle", BindingFlags.Instance | BindingFlags.NonPublic);
            if (method != null)
                method.Invoke(ctrl, new object[] { flag, value });
        }
        public void load2()
        {
            dataGridView1.BeginInvoke((Action)delegate ()
            {

                using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
            {
             
                dt.Rows.Clear();
                tblOTS.Open();

                string list = "SELECT EMPCODE,EMPLOYEENAME,PERIOD,COY,PROJCODE,DAYRATE,OTRATE,NUMDAY,NUMHOUR,SSS,TITLE,INT,GROSS,PERIODD,ADV,COOP,SSSLOANS,OTHERS,HDMFCONT,TOTALNET FROM tblFinalOTS ORDER BY PERIOD DESC,EMPLOYEENAME ASC";
                SqlCommand command = new SqlCommand(list, tblOTS);
                SqlDataReader reader = command.ExecuteReader();
                dt.Load(reader); 
           
                    dataGridView1.DataSource = dt;
                
                dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
                dataGridView1.RowHeadersVisible = false;
            
                tblOTS.Close();

                tblOTS.Dispose();

                guna2CustomCheckBox10.Checked = false;
                guna2CustomCheckBox1.Checked = false;
                guna2CustomCheckBox2.Checked = false;
                guna2CustomCheckBox3.Checked = false;
                guna2CustomCheckBox4.Checked = false;
                guna2CustomCheckBox5.Checked = false;
                guna2CustomCheckBox6.Checked = false;
                guna2CustomCheckBox7.Checked = false;
                guna2CustomCheckBox8.Checked = false;
                dataGridView1.Columns[19].Visible = false;
                dataGridView1.Columns[11].Visible = false;
                dataGridView1.Columns[10].Visible = false;
                dataGridView1.Columns[9].Visible = false;
                dataGridView1.Columns[8].Visible = false;
                dataGridView1.Columns[7].Visible = false;
                dataGridView1.Columns[6].Visible = false;
                dataGridView1.Columns[5].Visible = false;
                dataGridView1.Columns[4].Visible = false;
                dataGridView1.Columns[14].Visible = false;
                dataGridView1.Columns[12].Visible = false;
                dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
          
                //dataGridView1.Columns[0].DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Regular);
                //dataGridView1.Columns[1].DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Regular);
                //dataGridView1.Columns[2].DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Regular);
                //dataGridView1.Columns[3].DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Regular);
                //dataGridView1.Columns[4].DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Regular);
                //dataGridView1.Columns[5].DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Regular);
                //dataGridView1.Columns[6].DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Regular);
                //dataGridView1.Columns[7].DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Regular);
                //dataGridView1.Columns[8].DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Regular);
                //dataGridView1.Columns[9].DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Regular);
                //dataGridView1.Columns[10].DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Regular);
                //dataGridView1.Columns[11].DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Regular);
                //dataGridView1.Columns[12].DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Regular);
                //dataGridView1.Columns[13].DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                //dataGridView1.Columns[14].DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Regular);
                //dataGridView1.Columns[15].DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Regular);
                //dataGridView1.Columns[16].DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Regular);
                //dataGridView1.Columns[17].DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Regular);
                //dataGridView1.Columns[18].DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Regular);
                //dataGridView1.Columns[19].DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Regular);
                //dataGridView1.Columns[20].DefaultCellStyle.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            }


            });
        }

        private void bunifuFlatButton3_Click(object sender, EventArgs e)
        {
            
        }

        private void bunifuCheckbox1_OnChange(object sender, EventArgs e)
        {

           
        }

        private void bunifuCheckbox2_OnChange(object sender, EventArgs e)
        {
           
        }

        private void bunifuCheckbox3_OnChange(object sender, EventArgs e)
        {
          
        }

        private void bunifuCheckbox4_OnChange(object sender, EventArgs e)
        {
           
        }

        private void bunifuCheckbox5_OnChange(object sender, EventArgs e)
        {
            
        }

        private void bunifuCheckbox6_OnChange(object sender, EventArgs e)
        {
           
        }

        private void bunifuCheckbox7_OnChange(object sender, EventArgs e)
        {
          
        }

        private void bunifuCheckbox8_OnChange(object sender, EventArgs e)
        {
      
        }

        private void bunifuCheckbox10_OnChange(object sender, EventArgs e)
        {
         
        }

        private void bunifuFlatButton1_Click(object sender, EventArgs e)
        {
           
            //}

        }

        private void bunifuFlatButton2_Click(object sender, EventArgs e)
        {
            

        }
        private void Doc_PrintPage(object sender, PrintPageEventArgs e)//PRINTING SETTINGS
        {


            Font drawFont = new Font("Arial", 10);
            SolidBrush drawBrush = new SolidBrush(Color.Black);
            // Set format of string.
            StringFormat drawFormat = new StringFormat();
            drawFormat.Alignment = StringAlignment.Center;

            //draw upper part
            float xx = 0F;
            float yy = 0F;
            float width2 = 300.0F;
            float height2 = 30;

            //        DateTime orderdt = DateTime.ParseExact(dataGridView1.CurrentRow.Cells[3].Value.ToString(), "MM/dd/yyyy",
            //System.Globalization.CultureInfo.InvariantCulture);

            //        DateTime oneYearBefore = orderdt.AddDays(-7);
            string newdate = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            DateTime DT = DateTime.Parse(newdate.ToString());

            DateTime oneYearBefore = DT.AddDays(-6);
            //TimeSpan timeSpan = DT - oneYearBefore;

            string drawString = "Pay Slip\n" + oneYearBefore.ToString("MM/dd/yyyy") + "-" + DT.ToString("MM/dd/yyyy");
            RectangleF drawRect = new RectangleF(xx, yy, width2, height2);
            e.Graphics.DrawString(drawString, drawFont, drawBrush, drawRect, drawFormat);

            //draw side DOWN
            using (var brush = new SolidBrush(ForeColor))
            {
                var stringFormat = new StringFormat
                {
                    Alignment = StringAlignment.Near,
                    LineAlignment = StringAlignment.Near
                };
                e.Graphics.TranslateTransform(20, 40);
                string text = "SITE: " + dataGridView1.CurrentRow.Cells[5].Value.ToString() + "\n" +
                              "PAYEE NAME : " + dataGridView1.CurrentRow.Cells[2].Value.ToString() + "\n \n" +
                              "Basic Weekly Salary/Advances:  ₱" + dataGridView1.CurrentRow.Cells[13].Value.ToString() + "\n" +
                              "No. of Hrs- " + dataGridView1.CurrentRow.Cells[8].Value.ToString() +
                              "\n" +
                              "No. of Hrs (O.T.)- " + dataGridView1.CurrentRow.Cells[9].Value.ToString() +
                              "\n\n" +
                              "Less Deduction: \n" +
                              "SSS Cont./HDMF/Philhealth: " + dataGridView1.CurrentRow.Cells[10].Value.ToString() + "\n" +
                              "Coop:                                        " + dataGridView1.CurrentRow.Cells[16].Value.ToString() + "\n" +
                              "C / A, S/ C:                                " + dataGridView1.CurrentRow.Cells[15].Value.ToString() + "\n" +
                              "SSS Loan:                                " + dataGridView1.CurrentRow.Cells[17].Value.ToString() + "\n" +
                              "Others:                                      " + dataGridView1.CurrentRow.Cells[18].Value.ToString() + "\n\n" +
                              "Net Take Home Pay:              ₱" + dataGridView1.CurrentRow.Cells[20].Value.ToString();


                e.Graphics.DrawString(text, Font, brush, DisplayRectangle, stringFormat);
            }


            // -extra TEXT -end

        }

        private void bunifuFlatButton4_Click(object sender, EventArgs e)
        {

        }
        private void Doc_PrintPage2(object sender, PrintPageEventArgs e)//PRINTING SETTINGS
        {
            
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                Font drawFont = new Font("Arial", 10);
                SolidBrush drawBrush = new SolidBrush(Color.Black);
                StringFormat drawFormat = new StringFormat();
                drawFormat.Alignment = StringAlignment.Center;
                float xx = 0F;
                float yy = 0F;
                float width2 = 300.0F;
                float height2 = 30;



                string newdate = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                DateTime DT = DateTime.Parse(newdate.ToString());
                DateTime oneYearBefore = DT.AddDays(-6);
                string drawString = "Pay Slip\n" + oneYearBefore.ToString("MM/dd/yyyy") + "-" + DT.ToString("MM/dd/yyyy");



                RectangleF drawRect = new RectangleF(xx, yy, width2, height2);
                e.Graphics.DrawString(drawString, drawFont, drawBrush, drawRect, drawFormat);
                using (var brush = new SolidBrush(ForeColor))
                {
                    var stringFormat = new StringFormat
                    {
                        Alignment = StringAlignment.Near,
                        LineAlignment = StringAlignment.Near
                    };
                    e.Graphics.TranslateTransform(20, 40);
                    string text = "SITE: " + dataGridView1.CurrentRow.Cells[5].Value.ToString() + "\n" +
                                  "PAYEE NAME : " + dataGridView1.CurrentRow.Cells[2].Value.ToString() + "\n \n" +
                                  "Basic Weekly Salary/Advances:  ₱" + dataGridView1.CurrentRow.Cells[13].Value.ToString() + "\n" +
                                  "No. of Hrs- " + dataGridView1.CurrentRow.Cells[8].Value.ToString()  +
                                  "\n" +
                                  "No. of Hrs (O.T.)- " + dataGridView1.CurrentRow.Cells[9].Value.ToString() +
                                  "\n\n" +
                                  "Less Deduction: \n" +
                                  "SSS Cont./HDMF/Philhealth: " + dataGridView1.CurrentRow.Cells[10].Value.ToString() + "\n" +
                                  "Coop:                                        " + dataGridView1.CurrentRow.Cells[16].Value.ToString() + "\n" +
                                  "C / A, S/ C:                                " + dataGridView1.CurrentRow.Cells[15].Value.ToString() + "\n" +
                                  "SSS Loan:                                " + dataGridView1.CurrentRow.Cells[17].Value.ToString() + "\n" +
                                  "Others:                                      " + dataGridView1.CurrentRow.Cells[18].Value.ToString() + "\n\n" +
                                  "Net Take Home Pay:              ₱" + dataGridView1.CurrentRow.Cells[20].Value.ToString();
                    e.Graphics.DrawString(text, Font, brush, DisplayRectangle, stringFormat);
                }

            }
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                this.Hide();//Hide the 'current' form, i.e frm_form1 
                            //show another form ( frm_form2 )   
                selection frm = new selection();
                frm.ShowDialog();
                //Close the form.(frm_form1)
                this.Dispose();
            });
        }

        private void bunifuFlatButton5_Click(object sender, EventArgs e)
        {
          
        }

        private void bunifuFlatButton6_Click(object sender, EventArgs e)
        {
            //string stmt = "SELECT COUNT(*) FROM tblFinalOTS";
            //int count = 0;

            //using (SqlConnection thisConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["tblOTS"].ConnectionString))
            //{
            //    using (SqlCommand cmdCount = new SqlCommand(stmt, thisConnection))
            //    {
            //        thisConnection.Open();
            //        count = (int)cmdCount.ExecuteScalar();
            //        thisConnection.Close();
            //        MessageBox.Show(count.ToString());
            //    }
            //}
        }

        private void bunifuCustomLabel16_TextChanged(object sender, EventArgs e)
        {

        }
        private void timer1_Tick(object sender, EventArgs e)
        {

        }

        private void bunifuFlatButton4_Click_1(object sender, EventArgs e)
        {

        }

        private void bunifuCheckbox9_OnChange(object sender, EventArgs e)
        {

        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
      
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void bunifuFlatButton4_Click_2(object sender, EventArgs e)
        {
        }
        private void InsertDgvIntoForm()
        {
            //create a data set
            DataSet ds = new DataSet();
            //create a data table for the data set
            //create some columns for the datatable
            //add the columns to the datatable



            //create 5 rows of irrelevant information
            for (int i = 0; i < 0; i++)
            {
                DataRow dr = dt.NewRow();
                dt.Rows.Add(dr);
            }
            //add the datatable to the datasource
            ds.Tables.Add(dt);
            //just because it looks better on my screen
            //make this data the datasource of our gridview
            dataGridView2.DataSource = ds.Tables[0];
            ds.Tables.Remove(dt);//This works
        }

        private void ExportDgvToXML()
        {
            DataTable dt = (DataTable)dataGridView2.DataSource;
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "OTS|*.ots";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    dt.WriteXml(sfd.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void bunifuFlatButton6_Click_1(object sender, EventArgs e)
        {
            //dataGridView1.Rows.Clear();
            //XmlReader xmlFile = XmlReader.Create(@"C:\Users\Jed Zerna\Desktop\ggg.xml", new XmlReaderSettings());
            //DataSet dataSet = new DataSet();
            ////Read xml to dataset
            //dataSet.ReadXml(xmlFile);
            ////Pass empdetails table to datagridview datasource
            //dataGridView1.DataSource = dataSet.Tables["Table1"];
            ////Close xml reader
            //xmlFile.Close();
            //xmlFile.Close();
        }

        private void bunifuFlatButton6_Click_2(object sender, EventArgs e)
        {
            //dataGridView1.Rows.Clear();
            //XmlReader xmlFile = XmlReader.Create(@"C:\Users\Jed Zerna\Desktop\ggg.ots", new XmlReaderSettings());
            //DataSet dataSet = new DataSet();
            //Read xml to dataset
            //dataSet.ReadXml(xmlFile);
            //Pass empdetails table to datagridview datasource
            //dataGridView1.DataSource = dataSet.Tables["Table1"];
            //Close xml reader
            //xmlFile.Close();
            //xmlFile.Close();
        }
        DataSet dataSet = new DataSet();

        private void bunifuFlatButton6_Click_3(object sender, EventArgs e)
        {
           
        }
        string id;
        string lb = "{";
        string rb = "}";
        private void bunifuCustomTextbox2_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            id = "";
            id = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = string.Format("EMPCODE LIKE '{0}'", id);
            id = "";
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {


            for (int i = dataGridView2.Rows.Count - 1; i > -1; i--)
            {
                DataGridViewRow row = dataGridView2.Rows[i];
                if (row.Cells[0].Value == null || row.Cells[0].Value.ToString() == "")
                {
                    dataGridView2.Rows.RemoveAt(i);
                }
            }
            InsertDgvIntoForm();

            ExportDgvToXML();
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            Stream myStream = null;
            OpenFileDialog theDialog = new OpenFileDialog();
            theDialog.Title = "Open Text File";
            theDialog.Filter = "OTS files|*.ots";
            theDialog.InitialDirectory = @"C:\";
            if (theDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = theDialog.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            XmlReader xmlFile = XmlReader.Create(theDialog.FileName, new XmlReaderSettings());
                            dataSet.ReadXml(xmlFile);
                            dataGridView1.DataSource = dataSet.Tables["Table1"];
                            //Close xml reader
                            xmlFile.Close();
                            xmlFile.Close();
                            foreach (DataGridViewRow row in dataGridView1.Rows)
                            {
                                string date;
                                string rem = row.Cells[3].Value.ToString();
                                date = rem.Remove(rem.Length - 15, 15).Replace("-", "/");
                                DateTime DT = DateTime.Parse(row.Cells[3].Value.ToString().Trim());
                                row.Cells[3].Value = DT.ToString("MM/dd/yyyy");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            //dataGridView1.DataSource = null;
            //dt.Rows.Clear();
            //dataGridView1.Rows.Clear();
            load2();
            Cursor.Current = Cursors.WaitCursor;
            (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = string.Format("PERIOD = #{0:MM/dd/yyyy}#",
   guna2DateTimePicker2.Value);
            Cursor.Current = Cursors.Default;
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            try
            {
                //Show print dialog
                PrintDialog pd = new PrintDialog();
                PrintDocument doc = new PrintDocument();
                doc.PrintPage += Doc_PrintPage;
                pd.Document = doc;
                doc.PrinterSettings.PrinterName = Properties.Settings.Default.Printername; //printer name          
                doc.DefaultPageSettings.Landscape = false;

                //Call ShowDialog  
                if (guna2CheckBox1.Checked == false)
                {

                    if (pd.ShowDialog() == DialogResult.OK)
                    {

                        Properties.Settings.Default.Printername = doc.PrinterSettings.PrinterName;
                        Properties.Settings.Default.Save();
                        doc.Print();
                    }

                }
                else
                {
                    doc.Print();
                }
            }
            catch
            {
                guna2CheckBox1.Checked = false;
            }
        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {
            Pay_Slip p = new Pay_Slip();
            p.ShowDialog();
        }

        private void guna2Button7_Click(object sender, EventArgs e)
        {
            guna2TextBox1.Text = "";
            guna2DateTimePicker2.Value = DateTime.Today;
            Cursor.Current = Cursors.WaitCursor;
            if ((dataGridView1.DataSource as DataTable).DefaultView.RowFilter != null)
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = null;
            }

            //dataGridView1.DataSource = null;
            //dt.Rows.Clear();
            //dataGridView1.Rows.Clear();
            load2();
            Cursor.Current = Cursors.Default;
          

            //dataGridView1.Columns[0].DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            //dataGridView1.Columns[1].DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            //dataGridView1.Columns[2].DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            //dataGridView1.Columns[3].DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            //dataGridView1.Columns[4].DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            //dataGridView1.Columns[5].DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            //dataGridView1.Columns[6].DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            //dataGridView1.Columns[7].DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            //dataGridView1.Columns[8].DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            //dataGridView1.Columns[9].DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            //dataGridView1.Columns[10].DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            //dataGridView1.Columns[11].DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            //dataGridView1.Columns[12].DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            //dataGridView1.Columns[13].DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            //dataGridView1.Columns[14].DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            //dataGridView1.Columns[15].DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            //dataGridView1.Columns[16].DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            //dataGridView1.Columns[17].DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            //dataGridView1.Columns[18].DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            //dataGridView1.Columns[19].DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            //dataGridView1.Columns[20].DefaultCellStyle.Font = new Font("Segoe UI", 11, FontStyle.Bold);
        }

        private void bunifuDatepicker4_onValueChanged(object sender, EventArgs e)
        {

        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {
            (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = string.Format("EMPLOYEENAME LIKE '%{0}%'", guna2TextBox1.Text.Replace("[", lb).Replace("]", rb).Replace("​*", "[*​]").Replace("%", "[%]").Replace("'", "''"));
        }

        private void bunifuCustomLabel14_Click(object sender, EventArgs e)
        {
         
          
        }

        private void guna2CustomCheckBox10_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2CustomCheckBox10.Checked == true)
            {

                if (guna2CustomCheckBox10.Checked == false && guna2CustomCheckBox1.Checked == false && guna2CustomCheckBox2.Checked == false && guna2CustomCheckBox3.Checked == false && guna2CustomCheckBox4.Checked == false && guna2CustomCheckBox5.Checked == false && guna2CustomCheckBox6.Checked == false && guna2CustomCheckBox7.Checked == false && guna2CustomCheckBox8.Checked == false)
                {
                    dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
                else
                {
                    dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                }
                dataGridView1.Columns[4].Visible = true;
            }
            else
            {
                dataGridView1.Columns[4].Visible = false;
            }
        }

        private void guna2CustomCheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2CustomCheckBox1.Checked == true)
            {

                if (guna2CustomCheckBox10.Checked == false && guna2CustomCheckBox1.Checked == false && guna2CustomCheckBox2.Checked == false && guna2CustomCheckBox3.Checked == false && guna2CustomCheckBox4.Checked == false && guna2CustomCheckBox5.Checked == false && guna2CustomCheckBox6.Checked == false && guna2CustomCheckBox7.Checked == false && guna2CustomCheckBox8.Checked == false)
                {
                    dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
                else
                {
                    dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                }
                dataGridView1.Columns[5].Visible = true;
            }
            else
            {
                dataGridView1.Columns[5].Visible = false;
            }
        }

        private void guna2CustomCheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2CustomCheckBox2.Checked == true)
            {

                if (guna2CustomCheckBox10.Checked == false && guna2CustomCheckBox1.Checked == false && guna2CustomCheckBox2.Checked == false && guna2CustomCheckBox3.Checked == false && guna2CustomCheckBox4.Checked == false && guna2CustomCheckBox5.Checked == false && guna2CustomCheckBox6.Checked == false && guna2CustomCheckBox7.Checked == false && guna2CustomCheckBox8.Checked == false)
                {
                    dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
                else
                {
                    dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                }
                dataGridView1.Columns[6].Visible = true;
            }
            else
            {
                dataGridView1.Columns[6].Visible = false;
            }
        }

        private void guna2CustomCheckBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2CustomCheckBox3.Checked == true)
            {

                if (guna2CustomCheckBox10.Checked == false && guna2CustomCheckBox1.Checked == false && guna2CustomCheckBox2.Checked == false && guna2CustomCheckBox3.Checked == false && guna2CustomCheckBox4.Checked == false && guna2CustomCheckBox5.Checked == false && guna2CustomCheckBox6.Checked == false && guna2CustomCheckBox7.Checked == false && guna2CustomCheckBox8.Checked == false)
                {
                    dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
                else
                {
                    dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                }
                dataGridView1.Columns[7].Visible = true;
            }
            else
            {
                dataGridView1.Columns[7].Visible = false;
            }
        }

        private void guna2CustomCheckBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2CustomCheckBox4.Checked == true)
            {

                if (guna2CustomCheckBox10.Checked == false && guna2CustomCheckBox1.Checked == false && guna2CustomCheckBox2.Checked == false && guna2CustomCheckBox3.Checked == false && guna2CustomCheckBox4.Checked == false && guna2CustomCheckBox5.Checked == false && guna2CustomCheckBox6.Checked == false && guna2CustomCheckBox7.Checked == false && guna2CustomCheckBox8.Checked == false)
                {
                    dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
                else
                {
                    dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                }
                dataGridView1.Columns[8].Visible = true;
            }
            else
            {
                dataGridView1.Columns[8].Visible = false;
            }
        }

        private void guna2CustomCheckBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2CustomCheckBox5.Checked == true)
            {

                if (guna2CustomCheckBox10.Checked == false && guna2CustomCheckBox1.Checked == false && guna2CustomCheckBox2.Checked == false && guna2CustomCheckBox3.Checked == false && guna2CustomCheckBox4.Checked == false && guna2CustomCheckBox5.Checked == false && guna2CustomCheckBox6.Checked == false && guna2CustomCheckBox7.Checked == false && guna2CustomCheckBox8.Checked == false)
                {
                    dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
                else
                {
                    dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                }
                dataGridView1.Columns[9].Visible = true;
            }
            else
            {
                dataGridView1.Columns[9].Visible = false;
            }
        }

        private void guna2CustomCheckBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2CustomCheckBox6.Checked == true)
            {

                if (guna2CustomCheckBox10.Checked == false && guna2CustomCheckBox1.Checked == false && guna2CustomCheckBox2.Checked == false && guna2CustomCheckBox3.Checked == false && guna2CustomCheckBox4.Checked == false && guna2CustomCheckBox5.Checked == false && guna2CustomCheckBox6.Checked == false && guna2CustomCheckBox7.Checked == false && guna2CustomCheckBox8.Checked == false)
                {
                    dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
                else
                {
                    dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                }
                dataGridView1.Columns[10].Visible = true;
            }
            else
            {
                dataGridView1.Columns[10].Visible = false;
            }
        }

        private void guna2CustomCheckBox7_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2CustomCheckBox7.Checked == true)
            {

                if (guna2CustomCheckBox10.Checked == false && guna2CustomCheckBox1.Checked == false && guna2CustomCheckBox2.Checked == false && guna2CustomCheckBox3.Checked == false && guna2CustomCheckBox4.Checked == false && guna2CustomCheckBox5.Checked == false && guna2CustomCheckBox6.Checked == false && guna2CustomCheckBox7.Checked == false && guna2CustomCheckBox8.Checked == false)
                {
                    dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
                else
                {
                    dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                }
                dataGridView1.Columns[11].Visible = true;
            }
            else
            {
                dataGridView1.Columns[11].Visible = false;
            }
        }

        private void guna2CustomCheckBox8_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2CustomCheckBox8.Checked == true)
            {
                if (guna2CustomCheckBox10.Checked == false && guna2CustomCheckBox1.Checked == false && guna2CustomCheckBox2.Checked == false && guna2CustomCheckBox3.Checked == false && guna2CustomCheckBox4.Checked == false && guna2CustomCheckBox5.Checked == false && guna2CustomCheckBox6.Checked == false && guna2CustomCheckBox7.Checked == false && guna2CustomCheckBox8.Checked == false)
                {
                    dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
                else
                {
                    dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                }
                dataGridView1.Columns[19].Visible = true;
            }
            else
            {
                dataGridView1.Columns[19].Visible = false;
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {
            //base.Height = 100;
            //base.Width = 100;
            //this.Size = new Size(420, 200);

        }
    }
}
