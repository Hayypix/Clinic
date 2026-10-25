using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace Clinic
{
    public partial class MainForm : Form
    {
        private string currentLogin = "admin";

        // Дочерние формы разделов
        private AppointmentsForm appointmentsForm;
        private DoctorsForm doctorsForm;
        private PatientsForm patientsForm;
        private ReportsForm reportsForm;
        private SummaryForm summaryForm;

        private enum NavTab
        {
            Appointments,
            Doctors,
            Patients,
            Reports,
            Summary
        }

        private NavTab currentTab = NavTab.Appointments;

        public MainForm()
        {
            InitializeComponent();
        }

        public MainForm(string login) : this()
        {
            currentLogin = login;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            lblUser.Text = "Администратор: " + currentLogin;
            lblDate.Text = DateTime.Now.ToString("dd.MM.yyyy", CultureInfo.CurrentCulture);

            // Создаем экземпляры дочерних форм
            appointmentsForm = new AppointmentsForm();
            doctorsForm = new DoctorsForm();
            patientsForm = new PatientsForm();
            reportsForm = new ReportsForm();
            summaryForm = new SummaryForm();

            summaryForm.RequestRefreshAll += (s, ev) => RefreshAllMetrics();

            // Центрируем нижний HUD-бар
            CenterHudBar();

            // Открываем первую вкладку по умолчанию (Записи на приём)
            SetTab(NavTab.Appointments);

            // Первичное обновление сводки
            RefreshAllMetrics();
        }

        // Центрирование нижнего HUD-бара по горизонтали
        private void CenterHudBar()
        {
            if (panelHud != null && panelBottomBar != null)
            {
                panelHud.Left = Math.Max(10, (panelBottomBar.ClientSize.Width - panelHud.Width) / 2);
                panelHud.Top = 6;
            }
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            CenterHudBar();
        }

        // Переключение между вкладками через нижний HUD-бар
        private void SetTab(NavTab tab)
        {
            currentTab = tab;

            Form targetForm = null;

            switch (tab)
            {
                case NavTab.Appointments:
                    targetForm = appointmentsForm;
                    appointmentsForm.FillAppointments();
                    break;
                case NavTab.Doctors:
                    targetForm = doctorsForm;
                    doctorsForm.FillDoctors();
                    break;
                case NavTab.Patients:
                    targetForm = patientsForm;
                    patientsForm.FillPatients();
                    break;
                case NavTab.Reports:
                    targetForm = reportsForm;
                    reportsForm.GenerateReport();
                    break;
                case NavTab.Summary:
                    targetForm = summaryForm;
                    summaryForm.RefreshData();
                    break;
            }

            // Подсветка кнопок нижнего HUD-бара
            UpdateButtonStyles();

            // Встраивание дочерней формы в panelContent
            if (targetForm != null)
            {
                panelContent.SuspendLayout();
                panelContent.Controls.Clear();

                targetForm.TopLevel = false;
                targetForm.FormBorderStyle = FormBorderStyle.None;
                targetForm.Dock = DockStyle.Fill;

                panelContent.Controls.Add(targetForm);
                targetForm.Show();
                panelContent.ResumeLayout();
            }

            // Обновляем статусную строку внизу
            RefreshAllMetrics();
        }

        // Стили кнопок нижнего HUD-бара
        private void UpdateButtonStyles()
        {
            Color activeBg = Color.FromArgb(15, 118, 110);    // Медицинский глубокий бирюзовый (#0F766E)
            Color activeFg = Color.White;

            Color inactiveBg = Color.FromArgb(241, 245, 249);  // Мягкий светлый сланец (#F1F5F9)
            Color inactiveFg = Color.FromArgb(30, 41, 59);     // Тёмный сланец (#1E293B)
            Color borderColor = Color.FromArgb(203, 213, 225);

            StyleNavButton(btnNavAppointments, currentTab == NavTab.Appointments, activeBg, activeFg, inactiveBg, inactiveFg, borderColor);
            StyleNavButton(btnNavDoctors, currentTab == NavTab.Doctors, activeBg, activeFg, inactiveBg, inactiveFg, borderColor);
            StyleNavButton(btnNavPatients, currentTab == NavTab.Patients, activeBg, activeFg, inactiveBg, inactiveFg, borderColor);
            StyleNavButton(btnNavReports, currentTab == NavTab.Reports, activeBg, activeFg, inactiveBg, inactiveFg, borderColor);
            StyleNavButton(btnNavSummary, currentTab == NavTab.Summary, activeBg, activeFg, inactiveBg, inactiveFg, borderColor);
        }

        private void StyleNavButton(Button btn, bool isActive, Color activeBg, Color activeFg, Color inactiveBg, Color inactiveFg, Color borderColor)
        {
            if (isActive)
            {
                btn.BackColor = activeBg;
                btn.ForeColor = activeFg;
                btn.Font = new Font("Segoe UI", 9.25f, FontStyle.Bold);
                btn.FlatAppearance.BorderSize = 0;
            }
            else
            {
                btn.BackColor = inactiveBg;
                btn.ForeColor = inactiveFg;
                btn.Font = new Font("Segoe UI", 9.25f, FontStyle.Regular);
                btn.FlatAppearance.BorderSize = 1;
                btn.FlatAppearance.BorderColor = borderColor;
                btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(226, 232, 240);
            }
        }

        // Обновление метрик в нижней полосе
        public void RefreshAllMetrics()
        {
            try
            {
                Database.ClinicStats stats = Database.GetStats();
                lblStatusMetrics.Text = string.Format(
                    "👨‍⚕️ Врачей: {0}  |  👥 Пациентов: {1}  |  📅 Приёмов сегодня: {2}  |  💰 Выручка: {3} руб.",
                    stats.DoctorsCount, stats.PatientsCount, stats.TodayAppointments, stats.TotalRevenue.ToString("N0", CultureInfo.CurrentCulture));
            }
            catch
            {
            }
        }

        // Обработчики кнопок HUD-бара
        private void btnNavAppointments_Click(object sender, EventArgs e)
        {
            SetTab(NavTab.Appointments);
        }

        private void btnNavDoctors_Click(object sender, EventArgs e)
        {
            SetTab(NavTab.Doctors);
        }

        private void btnNavPatients_Click(object sender, EventArgs e)
        {
            SetTab(NavTab.Patients);
        }

        private void btnNavReports_Click(object sender, EventArgs e)
        {
            SetTab(NavTab.Reports);
        }

        private void btnNavSummary_Click(object sender, EventArgs e)
        {
            SetTab(NavTab.Summary);
        }

        // Выход из программы
        private void btnExit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите завершить работу с системой клиники?",
                "Выход из системы", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Close();
            }
        }
    }
}
