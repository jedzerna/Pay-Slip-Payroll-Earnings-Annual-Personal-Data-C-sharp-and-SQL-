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
    public partial class systemAccounts : Form
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
        public systemAccounts()
        {
            InitializeComponent();
        }
        public string name;
        private void systemAccounts_Load(object sender, EventArgs e)
        {
            load();
            dataGridView2.ClearSelection();
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
        public string id;
        public void load()
        {
            using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
            {
                //dataGridView2.Rows.Clear();
                DataTable distinct = new DataTable();
                tblOTS.Open();
                string list = "SELECT id,username,fullname,type,status FROM systemAccounts ORDER BY fullname ASC";
                SqlDataAdapter command = new SqlDataAdapter(list, tblOTS);
                command.Fill(distinct);

                dataGridView2.DataSource = distinct;
                tblOTS.Close();
            }
            label12.Text = "ROWS FOUND: " + dataGridView2.Rows.Count.ToString();
        }

        private void systemAccounts_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Hide();
            payrollDashboard frm = new payrollDashboard();
            frm.id = id;
            frm.ShowDialog();
            this.Close();
        }

        private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            systemAccountsAdd frm = new systemAccountsAdd();
            frm.id = dataGridView2.CurrentRow.Cells[0].Value.ToString();
            frm.form = "update";
            frm.ShowDialog();
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            systemAccountsAdd frm = new systemAccountsAdd();
            frm.id = dataGridView2.CurrentRow.Cells[0].Value.ToString();
            frm.form = "add";
            frm.name = name;
            frm.ShowDialog();
        }

        private void guna2TextBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
