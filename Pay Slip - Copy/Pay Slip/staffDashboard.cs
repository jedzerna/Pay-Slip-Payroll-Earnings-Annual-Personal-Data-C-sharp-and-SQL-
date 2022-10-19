using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pay_Slip
{
    public partial class staffDashboard : Form
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
        public staffDashboard()
        {
            InitializeComponent();
        }

        private void staffDashboard_Load(object sender, EventArgs e)
        {
            load();
            ChangeControlStyles(dataGridView2, ControlStyles.OptimizedDoubleBuffer, true);
            this.dataGridView2.ColumnHeadersDefaultCellStyle.SelectionBackColor = this.dataGridView2.ColumnHeadersDefaultCellStyle.BackColor;
            dataGridView2.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
        }
        private void ChangeControlStyles(Control ctrl, ControlStyles flag, bool value)
        {
            MethodInfo method = ctrl.GetType().GetMethod("SetStyle", BindingFlags.Instance | BindingFlags.NonPublic);
            if (method != null)
                method.Invoke(ctrl, new object[] { flag, value });
        }
        public string type;
        public void load()
        {
            using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
            {
                dataGridView2.Rows.Clear();
                DataTable distinct = new DataTable();
                tblOTS.Open();
                string list = "SELECT EMPCODE,EMPLOYEENAME,STATUS FROM tblStaffData ORDER BY EMPLOYEENAME ASC";
                SqlDataAdapter command = new SqlDataAdapter(list, tblOTS);
                command.Fill(distinct);

                dataGridView2.DataSource = distinct;
                tblOTS.Close();
            }
            dataGridView2.ClearSelection();
            label12.Text = "ROWS FOUND: "+ dataGridView2.Rows.Count.ToString();
        }
        public string name;
        private void guna2Button4_Click(object sender, EventArgs e)
        {
            staffAdd frm = new staffAdd();
            frm.name = name;
            frm.id = id;
            frm.type = type;
            frm.ShowDialog();
        }

        private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            staffInformations frm = new staffInformations();
            frm.empcode = dataGridView2.CurrentRow.Cells["EMPCODE"].Value.ToString();
            frm.ShowDialog();
        }
        public string id;
        private void staffDashboard_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Hide();
            payrollDashboard frm = new payrollDashboard();
            frm.id = id;
            frm.ShowDialog();
            this.Close();
        }

        private void guna2TextBox2_TextChanged(object sender, EventArgs e)
        {
            (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = string.Format("EMPCODE LIKE '%{0}%'", guna2TextBox2.Text);
        }
    }
}
