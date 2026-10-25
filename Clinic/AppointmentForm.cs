using System;
using System.Data;
using System.Data.SQLite;
using System.Globalization;
using System.Windows.Forms;

namespace Clinic
{
    public partial class AppointmentForm : Form
    {
        private DataTable doctorsTable;
        private double price = 0;

        public AppointmentForm()
        {
            InitializeComponent();
            LoadPatients();
            LoadDoctors();

            dtpDate.Value = DateTime.Today;
            // По умолчанию округляем время до следующего часа
            DateTime defaultTime = DateTime.Today.AddHours(Math.Max(8, Math.Min(18, DateTime.Now.Hour + 1)));
            dtpTime.Value = defaultTime;
            ShowPrice();
        }

        private void LoadPatients()
        {
            DataTable patients = new DataTable();
            using (SQLiteConnection con = Database.GetConnection())
            using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(
                "SELECT Id, FullName, Phone FROM Patients ORDER BY FullName", con))
            {
                adapter.Fill(patients);
            }

            // Добавляем телефон для наглядности
            patients.Columns.Add("DisplayInfo", typeof(string), "FullName + IIF(Phone IS NULL OR Phone = '', '', ' (' + Phone + ')')");

            cmbPatient.DataSource = patients;
            cmbPatient.DisplayMember = "DisplayInfo";
            cmbPatient.ValueMember = "Id";
        }

        private void LoadDoctors()
        {
            doctorsTable = new DataTable();
            using (SQLiteConnection con = Database.GetConnection())
            using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(
                "SELECT Id, FullName, Specialty, Room, Price FROM Doctors ORDER BY FullName", con))
            {
                adapter.Fill(doctorsTable);
            }

            // Колонка с кратким описанием врача
            doctorsTable.Columns.Add("Info", typeof(string),
                "FullName + ' (' + Specialty + ', каб. ' + Room + ')'");
            cmbDoctor.DataSource = doctorsTable;
            cmbDoctor.DisplayMember = "Info";
            cmbDoctor.ValueMember = "Id";
        }

        // Показ цены приёма выбранного врача
        private void ShowPrice()
        {
            DataRowView row = cmbDoctor.SelectedItem as DataRowView;
            if (row != null && row["Price"] != DBNull.Value)
                price = Convert.ToDouble(row["Price"]);
            else
                price = 0;

            lblPriceValue.Text = price.ToString("N0", CultureInfo.CurrentCulture) + " руб.";
        }

        private void cmbDoctor_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowPrice();
        }

        // Записать пациента на приём
        private void btnBook_Click(object sender, EventArgs e)
        {
            if (cmbPatient.SelectedItem == null)
            {
                MessageBox.Show("Выберите пациента из списка", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbPatient.Focus();
                return;
            }

            if (cmbDoctor.SelectedItem == null)
            {
                MessageBox.Show("Выберите врача из списка", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbDoctor.Focus();
                return;
            }

            DateTime appointmentDateTime = dtpDate.Value.Date + dtpTime.Value.TimeOfDay;

            // Разрешаем небольшой люфт 10 минут, если запись оформляется на текущее время
            if (appointmentDateTime < DateTime.Now.AddMinutes(-10))
            {
                if (MessageBox.Show("Выбранная дата и время уже прошли.\nВсё равно оформить запись на это время?",
                    "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    return;
                }
            }

            if (dtpTime.Value.Hour < 8 || (dtpTime.Value.Hour == 19 && dtpTime.Value.Minute > 0) || dtpTime.Value.Hour > 19)
            {
                MessageBox.Show("Часы работы клиники: с 08:00 до 19:00", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpTime.Focus();
                return;
            }

            int patientId = Convert.ToInt32(((DataRowView)cmbPatient.SelectedItem)["Id"]);
            int doctorId = Convert.ToInt32(((DataRowView)cmbDoctor.SelectedItem)["Id"]);
            string date = dtpDate.Value.ToString("yyyy-MM-dd");
            string time = dtpTime.Value.ToString("HH:mm");

            try
            {
                using (SQLiteConnection con = Database.GetConnection())
                {
                    con.Open();

                    // Проверяем, не занят ли врач в это время (активной записью)
                    using (SQLiteCommand check = new SQLiteCommand(
                        "SELECT COUNT(*) FROM Appointments " +
                        "WHERE DoctorId = @d AND Date = @date AND Time = @time AND Status <> 'Отменен'", con))
                    {
                        check.Parameters.AddWithValue("@d", doctorId);
                        check.Parameters.AddWithValue("@date", date);
                        check.Parameters.AddWithValue("@time", time);

                        if (Convert.ToInt32(check.ExecuteScalar()) > 0)
                        {
                            MessageBox.Show("Выбранный врач уже занят в это время.\nПожалуйста, выберите другое время приёма.",
                                "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    // Проверяем, не записан ли этот пациент уже к другому врачу на это же время
                    using (SQLiteCommand checkPatient = new SQLiteCommand(
                        "SELECT COUNT(*) FROM Appointments " +
                        "WHERE PatientId = @p AND Date = @date AND Time = @time AND Status <> 'Отменен'", con))
                    {
                        checkPatient.Parameters.AddWithValue("@p", patientId);
                        checkPatient.Parameters.AddWithValue("@date", date);
                        checkPatient.Parameters.AddWithValue("@time", time);

                        if (Convert.ToInt32(checkPatient.ExecuteScalar()) > 0)
                        {
                            MessageBox.Show("У данного пациента уже есть активная запись на это же время.",
                                "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    using (SQLiteCommand cmd = new SQLiteCommand(
                        "INSERT INTO Appointments (PatientId, DoctorId, Date, Time, Price, Status) " +
                        "VALUES (@p, @d, @date, @time, @price, 'Запланирован')", con))
                    {
                        cmd.Parameters.AddWithValue("@p", patientId);
                        cmd.Parameters.AddWithValue("@d", doctorId);
                        cmd.Parameters.AddWithValue("@date", date);
                        cmd.Parameters.AddWithValue("@time", time);
                        cmd.Parameters.AddWithValue("@price", price);
                        cmd.ExecuteNonQuery();
                    }
                }

                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при записи: " + ex.Message, "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
