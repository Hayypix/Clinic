namespace Clinic
{
    partial class CompleteForm
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
            this.lblPatientValue = new System.Windows.Forms.Label();
            this.lblDoctor = new System.Windows.Forms.Label();
            this.lblDoctorValue = new System.Windows.Forms.Label();
            this.lblDate = new System.Windows.Forms.Label();
            this.lblDateValue = new System.Windows.Forms.Label();
            this.lblPrice = new System.Windows.Forms.Label();
            this.lblPriceValue = new System.Windows.Forms.Label();
            this.lblDiagnosis = new System.Windows.Forms.Label();
            this.txtDiagnosis = new System.Windows.Forms.TextBox();
            this.btnComplete = new System.Windows.Forms.Button();
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
            // lblPatientValue
            // 
            this.lblPatientValue.AutoSize = true;
            this.lblPatientValue.Location = new System.Drawing.Point(110, 20);
            this.lblPatientValue.Name = "lblPatientValue";
            this.lblPatientValue.Size = new System.Drawing.Size(0, 13);
            this.lblPatientValue.TabIndex = 1;
            // 
            // lblDoctor
            // 
            this.lblDoctor.AutoSize = true;
            this.lblDoctor.Location = new System.Drawing.Point(15, 46);
            this.lblDoctor.Name = "lblDoctor";
            this.lblDoctor.Size = new System.Drawing.Size(41, 13);
            this.lblDoctor.TabIndex = 2;
            this.lblDoctor.Text = "Врач:";
            // 
            // lblDoctorValue
            // 
            this.lblDoctorValue.AutoSize = true;
            this.lblDoctorValue.Location = new System.Drawing.Point(110, 46);
            this.lblDoctorValue.Name = "lblDoctorValue";
            this.lblDoctorValue.Size = new System.Drawing.Size(0, 13);
            this.lblDoctorValue.TabIndex = 3;
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Location = new System.Drawing.Point(15, 72);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(38, 13);
            this.lblDate.TabIndex = 4;
            this.lblDate.Text = "Дата:";
            // 
            // lblDateValue
            // 
            this.lblDateValue.AutoSize = true;
            this.lblDateValue.Location = new System.Drawing.Point(110, 72);
            this.lblDateValue.Name = "lblDateValue";
            this.lblDateValue.Size = new System.Drawing.Size(0, 13);
            this.lblDateValue.TabIndex = 5;
            // 
            // lblPrice
            // 
            this.lblPrice.AutoSize = true;
            this.lblPrice.Location = new System.Drawing.Point(15, 98);
            this.lblPrice.Name = "lblPrice";
            this.lblPrice.Size = new System.Drawing.Size(97, 13);
            this.lblPrice.TabIndex = 6;
            this.lblPrice.Text = "Цена приёма:";
            // 
            // lblPriceValue
            // 
            this.lblPriceValue.AutoSize = true;
            this.lblPriceValue.Location = new System.Drawing.Point(110, 98);
            this.lblPriceValue.Name = "lblPriceValue";
            this.lblPriceValue.Size = new System.Drawing.Size(0, 13);
            this.lblPriceValue.TabIndex = 7;
            // 
            // lblDiagnosis
            // 
            this.lblDiagnosis.AutoSize = true;
            this.lblDiagnosis.Location = new System.Drawing.Point(15, 130);
            this.lblDiagnosis.Name = "lblDiagnosis";
            this.lblDiagnosis.Size = new System.Drawing.Size(58, 13);
            this.lblDiagnosis.TabIndex = 8;
            this.lblDiagnosis.Text = "Диагноз:";
            // 
            // txtDiagnosis
            // 
            this.txtDiagnosis.Location = new System.Drawing.Point(15, 150);
            this.txtDiagnosis.Multiline = true;
            this.txtDiagnosis.Name = "txtDiagnosis";
            this.txtDiagnosis.Size = new System.Drawing.Size(385, 70);
            this.txtDiagnosis.TabIndex = 9;
            // 
            // btnComplete
            // 
            this.btnComplete.Location = new System.Drawing.Point(140, 235);
            this.btnComplete.Name = "btnComplete";
            this.btnComplete.Size = new System.Drawing.Size(120, 30);
            this.btnComplete.TabIndex = 10;
            this.btnComplete.Text = "Провести";
            this.btnComplete.UseVisualStyleBackColor = true;
            this.btnComplete.Click += new System.EventHandler(this.btnComplete_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(280, 235);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(120, 30);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // CompleteForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(426, 278);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnComplete);
            this.Controls.Add(this.txtDiagnosis);
            this.Controls.Add(this.lblDiagnosis);
            this.Controls.Add(this.lblPriceValue);
            this.Controls.Add(this.lblPrice);
            this.Controls.Add(this.lblDateValue);
            this.Controls.Add(this.lblDate);
            this.Controls.Add(this.lblDoctorValue);
            this.Controls.Add(this.lblDoctor);
            this.Controls.Add(this.lblPatientValue);
            this.Controls.Add(this.lblPatient);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "CompleteForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Проведение приёма";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblPatient;
        private System.Windows.Forms.Label lblPatientValue;
        private System.Windows.Forms.Label lblDoctor;
        private System.Windows.Forms.Label lblDoctorValue;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Label lblDateValue;
        private System.Windows.Forms.Label lblPrice;
        private System.Windows.Forms.Label lblPriceValue;
        private System.Windows.Forms.Label lblDiagnosis;
        private System.Windows.Forms.TextBox txtDiagnosis;
        private System.Windows.Forms.Button btnComplete;
        private System.Windows.Forms.Button btnCancel;
    }
}
