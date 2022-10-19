using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
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
    public partial class workerspage : Form
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
        public workerspage()
        {
            InitializeComponent();
            //ApplyShadows(this);
            //this.FormBorderStyle = FormBorderStyle.None;
            //Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 15, 15));
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
        selection obj = (selection)Application.OpenForms["selection"];
        //DataTable dt = new DataTable();
        public DataTable dt = new DataTable();
        private void workerspage_Load(object sender, EventArgs e)
        {
          
            //System.Threading.Thread thread =
            //       new System.Threading.Thread(new System.Threading.ThreadStart(loadfirst));
            //thread.Start();
            loadfirst();
            rowsfound.Columns.Add("GROSS");
            rowsfound.Columns.Add("DAYS");
            rowsfound.Columns.Add("OT");
            rowsfound.Columns.Add("PROJECT");
        }
        private void loadfirst()
        {
            //obj.guna2ProgressBar1.BeginInvoke((Action)delegate ()
            //{
            //    obj.guna2ProgressBar1.Value = 60;
            //});
            //SuspendLayout();
            //dataGridView1.BeginInvoke((Action)delegate ()
            //{
                dataGridView1.ClearSelection();
            //});
            DateTime d = DateTime.Now;
            string date = d.ToString("dddd");
            //guna2DateTimePicker1.BeginInvoke((Action)delegate ()
            //{
                if (date == "Sunday")
                {
                    guna2DateTimePicker1.Text = d.AddDays(-4).ToString("MMM dd, yyyy");
                }
                else if (date == "Monday")
                {
                    guna2DateTimePicker1.Text = d.AddDays(-5).ToString("MMM dd, yyyy");
                }
                else if (date == "Tuesday")
                {
                    guna2DateTimePicker1.Text = d.AddDays(-6).ToString("MMM dd, yyyy");
                }
                else if (date == "Wednesday")
                {
                    guna2DateTimePicker1.Text = d.AddDays(-7).ToString("MMM dd, yyyy");
                }
                else if (date == "Thursday")
                {
                    guna2DateTimePicker1.Text = d.AddDays(-8).ToString("MMM dd, yyyy");
                }
                else if (date == "Friday")
                {
                    guna2DateTimePicker1.Text = d.AddDays(-2).ToString("MMM dd, yyyy");
                }
                else if (date == "Saturday")
                {
                    guna2DateTimePicker1.Text = d.AddDays(-3).ToString("MMM dd, yyyy");
                }
            //}); 
            //obj.guna2ProgressBar1.BeginInvoke((Action)delegate ()
            //{
            //    obj.guna2ProgressBar1.Value = 63;
            //});
            //MessageBox.Show(date);
            //string fdate = d.ToString("MMM dd, yyyy");



            try
            {
                load();
                //dataGridView1.DataSource = dt;


                string datenow;
                string datenow2;
                DateTime da = DateTime.Now;
                datenow = da.ToString("MM/dd/yyyy");
                DateTime oneYearBefore = DateTime.Now.AddYears(-2);
                datenow2 = oneYearBefore.ToString("MM/dd/yyyy");
                //obj.guna2ProgressBar1.BeginInvoke((Action)delegate ()
                //{
                //    obj.guna2ProgressBar1.Value = 69;
                //});

                dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
                dataGridView1.RowHeadersVisible = false;
                dataGridView1.Columns[5].Visible = false;
                dataGridView1.Columns[0].Visible = false;
                dataGridView1.Columns[6].Visible = false;
                dataGridView1.Columns[4].Visible = false;
                dataGridView1.Columns[7].Visible = false;
                dataGridView1.Columns[8].Visible = false;
                dataGridView1.Columns[9].Visible = false;
                dataGridView1.Columns[10].Visible = false;
                dataGridView1.Columns[11].Visible = false;
                dataGridView1.Columns[12].Visible = false;
                dataGridView1.Columns[18].Visible = false;
                dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                //obj.guna2ProgressBar1.BeginInvoke((Action)delegate ()
                //{
                //    obj.guna2ProgressBar1.Value = 75;
                //});
                int a = dataGridView1.Rows.Count;
                label7.Text = a.ToString();
            }
            catch
            {
                MessageBox.Show("Connection Failed. Please change the server settings to continue");
            }

            ChangeControlStyles(dataGridView1, ControlStyles.OptimizedDoubleBuffer, true);
            this.dataGridView1.ColumnHeadersDefaultCellStyle.SelectionBackColor = this.dataGridView1.ColumnHeadersDefaultCellStyle.BackColor;
            dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
            //load();
            ResumeLayout();
            //obj.guna2ProgressBar1.BeginInvoke((Action)delegate ()
            //{
            //    obj.guna2ProgressBar1.Value = 86;
            //});
        }
        private void ChangeControlStyles(Control ctrl, ControlStyles flag, bool value)
        {
            MethodInfo method = ctrl.GetType().GetMethod("SetStyle", BindingFlags.Instance | BindingFlags.NonPublic);
            if (method != null)
                method.Invoke(ctrl, new object[] { flag, value });
        }
        public void load()
        {
           
            //dataGridView1.BeginInvoke((Action)delegate ()
            //{
            using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
            {
               
                    dt.Rows.Clear();


                tblOTS.Open();

                string list = "SELECT id,EMPCODE,EMPLOYEENAME,PERIOD,COY,ACCTCODE,ACCTDESC,DAYRATE,OTRATE,NUMDAY,NUMHOUR,SSS,TITLE,GROSS,COOP,CA,SSSLOANS,OTHERS,SC,NET FROM tblFinalWorkers WHERE PERIOD = '" + guna2DateTimePicker1.Value + "' ORDER BY EMPLOYEENAME ASC";
                SqlCommand command = new SqlCommand(list, tblOTS);
                SqlDataReader reader = command.ExecuteReader();
                dt.Load(reader);

                dataGridView1.DataSource = dt;
                string datenow;
                string datenow2;
                DateTime d = DateTime.Now;
                datenow = d.ToString("MM/dd/yyyy");
                DateTime oneYearBefore = DateTime.Now.AddYears(-2);
                datenow2 = oneYearBefore.ToString("MM/dd/yyyy");
             

                    dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
                dataGridView1.RowHeadersVisible = false;
                tblOTS.Close();

                tblOTS.Dispose();
                dataGridView1.Columns[5].Visible = false;
                dataGridView1.Columns[0].Visible = false;
                dataGridView1.Columns[6].Visible = false;
                dataGridView1.Columns[4].Visible = false;
                dataGridView1.Columns[7].Visible = false;
                dataGridView1.Columns[8].Visible = false;
                dataGridView1.Columns[9].Visible = false;
                dataGridView1.Columns[10].Visible = false;
                dataGridView1.Columns[11].Visible = false;
                dataGridView1.Columns[12].Visible = false;
                dataGridView1.Columns[18].Visible = false;


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
                    //dataGridView1.Columns[19].DefaultCellStyle.Font = new Font("Segoe UI", 11, FontStyle.Bold);


                }
            int a = dataGridView1.Rows.Count;
            label7.Text = a.ToString(); 
          
        }

        private void bunifuCheckbox2_OnChange(object sender, EventArgs e)
        {
           
        }

        private void bunifuCheckbox1_OnChange(object sender, EventArgs e)
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

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
           
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
        
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
        private void print_Click(object sender, EventArgs e)
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
        private string name;
        private string salary;
        private string ssscontri;
        private string coop;
        private string casc;
        private string loan;
        private string Others;
        private string net;
        private string acctcode;
        private string hours;
        private string days;
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
                string text = "PAYEE NAME : " + name + "\n \n" +

                              "PROJECT: " + acctcode + "\n" +
                              "Basic Weekly Salary/Advances:  ₱" + salary + "\n" +
                              "No. of Days- " + days + "\n" +
                              "No. of Hrs (O.T.)- " + hours +

                              "\n\n" +
                              "Less Deduction: \n" +
                              "SSS Cont./HDMF/Philhealth: " + ssscontri + "\n" +
                              "Coop:                                        " + coop + "\n" +
                              "C / A, S/ C:                                " + casc + "\n" +
                              "SSS Loan:                                " + loan + "\n" +
                              "Others:                                      " + Others + "\n\n" +
                              "Net Take Home Pay:              ₱" + net;


                e.Graphics.DrawString(text, Font, brush, DisplayRectangle, stringFormat);
            }


            // -extra TEXT -end

        }
        private void bunifuFlatButton2_Click(object sender, EventArgs e)
        {
           

        }

        private void bunifuFlatButton1_Click(object sender, EventArgs e)
        {
            

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

        private void dataGridView1_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void bunifuFlatButton4_Click(object sender, EventArgs e)
        {
            
        }

        private void bunifuCheckbox11_OnChange(object sender, EventArgs e)
        {
           


        }

        private void bunifuCheckbox12_OnChange(object sender, EventArgs e)
        {
            if (guna2CheckBox3.Checked == true)
            {
                guna2CheckBox2.Checked = false;
                guna2TextBox2.PlaceholderText = "Employee Code:";
                guna2TextBox1.PlaceholderText = "Search Employee";
            }
            else
            {
                guna2TextBox2.PlaceholderText = "Code:";
                guna2TextBox1.PlaceholderText = "Search";
            }
        }

        string lb = "{";
        string rb = "}";
        private void bunifuCustomTextbox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void bunifuCustomLabel17_Click(object sender, EventArgs e)
        {

        }

        private void bunifuCustomTextbox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (guna2CheckBox2.Checked == true)
            {
                guna2TextBox2.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString().Trim();
                //bunifuCustomTextbox2.Text = dataGridView1.CurrentRow.Cells[6].Value.ToString();
                if (guna2CustomCheckBox10.Checked == false && guna2CustomCheckBox11.Checked == false && guna2CustomCheckBox1.Checked == false && guna2CustomCheckBox12.Checked == false && guna2CustomCheckBox2.Checked == false && guna2CustomCheckBox3.Checked == false && guna2CustomCheckBox4.Checked == false && guna2CustomCheckBox5.Checked == false && guna2CustomCheckBox9.Checked == false)
                {
                    dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
                else
                {
                    dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                }
                dataGridView1.Columns[5].Visible = true;
                dataGridView1.Columns[6].Visible = true;
            }
            else if (guna2CheckBox3.Checked == true)
            {
                guna2TextBox2.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString().Trim();
                //bunifuCustomTextbox2.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            }
        }

        private void bunifuFlatButton3_Click(object sender, EventArgs e)
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
            dataGridView1.DataSource = ds.Tables[0];
            ds.Tables.Remove(dt);//This works
        }

        private void ExportDgvToXML()
        {
            DataTable dt = (DataTable)dataGridView1.DataSource;
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "WRKS|*.wrks";
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
        DataSet dataset = new DataSet();
        private void bunifuFlatButton6_Click(object sender, EventArgs e)
        {
          
        }

        private void bunifuFlatButton7_Click(object sender, EventArgs e)
        {

        }

        private void bunifuFlatButton7_Click_1(object sender, EventArgs e)
        {

        }

        private void bunifuFlatButton8_Click(object sender, EventArgs e)
        {

        }

        private void bunifuCustomLabel15_Click(object sender, EventArgs e)
        {

        }

        private void guna2DateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            //dt.Rows.Clear();
            load();
            guna2CheckBox2_CheckedChanged(sender, e);


        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {
            if (guna2CheckBox2.Checked == true)
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = string.Format("ACCTDESC LIKE '%{0}%'", guna2TextBox1.Text.Replace("[", lb).Replace("]", rb).Replace("​*", "[*​]").Replace("%", "[%]").Replace("'", "''"));

                int a = dataGridView1.Rows.Count;
                label7.Text = a.ToString();
            }
            else if (guna2CheckBox3.Checked == true)
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = string.Format("EMPLOYEENAME LIKE '%{0}%'", guna2TextBox1.Text.Replace("[", lb).Replace("]", rb).Replace("​*", "[*​]").Replace("%", "[%]").Replace("'", "''"));

                int a = dataGridView1.Rows.Count;
                label7.Text = a.ToString();
            }
            else
            {
                MessageBox.Show("Please select what do you want to search...");
            }
        }

        private void guna2TextBox2_TextChanged(object sender, EventArgs e)
        {
            if (guna2TextBox2.Text == "")
            {
                if ((dataGridView1.DataSource as DataTable).DefaultView.RowFilter != null || (dataGridView1.DataSource as DataTable).DefaultView.RowFilter != string.Empty)
                {
                    (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = null;
                }
            }
            else
            {
                if (guna2CheckBox2.Checked == true)
                {
                    (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = string.Format("ACCTCODE LIKE '{0}'", guna2TextBox2.Text.Replace("[", lb).Replace("]", rb).Replace("​*", "[*​]").Replace("%", "[%]").Replace("'", "''"));

                    int a = dataGridView1.Rows.Count;
                    label7.Text = a.ToString();
                }
                else if (guna2CheckBox3.Checked == true)
                {
                    (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = string.Format("EMPCODE LIKE '{0}'", guna2TextBox2.Text.Replace("[", lb).Replace("]", rb).Replace("​*", "[*​]").Replace("%", "[%]").Replace("'", "''"));

                    int a = dataGridView1.Rows.Count;
                    label7.Text = a.ToString();
                }
                else
                {
                    MessageBox.Show("Please select what do you want to search...");
                }
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            try
            {
                for (int intI = dataGridView1.Rows.Count - 2; intI >= 0; intI += -1)
                {
                    for (var intJ = intI - 1; intJ >= 0; intJ += -1)
                    {
                        if (dataGridView1.Rows[intI].Cells["empcode"].Value == dataGridView1.Rows[intJ].Cells["empcode"].Value)
                        {
                            dataGridView1.Rows[intI].Visible = false; 
                            //break;
                        }
                    }
                }

            }
            catch (Exception ex) { string errormsg = ex.ToString(); }
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                try
                {
                    foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                    {
                        name = "";
                        salary = "";
                        ssscontri = "";
                        coop = "";
                        loan = "";
                        Others = "";
                        net = "";
                        acctcode = "";
                        hours = "";
                        days = "";
                        casc = "";
                        name = row.Cells[2].Value.ToString();
                        salary = row.Cells[13].Value.ToString();
                        ssscontri = row.Cells[11].Value.ToString();
                        coop = row.Cells[14].Value.ToString();
                        loan = row.Cells[16].Value.ToString();
                        Others = row.Cells[17].Value.ToString();
                        net = row.Cells[19].Value.ToString();
                        acctcode = row.Cells[6].Value.ToString();
                        hours = row.Cells[10].Value.ToString();
                        days = row.Cells[9].Value.ToString();
                        decimal sum = 0.00M;
                        sum += Math.Round(Convert.ToDecimal(row.Cells[15].Value.ToString()), 2) + Math.Round(Convert.ToDecimal(row.Cells[18].Value.ToString()), 2);
                        casc = sum.ToString();
                    }
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
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            //foreach (DataGridViewRow rows in dataGridView1.Rows)//finding pair
            //{
                //dataGridView1.ClearSelection();
                //int rowIndex = rows.Index;
                //dataGridView1.Rows[rowIndex].Selected = true;
                //MessageBox.Show(rows.Selected.ToString());
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    name = "";
                    salary = "";
                    ssscontri = "";
                    coop = "";
                    loan = "";
                    Others = "";
                    net = "";
                    acctcode = "";
                    hours = "";
                    days = "";
                    name = row.Cells[2].Value.ToString();
                    salary = row.Cells[13].Value.ToString();
                    ssscontri = row.Cells[11].Value.ToString();
                    coop = row.Cells[14].Value.ToString();
                    loan = row.Cells[16].Value.ToString();
                    Others = row.Cells[17].Value.ToString();
                    net = row.Cells[19].Value.ToString();
                    acctcode = row.Cells[6].Value.ToString();
                    hours = row.Cells[10].Value.ToString();
                    days = row.Cells[9].Value.ToString();

                    decimal sum = 0.00M;
                    sum += Math.Round(Convert.ToDecimal(row.Cells[15].Value.ToString()), 2) + Math.Round(Convert.ToDecimal(row.Cells[18].Value.ToString()), 2);
                    casc = sum.ToString();
                print_Click(sender, e);
            }
            //}
        }
        DataTable rowsfound = new DataTable();
        private void guna2Button4_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                {
                    name = "";
                    salary = "";
                    ssscontri = "";
                    coop = "";
                    loan = "";
                    Others = "";
                    net = "";
                    acctcode = "";
                    hours = "";
                    days = "";
                    casc = "";
                    name = row.Cells[2].Value.ToString();
                    salary = row.Cells[13].Value.ToString();
                    ssscontri = row.Cells[11].Value.ToString();
                    coop = row.Cells[14].Value.ToString();
                    loan = row.Cells[16].Value.ToString();
                    Others = row.Cells[17].Value.ToString();
                    net = row.Cells[19].Value.ToString();
                    acctcode = row.Cells[6].Value.ToString();
                    hours = row.Cells[10].Value.ToString();
                    days = row.Cells[9].Value.ToString();
                    decimal sum = 0.00M;
                    sum += Math.Round(Convert.ToDecimal(row.Cells[15].Value.ToString()), 2) + Math.Round(Convert.ToDecimal(row.Cells[18].Value.ToString()), 2);
                    casc = sum.ToString();
                }
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

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            workerspayslip w = new workerspayslip();
            w.ShowDialog();
        }
        Thread thread;
        private void guna2Button6_Click(object sender, EventArgs e)
        {

            thread =
                   new System.Threading.Thread(new System.Threading.ThreadStart(reload));
            thread.Start();
        }
        private void reload()
        {
            guna2TextBox2.BeginInvoke((Action)delegate ()
            {
                guna2TextBox1.BeginInvoke((Action)delegate ()
                {
                    dataGridView1.BeginInvoke((Action)delegate ()
                    {
                        guna2TextBox2.Text = "";
                        guna2TextBox1.Text = "";
                        Cursor.Current = Cursors.WaitCursor;
                        (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = null;
                        //dataGridView1.DataSource = null;
                      
                        //dataGridView1.Rows.Clear();
                        load();
                        Cursor.Current = Cursors.WaitCursor;

                    });
                });
            });


            thread.Abort();
            guna2CustomCheckBox10.Checked = false;
                        guna2CustomCheckBox11.Checked = false;
                        guna2CustomCheckBox1.Checked = false;
                        guna2CustomCheckBox12.Checked = false;
                        guna2CustomCheckBox2.Checked = false;
                        guna2CustomCheckBox3.Checked = false;
                        guna2CustomCheckBox4.Checked = false;
                        guna2CustomCheckBox5.Checked = false;
                        guna2CustomCheckBox9.Checked = false;
                        guna2CheckBox2.Checked = false;
                        guna2CheckBox3.Checked = false;
          
        }

        private void guna2CheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2CheckBox2.Checked == true)
            {
                //dataGridView1.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[6].Width = 300;
                guna2CheckBox3.Checked = false;
                guna2TextBox2.PlaceholderText = "Site Code:";
                guna2TextBox1.PlaceholderText = "Search Site";
                guna2CustomCheckBox11.Checked = true;
                if (guna2CustomCheckBox10.Checked == false && guna2CustomCheckBox11.Checked == false && guna2CustomCheckBox1.Checked == false && guna2CustomCheckBox12.Checked == false && guna2CustomCheckBox2.Checked == false && guna2CustomCheckBox3.Checked == false && guna2CustomCheckBox4.Checked == false && guna2CustomCheckBox5.Checked == false && guna2CustomCheckBox9.Checked == false)
                {
                    dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
                else
                {
                    dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                }
                dataGridView1.Columns[5].Visible = true;
                dataGridView1.Columns[6].Visible = true;
            }
            else
            {
                dataGridView1.Columns[6].Width = 100;
                guna2TextBox2.PlaceholderText = "Code:";
                guna2TextBox1.PlaceholderText = "Search";
                guna2CustomCheckBox11.Checked = false;
                dataGridView1.Columns[5].Visible = false;
                dataGridView1.Columns[6].Visible = false;
            }
        }

        private void guna2CheckBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2CheckBox3.Checked == true)
            {
                guna2CheckBox2.Checked = false;
                guna2TextBox2.PlaceholderText = "Employee Code:";
                guna2TextBox1.PlaceholderText = "Search Employee";
            }
            else
            {
                guna2TextBox2.PlaceholderText = "Code:";
                guna2TextBox1.PlaceholderText = "Search";
            }
        }

        private void guna2CustomCheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void guna2CustomCheckBox2_CheckedChanged(object sender, EventArgs e)
        {
           
        }

        private void guna2CustomCheckBox3_CheckedChanged(object sender, EventArgs e)
        {
          
        }

        private void guna2CustomCheckBox4_CheckedChanged(object sender, EventArgs e)
        {
           
        }

        private void guna2CustomCheckBox5_CheckedChanged(object sender, EventArgs e)
        {
           
        }

        private void guna2CustomCheckBox6_CheckedChanged(object sender, EventArgs e)
        {
          
        }

        private void guna2CustomCheckBox7_CheckedChanged(object sender, EventArgs e)
        {
           
        }

        private void guna2CustomCheckBox8_CheckedChanged(object sender, EventArgs e)
        {
    
        }

        private void guna2CustomCheckBox9_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2CustomCheckBox9.Checked == true)
            {

                if (guna2CustomCheckBox10.Checked == false && guna2CustomCheckBox11.Checked == false && guna2CustomCheckBox1.Checked == false && guna2CustomCheckBox12.Checked == false && guna2CustomCheckBox2.Checked == false && guna2CustomCheckBox3.Checked == false && guna2CustomCheckBox4.Checked == false && guna2CustomCheckBox5.Checked == false && guna2CustomCheckBox9.Checked == false)
                {
                    dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
                else
                {
                    dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                }
                dataGridView1.Columns[18].Visible = true;
            }
            else
            {
                dataGridView1.Columns[18].Visible = false;
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            Deletion d = new Deletion();
            d.ShowDialog();
        }

        private void guna2Button2_Click_1(object sender, EventArgs e)
        {
            for (int i = dataGridView1.Rows.Count - 1; i > -1; i--)
            {
                DataGridViewRow row = dataGridView1.Rows[i];
                if (row.Cells[0].Value == null || row.Cells[0].Value.ToString() == "")
                {
                    dataGridView1.Rows.RemoveAt(i);
                }
            }
            InsertDgvIntoForm();


            ExportDgvToXML();
        }

        private void guna2Button7_Click(object sender, EventArgs e)
        {
            Stream myStream = null;
            OpenFileDialog theDialog = new OpenFileDialog();
            theDialog.Title = "Open Text File";
            theDialog.Filter = "WRKS files|*.wrks";
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
                            dataset.ReadXml(xmlFile);
                            dataGridView1.DataSource = dataset.Tables["Table1"];
                            //Close xml reader
                            xmlFile.Close();
                            xmlFile.Close();
                            foreach (DataGridViewRow row in dataGridView1.Rows)
                            {
                                if (row.Cells[3].Value != null || row.Cells[3].Value.ToString() != "")
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
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void guna2CustomCheckBox10_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2CustomCheckBox10.Checked == true)
            {

                if (guna2CustomCheckBox10.Checked == false && guna2CustomCheckBox11.Checked == false && guna2CustomCheckBox1.Checked == false && guna2CustomCheckBox12.Checked == false && guna2CustomCheckBox2.Checked == false && guna2CustomCheckBox3.Checked == false && guna2CustomCheckBox4.Checked == false && guna2CustomCheckBox5.Checked == false && guna2CustomCheckBox9.Checked == false)
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

        private void guna2CustomCheckBox11_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2CustomCheckBox11.Checked == true)
            {

                if (guna2CustomCheckBox10.Checked == false && guna2CustomCheckBox11.Checked == false && guna2CustomCheckBox1.Checked == false && guna2CustomCheckBox12.Checked == false && guna2CustomCheckBox2.Checked == false && guna2CustomCheckBox3.Checked == false && guna2CustomCheckBox4.Checked == false && guna2CustomCheckBox5.Checked == false && guna2CustomCheckBox9.Checked == false)
                {
                    dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
                else
                {
                    dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                }
                dataGridView1.Columns[5].Visible = true;
                dataGridView1.Columns[6].Visible = true;
                dataGridView1.Columns["projcode"].Width = 300;
            }
            else
            {
                dataGridView1.Columns[5].Visible = false;
                dataGridView1.Columns[6].Visible = false;
            }
        }

        private void guna2CustomCheckBox1_CheckedChanged_1(object sender, EventArgs e)
        {
            if (guna2CustomCheckBox1.Checked == true)
            {

                if (guna2CustomCheckBox10.Checked == false && guna2CustomCheckBox11.Checked == false && guna2CustomCheckBox1.Checked == false && guna2CustomCheckBox12.Checked == false && guna2CustomCheckBox2.Checked == false && guna2CustomCheckBox3.Checked == false && guna2CustomCheckBox4.Checked == false && guna2CustomCheckBox5.Checked == false && guna2CustomCheckBox9.Checked == false)
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

        private void guna2CustomCheckBox12_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2CustomCheckBox12.Checked == true)
            {

                if (guna2CustomCheckBox10.Checked == false && guna2CustomCheckBox11.Checked == false && guna2CustomCheckBox1.Checked == false && guna2CustomCheckBox12.Checked == false && guna2CustomCheckBox2.Checked == false && guna2CustomCheckBox3.Checked == false && guna2CustomCheckBox4.Checked == false && guna2CustomCheckBox5.Checked == false && guna2CustomCheckBox9.Checked == false)
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

        private void guna2CustomCheckBox2_CheckedChanged_1(object sender, EventArgs e)
        {
            if (guna2CustomCheckBox2.Checked == true)
            {

                if (guna2CustomCheckBox10.Checked == false && guna2CustomCheckBox11.Checked == false && guna2CustomCheckBox1.Checked == false && guna2CustomCheckBox12.Checked == false && guna2CustomCheckBox2.Checked == false && guna2CustomCheckBox3.Checked == false && guna2CustomCheckBox4.Checked == false && guna2CustomCheckBox5.Checked == false && guna2CustomCheckBox9.Checked == false)
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

        private void guna2CustomCheckBox3_CheckedChanged_1(object sender, EventArgs e)
        {
            if (guna2CustomCheckBox3.Checked == true)
            {

                if (guna2CustomCheckBox10.Checked == false && guna2CustomCheckBox11.Checked == false && guna2CustomCheckBox1.Checked == false && guna2CustomCheckBox12.Checked == false && guna2CustomCheckBox2.Checked == false && guna2CustomCheckBox3.Checked == false && guna2CustomCheckBox4.Checked == false && guna2CustomCheckBox5.Checked == false && guna2CustomCheckBox9.Checked == false)
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

        private void guna2CustomCheckBox4_CheckedChanged_1(object sender, EventArgs e)
        {
            if (guna2CustomCheckBox4.Checked == true)
            {

                if (guna2CustomCheckBox10.Checked == false && guna2CustomCheckBox11.Checked == false && guna2CustomCheckBox1.Checked == false && guna2CustomCheckBox12.Checked == false && guna2CustomCheckBox2.Checked == false && guna2CustomCheckBox3.Checked == false && guna2CustomCheckBox4.Checked == false && guna2CustomCheckBox5.Checked == false && guna2CustomCheckBox9.Checked == false)
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

        private void guna2CustomCheckBox5_CheckedChanged_1(object sender, EventArgs e)
        {
            if (guna2CustomCheckBox5.Checked == true)
            {

                if (guna2CustomCheckBox10.Checked == false && guna2CustomCheckBox11.Checked == false && guna2CustomCheckBox1.Checked == false && guna2CustomCheckBox12.Checked == false && guna2CustomCheckBox2.Checked == false && guna2CustomCheckBox3.Checked == false && guna2CustomCheckBox4.Checked == false && guna2CustomCheckBox5.Checked == false && guna2CustomCheckBox9.Checked == false)
                {
                    dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
                else
                {
                    dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                }
                dataGridView1.Columns[12].Visible = true;
            }
            else
            {
                dataGridView1.Columns[12].Visible = false;
            }
        }
    }
  }
