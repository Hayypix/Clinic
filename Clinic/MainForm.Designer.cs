namespace Clinic
{
    partial class MainForm
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
            this.lblHeader = new System.Windows.Forms.Label();
            this.btnDoctors = new System.Windows.Forms.Button();
            this.btnPatients = new System.Windows.Forms.Button();
            this.btnAppointments = new System.Windows.Forms.Button();
            this.btnAbout = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblUser = new System.Windows.Forms.Label();
            this.grpStats = new System.Windows.Forms.GroupBox();
            this.lblStatRevenue = new System.Windows.Forms.Label();
            this.lblStatCompleted = new System.Windows.Forms.Label();
            this.lblStatPlanned = new System.Windows.Forms.Label();
            this.lblStatPatients = new System.Windows.Forms.Label();
            this.lblStatDoctors = new System.Windows.Forms.Label();
            this.btnRefreshStats = new System.Windows.Forms.Button();
            this.grpStats.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblHeader
            // 
            this.lblHeader.AutoSize = true;
            this.lblHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblHeader.Location = new System.Drawing.Point(26, 25);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(320, 24);
            this.lblHeader.TabIndex = 0;
            this.lblHeader.Text = "Система управления клиникой";
            // 
            // btnDoctors
            // 
            this.btnDoctors.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnDoctors.Location = new System.Drawing.Point(30, 75);
            this.btnDoctors.Name = "btnDoctors";
            this.btnDoctors.Size = new System.Drawing.Size(280, 50);
            this.btnDoctors.TabIndex = 1;
            this.btnDoctors.Text = "Врачи";
            this.btnDoctors.UseVisualStyleBackColor = true;
            this.btnDoctors.Click += new System.EventHandler(this.btnDoctors_Click);
            // 
            // btnPatients
            // 
            this.btnPatients.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnPatients.Location = new System.Drawing.Point(30, 135);
            this.btnPatients.Name = "btnPatients";
            this.btnPatients.Size = new System.Drawing.Size(280, 50);
            this.btnPatients.TabIndex = 2;
            this.btnPatients.Text = "Пациенты";
            this.btnPatients.UseVisualStyleBackColor = true;
            this.btnPatients.Click += new System.EventHandler(this.btnPatients_Click);
            // 
            // btnAppointments
            // 
            this.btnAppointments.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnAppointments.Location = new System.Drawing.Point(30, 195);
            this.btnAppointments.Name = "btnAppointments";
            this.btnAppointments.Size = new System.Drawing.Size(280, 50);
            this.btnAppointments.TabIndex = 3;
            this.btnAppointments.Text = "Записи на приём";
            this.btnAppointments.UseVisualStyleBackColor = true;
            this.btnAppointments.Click += new System.EventHandler(this.btnAppointments_Click);
            // 
            // btnAbout
            // 
            this.btnAbout.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnAbout.Location = new System.Drawing.Point(30, 255);
            this.btnAbout.Name = "btnAbout";
            this.btnAbout.Size = new System.Drawing.Size(280, 42);
            this.btnAbout.TabIndex = 4;
            this.btnAbout.Text = "О программе";
            this.btnAbout.UseVisualStyleBackColor = true;
            this.btnAbout.Click += new System.EventHandler(this.btnAbout_Click);
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnExit.Location = new System.Drawing.Point(30, 307);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(280, 42);
            this.btnExit.TabIndex = 5;
            this.btnExit.Text = "Выход";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // lblUser
            // 
            this.lblUser.AutoSize = true;
            this.lblUser.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblUser.Location = new System.Drawing.Point(27, 375);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(148, 15);
            this.lblUser.TabIndex = 6;
            this.lblUser.Text = "Администратор: admin";
            // 
            // grpStats
            // 
            this.grpStats.Controls.Add(this.btnRefreshStats);
            this.grpStats.Controls.Add(this.lblStatRevenue);
            this.grpStats.Controls.Add(this.lblStatCompleted);
            this.grpStats.Controls.Add(this.lblStatPlanned);
            this.grpStats.Controls.Add(this.lblStatPatients);
            this.grpStats.Controls.Add(this.lblStatDoctors);
            this.grpStats.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.grpStats.Location = new System.Drawing.Point(340, 75);
            this.grpStats.Name = "grpStats";
            this.grpStats.Size = new System.Drawing.Size(305, 274);
            this.grpStats.TabIndex = 7;
            this.grpStats.TabStop = false;
            this.grpStats.Text = "Статистика клиники";
            // 
            // lblStatRevenue
            // 
            this.lblStatRevenue.AutoSize = true;
            this.lblStatRevenue.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblStatRevenue.Location = new System.Drawing.Point(15, 175);
            this.lblStatRevenue.Name = "lblStatRevenue";
            this.lblStatRevenue.Size = new System.Drawing.Size(145, 15);
            this.lblStatRevenue.TabIndex = 4;
            this.lblStatRevenue.Text = "• Выручка: 0 руб.";
            // 
            // lblStatCompleted
            // 
            this.lblStatCompleted.AutoSize = true;
            this.lblStatCompleted.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblStatCompleted.Location = new System.Drawing.Point(15, 140);
            this.lblStatCompleted.Name = "lblStatCompleted";
            this.lblStatCompleted.Size = new System.Drawing.Size(155, 15);
            this.lblStatCompleted.TabIndex = 3;
            this.lblStatCompleted.Text = "• Проведено приёмов: 0";
            // 
            // lblStatPlanned
            // 
            this.lblStatPlanned.AutoSize = true;
            this.lblStatPlanned.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblStatPlanned.Location = new System.Drawing.Point(15, 105);
            this.lblStatPlanned.Name = "lblStatPlanned";
            this.lblStatPlanned.Size = new System.Drawing.Size(167, 15);
            this.lblStatPlanned.TabIndex = 2;
            this.lblStatPlanned.Text = "• Запланировано приёмов: 0";
            // 
            // lblStatPatients
            // 
            this.lblStatPatients.AutoSize = true;
            this.lblStatPatients.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblStatPatients.Location = new System.Drawing.Point(15, 70);
            this.lblStatPatients.Name = "lblStatPatients";
            this.lblStatPatients.Size = new System.Drawing.Size(151, 15);
            this.lblStatPatients.TabIndex = 1;
            this.lblStatPatients.Text = "• Пациентов в базе: 0";
            // 
            // lblStatDoctors
            // 
            this.lblStatDoctors.AutoSize = true;
            this.lblStatDoctors.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblStatDoctors.Location = new System.Drawing.Point(15, 35);
            this.lblStatDoctors.Name = "lblStatDoctors";
            this.lblStatDoctors.Size = new System.Drawing.Size(126, 15);
            this.lblStatDoctors.TabIndex = 0;
            this.lblStatDoctors.Text = "• Врачей в штате: 0";
            // 
            // btnRefreshStats
            // 
            this.btnRefreshStats.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnRefreshStats.Location = new System.Drawing.Point(18, 220);
            this.btnRefreshStats.Name = "btnRefreshStats";
            this.btnRefreshStats.Size = new System.Drawing.Size(265, 32);
            this.btnRefreshStats.TabIndex = 5;
            this.btnRefreshStats.Text = "Обновить показатели";
            this.btnRefreshStats.UseVisualStyleBackColor = true;
            this.btnRefreshStats.Click += new System.EventHandler(this.btnRefreshStats_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(674, 405);
            this.Controls.Add(this.grpStats);
            this.Controls.Add(this.lblUser);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnAbout);
            this.Controls.Add(this.btnAppointments);
            this.Controls.Add(this.btnPatients);
            this.Controls.Add(this.btnDoctors);
            this.Controls.Add(this.lblHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Частная клиника";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.grpStats.ResumeLayout(false);
            this.grpStats.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.Button btnDoctors;
        private System.Windows.Forms.Button btnPatients;
        private System.Windows.Forms.Button btnAppointments;
        private System.Windows.Forms.Button btnAbout;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.GroupBox grpStats;
        private System.Windows.Forms.Label lblStatRevenue;
        private System.Windows.Forms.Label lblStatCompleted;
        private System.Windows.Forms.Label lblStatPlanned;
        private System.Windows.Forms.Label lblStatPatients;
        private System.Windows.Forms.Label lblStatDoctors;
        private System.Windows.Forms.Button btnRefreshStats;
    }
}
