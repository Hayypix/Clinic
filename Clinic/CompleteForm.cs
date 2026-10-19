using System;
using System.Data.SQLite;
using System.Windows.Forms;

namespace Clinic
{
    // проведение приема: врач вносит диагноз, приём закрывается
    public partial class CompleteForm : Form
    {
        private int appointmentId;
        private bool isViewOnly;

        public CompleteForm(int id, bool viewOnly = false)
        {
            InitializeComponent();
            appointmentId = id;
            this.isViewOnly = viewOnly;
            LoadInfo();
        }

        // показ информации о приеме
        private void LoadInfo()
        {
            try
            {
                using (SQLiteConnection con = Database.GetConnection())
                {
                    con.Open();
                    SQLiteCommand cmd = new SQLiteCommand(
                        "SELECT p.FullName AS Patient, d.FullName AS Doctor, d.Specialty, " +
                        "strftime('%d.%m.%Y', a.Date) AS DateText, a.Time, a.Price, a.Diagnosis, a.Status " +
                        "FROM Appointments a, Patients p, Doctors d " +
                        "WHERE a.PatientId = p.Id AND a.DoctorId = d.Id AND a.Id = @id", con);
                    cmd.Parameters.AddWithValue("@id", appointmentId);

                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            lblPatientValue.Text = reader["Patient"].ToString();
                            lblDoctorValue.Text = reader["Doctor"].ToString() +
                                " (" + reader["Specialty"].ToString() + ")";
                            lblDateValue.Text = reader["DateText"].ToString() +
                                " в " + reader["Time"].ToString();
                            lblPriceValue.Text = Convert.ToDouble(reader["Price"]).ToString("0.##") + " руб.";

                            string status = reader["Status"].ToString();
                            string diag = reader["Diagnosis"] != DBNull.Value ? reader["Diagnosis"].ToString() : "";

                            if (status != "Запланирован")
                            {
                                isViewOnly = true;
                            }

                            if (isViewOnly)
                            {
                                txtDiagnosis.Text = diag;
                                txtDiagnosis.ReadOnly = true;
                                btnComplete.Visible = false;
                                btnCancel.Text = "Закрыть";
                                Text = "Данные приёма (" + status + ")";
                            }
                            else
                            {
                                Text = "Проведение приёма";
                                btnComplete.Text = "Провести";
                                btnCancel.Text = "Отмена";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось загрузить данные приёма: " + ex.Message, "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnComplete_Click(object sender, EventArgs e)
        {
            if (txtDiagnosis.Text.Trim() == "")
            {
                MessageBox.Show("Введите диагноз (или напишите 'здоров')", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SQLiteConnection con = Database.GetConnection())
                {
                    con.Open();
                    SQLiteCommand cmd = new SQLiteCommand(
                        "UPDATE Appointments SET Diagnosis = @diag, Status = 'Проведен' WHERE Id = @id", con);
                    cmd.Parameters.AddWithValue("@diag", txtDiagnosis.Text.Trim());
                    cmd.Parameters.AddWithValue("@id", appointmentId);
                    cmd.ExecuteNonQuery();
                }

                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении: " + ex.Message, "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
