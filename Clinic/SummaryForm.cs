using System;
using System.Data;
using System.Data.SQLite;
using System.Globalization;
using System.Windows.Forms;

namespace Clinic
{
    public partial class SummaryForm : Form
    {
        public event EventHandler RequestRefreshAll;

        public SummaryForm()
        {
            InitializeComponent();
            RefreshData();
        }

        public void RefreshData()
        {
            try
            {
                Database.ClinicStats stats = Database.GetStats();

                lblValDoctors.Text = stats.DoctorsCount.ToString();
                lblValPatients.Text = stats.PatientsCount.ToString();
                lblValPlanned.Text = stats.PlannedAppointments.ToString();
                lblValRevenue.Text = stats.TotalRevenue.ToString("N0", CultureInfo.CurrentCulture) + " руб.";

                // Загрузка ближайших приёмов
                DataTable table = new DataTable();
                string sql = @"
                    SELECT 
                        strftime('%d.%m.%Y', a.Date) AS 'Дата',
                        a.Time AS 'Время',
                        p.FullName AS 'Пациент',
                        p.Phone AS 'Телефон',
                        d.FullName AS 'Врач',
                        d.Specialty AS 'Специальность',
                        d.Room AS 'Кабинет',
                        a.Price AS 'Стоимость, руб.',
                        a.Status AS 'Статус'
                    FROM Appointments a
                    JOIN Patients p ON a.PatientId = p.Id
                    JOIN Doctors d ON a.DoctorId = d.Id
                    WHERE a.Status = 'Запланирован'
                    ORDER BY a.Date ASC, a.Time ASC
                    LIMIT 20";

                using (SQLiteConnection con = Database.GetConnection())
                using (SQLiteDataAdapter da = new SQLiteDataAdapter(sql, con))
                {
                    da.Fill(table);
                }

                dgvUpcoming.DataSource = table;

                if (dgvUpcoming.Columns.Contains("Дата"))
                    dgvUpcoming.Columns["Дата"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                if (dgvUpcoming.Columns.Contains("Время"))
                    dgvUpcoming.Columns["Время"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                if (dgvUpcoming.Columns.Contains("Кабинет"))
                    dgvUpcoming.Columns["Кабинет"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                if (dgvUpcoming.Columns.Contains("Стоимость, руб."))
                {
                    dgvUpcoming.Columns["Стоимость, руб."].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvUpcoming.Columns["Стоимость, руб."].DefaultCellStyle.Format = "N0";
                }
                if (dgvUpcoming.Columns.Contains("Статус"))
                    dgvUpcoming.Columns["Статус"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            catch
            {
                // Не крашим приложение при ошибках сводки
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshData();
            if (RequestRefreshAll != null)
                RequestRefreshAll(this, EventArgs.Empty);
        }

        private void btnQuickBook_Click(object sender, EventArgs e)
        {
            AppointmentForm f = new AppointmentForm();
            if (f.ShowDialog() == DialogResult.OK)
            {
                RefreshData();
                if (RequestRefreshAll != null)
                    RequestRefreshAll(this, EventArgs.Empty);
            }
        }

        private void btnQuickPatient_Click(object sender, EventArgs e)
        {
            PatientEditForm f = new PatientEditForm();
            if (f.ShowDialog() == DialogResult.OK)
            {
                RefreshData();
                if (RequestRefreshAll != null)
                    RequestRefreshAll(this, EventArgs.Empty);
            }
        }

        private void btnQuickDoctor_Click(object sender, EventArgs e)
        {
            DoctorEditForm f = new DoctorEditForm();
            if (f.ShowDialog() == DialogResult.OK)
            {
                RefreshData();
                if (RequestRefreshAll != null)
                    RequestRefreshAll(this, EventArgs.Empty);
            }
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Информационная система управления частной медицинской клиникой\n" +
                "Версия 2.0 (С встроенным модулем отчётности и экспорта Excel)\n\n" +
                "Разработано для автоматизации регистратуры, учёта врачей, пациентов и приёмов.",
                "О программе",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void SummaryForm_Resize(object sender, EventArgs e)
        {
            // Адаптивное распределение карточек по ширине
            int availableWidth = panelCards.ClientSize.Width - 28;
            int cardWidth = Math.Max(180, (availableWidth - 36) / 4);
            cardDoctors.Width = cardWidth;
            cardPatients.Width = cardWidth;
            cardAppointments.Width = cardWidth;
            cardRevenue.Width = cardWidth;

            cardPatients.Left = cardDoctors.Right + 12;
            cardAppointments.Left = cardPatients.Right + 12;
            cardRevenue.Left = cardAppointments.Right + 12;
        }
    }
}
