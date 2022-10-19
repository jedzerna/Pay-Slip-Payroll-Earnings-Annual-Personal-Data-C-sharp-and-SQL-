namespace Pay_Slip
{
    partial class ComputerList
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.ids = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.po = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ms = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.takenby = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.remarks = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.guna2Button2 = new Guna.UI2.WinForms.Guna2Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ids,
            this.po,
            this.ms,
            this.takenby,
            this.date,
            this.remarks});
            this.dataGridView1.Location = new System.Drawing.Point(47, 172);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.Size = new System.Drawing.Size(669, 342);
            this.dataGridView1.TabIndex = 300;
            // 
            // ids
            // 
            this.ids.DataPropertyName = "id";
            this.ids.HeaderText = "ids";
            this.ids.Name = "ids";
            this.ids.ReadOnly = true;
            // 
            // po
            // 
            this.po.DataPropertyName = "po";
            this.po.HeaderText = "po";
            this.po.Name = "po";
            this.po.ReadOnly = true;
            // 
            // ms
            // 
            this.ms.DataPropertyName = "ms";
            this.ms.HeaderText = "ms";
            this.ms.Name = "ms";
            this.ms.ReadOnly = true;
            // 
            // takenby
            // 
            this.takenby.DataPropertyName = "takenby";
            this.takenby.FillWeight = 150F;
            this.takenby.HeaderText = "takenby";
            this.takenby.Name = "takenby";
            this.takenby.ReadOnly = true;
            this.takenby.Width = 150;
            // 
            // date
            // 
            this.date.DataPropertyName = "date";
            this.date.HeaderText = "date";
            this.date.Name = "date";
            this.date.ReadOnly = true;
            // 
            // remarks
            // 
            this.remarks.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.remarks.DataPropertyName = "remarks";
            this.remarks.HeaderText = "remarks";
            this.remarks.Name = "remarks";
            this.remarks.ReadOnly = true;
            // 
            // guna2Button2
            // 
            this.guna2Button2.BorderRadius = 8;
            this.guna2Button2.CheckedState.Parent = this.guna2Button2;
            this.guna2Button2.CustomImages.Parent = this.guna2Button2;
            this.guna2Button2.FillColor = System.Drawing.Color.DodgerBlue;
            this.guna2Button2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.guna2Button2.ForeColor = System.Drawing.Color.White;
            this.guna2Button2.HoverState.Parent = this.guna2Button2;
            this.guna2Button2.Location = new System.Drawing.Point(531, 134);
            this.guna2Button2.Name = "guna2Button2";
            this.guna2Button2.ShadowDecoration.Parent = this.guna2Button2;
            this.guna2Button2.Size = new System.Drawing.Size(185, 32);
            this.guna2Button2.TabIndex = 301;
            this.guna2Button2.Text = "Add Receiving Report";
            this.guna2Button2.Click += new System.EventHandler(this.guna2Button2_Click);
            // 
            // ComputerList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(800, 566);
            this.Controls.Add(this.guna2Button2);
            this.Controls.Add(this.dataGridView1);
            this.DoubleBuffered = true;
            this.Name = "ComputerList";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ComputerList";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ComputerList_FormClosed);
            this.Load += new System.EventHandler(this.ComputerList_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ids;
        private System.Windows.Forms.DataGridViewTextBoxColumn po;
        private System.Windows.Forms.DataGridViewTextBoxColumn ms;
        private System.Windows.Forms.DataGridViewTextBoxColumn takenby;
        private System.Windows.Forms.DataGridViewTextBoxColumn date;
        private System.Windows.Forms.DataGridViewTextBoxColumn remarks;
        private Guna.UI2.WinForms.Guna2Button guna2Button2;
    }
}