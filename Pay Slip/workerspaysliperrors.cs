using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pay_Slip
{
    public partial class workerspaysliperrors : Form
    {
        public workerspaysliperrors()
        {
            InitializeComponent();
        }


        private workerspayslip workers = null;
        public workerspaysliperrors(workerspayslip callingForm)
        {
            workers = callingForm as workerspayslip;
            InitializeComponent();
        }
        private void workerspaysliperrors_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = workers.perglu;
        }
    }
}
