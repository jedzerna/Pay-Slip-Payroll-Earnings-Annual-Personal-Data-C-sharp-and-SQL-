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
    public partial class Deletion : Form
    {
        public Deletion()
        {
            InitializeComponent();
        }

        private void Deletion_Load(object sender, EventArgs e)
        {
            load();
        }
        public void load()
        {
            //dataGridView1.BeginInvoke((Action)delegate ()
            //{
            using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
            {


                DataTable dt = new DataTable();
               
                tblOTS.Open();

                string list = "SELECT DISTINCT PERIOD FROM tblFinalWorkers ORDER BY PERIOD DESC";
                SqlCommand command = new SqlCommand(list, tblOTS);
                SqlDataReader reader = command.ExecuteReader();
                dt.Load(reader);

                foreach (DataRow row in dt.Rows)
                {
                  guna2ComboBox1.Items.Add(row["PERIOD"].ToString());
                    //guna2ComboBox1.ValueMember = "";
                }
                tblOTS.Close();

            }
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult1 = MessageBox.Show("Are you sure to DELETE this record???", "Delete?", MessageBoxButtons.YesNo);
            if (dialogResult1 == DialogResult.Yes)
            {
                using (SqlConnection tblOTS = new SqlConnection(ConfigurationManager.ConnectionStrings["otspayroll"].ConnectionString))
                {
                    tblOTS.Open();
                    using (SqlCommand command = new SqlCommand("DELETE FROM tblFinalWorkers WHERE PERIOD = '" + guna2ComboBox1.Text + "'", tblOTS))
                    {
                        command.ExecuteNonQuery();
                    }
                    tblOTS.Close();
                }
                MessageBox.Show("Deleted");
            }
        }
    }
}
