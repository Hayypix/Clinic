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
            this.panelTopHeader = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblDate = new System.Windows.Forms.Label();
            this.lblUser = new System.Windows.Forms.Label();
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.lblAppTitle = new System.Windows.Forms.Label();
            this.panelContent = new System.Windows.Forms.Panel();
            this.panelBottomBar = new System.Windows.Forms.Panel();
            this.statusStripBottom = new System.Windows.Forms.StatusStrip();
            this.lblStatusMetrics = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblStatusSpring = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblStatusDb = new System.Windows.Forms.ToolStripStatusLabel();
            this.panelHud = new System.Windows.Forms.Panel();
            this.btnNavSummary = new System.Windows.Forms.Button();
            this.btnNavReports = new System.Windows.Forms.Button();
            this.btnNavPatients = new System.Windows.Forms.Button();
            this.btnNavDoctors = new System.Windows.Forms.Button();
            this.btnNavAppointments = new System.Windows.Forms.Button();
            this.panelTopHeader.SuspendLayout();
            this.panelBottomBar.SuspendLayout();
            this.statusStripBottom.SuspendLayout();
            this.panelHud.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTopHeader
            // 
            this.panelTopHeader.BackColor = System.Drawing.Color.White;
            this.panelTopHeader.Controls.Add(this.btnExit);
            this.panelTopHeader.Controls.Add(this.lblDate);
            this.panelTopHeader.Controls.Add(this.lblUser);
            this.panelTopHeader.Controls.Add(this.lblSubtitle);
            this.panelTopHeader.Controls.Add(this.lblAppTitle);
            this.panelTopHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTopHeader.Location = new System.Drawing.Point(0, 0);
            this.panelTopHeader.Name = "panelTopHeader";
            this.panelTopHeader.Size = new System.Drawing.Size(1024, 52);
            this.panelTopHeader.TabIndex = 0;
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnExit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.btnExit.Location = new System.Drawing.Point(934, 11);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(78, 28);
            this.btnExit.TabIndex = 4;
            this.btnExit.Text = "Выход";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // lblDate
            // 
            this.lblDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblDate.Location = new System.Drawing.Point(800, 16);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(120, 20);
            this.lblDate.TabIndex = 3;
            this.lblDate.Text = "26.10.2026";
            this.lblDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblUser
            // 
            this.lblUser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblUser.AutoSize = true;
            this.lblUser.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblUser.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.lblUser.Location = new System.Drawing.Point(620, 18);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(150, 15);
            this.lblUser.TabIndex = 2;
            this.lblUser.Text = "Администратор: admin";
            // 
            // lblSubtitle
            // 
            this.lblSubtitle.AutoSize = true;
            this.lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblSubtitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblSubtitle.Location = new System.Drawing.Point(16, 29);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Size = new System.Drawing.Size(227, 15);
            this.lblSubtitle.TabIndex = 1;
            this.lblSubtitle.Text = "Информационная система управления";
            // 
            // lblAppTitle
            // 
            this.lblAppTitle.AutoSize = true;
            this.lblAppTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblAppTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(118)))), ((int)(((byte)(110)))));
            this.lblAppTitle.Location = new System.Drawing.Point(15, 7);
            this.lblAppTitle.Name = "lblAppTitle";
            this.lblAppTitle.Size = new System.Drawing.Size(268, 21);
            this.lblAppTitle.TabIndex = 0;
            this.lblAppTitle.Text = "Медицинский центр «Здоровье»";
            // 
            // panelContent
            // 
            this.panelContent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.panelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContent.Location = new System.Drawing.Point(0, 52);
            this.panelContent.Name = "panelContent";
            this.panelContent.Size = new System.Drawing.Size(1024, 538);
            this.panelContent.TabIndex = 1;
            // 
            // panelBottomBar
            // 
            this.panelBottomBar.BackColor = System.Drawing.Color.White;
            this.panelBottomBar.Controls.Add(this.statusStripBottom);
            this.panelBottomBar.Controls.Add(this.panelHud);
            this.panelBottomBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottomBar.Location = new System.Drawing.Point(0, 590);
            this.panelBottomBar.Name = "panelBottomBar";
            this.panelBottomBar.Size = new System.Drawing.Size(1024, 76);
            this.panelBottomBar.TabIndex = 2;
            // 
            // statusStripBottom
            // 
            this.statusStripBottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.statusStripBottom.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatusMetrics,
            this.lblStatusSpring,
            this.lblStatusDb});
            this.statusStripBottom.Location = new System.Drawing.Point(0, 52);
            this.statusStripBottom.Name = "statusStripBottom";
            this.statusStripBottom.Size = new System.Drawing.Size(1024, 24);
            this.statusStripBottom.TabIndex = 1;
            // 
            // lblStatusMetrics
            // 
            this.lblStatusMetrics.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblStatusMetrics.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.lblStatusMetrics.Name = "lblStatusMetrics";
            this.lblStatusMetrics.Size = new System.Drawing.Size(370, 19);
            this.lblStatusMetrics.Text = "Врачей: 0  |  Пациентов: 0  |  Приёмов: 0  |  Выручка: 0 руб.";
            // 
            // lblStatusSpring
            // 
            this.lblStatusSpring.Name = "lblStatusSpring";
            this.lblStatusSpring.Size = new System.Drawing.Size(514, 19);
            this.lblStatusSpring.Spring = true;
            // 
            // lblStatusDb
            // 
            this.lblStatusDb.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblStatusDb.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(118)))), ((int)(((byte)(110)))));
            this.lblStatusDb.Name = "lblStatusDb";
            this.lblStatusDb.Size = new System.Drawing.Size(125, 19);
            this.lblStatusDb.Text = "● SQLite подключена";
            // 
            // panelHud
            // 
            this.panelHud.BackColor = System.Drawing.Color.Transparent;
            this.panelHud.Controls.Add(this.btnNavSummary);
            this.panelHud.Controls.Add(this.btnNavReports);
            this.panelHud.Controls.Add(this.btnNavPatients);
            this.panelHud.Controls.Add(this.btnNavDoctors);
            this.panelHud.Controls.Add(this.btnNavAppointments);
            this.panelHud.Location = new System.Drawing.Point(107, 6);
            this.panelHud.Name = "panelHud";
            this.panelHud.Size = new System.Drawing.Size(810, 42);
            this.panelHud.TabIndex = 0;
            // 
            // btnNavSummary
            // 
            this.btnNavSummary.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNavSummary.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNavSummary.Font = new System.Drawing.Font("Segoe UI", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnNavSummary.Location = new System.Drawing.Point(648, 2);
            this.btnNavSummary.Name = "btnNavSummary";
            this.btnNavSummary.Size = new System.Drawing.Size(155, 38);
            this.btnNavSummary.TabIndex = 4;
            this.btnNavSummary.Text = "📋 Сводка";
            this.btnNavSummary.UseVisualStyleBackColor = true;
            this.btnNavSummary.Click += new System.EventHandler(this.btnNavSummary_Click);
            // 
            // btnNavReports
            // 
            this.btnNavReports.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNavReports.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNavReports.Font = new System.Drawing.Font("Segoe UI", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnNavReports.Location = new System.Drawing.Point(486, 2);
            this.btnNavReports.Name = "btnNavReports";
            this.btnNavReports.Size = new System.Drawing.Size(156, 38);
            this.btnNavReports.TabIndex = 3;
            this.btnNavReports.Text = "📊 Отчёты и Excel";
            this.btnNavReports.UseVisualStyleBackColor = true;
            this.btnNavReports.Click += new System.EventHandler(this.btnNavReports_Click);
            // 
            // btnNavPatients
            // 
            this.btnNavPatients.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNavPatients.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNavPatients.Font = new System.Drawing.Font("Segoe UI", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnNavPatients.Location = new System.Drawing.Point(324, 2);
            this.btnNavPatients.Name = "btnNavPatients";
            this.btnNavPatients.Size = new System.Drawing.Size(156, 38);
            this.btnNavPatients.TabIndex = 2;
            this.btnNavPatients.Text = "👥 Пациенты";
            this.btnNavPatients.UseVisualStyleBackColor = true;
            this.btnNavPatients.Click += new System.EventHandler(this.btnNavPatients_Click);
            // 
            // btnNavDoctors
            // 
            this.btnNavDoctors.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNavDoctors.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNavDoctors.Font = new System.Drawing.Font("Segoe UI", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnNavDoctors.Location = new System.Drawing.Point(162, 2);
            this.btnNavDoctors.Name = "btnNavDoctors";
            this.btnNavDoctors.Size = new System.Drawing.Size(156, 38);
            this.btnNavDoctors.TabIndex = 1;
            this.btnNavDoctors.Text = "👨‍⚕️ Врачи";
            this.btnNavDoctors.UseVisualStyleBackColor = true;
            this.btnNavDoctors.Click += new System.EventHandler(this.btnNavDoctors_Click);
            // 
            // btnNavAppointments
            // 
            this.btnNavAppointments.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNavAppointments.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNavAppointments.Font = new System.Drawing.Font("Segoe UI", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnNavAppointments.Location = new System.Drawing.Point(0, 2);
            this.btnNavAppointments.Name = "btnNavAppointments";
            this.btnNavAppointments.Size = new System.Drawing.Size(156, 38);
            this.btnNavAppointments.TabIndex = 0;
            this.btnNavAppointments.Text = "📅 Записи на приём";
            this.btnNavAppointments.UseVisualStyleBackColor = true;
            this.btnNavAppointments.Click += new System.EventHandler(this.btnNavAppointments_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.ClientSize = new System.Drawing.Size(1024, 666);
            this.Controls.Add(this.panelContent);
            this.Controls.Add(this.panelBottomBar);
            this.Controls.Add(this.panelTopHeader);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.MinimumSize = new System.Drawing.Size(900, 600);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Медицинский центр «Здоровье» — Информационная система клиники";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.panelTopHeader.ResumeLayout(false);
            this.panelTopHeader.PerformLayout();
            this.panelBottomBar.ResumeLayout(false);
            this.panelBottomBar.PerformLayout();
            this.statusStripBottom.ResumeLayout(false);
            this.statusStripBottom.PerformLayout();
            this.panelHud.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelTopHeader;
        private System.Windows.Forms.Label lblAppTitle;
        private System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Panel panelContent;
        private System.Windows.Forms.Panel panelBottomBar;
        private System.Windows.Forms.Panel panelHud;
        private System.Windows.Forms.Button btnNavAppointments;
        private System.Windows.Forms.Button btnNavDoctors;
        private System.Windows.Forms.Button btnNavPatients;
        private System.Windows.Forms.Button btnNavReports;
        private System.Windows.Forms.Button btnNavSummary;
        private System.Windows.Forms.StatusStrip statusStripBottom;
        private System.Windows.Forms.ToolStripStatusLabel lblStatusMetrics;
        private System.Windows.Forms.ToolStripStatusLabel lblStatusSpring;
        private System.Windows.Forms.ToolStripStatusLabel lblStatusDb;
    }
}
