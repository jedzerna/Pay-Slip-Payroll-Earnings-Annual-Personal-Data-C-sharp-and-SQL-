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
    public partial class staffCA : Form
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
        public staffCA()
        {
            InitializeComponent();
        }

        public string EMPC;
        public string EMPN;
        private void staffCA_Load(object sender, EventArgs e)
        {
            calculate();
            load();
            label28.Text = EMPC;
            label1.Text = EMPN;
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
        private void calculate()
        {
            try
            {
                try
                {
                    using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
                    {
                        tblOTS.Open();
                        string list = "SELECT ROUND(SUM(AMOUNT), 2) FROM tblLOANS WHERE EMPCODE = '" + EMPC + "'";
                        SqlCommand command = new SqlCommand(list, tblOTS);
                        decimal count = (decimal)command.ExecuteScalar();
                        label6.Text = string.Format("{0:#,##0.00}", count);
                        tblOTS.Close();
                    }
                }
                catch
                {
                    label6.Text = "0.00";
                }
                try
                {
                    using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
                    {
                        tblOTS.Open();
                        string list = "SELECT ROUND(SUM(PAYMENT), 2) FROM tblLOANS WHERE EMPCODE = '" + EMPC + "'";
                        SqlCommand command = new SqlCommand(list, tblOTS);
                        decimal count = (decimal)command.ExecuteScalar();
                        label2.Text = string.Format("{0:#,##0.00}", count);
                        tblOTS.Close();
                    }
                }
                catch
                {
                    label2.Text = "0.00";
                }
                try
                {
                    using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
                    {
                        tblOTS.Open();
                        string list = "SELECT ROUND(SUM(BALANCE), 2) FROM tblLOANS WHERE EMPCODE = '" + EMPC + "'";
                        SqlCommand command = new SqlCommand(list, tblOTS);
                        decimal count = (decimal)command.ExecuteScalar();
                        label13.Text = string.Format("{0:#,##0.00}", count);
                        tblOTS.Close();
                    }
                }
                catch
                {
                    label13.Text = "0.00";
                }
                try
                {
                    using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
                    {
                        tblOTS.Open();
                        string list = "SELECT ROUND(SUM(SURCHARGE), 2) FROM tblLOANS WHERE EMPCODE = '" + EMPC + "'";
                        SqlCommand command = new SqlCommand(list, tblOTS);
                        decimal count = (decimal)command.ExecuteScalar();
                        label4.Text = string.Format("{0:#,##0.00}", count);
                        tblOTS.Close();
                    }
                }
                catch
                {
                    label4.Text = "0.00";
                }

                decimal all = 0.00M;
                all = Convert.ToDecimal(label13.Text) + Convert.ToDecimal(label6.Text);
                decimal bal = 0.00M;
                bal = all - Convert.ToDecimal(label2.Text);

                label9.Text = string.Format("{0:#,##0.00}", bal);
            }
            catch
            {
                label10.Visible = true;
                label10.Text = "No Record Founds";
            }
            using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
            {
                tblOTS.Open();
                String query = "SELECT dateupdated FROM payrollSettings WHERE details = 'LOANDBF'";
                SqlCommand cmd = new SqlCommand(query, tblOTS);
                SqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    label11.Text = "PAYMENT IS UPDATED ON " + (rdr["dateupdated"].ToString());
                }
                tblOTS.Close();

            }
        }
        private void load()
        {
            using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
            {
                DataTable dt = new DataTable();


                tblOTS.Open();

                string list = "SELECT DATEL,AMOUNT,PAYMENT,BALANCE,SURCHARGE,REFERENCE,ENTERAS,ENTEREDBY FROM tblLOANS WHERE EMPCODE = '" + EMPC + "' ORDER BY DATEL DESC";
                SqlCommand command = new SqlCommand(list, tblOTS);
                SqlDataReader reader = command.ExecuteReader();
                dt.Load(reader);

                dataGridView2.DataSource = dt;
            }
        }
    }
}
