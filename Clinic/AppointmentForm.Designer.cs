namespace Clinic
{
    partial class AppointmentForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
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
            this.lblPatient = new System.Windows.Forms.Label();
            this.lblDoctor = new System.Windows.Forms.Label();
            this.lblDate = new System.Windows.Forms.Label();
            this.lblTime = new System.Windows.Forms.Label();
            this.lblPrice = new System.Windows.Forms.Label();
            this.lblPriceValue = new System.Windows.Forms.Label();
            this.cmbPatient = new System.Windows.Forms.ComboBox();
            this.cmbDoctor = new System.Windows.Forms.ComboBox();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.dtpTime = new System.Windows.Forms.DateTimePicker();
            this.btnBook = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblPatient
            // 
            this.lblPatient.AutoSize = true;
            this.lblPatient.Location = new System.Drawing.Point(15, 20);
            this.lblPatient.Name = "lblPatient";
            this.lblPatient.Size = new System.Drawing.Size(56, 13);
            this.lblPatient.TabIndex = 0;
            this.lblPatient.Text = "Пациент:";
            // 
            // lblDoctor
            // 
            this.lblDoctor.AutoSize = true;
            this.lblDoctor.Location = new System.Drawing.Point(15, 52);
            this.lblDoctor.Name = "lblDoctor";
            this.lblDoctor.Size = new System.Drawing.Size(41, 13);
            this.lblDoctor.TabIndex = 1;
            this.lblDoctor.Text = "Врач:";
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Location = new System.Drawing.Point(15, 84);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(38, 13);
            this.lblDate.TabIndex = 2;
            this.lblDate.Text = "Дата:";
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Location = new System.Drawing.Point(15, 116);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(41, 13);
            this.lblTime.TabIndex = 3;
            this.lblTime.Text = "Время:";
            // 
            // lblPrice
            // 
            this.lblPrice.AutoSize = true;
            this.lblPrice.Location = new System.Drawing.Point(15, 148);
            this.lblPrice.Name = "lblPrice";
            this.lblPrice.Size = new System.Drawing.Size(97, 13);
            this.lblPrice.TabIndex = 4;
            this.lblPrice.Text = "Цена приёма:";
            // 
            // lblPriceValue
            // 
            this.lblPriceValue.AutoSize = true;
            this.lblPriceValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblPriceValue.Location = new System.Drawing.Point(140, 148);
            this.lblPriceValue.Name = "lblPriceValue";
            this.lblPriceValue.Size = new System.Drawing.Size(0, 13);
            this.lblPriceValue.TabIndex = 5;
            // 
            // cmbPatient
            // 
            this.cmbPatient.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPatient.FormattingEnabled = true;
            this.cmbPatient.Location = new System.Drawing.Point(140, 17);
            this.cmbPatient.Name = "cmbPatient";
            this.cmbPatient.Size = new System.Drawing.Size(260, 21);
            this.cmbPatient.TabIndex = 6;
            // 
            // cmbDoctor
            // 
            this.cmbDoctor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDoctor.FormattingEnabled = true;
            this.cmbDoctor.Location = new System.Drawing.Point(140, 49);
            this.cmbDoctor.Name = "cmbDoctor";
            this.cmbDoctor.Size = new System.Drawing.Size(260, 21);
            this.cmbDoctor.TabIndex = 7;
            this.cmbDoctor.SelectedIndexChanged += new System.EventHandler(this.cmbDoctor_SelectedIndexChanged);
            // 
            // dtpDate
            // 
            this.dtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDate.Location = new System.Drawing.Point(140, 81);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new System.Drawing.Size(260, 20);
            this.dtpDate.TabIndex = 8;
            // 
            // dtpTime
            // 
            this.dtpTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTime.CustomFormat = "HH:mm";
            this.dtpTime.ShowUpDown = true;
            this.dtpTime.Location = new System.Drawing.Point(140, 113);
            this.dtpTime.Name = "dtpTime";
            this.dtpTime.Size = new System.Drawing.Size(260, 20);
            this.dtpTime.TabIndex = 9;
            // 
            // btnBook
            // 
            this.btnBook.Location = new System.Drawing.Point(140, 190);
            this.btnBook.Name = "btnBook";
            this.btnBook.Size = new System.Drawing.Size(120, 30);
            this.btnBook.TabIndex = 10;
            this.btnBook.Text = "Записать";
            this.btnBook.UseVisualStyleBackColor = true;
            this.btnBook.Click += new System.EventHandler(this.btnBook_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(280, 190);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(120, 30);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // AppointmentForm
            // 
            this.AcceptButton = this.btnBook;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(426, 235);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnBook);
            this.Controls.Add(this.dtpTime);
            this.Controls.Add(this.dtpDate);
            this.Controls.Add(this.cmbDoctor);
            this.Controls.Add(this.cmbPatient);
            this.Controls.Add(this.lblPriceValue);
            this.Controls.Add(this.lblPrice);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.lblDate);
            this.Controls.Add(this.lblDoctor);
            this.Controls.Add(this.lblPatient);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "AppointmentForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Запись на приём";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblPatient;
        private System.Windows.Forms.Label lblDoctor;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Label lblPrice;
        private System.Windows.Forms.Label lblPriceValue;
        private System.Windows.Forms.ComboBox cmbPatient;
        private System.Windows.Forms.ComboBox cmbDoctor;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.DateTimePicker dtpTime;
        private System.Windows.Forms.Button btnBook;
        private System.Windows.Forms.Button btnCancel;
    }
}
