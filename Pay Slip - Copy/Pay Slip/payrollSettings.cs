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
                path = openFileDialog1.FileName;
                guna2TextBox1.Text=Path.GetFileName(openFileDialog1.FileName);
                guna2Button2.Visible = true;
            }
        }
        string path;
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

                OleDbConnection oConn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + path.Replace(guna2TextBox1.Text,"") + "';Extended Properties=dBase IV;");
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
    }
}
