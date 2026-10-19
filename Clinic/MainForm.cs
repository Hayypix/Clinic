using System;
using System.Windows.Forms;

namespace Clinic
{
    public partial class MainForm : Form
    {
        private string currentLogin = "admin";

        public MainForm()
        {
            InitializeComponent();
        }

        public MainForm(string login) : this()
        {
            currentLogin = login;
            lblUser.Text = "Администратор: " + login;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(currentLogin))
                lblUser.Text = "Администратор: " + currentLogin;

            RefreshStatistics();
        }

        // обновление сводки на главном окне
        public void RefreshStatistics()
        {
            Database.ClinicStats stats = Database.GetStats();
            lblStatDoctors.Text = "• Врачей в штате: " + stats.DoctorsCount;
            lblStatPatients.Text = "• Пациентов в базе: " + stats.PatientsCount;
            lblStatPlanned.Text = "• Запланировано приёмов: " + stats.PlannedAppointments;
            lblStatCompleted.Text = "• Проведено приёмов: " + stats.CompletedAppointments;
            lblStatRevenue.Text = "• Общая выручка: " + stats.TotalRevenue.ToString("N0") + " руб.";
        }

        private void btnDoctors_Click(object sender, EventArgs e)
        {
            DoctorsForm f = new DoctorsForm();
            f.ShowDialog();
            RefreshStatistics();
        }

        private void btnPatients_Click(object sender, EventArgs e)
        {
            PatientsForm f = new PatientsForm();
            f.ShowDialog();
            RefreshStatistics();
        }

        private void btnAppointments_Click(object sender, EventArgs e)
        {
            AppointmentsForm f = new AppointmentsForm();
            f.ShowDialog();
            RefreshStatistics();
        }

        private void btnRefreshStats_Click(object sender, EventArgs e)
        {
            RefreshStatistics();
        }

        // О программе
        private void btnAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Система управления частной клиникой\nВерсия 1.0",
                "О программе",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
