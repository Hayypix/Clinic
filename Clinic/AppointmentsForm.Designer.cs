namespace Clinic
{
    partial class AppointmentsForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        private void InitializeComponent()
        {
            this.dgvAppointments = new System.Windows.Forms.DataGridView();
            this.btnBook = new System.Windows.Forms.Button();
            this.btnComplete = new System.Windows.Forms.Button();
            this.btnView = new System.Windows.Forms.Button();
            this.btnCancelApp = new System.Windows.Forms.Button();
            this.lblSearch = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAppointments)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvAppointments
            // 
            this.dgvAppointments.AllowUserToAddRows = false;
            this.dgvAppointments.AllowUserToDeleteRows = false;
            this.dgvAppointments.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvAppointments.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAppointments.Location = new System.Drawing.Point(12, 45);
            this.dgvAppointments.MultiSelect = false;
            this.dgvAppointments.Name = "dgvAppointments";
            this.dgvAppointments.ReadOnly = true;
            this.dgvAppointments.RowHeadersVisible = false;
            this.dgvAppointments.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAppointments.Size = new System.Drawing.Size(760, 395);
            this.dgvAppointments.TabIndex = 0;
            this.dgvAppointments.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAppointments_CellDoubleClick);
            // 
            // btnBook
            // 
            this.btnBook.Location = new System.Drawing.Point(785, 45);
            this.btnBook.Name = "btnBook";
            this.btnBook.Size = new System.Drawing.Size(115, 32);
            this.btnBook.TabIndex = 1;
            this.btnBook.Text = "Записать";
            this.btnBook.UseVisualStyleBackColor = true;
            this.btnBook.Click += new System.EventHandler(this.btnBook_Click);
            // 
            // btnComplete
            // 
            this.btnComplete.Location = new System.Drawing.Point(785, 87);
            this.btnComplete.Name = "btnComplete";
            this.btnComplete.Size = new System.Drawing.Size(115, 32);
            this.btnComplete.TabIndex = 2;
            this.btnComplete.Text = "Провести";
            this.btnComplete.UseVisualStyleBackColor = true;
            this.btnComplete.Click += new System.EventHandler(this.btnComplete_Click);
            // 
            // btnView
            // 
            this.btnView.Location = new System.Drawing.Point(785, 129);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(115, 32);
            this.btnView.TabIndex = 3;
            this.btnView.Text = "Просмотр";
            this.btnView.UseVisualStyleBackColor = true;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // btnCancelApp
            // 
            this.btnCancelApp.Location = new System.Drawing.Point(785, 171);
            this.btnCancelApp.Name = "btnCancelApp";
            this.btnCancelApp.Size = new System.Drawing.Size(115, 32);
            this.btnCancelApp.TabIndex = 4;
            this.btnCancelApp.Text = "Отменить";
            this.btnCancelApp.UseVisualStyleBackColor = true;
            this.btnCancelApp.Click += new System.EventHandler(this.btnCancelApp_Click);
            // 
            // lblSearch
            // 
            this.lblSearch.AutoSize = true;
            this.lblSearch.Location = new System.Drawing.Point(12, 16);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(42, 13);
            this.lblSearch.TabIndex = 5;
            this.lblSearch.Text = "Поиск:";
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(60, 13);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(260, 20);
            this.txtSearch.TabIndex = 6;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(340, 16);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(44, 13);
            this.lblStatus.TabIndex = 7;
            this.lblStatus.Text = "Статус:";
            // 
            // cmbStatus
            // 
            this.cmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStatus.FormattingEnabled = true;
            this.cmbStatus.Location = new System.Drawing.Point(390, 13);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(140, 21);
            this.cmbStatus.TabIndex = 8;
            this.cmbStatus.SelectedIndexChanged += new System.EventHandler(this.cmbStatus_SelectedIndexChanged);
            // 
            // AppointmentsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(914, 452);
            this.Controls.Add(this.cmbStatus);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.lblSearch);
            this.Controls.Add(this.btnCancelApp);
            this.Controls.Add(this.btnView);
            this.Controls.Add(this.btnComplete);
            this.Controls.Add(this.btnBook);
            this.Controls.Add(this.dgvAppointments);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "AppointmentsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Записи на приём";
            ((System.ComponentModel.ISupportInitialize)(this.dgvAppointments)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.DataGridView dgvAppointments;
        private System.Windows.Forms.Button btnBook;
        private System.Windows.Forms.Button btnComplete;
        private System.Windows.Forms.Button btnView;
        private System.Windows.Forms.Button btnCancelApp;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ComboBox cmbStatus;
    }
}
