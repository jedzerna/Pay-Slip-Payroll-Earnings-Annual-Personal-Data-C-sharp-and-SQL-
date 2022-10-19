using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pay_Slip
{
    public partial class stafPayrollPopup : Form
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
        public string basicsalary;
        public string allowance;
        public string overleave;
        public string sss;
        public string phil;
        public string hdmf;
        public string others;
        public string ca;
        public string sc;
        public string sssl;
        public string hdmfl;
        public string coop;
        public string financing;
        public string rsssloans;
        public string salesout;
        public string tmf;
        public string amount;
        public string net;
        public stafPayrollPopup()
        {
            InitializeComponent();
        }

        private void stafPayrollPopup_Load(object sender, EventArgs e)
        {
            label1.Text = empname;
            label28.Text = empcode;
            label5.Text = basicsalary;
            label7.Text = allowance;
            OVERLEAVE1.Text = overleave;
            label9.Text = sss;
            label11.Text = phil;
            label13.Text = hdmf;
            label15.Text = others;
            guna2TextBox2.Text = ca;
            guna2TextBox3.Text = sc;
            guna2TextBox4.Text = sssl;
            guna2TextBox5.Text = hdmfl;
            guna2TextBox10.Text = coop;
            guna2TextBox6.Text = financing;
            guna2TextBox7.Text = rsssloans;
            guna2TextBox8.Text = salesout;
            guna2TextBox9.Text = tmf;
            label25.Text = amount;
            label27.Text = net;

        }
        private void count()
        {

                decimal basicsalary = 0.00M;
                decimal allowance = 0.00M;
                decimal sss = 0.00M;
                decimal phil = 0.00M;
                decimal hdmf = 0.00M;
                decimal others = 0.00M;
                decimal ca = 0.00M;
                decimal sc = 0.00M;
                decimal sssl = 0.00M;
                decimal hdmfl = 0.00M;
                decimal coop = 0.00M;
                decimal financing = 0.00M;
                decimal rsssloans = 0.00M;
                decimal salesout = 0.00M;
                decimal tmf = 0.00M;

                if (Convert.ToString(label5) != string.Empty)
                {
                    basicsalary = Convert.ToDecimal(label5.Text.ToString());
                }
                else
                {
                    basicsalary = 0.00M;
                }
                if (Convert.ToString(label7.Text) != string.Empty)
                {
                    allowance = Convert.ToDecimal(label7.Text.ToString());
                }
                else
                {
                    allowance = 0.00M;
                }


                decimal sum = 0.00M;
                decimal overleave = 0.00M;
                if (Convert.ToString(OVERLEAVE1.Text) != string.Empty)
                {
                    sum += basicsalary + allowance;
                    int aa = OVERLEAVE1.Text.Replace("-", "").Length;
                    if (OVERLEAVE1.Text != ".")
                    {
                        if (OVERLEAVE1.Text != "-.")
                        {
                            if (aa > 0)
                            {

                                if (Convert.ToDecimal(OVERLEAVE1.Text.ToString()) >= 0.00M)
                                {
                                    overleave = sum + Convert.ToDecimal(OVERLEAVE1.Text.ToString());
                                }
                                else
                                {
                                    overleave = sum - Convert.ToDecimal(OVERLEAVE1.Text.ToString().Replace("-", ""));
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (Convert.ToString(label5.Text) != string.Empty)
                    {
                        overleave += basicsalary + allowance;
                    }
                }

                label25.Text = string.Format("{0:#,##0.00}", Convert.ToDecimal(overleave));
                if (Convert.ToString(label9.Text) != string.Empty)
                {
                    sss = Convert.ToDecimal(label9.Text);
                }
                else
                {
                    sss = 0.00M;
                }
                if (Convert.ToString(label11.Text) != string.Empty)
                {
                    phil = Convert.ToDecimal(label11.Text);
                }
                else
                {
                    phil = 0.00M;
                }
                if (Convert.ToString(label13.Text) != string.Empty)
                {
                    hdmf = Convert.ToDecimal(label13.Text);
                }
                else
                {
                    hdmf = 0.00M;
                }
                if (Convert.ToString(label15.Text) != string.Empty)
                {
                    others = Convert.ToDecimal(label15.Text);
                }
                else
                {
                    others = 0.00M;
                }
                if (Convert.ToString(guna2TextBox2.Text) != string.Empty)
                {
                    ca = Convert.ToDecimal(guna2TextBox2.Text);
                }
                else
                {
                    ca = 0.00M;
                }
                if (Convert.ToString(guna2TextBox3.Text) != string.Empty)
                {
                    sc = Convert.ToDecimal(guna2TextBox3.Text);
                }
                else
                {
                    sc = 0.00M;
                }
                if (Convert.ToString(guna2TextBox4.Text) != string.Empty)
                {
                    sssl = Convert.ToDecimal(guna2TextBox4.Text);
                }
                else
                {
                    sssl = 0.00M;
                }


                if (Convert.ToString(guna2TextBox5.Text) != string.Empty)
                {
                    hdmfl = Convert.ToDecimal(guna2TextBox5.Text);
                }
                else
                {
                    hdmfl = 0.00M;
                }
                if (Convert.ToString(guna2TextBox10.Text) != string.Empty)
                {
                    coop = Convert.ToDecimal(guna2TextBox10.Text);
                }
                else
                {
                    coop = 0.00M;
                }
                if (Convert.ToString(guna2TextBox6.Text) != string.Empty)
                {
                    financing = Convert.ToDecimal(guna2TextBox6.Text);
                }
                else
                {
                    financing = 0.00M;
                }
                if (Convert.ToString(guna2TextBox7.Text) != string.Empty)
                {
                    rsssloans = Convert.ToDecimal(guna2TextBox7.Text);
                }
                else
                {
                    rsssloans = 0.00M;
                }
                if (Convert.ToString(guna2TextBox8.Text) != string.Empty)
                {
                    salesout = Convert.ToDecimal(guna2TextBox8.Text);
                }
                else
                {
                    salesout = 0.00M;
                }
                if (Convert.ToString(guna2TextBox9.Text) != string.Empty)
                {
                    tmf = Convert.ToDecimal(guna2TextBox9.Text);
                }
                else
                {
                    tmf = 0.00M;
                }
                decimal sumall = 0.00M;
                sumall += sss + phil + hdmf + others + ca + sc + sssl + hdmfl + coop + financing + rsssloans + salesout + tmf;

                decimal netall = 0.00M;
                netall = overleave - sumall;
                label27.Text = string.Format("{0:#,##0.00}", Convert.ToDecimal(netall));


                //tbasicsalary += basicsalary;
                //tallowance += allowance;

                //if (String.IsNullOrEmpty(OVERLEAVE1.Text as String))
                //{
                //}
                //else
                //{

                //    toverleave += Convert.ToDecimal(OVERLEAVE1.Text);
                //}
                //ttotalamount += overleave;
                //tsss += sss;
                //tphil += phil;
                //thdmf += hdmf;
                //tothers += others;
                //tca += ca;
                //tsc += sc;
                //tsssl += sssl;
                //thdmfl += hdmfl;
                //tcoop += coop;
                //tfinancing += financing;
                //trsssloans += rsssloans;
                //tsalesout += salesout;
                //ttmf += tmf;
                //ttotalnet += netall;



                //label5.Text = string.Format("{0:#,##0.00}", tbasicsalary);
                //label1.Text = string.Format("{0:#,##0.00}", tallowance);
                //label3.Text = string.Format("{0:#,##0.00}", toverleave);
                //label7.Text = string.Format("{0:#,##0.00}", ttotalamount);
                //label9.Text = string.Format("{0:#,##0.00}", tsss);
                //label11.Text = string.Format("{0:#,##0.00}", tphil);
                //label13.Text = string.Format("{0:#,##0.00}", thdmf);
                //label15.Text = string.Format("{0:#,##0.00}", tothers);
                //label38.Text = string.Format("{0:#,##0.00}", tca);
                //label17.Text = string.Format("{0:#,##0.00}", tsc);
                //label19.Text = string.Format("{0:#,##0.00}", tsssl);
                //label21.Text = string.Format("{0:#,##0.00}", thdmfl);
                //label36.Text = string.Format("{0:#,##0.00}", tcoop);
                //label34.Text = string.Format("{0:#,##0.00}", tfinancing);
                //label32.Text = string.Format("{0:#,##0.00}", trsssloans);
                //label30.Text = string.Format("{0:#,##0.00}", tsalesout);
                //label25.Text = string.Format("{0:#,##0.00}", ttmf);
                //label23.Text = string.Format("{0:#,##0.00}", ttotalnet);

        }

        private void OVERLEAVE1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                count();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                OVERLEAVE1.Text = "0.00";
            }
        }

        private void guna2TextBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!isNumber(e.KeyChar, guna2TextBox2.Text))
                e.Handled = true;
        }
        public bool isNumber(char ch, string text)
        {
            bool res = true;
            char decimalChar = Convert.ToChar(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);

            //check if it´s a decimal separator and if doesn´t already have one in the text string
            if (ch == decimalChar && text.IndexOf(decimalChar) != -1)
            {
                res = false;
                return res;
            }

            //check if it´s a digit, decimal separator and backspace
            if (!Char.IsDigit(ch) && ch != decimalChar && ch != (char)Keys.Back)
                res = false;

            return res;
        }

        private void guna2TextBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!isNumber(e.KeyChar, guna2TextBox3.Text))
                e.Handled = true;
        }

        private void guna2TextBox4_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (!isNumber(e.KeyChar, guna2TextBox4.Text))
                e.Handled = true;
        }

        private void guna2TextBox5_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (!isNumber(e.KeyChar, guna2TextBox5.Text))
                e.Handled = true;
        }

        private void guna2TextBox10_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (!isNumber(e.KeyChar, guna2TextBox10.Text))
                e.Handled = true;
        }

        private void guna2TextBox6_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (!isNumber(e.KeyChar, guna2TextBox6.Text))
                e.Handled = true;
        }

        private void guna2TextBox7_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (!isNumber(e.KeyChar, guna2TextBox7.Text))
                e.Handled = true;
        }

        private void guna2TextBox8_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!isNumber(e.KeyChar, guna2TextBox8.Text))
                e.Handled = true;
        }

        private void guna2TextBox9_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!isNumber(e.KeyChar, guna2TextBox9.Text))
                e.Handled = true;
        }
      

        private void OVERLEAVE1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar)
   && !char.IsDigit(e.KeyChar)
   && e.KeyChar != '.' && e.KeyChar != '-')
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if (e.KeyChar == '.'
                && (sender as Guna.UI2.WinForms.Guna2TextBox).Text.IndexOf('.') > -1 )
            {
                e.Handled = true;
            }
            if (e.KeyChar == '-'
              && (sender as Guna.UI2.WinForms.Guna2TextBox).Text.IndexOf('-') > -1)
            {
                e.Handled = true;
            }
        }

        staffPayroll staffPayroll = (staffPayroll)Application.OpenForms["staffPayroll"];
        private void guna2Button4_Click(object sender, EventArgs e)
        {
            try
            {
                count();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                OVERLEAVE1.Text = "0.00";
                return;
            }
            if (OVERLEAVE1.Text != "")
            {
                staffPayroll.dataGridView2.CurrentRow.Cells["OVERLEAVE"].Value = string.Format("{0:#,##0.00}", Convert.ToDecimal(OVERLEAVE1.Text));
            }
            else
            {
                staffPayroll.dataGridView2.CurrentRow.Cells["OVERLEAVE"].Value = "";
            }
            if (label25.Text != "")
            {
                staffPayroll.dataGridView2.CurrentRow.Cells["TOTALAMOUNT"].Value = string.Format("{0:#,##0.00}", Convert.ToDecimal(label25.Text));
            }
            else
            {
                staffPayroll.dataGridView2.CurrentRow.Cells["TOTALAMOUNT"].Value = "";
            }
            if (guna2TextBox2.Text != "")
            {
                staffPayroll.dataGridView2.CurrentRow.Cells["CA"].Value = string.Format("{0:#,##0.00}", Convert.ToDecimal(guna2TextBox2.Text));
            }
            else
            {
                staffPayroll.dataGridView2.CurrentRow.Cells["CA"].Value = "";
            }
            if (guna2TextBox3.Text != "")
            {
                staffPayroll.dataGridView2.CurrentRow.Cells["SC"].Value = string.Format("{0:#,##0.00}", Convert.ToDecimal(guna2TextBox3.Text));
            }
            else
            {
                staffPayroll.dataGridView2.CurrentRow.Cells["SC"].Value = "";
            }
            if (guna2TextBox4.Text != "")
            {
                staffPayroll.dataGridView2.CurrentRow.Cells["SSSLOANS"].Value = string.Format("{0:#,##0.00}", Convert.ToDecimal(guna2TextBox4.Text));
            }
            else
            {
                staffPayroll.dataGridView2.CurrentRow.Cells["SSSLOANS"].Value = "";
            }
            if (guna2TextBox5.Text != "")
            {
                staffPayroll.dataGridView2.CurrentRow.Cells["HDMFLOANS"].Value = string.Format("{0:#,##0.00}", Convert.ToDecimal(guna2TextBox5.Text));
            }
            else
            {
                staffPayroll.dataGridView2.CurrentRow.Cells["HDMFLOANS"].Value = "";
            }
            if (guna2TextBox10.Text != "")
            {
                staffPayroll.dataGridView2.CurrentRow.Cells["COOP"].Value = string.Format("{0:#,##0.00}", Convert.ToDecimal(guna2TextBox10.Text));
            }
            else
            {
                staffPayroll.dataGridView2.CurrentRow.Cells["COOP"].Value = "";
            }
            if (guna2TextBox6.Text != "")
            {
                staffPayroll.dataGridView2.CurrentRow.Cells["FINANCING"].Value = string.Format("{0:#,##0.00}", Convert.ToDecimal(guna2TextBox6.Text));
            }
            else
            {
                staffPayroll.dataGridView2.CurrentRow.Cells["FINANCING"].Value = "";
            }
            if (guna2TextBox7.Text != "")
            {
                staffPayroll.dataGridView2.CurrentRow.Cells["RSSSLOANS"].Value = string.Format("{0:#,##0.00}", Convert.ToDecimal(guna2TextBox7.Text));
            }
            else
            {
                staffPayroll.dataGridView2.CurrentRow.Cells["RSSSLOANS"].Value = "";
            }
            if (guna2TextBox8.Text != "")
            {
                staffPayroll.dataGridView2.CurrentRow.Cells["SALESO"].Value = string.Format("{0:#,##0.00}", Convert.ToDecimal(guna2TextBox8.Text));
            }
            else
            {
                staffPayroll.dataGridView2.CurrentRow.Cells["SALESO"].Value = "";
            }
            if (guna2TextBox9.Text != "")
            {
                staffPayroll.dataGridView2.CurrentRow.Cells["TMF"].Value = string.Format("{0:#,##0.00}", Convert.ToDecimal(guna2TextBox9.Text));
            }
            else
            {
                staffPayroll.dataGridView2.CurrentRow.Cells["TMF"].Value = "";
            }
            if (label27.Text != "")
            {
                staffPayroll.dataGridView2.CurrentRow.Cells["NET"].Value = string.Format("{0:#,##0.00}", Convert.ToDecimal(label27.Text));
            }
            else
            {
                staffPayroll.dataGridView2.CurrentRow.Cells["NET"].Value = "";
            }
            staffPayroll.count();
            this.Close();
        }

        private void guna2TextBox2_TextChanged(object sender, EventArgs e)
        {
            try
            {
                count();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                OVERLEAVE1.Text = "0.00";
            }
        }

        private void guna2TextBox3_TextChanged(object sender, EventArgs e)
        {
            try
            {
                count();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                OVERLEAVE1.Text = "0.00";
            }
        }

        private void guna2TextBox4_TextChanged(object sender, EventArgs e)
        {
            try
            {
                count();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                OVERLEAVE1.Text = "0.00";
            }
        }

        private void guna2TextBox5_TextChanged(object sender, EventArgs e)
        {
            try
            {
                count();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                OVERLEAVE1.Text = "0.00";
            }
        }

        private void guna2TextBox10_TextChanged(object sender, EventArgs e)
        {
            try
            {
                count();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                OVERLEAVE1.Text = "0.00";
            }
        }

        private void guna2TextBox6_TextChanged(object sender, EventArgs e)
        {
            try
            {
                count();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                OVERLEAVE1.Text = "0.00";
            }
        }

        private void guna2TextBox7_TextChanged(object sender, EventArgs e)
        {
            try
            {
                count();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                OVERLEAVE1.Text = "0.00";
            }
        }

        private void guna2TextBox8_TextChanged(object sender, EventArgs e)
        {
            try
            {
                count();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                OVERLEAVE1.Text = "0.00";
            }
        }

        private void guna2TextBox9_TextChanged(object sender, EventArgs e)
        {
            try
            {
                count();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                OVERLEAVE1.Text = "0.00";
            }
        }

        private void OVERLEAVE1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Insert)
            {
                guna2Button4_Click(sender, e);
            }
        }
    }
}
