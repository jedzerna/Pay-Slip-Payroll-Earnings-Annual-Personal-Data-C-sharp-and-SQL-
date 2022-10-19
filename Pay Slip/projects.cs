using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pay_Slip
{
    public partial class projects : Form
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
        public projects()
        {
            InitializeComponent();
            ApplyShadows(this);
        }

        private void projects_Load(object sender, EventArgs e)
        {
            SuspendLayout();
            try
            {
                //      System.Threading.Thread thread =
                //new System.Threading.Thread(new System.Threading.ThreadStart(load));
                //      thread.Start();
                load();
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
        }
        private void ChangeControlStyles(Control ctrl, ControlStyles flag, bool value)
        {
            MethodInfo method = ctrl.GetType().GetMethod("SetStyle", BindingFlags.Instance | BindingFlags.NonPublic);
            if (method != null)
                method.Invoke(ctrl, new object[] { flag, value });
        }
        DataTable dt = new DataTable();
        public void load()
        {
            //dataGridView1.BeginInvoke((Action)delegate ()
            //{
            using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
            {
                tblOTS.Open();
                string list = "SELECT ACCTCODE,ACCTDESC FROM GLU4 ORDER BY ACCTCODE ASC";
                SqlCommand command = new SqlCommand(list, tblOTS);
                SqlDataReader reader = command.ExecuteReader();
                dt.Load(reader);
                dataGridView1.DataSource = dt;
                dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
                dataGridView1.RowHeadersVisible = false;

                tblOTS.Close();
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void bunifuCheckbox11_OnChange(object sender, EventArgs e)
        {
       
        }

        private void bunifuCheckbox12_OnChange(object sender, EventArgs e)
        {
        
        }
        string lb = "{";
        string rb = "}";
        private void bunifuCustomTextbox1_TextChanged(object sender, EventArgs e)
        {
         
        }

        private void bunifuCustomTextbox2_TextChanged(object sender, EventArgs e)
        {
          
        }

        private void bunifuFlatButton4_Click(object sender, EventArgs e)
        {
         
        }

        private void bunifuFlatButton4_Click_1(object sender, EventArgs e)
        {
          

            //using (SqlConnection tblIn = new SqlConnection(@"Data Source=JED-PC\GLUINVENTORY;Initial Catalog=other;Persist Security Info=True;User ID=zerna; Password=zerna;"))
            //{
            //    for (int i = 0; i < dataGridView1.Rows.Count; i++)
            //    {
            //        RegexOptions options = RegexOptions.None;
            //        Regex regex = new Regex("[ ]{2,}", options);

            //        string F1 = dataGridView1.Rows[i].Cells[1].Value.ToString();

            //        F1 = regex.Replace(F1, " ");
            //        tblIn.Open();
            //        string insStmt = "insert into GLU4 ([ACCTCODE], [ACCTDESC], [createdby]) values" +
            //            " (@ACCTCODE,@ACCTDESC,@createdby)";
            //        SqlCommand insCmd = new SqlCommand(insStmt, tblIn);
            //        insCmd.Parameters.Clear();
            //        insCmd.Parameters.AddWithValue("@ACCTCODE", dataGridView1.Rows[i].Cells[0].Value.ToString());
            //        insCmd.Parameters.AddWithValue("@ACCTDESC", F1);
            //        insCmd.Parameters.AddWithValue("@createdby", "Jed R. Zerna");
            //        int affectedRows = insCmd.ExecuteNonQuery();
            //        tblIn.Close();
            //    }
            //}
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {
            (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = string.Format("ACCTCODE LIKE '%{0}%'", guna2TextBox1.Text.Replace("[", lb).Replace("]", rb).Replace("​*", "[*​]").Replace("%", "[%]").Replace("'", "''"));

        }

        private void guna2TextBox2_TextChanged(object sender, EventArgs e)
        {
            (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = string.Format("ACCTDESC LIKE '%{0}%'", guna2TextBox2.Text.Replace("[", lb).Replace("]", rb).Replace("​*", "[*​]").Replace("%", "[%]").Replace("'", "''"));

        }
    }
}
