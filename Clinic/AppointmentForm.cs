using System;
using System.Data;
using System.Data.SQLite;
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
            dtpTime.Value = DateTime.Today.AddHours(10);
            ShowPrice();
        }

        private void LoadPatients()
        {
            DataTable patients = new DataTable();
            using (SQLiteConnection con = Database.GetConnection())
            using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(
                "SELECT Id, FullName FROM Patients ORDER BY FullName", con))
            {
                adapter.Fill(patients);
            }
            cmbPatient.DataSource = patients;
            cmbPatient.DisplayMember = "FullName";
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

            // колонка с кратким описанием врача
            doctorsTable.Columns.Add("Info", typeof(string),
                "FullName + ', ' + Specialty + ', каб. ' + Room");
            cmbDoctor.DataSource = doctorsTable;
            cmbDoctor.DisplayMember = "Info";
            cmbDoctor.ValueMember = "Id";
        }

        // показ цены приёма выбранного врача
        private void ShowPrice()
        {
            DataRowView row = cmbDoctor.SelectedItem as DataRowView;
            if (row != null)
                price = Convert.ToDouble(row["Price"]);
            else
                price = 0;

            lblPriceValue.Text = price.ToString("0.##") + " руб.";
        }

        private void cmbDoctor_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowPrice();
        }

        // записать пациента на приём
        private void btnBook_Click(object sender, EventArgs e)
        {
            if (cmbPatient.SelectedItem == null)
            {
                MessageBox.Show("Выберите пациента", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cmbDoctor.SelectedItem == null)
            {
                MessageBox.Show("Выберите врача", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DateTime appointmentDateTime = dtpDate.Value.Date + dtpTime.Value.TimeOfDay;
            if (appointmentDateTime < DateTime.Now)
            {
                MessageBox.Show("Нельзя записать на прошедшее время", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dtpTime.Value.Hour < 8 || (dtpTime.Value.Hour == 19 && dtpTime.Value.Minute > 0) || dtpTime.Value.Hour > 19)
            {
                MessageBox.Show("Клиника работает с 8:00 до 19:00", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

                    // проверяем, не занят ли врач в это время
                    SQLiteCommand check = new SQLiteCommand(
                        "SELECT COUNT(*) FROM Appointments " +
                        "WHERE DoctorId = @d AND Date = @date AND Time = @time AND Status = 'Запланирован'", con);
                    check.Parameters.AddWithValue("@d", doctorId);
                    check.Parameters.AddWithValue("@date", date);
                    check.Parameters.AddWithValue("@time", time);

                    if (Convert.ToInt32(check.ExecuteScalar()) > 0)
                    {
                        MessageBox.Show("Этот врач уже занят в выбранное время", "Внимание",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    SQLiteCommand cmd = new SQLiteCommand(
                        "INSERT INTO Appointments (PatientId, DoctorId, Date, Time, Price, Status) " +
                        "VALUES (@p, @d, @date, @time, @price, 'Запланирован')", con);
                    cmd.Parameters.AddWithValue("@p", patientId);
                    cmd.Parameters.AddWithValue("@d", doctorId);
                    cmd.Parameters.AddWithValue("@date", date);
                    cmd.Parameters.AddWithValue("@time", time);
                    cmd.Parameters.AddWithValue("@price", price);
                    cmd.ExecuteNonQuery();
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
