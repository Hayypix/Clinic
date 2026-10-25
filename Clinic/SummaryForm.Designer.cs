namespace Clinic
{
    partial class SummaryForm
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
            this.panelHeader = new System.Windows.Forms.Panel();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panelCards = new System.Windows.Forms.Panel();
            this.cardDoctors = new System.Windows.Forms.Panel();
            this.lblValDoctors = new System.Windows.Forms.Label();
            this.lblTitleDoctors = new System.Windows.Forms.Label();
            this.cardPatients = new System.Windows.Forms.Panel();
            this.lblValPatients = new System.Windows.Forms.Label();
            this.lblTitlePatients = new System.Windows.Forms.Label();
            this.cardAppointments = new System.Windows.Forms.Panel();
            this.lblValPlanned = new System.Windows.Forms.Label();
            this.lblTitleAppointments = new System.Windows.Forms.Label();
            this.cardRevenue = new System.Windows.Forms.Panel();
            this.lblValRevenue = new System.Windows.Forms.Label();
            this.lblTitleRevenue = new System.Windows.Forms.Label();
            this.panelQuickActions = new System.Windows.Forms.Panel();
            this.lblActionsTitle = new System.Windows.Forms.Label();
            this.btnQuickBook = new System.Windows.Forms.Button();
            this.btnQuickPatient = new System.Windows.Forms.Button();
            this.btnQuickDoctor = new System.Windows.Forms.Button();
            this.btnAbout = new System.Windows.Forms.Button();
            this.panelUpcoming = new System.Windows.Forms.Panel();
            this.dgvUpcoming = new System.Windows.Forms.DataGridView();
            this.lblUpcomingTitle = new System.Windows.Forms.Label();
            this.panelHeader.SuspendLayout();
            this.panelCards.SuspendLayout();
            this.cardDoctors.SuspendLayout();
            this.cardPatients.SuspendLayout();
            this.cardAppointments.SuspendLayout();
            this.cardRevenue.SuspendLayout();
            this.panelQuickActions.SuspendLayout();
            this.panelUpcoming.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUpcoming)).BeginInit();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.White;
            this.panelHeader.Controls.Add(this.btnRefresh);
            this.panelHeader.Controls.Add(this.lblSubtitle);
            this.panelHeader.Controls.Add(this.lblTitle);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(960, 60);
            this.panelHeader.TabIndex = 0;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefresh.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnRefresh.Location = new System.Drawing.Point(800, 14);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(145, 32);
            this.btnRefresh.TabIndex = 2;
            this.btnRefresh.Text = "Обновить показатели";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // lblSubtitle
            // 
            this.lblSubtitle.AutoSize = true;
            this.lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblSubtitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.lblSubtitle.Location = new System.Drawing.Point(15, 33);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Size = new System.Drawing.Size(269, 15);
            this.lblSubtitle.TabIndex = 1;
            this.lblSubtitle.Text = "Оперативные показатели и ближайшие приёмы";
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(118)))), ((int)(((byte)(110)))));
            this.lblTitle.Location = new System.Drawing.Point(14, 8);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(262, 25);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Сводка и аналитика клиники";
            // 
            // panelCards
            // 
            this.panelCards.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.panelCards.Controls.Add(this.cardRevenue);
            this.panelCards.Controls.Add(this.cardAppointments);
            this.panelCards.Controls.Add(this.cardPatients);
            this.panelCards.Controls.Add(this.cardDoctors);
            this.panelCards.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelCards.Location = new System.Drawing.Point(0, 60);
            this.panelCards.Name = "panelCards";
            this.panelCards.Padding = new System.Windows.Forms.Padding(12, 10, 12, 10);
            this.panelCards.Size = new System.Drawing.Size(960, 100);
            this.panelCards.TabIndex = 1;
            // 
            // cardDoctors
            // 
            this.cardDoctors.BackColor = System.Drawing.Color.White;
            this.cardDoctors.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cardDoctors.Controls.Add(this.lblValDoctors);
            this.cardDoctors.Controls.Add(this.lblTitleDoctors);
            this.cardDoctors.Location = new System.Drawing.Point(14, 10);
            this.cardDoctors.Name = "cardDoctors";
            this.cardDoctors.Size = new System.Drawing.Size(220, 78);
            this.cardDoctors.TabIndex = 0;
            // 
            // lblValDoctors
            // 
            this.lblValDoctors.AutoSize = true;
            this.lblValDoctors.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblValDoctors.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(118)))), ((int)(((byte)(110)))));
            this.lblValDoctors.Location = new System.Drawing.Point(14, 32);
            this.lblValDoctors.Name = "lblValDoctors";
            this.lblValDoctors.Size = new System.Drawing.Size(28, 32);
            this.lblValDoctors.TabIndex = 1;
            this.lblValDoctors.Text = "0";
            // 
            // lblTitleDoctors
            // 
            this.lblTitleDoctors.AutoSize = true;
            this.lblTitleDoctors.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblTitleDoctors.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblTitleDoctors.Location = new System.Drawing.Point(14, 10);
            this.lblTitleDoctors.Name = "lblTitleDoctors";
            this.lblTitleDoctors.Size = new System.Drawing.Size(92, 15);
            this.lblTitleDoctors.TabIndex = 0;
            this.lblTitleDoctors.Text = "Врачей в штате";
            // 
            // cardPatients
            // 
            this.cardPatients.BackColor = System.Drawing.Color.White;
            this.cardPatients.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cardPatients.Controls.Add(this.lblValPatients);
            this.cardPatients.Controls.Add(this.lblTitlePatients);
            this.cardPatients.Location = new System.Drawing.Point(248, 10);
            this.cardPatients.Name = "cardPatients";
            this.cardPatients.Size = new System.Drawing.Size(220, 78);
            this.cardPatients.TabIndex = 1;
            // 
            // lblValPatients
            // 
            this.lblValPatients.AutoSize = true;
            this.lblValPatients.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblValPatients.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(118)))), ((int)(((byte)(110)))));
            this.lblValPatients.Location = new System.Drawing.Point(14, 32);
            this.lblValPatients.Name = "lblValPatients";
            this.lblValPatients.Size = new System.Drawing.Size(28, 32);
            this.lblValPatients.TabIndex = 1;
            this.lblValPatients.Text = "0";
            // 
            // lblTitlePatients
            // 
            this.lblTitlePatients.AutoSize = true;
            this.lblTitlePatients.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblTitlePatients.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblTitlePatients.Location = new System.Drawing.Point(14, 10);
            this.lblTitlePatients.Name = "lblTitlePatients";
            this.lblTitlePatients.Size = new System.Drawing.Size(107, 15);
            this.lblTitlePatients.TabIndex = 0;
            this.lblTitlePatients.Text = "Пациентов в базе";
            // 
            // cardAppointments
            // 
            this.cardAppointments.BackColor = System.Drawing.Color.White;
            this.cardAppointments.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cardAppointments.Controls.Add(this.lblValPlanned);
            this.cardAppointments.Controls.Add(this.lblTitleAppointments);
            this.cardAppointments.Location = new System.Drawing.Point(482, 10);
            this.cardAppointments.Name = "cardAppointments";
            this.cardAppointments.Size = new System.Drawing.Size(220, 78);
            this.cardAppointments.TabIndex = 2;
            // 
            // lblValPlanned
            // 
            this.lblValPlanned.AutoSize = true;
            this.lblValPlanned.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblValPlanned.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(118)))), ((int)(((byte)(110)))));
            this.lblValPlanned.Location = new System.Drawing.Point(14, 32);
            this.lblValPlanned.Name = "lblValPlanned";
            this.lblValPlanned.Size = new System.Drawing.Size(28, 32);
            this.lblValPlanned.TabIndex = 1;
            this.lblValPlanned.Text = "0";
            // 
            // lblTitleAppointments
            // 
            this.lblTitleAppointments.AutoSize = true;
            this.lblTitleAppointments.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblTitleAppointments.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblTitleAppointments.Location = new System.Drawing.Point(14, 10);
            this.lblTitleAppointments.Name = "lblTitleAppointments";
            this.lblTitleAppointments.Size = new System.Drawing.Size(147, 15);
            this.lblTitleAppointments.TabIndex = 0;
            this.lblTitleAppointments.Text = "Запланировано приёмов";
            // 
            // cardRevenue
            // 
            this.cardRevenue.BackColor = System.Drawing.Color.White;
            this.cardRevenue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cardRevenue.Controls.Add(this.lblValRevenue);
            this.cardRevenue.Controls.Add(this.lblTitleRevenue);
            this.cardRevenue.Location = new System.Drawing.Point(716, 10);
            this.cardRevenue.Name = "cardRevenue";
            this.cardRevenue.Size = new System.Drawing.Size(228, 78);
            this.cardRevenue.TabIndex = 3;
            // 
            // lblValRevenue
            // 
            this.lblValRevenue.AutoSize = true;
            this.lblValRevenue.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblValRevenue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(118)))), ((int)(((byte)(110)))));
            this.lblValRevenue.Location = new System.Drawing.Point(14, 32);
            this.lblValRevenue.Name = "lblValRevenue";
            this.lblValRevenue.Size = new System.Drawing.Size(86, 32);
            this.lblValRevenue.TabIndex = 1;
            this.lblValRevenue.Text = "0 руб.";
            // 
            // lblTitleRevenue
            // 
            this.lblTitleRevenue.AutoSize = true;
            this.lblTitleRevenue.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblTitleRevenue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblTitleRevenue.Location = new System.Drawing.Point(14, 10);
            this.lblTitleRevenue.Name = "lblTitleRevenue";
            this.lblTitleRevenue.Size = new System.Drawing.Size(92, 15);
            this.lblTitleRevenue.TabIndex = 0;
            this.lblTitleRevenue.Text = "Общая выручка";
            // 
            // panelQuickActions
            // 
            this.panelQuickActions.BackColor = System.Drawing.Color.White;
            this.panelQuickActions.Controls.Add(this.btnAbout);
            this.panelQuickActions.Controls.Add(this.btnQuickDoctor);
            this.panelQuickActions.Controls.Add(this.btnQuickPatient);
            this.panelQuickActions.Controls.Add(this.btnQuickBook);
            this.panelQuickActions.Controls.Add(this.lblActionsTitle);
            this.panelQuickActions.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelQuickActions.Location = new System.Drawing.Point(740, 160);
            this.panelQuickActions.Name = "panelQuickActions";
            this.panelQuickActions.Padding = new System.Windows.Forms.Padding(12);
            this.panelQuickActions.Size = new System.Drawing.Size(220, 350);
            this.panelQuickActions.TabIndex = 2;
            // 
            // lblActionsTitle
            // 
            this.lblActionsTitle.AutoSize = true;
            this.lblActionsTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblActionsTitle.Location = new System.Drawing.Point(12, 12);
            this.lblActionsTitle.Name = "lblActionsTitle";
            this.lblActionsTitle.Size = new System.Drawing.Size(130, 19);
            this.lblActionsTitle.TabIndex = 0;
            this.lblActionsTitle.Text = "Быстрые действия:";
            // 
            // btnQuickBook
            // 
            this.btnQuickBook.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnQuickBook.Location = new System.Drawing.Point(12, 45);
            this.btnQuickBook.Name = "btnQuickBook";
            this.btnQuickBook.Size = new System.Drawing.Size(195, 36);
            this.btnQuickBook.TabIndex = 1;
            this.btnQuickBook.Text = "+ Записать на приём";
            this.btnQuickBook.UseVisualStyleBackColor = true;
            this.btnQuickBook.Click += new System.EventHandler(this.btnQuickBook_Click);
            // 
            // btnQuickPatient
            // 
            this.btnQuickPatient.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnQuickPatient.Location = new System.Drawing.Point(12, 90);
            this.btnQuickPatient.Name = "btnQuickPatient";
            this.btnQuickPatient.Size = new System.Drawing.Size(195, 36);
            this.btnQuickPatient.TabIndex = 2;
            this.btnQuickPatient.Text = "+ Новый пациент";
            this.btnQuickPatient.UseVisualStyleBackColor = true;
            this.btnQuickPatient.Click += new System.EventHandler(this.btnQuickPatient_Click);
            // 
            // btnQuickDoctor
            // 
            this.btnQuickDoctor.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnQuickDoctor.Location = new System.Drawing.Point(12, 135);
            this.btnQuickDoctor.Name = "btnQuickDoctor";
            this.btnQuickDoctor.Size = new System.Drawing.Size(195, 36);
            this.btnQuickDoctor.TabIndex = 3;
            this.btnQuickDoctor.Text = "+ Новый врач";
            this.btnQuickDoctor.UseVisualStyleBackColor = true;
            this.btnQuickDoctor.Click += new System.EventHandler(this.btnQuickDoctor_Click);
            // 
            // btnAbout
            // 
            this.btnAbout.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnAbout.Location = new System.Drawing.Point(12, 180);
            this.btnAbout.Name = "btnAbout";
            this.btnAbout.Size = new System.Drawing.Size(195, 36);
            this.btnAbout.TabIndex = 4;
            this.btnAbout.Text = "О программе";
            this.btnAbout.UseVisualStyleBackColor = true;
            this.btnAbout.Click += new System.EventHandler(this.btnAbout_Click);
            // 
            // panelUpcoming
            // 
            this.panelUpcoming.BackColor = System.Drawing.Color.White;
            this.panelUpcoming.Controls.Add(this.dgvUpcoming);
            this.panelUpcoming.Controls.Add(this.lblUpcomingTitle);
            this.panelUpcoming.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelUpcoming.Location = new System.Drawing.Point(0, 160);
            this.panelUpcoming.Name = "panelUpcoming";
            this.panelUpcoming.Padding = new System.Windows.Forms.Padding(14, 12, 12, 12);
            this.panelUpcoming.Size = new System.Drawing.Size(740, 350);
            this.panelUpcoming.TabIndex = 3;
            // 
            // dgvUpcoming
            // 
            this.dgvUpcoming.AllowUserToAddRows = false;
            this.dgvUpcoming.AllowUserToDeleteRows = false;
            this.dgvUpcoming.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvUpcoming.BackgroundColor = System.Drawing.Color.White;
            this.dgvUpcoming.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUpcoming.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvUpcoming.Location = new System.Drawing.Point(14, 37);
            this.dgvUpcoming.MultiSelect = false;
            this.dgvUpcoming.Name = "dgvUpcoming";
            this.dgvUpcoming.ReadOnly = true;
            this.dgvUpcoming.RowHeadersVisible = false;
            this.dgvUpcoming.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvUpcoming.Size = new System.Drawing.Size(714, 301);
            this.dgvUpcoming.TabIndex = 1;
            // 
            // lblUpcomingTitle
            // 
            this.lblUpcomingTitle.AutoSize = true;
            this.lblUpcomingTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblUpcomingTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblUpcomingTitle.Location = new System.Drawing.Point(14, 12);
            this.lblUpcomingTitle.Name = "lblUpcomingTitle";
            this.lblUpcomingTitle.Padding = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.lblUpcomingTitle.Size = new System.Drawing.Size(248, 25);
            this.lblUpcomingTitle.TabIndex = 0;
            this.lblUpcomingTitle.Text = "Ближайшие запланированные приёмы:";
            // 
            // SummaryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(960, 510);
            this.Controls.Add(this.panelUpcoming);
            this.Controls.Add(this.panelQuickActions);
            this.Controls.Add(this.panelCards);
            this.Controls.Add(this.panelHeader);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Name = "SummaryForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Сводка показателей";
            this.Resize += new System.EventHandler(this.SummaryForm_Resize);
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.panelCards.ResumeLayout(false);
            this.cardDoctors.ResumeLayout(false);
            this.cardDoctors.PerformLayout();
            this.cardPatients.ResumeLayout(false);
            this.cardPatients.PerformLayout();
            this.cardAppointments.ResumeLayout(false);
            this.cardAppointments.PerformLayout();
            this.cardRevenue.ResumeLayout(false);
            this.cardRevenue.PerformLayout();
            this.panelQuickActions.ResumeLayout(false);
            this.panelQuickActions.PerformLayout();
            this.panelUpcoming.ResumeLayout(false);
            this.panelUpcoming.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUpcoming)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panelCards;
        private System.Windows.Forms.Panel cardDoctors;
        private System.Windows.Forms.Label lblValDoctors;
        private System.Windows.Forms.Label lblTitleDoctors;
        private System.Windows.Forms.Panel cardPatients;
        private System.Windows.Forms.Label lblValPatients;
        private System.Windows.Forms.Label lblTitlePatients;
        private System.Windows.Forms.Panel cardAppointments;
        private System.Windows.Forms.Label lblValPlanned;
        private System.Windows.Forms.Label lblTitleAppointments;
        private System.Windows.Forms.Panel cardRevenue;
        private System.Windows.Forms.Label lblValRevenue;
        private System.Windows.Forms.Label lblTitleRevenue;
        private System.Windows.Forms.Panel panelQuickActions;
        private System.Windows.Forms.Label lblActionsTitle;
        private System.Windows.Forms.Button btnQuickBook;
        private System.Windows.Forms.Button btnQuickPatient;
        private System.Windows.Forms.Button btnQuickDoctor;
        private System.Windows.Forms.Button btnAbout;
        private System.Windows.Forms.Panel panelUpcoming;
        private System.Windows.Forms.DataGridView dgvUpcoming;
        private System.Windows.Forms.Label lblUpcomingTitle;
    }
}
