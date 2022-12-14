using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pay_Slip
{
    public partial class systemAccountsInfoandUpdate : Form
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
        public systemAccountsInfoandUpdate()
        {
            InitializeComponent();
        }
        public string id;
        private void systemAccountsInfoandUpdate_Load(object sender, EventArgs e)
        {
            using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
            {
                tblOTS.Open();
                String query1 = "SELECT * FROM systemAccounts where id like '" + id + "'";
                SqlCommand cmd2 = new SqlCommand(query1, tblOTS);
                SqlDataReader dr1 = cmd2.ExecuteReader();

                payrollDashboard rw = new payrollDashboard();
                if (dr1.Read())
                {

                    EMPCODE.Text = (dr1["username"].ToString());
                    user = (dr1["username"].ToString());
                    EMPLOYEENAME.Text = (dr1["fullname"].ToString());
                    pass = (dr1["password"].ToString());
                    if (dr1["image"] != DBNull.Value)
                    {
                        byte[] img = (byte[])(dr1["image"]);
                        MemoryStream mstream = new MemoryStream(img);
                        pictureBox1.Image = System.Drawing.Image.FromStream(mstream);
                    }

                }
                dr1.Close();
                tblOTS.Close();
            }
        }
        private string pass;
        private string user;

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            if (EMPCODE.Text == "" || EMPLOYEENAME.Text == "" || guna2TextBox2.Text == "" || guna2TextBox1.Text == "" || guna2TextBox3.Text == "")
            {
                MessageBox.Show("Please don't leave any blank in the label.");
                return;
            }
            if (guna2TextBox2.Text != pass)
            {
                label7.Text = "Password is incorrect";
                label7.Visible = true;
                return;
            }
            else
            {
                label7.Text = "Password";
                label7.Visible = false;
            }
            if (label8.Text == "Password doesn't match")
            {
                return;
            }
            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Please select an image.");
                return;
            }
            if (user != EMPCODE.Text)
            {
                using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
                {
                    tblOTS.Open();
                    SqlCommand check_User_Name = new SqlCommand("SELECT COUNT(*) FROM [systemAccounts] WHERE username = @username", tblOTS);
                    check_User_Name.Parameters.AddWithValue("@username", EMPCODE.Text);
                    int UserExist = (int)check_User_Name.ExecuteScalar();

                    if (UserExist > 0)
                    {
                        label9.Text = "Username is already exist";
                        label9.Visible = true;
                        return;
                    }
                    tblOTS.Close();
                }
            }
            using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
            {
                var imagehelper = new ImageHelper();
                ImageConverter _imageConverter = new ImageConverter();
                byte[] img11 = (byte[])_imageConverter.ConvertTo(imagehelper.CropImage(pictureBox1.Image), typeof(byte[]));

                SqlCommand cmd3 = new SqlCommand("update systemAccounts set username=@username,password=@password,fullname=@fullname,image=@image where id=@id", tblOTS);
                tblOTS.Open();
                cmd3.Parameters.AddWithValue("@id", id);
                cmd3.Parameters.AddWithValue("@username", EMPCODE.Text);
                cmd3.Parameters.AddWithValue("@password", guna2TextBox1.Text);
                cmd3.Parameters.AddWithValue("@fullname", EMPLOYEENAME.Text);
                cmd3.Parameters.AddWithValue("@image", SqlDbType.Image).Value = img11;
                cmd3.ExecuteNonQuery();
                tblOTS.Close();
            }
            MessageBox.Show("Saved");
            payrollDashboard.log();
            this.Close();
        }

        payrollDashboard payrollDashboard = (payrollDashboard)Application.OpenForms["payrollDashboard"];
        private void guna2TextBox2_TextChanged(object sender, EventArgs e)
        {
         
        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {
            if (guna2TextBox1.Text != guna2TextBox3.Text)
            {
                label8.Text = "Password doesn't match";
                label8.Visible = true;
            }
            else
            {
                label8.Text = "Password match";
                label8.Visible = true;
            }
        }

        private void guna2TextBox3_TextChanged(object sender, EventArgs e)
        {
            if (guna2TextBox1.Text != guna2TextBox3.Text)
            {
                label8.Text = "Password doesn't match";
                label8.Visible = true;
            }
            else
            {
                label8.Text = "Password match";
                label8.Visible = true;
            }
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.tif;";
            if (open.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = new Bitmap(open.FileName);

                var imagehelper = new ImageHelper();

                //pictureBox1.Image = imagehelper.CropImage(pictureBox1.Image);

                ImageConverter _imageConverter = new ImageConverter();
                byte[] xByte = (byte[])_imageConverter.ConvertTo(imagehelper.CropImage(pictureBox1.Image), typeof(byte[]));

                pictureBox1.Image = ByteToImage(xByte);
            }
        }
        public static Bitmap ByteToImage(byte[] blob)
        {
            MemoryStream mStream = new MemoryStream();
            byte[] pData = blob;
            mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
            Bitmap bm = new Bitmap(mStream, false);
            mStream.Dispose();
            return bm;
        }
    }
}
