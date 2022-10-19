using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pay_Slip
{
    public partial class payrollSettings : Form
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
        public payrollSettings()
        {
            InitializeComponent();
        }

        private void payrollSettings_Load(object sender, EventArgs e)
        {
            //dt.Columns.Add("ACCTDESC");
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                InitialDirectory = @"C:\",
                Title = "Browse DBF Files",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "txt",
                Filter = "DBF files (*.dbf)|*.dbf",
                FilterIndex = 2,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pathLOANS = openFileDialog1.FileName;
                guna2TextBox1.Text=Path.GetFileName(openFileDialog1.FileName);
                guna2Button2.Visible = true;
            }
        }
        string pathLOANS;
        string pathGLU2N;
        public string id;
        private void payrollSettings_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Hide();//Hide the 'current' form, i.e frm_form1 
                        //show another form ( frm_form2 )   
            payrollDashboard frm = new payrollDashboard();
            frm.id = id;
            frm.ShowDialog();
            //Close the form.(frm_form1)
            this.Dispose();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
            {
                string sqlTrunc = "TRUNCATE TABLE tblLOANS";
                SqlCommand cmd = new SqlCommand(sqlTrunc, tblOTS);
                tblOTS.Open();
                cmd.ExecuteNonQuery();
                tblOTS.Close();

                OleDbConnection oConn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + pathLOANS.Replace(guna2TextBox1.Text,"") + "';Extended Properties=dBase IV;");
                OleDbCommand command = new OleDbCommand("select * from LOANS.DBF", oConn);
                DataTable dt = new DataTable();
                oConn.Open();
                dt.Load(command.ExecuteReader());
                oConn.Close();
            
                DataTableReader reader = dt.CreateDataReader();
                tblOTS.Open();  ///this is my connection to the sql server
                SqlBulkCopy sbc = new SqlBulkCopy(tblOTS);
                sbc.DestinationTableName = "tblLOANS";  //copy the datatable to the sql table
                sbc.WriteToServer(dt);
                tblOTS.Close();
                reader.Close();

                SqlCommand cmd3 = new SqlCommand("update payrollSettings set dateupdated=@dateupdated where details=@details", tblOTS);
                tblOTS.Open();
                cmd3.Parameters.AddWithValue("@details", "LOANDBF");
                cmd3.Parameters.AddWithValue("@dateupdated", DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));
                cmd3.ExecuteNonQuery();
                tblOTS.Close();
            }
            MessageBox.Show("LOANS IS UPDATED");

        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                InitialDirectory = @"C:\",
                Title = "Browse DBF Files",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "txt",
                Filter = "DBF files (*.dbf)|*.dbf",
                FilterIndex = 2,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pathGLU2N = openFileDialog1.FileName;
                guna2TextBox2.Text = Path.GetFileName(openFileDialog1.FileName);
                guna2Button3.Visible = true;
            }
        }
        Thread thread;
        private void guna2Button3_Click(object sender, EventArgs e)
        {
            if (guna2TextBox2.Text != "GLU2N.DBF")
            {
                MessageBox.Show("Please select a valid GLU2N DBF. Thank You.");
                return;
            }



            importglu2n();

            

        }
        public DataTable dt = new DataTable();
        public DataTable sum = new DataTable();
        string d;
        string c;
        //public DataTable dtforimport = new DataTable();
        private void importglu2n()
        {
            dt.Rows.Clear();

            using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
            {
                OleDbConnection oConn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + pathGLU2N.Replace(guna2TextBox2.Text, "") + "';Extended Properties=dBase IV;");
                OleDbCommand command = new OleDbCommand("select * from GLU2N.DBF order by DATE asc", oConn);
                oConn.Open();
                dt.Load(command.ExecuteReader());
                sum.Load(command.ExecuteReader());
                //dtforimport.Load(command.ExecuteReader());
                oConn.Close();



                dt.DefaultView.Sort = "DATE ASC";



            }

            this.Hide();
            DashAnnual frm = new DashAnnual(this);
            frm.id = id;
            frm.label4.Text = d;
            frm.label5.Text = c;
            frm.form = "DATA";
            frm.ShowDialog();
            //Close the form.(frm_form1)
            this.Dispose();
            //MessageBox.Show("GLU2N IMPORTED SUCCESSFULLY");
            //    guna2Button3.Enabled = true;
        }
    }
}
