using System;
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
    public partial class AnnualPrint : Form
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
        public AnnualPrint()
        {
            InitializeComponent();
        }

        private DashAnnual annualdash = null;
        public AnnualPrint(Form callingForm)
        {
            annualdash = callingForm as DashAnnual;
            InitializeComponent();
        }
        private void AnnualPrint_Load(object sender, EventArgs e)
        {
            this.reportViewer1.Reset();
            thread =
            new Thread(new ThreadStart(start1));
            thread.Start();
        }
        Thread thread;
        private void start1()
        {
            Cursor.Current = Cursors.WaitCursor;
            reportViewer1.BeginInvoke((Action)delegate ()
            {
                reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);
            });
            //Thread.Sleep(1000);



            string exeFolder = Path.GetDirectoryName(Application.ExecutablePath) + @"\AnnualReport.rdlc";
            ReportDataSource dr = new ReportDataSource("DataSet1", annualdash.annualprint);
            ReportDataSource dr1 = new ReportDataSource("DataSet2", annualdash.annualgrandtotalprint);
            ReportDataSource dr3 = new ReportDataSource("DataSet3", annualdash.annualtitleprint);
            this.reportViewer1.LocalReport.ReportPath = exeFolder;
            this.reportViewer1.LocalReport.DataSources.Add(dr);
            this.reportViewer1.LocalReport.DataSources.Add(dr1);
            this.reportViewer1.LocalReport.DataSources.Add(dr3);
            reportViewer1.BeginInvoke((Action)delegate ()
            {
                this.reportViewer1.LocalReport.Refresh();
                this.reportViewer1.RefreshReport();
            });
            thread.Abort();
        }
        //    private void start1()
        //{

        //    Cursor.Current = Cursors.WaitCursor;
        //    reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);


        //    string exeFolder = Path.GetDirectoryName(Application.ExecutablePath) + @"\AnnualReport.rdlc";
        //    ReportDataSource dr = new ReportDataSource("DataSet1", annualdash.annualprint);
        //    this.reportViewer1.LocalReport.ReportPath = exeFolder;
        //    this.reportViewer1.LocalReport.DataSources.Add(dr);
        //    this.reportViewer1.LocalReport.Refresh();

        //    //var deviceInfo = @"<DeviceInfo>
        //    //        <EmbedFonts>None</EmbedFonts>
        //    //       </DeviceInfo>";

        //    //byte[] bytes = rdlc.Render("PDF", deviceInfo);

        //    //byte[] Bytes = reportViewer1.LocalReport.Render(format: "PDF", deviceInfo);
        //    //DateTime fadate = DateTime.Now;
        //    //string ffdate = fadate.ToString("MM-dd-yyyy");
        //    //using (FileStream stream = new FileStream(tb1 + "\\Biometric " + ffdate.ToString() + ".pdf", FileMode.Create))
        //    //{
        //    //    stream.Write(Bytes, 0, Bytes.Length);
        //    //}
        //    //filename = "Biometric " + ffdate.ToString() + ".pdf";
        //    Cursor.Current = Cursors.Default;
        //}
    }
}
