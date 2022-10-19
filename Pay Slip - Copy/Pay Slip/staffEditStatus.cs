using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pay_Slip
{
    public partial class staffEditStatus : Form
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
        public string empcode;
        public string empname;
        public string status;
        public string form;
        public staffEditStatus()
        {
            InitializeComponent();
        }

        private void staffEditStatus_Load(object sender, EventArgs e)
        {
            label1.Text = empcode;
            label4.Text = empname;
            guna2ComboBox1.Text = status;
        }

        staffInformations staffInformations = (staffInformations)Application.OpenForms["staffInformations"];
        staffDashboard staffDashboard = (staffDashboard)Application.OpenForms["staffDashboard"];
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
            {
                SqlCommand cmd3 = new SqlCommand("update tblStaffData set STATUS=@STATUS where EMPCODE=@EMPCODE", tblOTS);
                tblOTS.Open();
                cmd3.Parameters.AddWithValue("@EMPCODE", label1.Text);
                cmd3.Parameters.AddWithValue("@STATUS", guna2ComboBox1.Text);
                cmd3.ExecuteNonQuery();
                tblOTS.Close();
            }
            staffInformations.load();
            if (form == "A")
            {
                if (guna2ComboBox1.Text == "INACTIVE")
                {
                    staffPayroll staffPayroll = (staffPayroll)Application.OpenForms["staffPayroll"];
                    foreach (DataGridViewCell oneCell in staffPayroll.dataGridView2.SelectedCells)
                    {
                        //staffPayroll.dataGridView2.Rows.RemoveAt(row.Index); 
                        if (oneCell.Selected)
                            staffPayroll.dataGridView2.Rows.RemoveAt(oneCell.RowIndex);

                    }
                }
            }
            else
            {
                staffDashboard.load();
            }
            MessageBox.Show("Saved.");
            this.Close();
        }
    }
}
