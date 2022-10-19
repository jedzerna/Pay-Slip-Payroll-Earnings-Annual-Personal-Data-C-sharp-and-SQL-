using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pay_Slip
{
    public partial class selection : Form
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
        public selection()
        {
            InitializeComponent();
            ApplyShadows(this);
            //typeof(Panel).InvokeMember("DoubleBuffered", BindingFlags.SetProperty
            //| BindingFlags.Instance | BindingFlags.NonPublic, null,
            //panel2, new object[] { true });


            //typeof(Panel).InvokeMember("DoubleBuffered", BindingFlags.SetProperty
            //| BindingFlags.Instance | BindingFlags.NonPublic, null,
            //panel3, new object[] { true });
            //this.FormBorderStyle = FormBorderStyle.None;
        }

        private void bunifuFlatButton2_Click(object sender, EventArgs e)
        {
        
        }
        Thread thread;
        private void selection_Load(object sender, EventArgs e)
        {
            
            
            //guna2Transition1.ShowSync(panel2,true,Guna.UI2.AnimatorNS.Animation.Transparent);
            //using (SqlConnection tblSupplier = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
            //{
            //    try
            //    {

            //        //var path = Environment.GetEnvironmentVariable("ProgramFiles(x86)") + @"\GLUConstInc\Jr Stocks Inventory\AppUpdator";
            //        ////MessageBox.Show(path + "\AppUpdator.exe");
            //        //Process.Start(path + "\\AppUpdator.exe");

            //        string fileName1 = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location).ToString() + "\\psver.vsn";
            //        //string fileName1 = Environment.GetEnvironmentVariable("ProgramFiles(x86)") + @"\GLUConstInc\Jr Stocks Inventory\psver.vsn";


            //        version = "";
            //        if (File.Exists(fileName1))
            //        {
            //            // Read entire text file content in one string    
            //            string text = File.ReadAllText(fileName1);
            //            //MessageBox.Show(text.ToString().Trim());
            //            version = text.ToString().Trim();
            //        }
            //        //MessageBox.Show(version);
            //        tblSupplier.Open();
            //        String query = "SELECT * FROM tblUpdate where id = 1";
            //        SqlCommand cmd = new SqlCommand(query, tblSupplier);
            //        SqlDataReader dr = cmd.ExecuteReader();

            //        if (dr.Read())
            //        {

            //            if (dr["version"].ToString() != version)
            //            {
            //                //MessageBox.Show(version +"  ||  "+ dr["version"].ToString() + " Correct");
            //                MessageBox.Show("Updates are available!!!");
            //                var path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location).ToString() + "\\AppUpdator";
            //                //MessageBox.Show(path + "\AppUpdator.exe");
            //                Process.Start(path + "\\PSAppUpdator.exe");

            //                Process[] workers = Process.GetProcessesByName("Pay Slip");
            //                foreach (Process worker in workers)
            //                {
            //                    worker.Kill();
            //                    worker.WaitForExit();
            //                    worker.Dispose();
            //                }
            //            }
            //        }
            //        dr.Close();
            //        tblSupplier.Close();

            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show(ex.Message + "  Please contact service provider!ASAP");
            //        Process[] workers = Process.GetProcessesByName("Pay Slip");
            //        foreach (Process worker in workers)
            //        {
            //            worker.Kill();
            //            worker.WaitForExit();
            //            worker.Dispose();
            //        }
            //    }

            //}


            //string AssemblyPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location).ToString();
            //MessageBox.Show(AssemblyPath);
            thread =
                   new System.Threading.Thread(new System.Threading.ThreadStart(load));
            thread.Start();

        }
        private void load()
        {
            guna2Transition1.ShowSync(label1);
            label1.BeginInvoke((Action)delegate ()
            {
                label1.Visible = true;
            });
            guna2Transition1.ShowSync(guna2Button4);
            guna2Button4.BeginInvoke((Action)delegate ()
            {
                guna2Button4.Visible = true;
            });
            guna2Transition1.ShowSync(guna2Button5);
            guna2Button5.BeginInvoke((Action)delegate ()
            {
                guna2Button5.Visible = true;
            });
            guna2Transition1.ShowSync(guna2Button8);
            guna2Button8.BeginInvoke((Action)delegate ()
            {
                guna2Button8.Visible = true;
            });
            guna2Transition1.ShowSync(guna2Button1);
            guna2Button1.BeginInvoke((Action)delegate ()
            {
                guna2Button1.Visible = true;
            });
            guna2Transition1.ShowSync(guna2Panel1);
            guna2Panel1.BeginInvoke((Action)delegate ()
            {
                guna2Panel1.Visible = true;
            });

            //try
            //{
            //    using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
            //    {
            //        tblOTS.Open();
            //        tblOTS.Close();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message +Environment.NewLine + Environment.NewLine + Environment.NewLine + "Please make sure Jed-PC is turned ON!!");
            //    this.Dispose();
            //}




            //using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
            //{
            //    tblOTS.Open();
            //    string list = "SELECT id,EMPCODE,EMPLOYEENAME,PERIOD,COY,ACCTCODE,ACCTDESC,DAYRATE,OTRATE,NUMDAY,NUMHOUR,SSS,TITLE,GROSS,COOP,CA,SSSLOANS,OTHERS,SC,NET FROM tblFinalWorkers WHERE PERIOD = '" + fdate + "' ORDER BY EMPLOYEENAME ASC";
            //    SqlCommand command = new SqlCommand(list, tblOTS);
            //    SqlDataReader reader = command.ExecuteReader();
            //    dtWorkers.Load(reader);
            //    tblOTS.Close();
            //}

            //using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
            //{
            //    tblOTS.Open();
            //    string list = "SELECT EMPCODE,EMPLOYEENAME,PERIOD,COY,PROJCODE,DAYRATE,OTRATE,NUMDAY,NUMHOUR,SSS,TITLE,INT,GROSS,PERIODD,ADV,COOP,SSSLOANS,OTHERS,HDMFCONT,TOTALNET FROM tblFinalOTS ORDER BY PERIOD DESC,EMPLOYEENAME ASC";
            //    SqlCommand command = new SqlCommand(list, tblOTS);
            //    SqlDataReader reader = command.ExecuteReader();
            //    dtOTS.Load(reader);
            //    tblOTS.Close();
            //}




        }
        DataTable dtWorkers = new DataTable();
        DataTable dtOTS = new DataTable();
        private void cmbProspecten_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
         
        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {

        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {

        }

        private void selection_Paint(object sender, PaintEventArgs e)
        {
         
        }

        private void bunifuFlatButton3_Click(object sender, EventArgs e)
        {
         
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
    
        }

        private void bunifuFlatButton8_Click(object sender, EventArgs e)
        {
     
        }

        private void bunifuFlatButton7_Click(object sender, EventArgs e)
        {
          
        }

        private void bunifuCustomLabel5_Click(object sender, EventArgs e)
        {
          
        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {
           
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            //guna2Transition1.HideSync(label5);
            //guna2Transition1.HideSync(guna2ShadowPanel2);
            //guna2Transition1.HideSync(guna2ShadowPanel3);
            //guna2Transition1.HideSync(guna2ShadowPanel4);
            //guna2Transition1.HideSync(guna2ShadowPanel5);
            //this.Hide();//Hide the 'current' form, i.e frm_form1 
            //show another form ( frm_form2 )   
            projects frm = new projects();
            frm.ShowDialog();
            //guna2Transition1.ShowSync(label1);
            //guna2Transition1.ShowSync(guna2Button4);
            //guna2Transition1.ShowSync(guna2Button5);
            //guna2Transition1.ShowSync(guna2Button7);
            //guna2Transition1.ShowSync(guna2Button8);
        }

        private void label5_Click(object sender, EventArgs e)
        {


        }
        string form = "";
        private void guna2Button4_Click(object sender, EventArgs e)
        {
            guna2Panel1.FillColor = Color.FromArgb(237, 237, 236);
            guna2Panel1.Controls.Clear();
            guna2Transition1.HideSync(label1);

            label1.Text = "OTS Pay Slip";

            guna2Transition1.ShowSync(label1);
            payslipform pr = new payslipform();
            pr.TopLevel = false;

            guna2Panel1.Controls.Add(pr);

            guna2Panel1.AutoScroll = false;
            pr.SetBounds(10, 6, guna2Panel1.Width - 21, guna2Panel1.Height - 10);
            pr.BringToFront();

            pr.Show();
            form = "OTS";
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            guna2Panel1.FillColor = Color.FromArgb(237, 237, 236);
            guna2Panel1.Controls.Clear();
            guna2Transition1.HideSync(label1);
            label1.Text = "Worker's Pay Slip";
            guna2Transition1.ShowSync(label1);
            workerspage pr = new workerspage();
            pr.TopLevel = false;
            guna2Panel1.Controls.Add(pr);
            guna2Panel1.AutoScroll = false;
            pr.SetBounds(10, 6, guna2Panel1.Width - 21, guna2Panel1.Height - 10);
            pr.BringToFront();

            pr.Show();
            form = "WORKER";
        }

        private void guna2Button7_Click(object sender, EventArgs e)
        {
            employees frm = new employees();
            frm.ShowDialog();
        }

        private void guna2Button8_Click(object sender, EventArgs e)
        {
            projects frm = new projects();
            frm.ShowDialog();
        }

        private void guna2Button9_Click(object sender, EventArgs e)
        {

         
            //panel3.Visible = false;

            ////});
            ////panel2.BeginInvoke((Action)delegate ()
            ////{
            //    panel2.Visible = true;




        }

        private void guna2Button10_Click(object sender, EventArgs e)
        {
         
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            //WindowState = FormWindowState.Minimized;
        }

        private void selection_SizeChanged(object sender, EventArgs e)
        {
            SuspendLayout();
            if (form == "OTS")
            {
                guna2Button4_Click(sender, e);
            }
            else if (form == "WORKER")
            {
                guna2Button5_Click(sender, e);
            }
            ResumeLayout();
        }

        private void guna2Panel1_SizeChanged(object sender, EventArgs e)
        {
            //payslipform a = new payslipform();
            //a.Size = new Size(this.guna2Panel1.Width - 21, this.guna2Panel1.Height - 10);

        }

        private void selection_ResizeEnd(object sender, EventArgs e)
        {
          
        }
        public string id;
        private void guna2Button1_Click_1(object sender, EventArgs e)
        {
            this.Hide();//Hide the 'current' form, i.e frm_form1 
                        //show another form ( frm_form2 )   
            payrollDashboard frm = new payrollDashboard();
            frm.id = id;
            frm.ShowDialog();
            //Close the form.(frm_form1)
            this.Dispose();
        }
    }
}


