using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Pay_Slip
{
    public partial class ComputerList : Form
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
        public ComputerList()
        {
            InitializeComponent();
        }

        private void ComputerList_Load(object sender, EventArgs e)
        {
            loadstocks();
        }
        public void loadstocks()
        {
            DataTable items = new DataTable();
            using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
            {
                tblOTS.Open();
                string list = "Select * from tblComputerTransactions order by id desc";
                SqlCommand command = new SqlCommand(list, tblOTS);
                SqlDataReader reader = command.ExecuteReader();
                items.Load(reader);
                tblOTS.Close();
            }
            dataGridView1.DataSource = items;
        }
        private void ComputerList_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Hide();
            payrollDashboard frm = new payrollDashboard();
            frm.id = id;
            frm.ShowDialog();
            this.Close();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            ComputerSupplyForm frm = new ComputerSupplyForm();
            frm.userid = id;
            frm.ShowDialog();
            this.Close();
        }
    }
}
