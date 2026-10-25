using System;
using System.Data.SQLite;
using System.Globalization;
using System.Windows.Forms;

namespace Clinic
{
    // Проведение приёма: врач вносит диагноз, приём закрывается
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

        // Показ информации о приёме
        private void LoadInfo()
        {
            try
            {
                using (SQLiteConnection con = Database.GetConnection())
                {
                    con.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(
                        "SELECT p.FullName AS Patient, d.FullName AS Doctor, d.Specialty, " +
                        "strftime('%d.%m.%Y', a.Date) AS DateText, a.Time, a.Price, a.Diagnosis, a.Status " +
                        "FROM Appointments a, Patients p, Doctors d " +
                        "WHERE a.PatientId = p.Id AND a.DoctorId = d.Id AND a.Id = @id", con))
                    {
                        cmd.Parameters.AddWithValue("@id", appointmentId);

                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                lblPatientValue.Text = reader["Patient"] != DBNull.Value ? reader["Patient"].ToString() : "";
                                lblDoctorValue.Text = (reader["Doctor"] != DBNull.Value ? reader["Doctor"].ToString() : "") +
                                    " (" + (reader["Specialty"] != DBNull.Value ? reader["Specialty"].ToString() : "") + ")";
                                lblDateValue.Text = (reader["DateText"] != DBNull.Value ? reader["DateText"].ToString() : "") +
                                    " в " + (reader["Time"] != DBNull.Value ? reader["Time"].ToString() : "");

                                if (reader["Price"] != DBNull.Value)
                                {
                                    double pr = Convert.ToDouble(reader["Price"]);
                                    lblPriceValue.Text = pr.ToString("N0", CultureInfo.CurrentCulture) + " руб.";
                                }
                                else
                                {
                                    lblPriceValue.Text = "0 руб.";
                                }

                                string status = reader["Status"] != DBNull.Value ? reader["Status"].ToString() : "";
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
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось загрузить данные приёма: " + ex.Message, "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnComplete_Click(object sender, EventArgs e)
        {
            string diag = txtDiagnosis.Text.Trim();
            if (string.IsNullOrEmpty(diag))
            {
                MessageBox.Show("Введите диагноз или результаты осмотра (например, 'Здоров, жалоб нет')", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDiagnosis.Focus();
                return;
            }

            try
            {
                using (SQLiteConnection con = Database.GetConnection())
                {
                    con.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(
                        "UPDATE Appointments SET Diagnosis = @diag, Status = 'Проведен' WHERE Id = @id", con))
                    {
                        cmd.Parameters.AddWithValue("@diag", diag);
                        cmd.Parameters.AddWithValue("@id", appointmentId);
                        cmd.ExecuteNonQuery();
                    }
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
