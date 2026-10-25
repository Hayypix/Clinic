using System;
using System.Data.SQLite;
using System.Globalization;
using System.Windows.Forms;

namespace Clinic
{
    public partial class PatientEditForm : Form
    {
        private int patientId;   // 0 - новый пациент, иначе редактирование

        public PatientEditForm()
        {
            InitializeComponent();
            patientId = 0;
            dtpBirth.Value = new DateTime(1990, 1, 1);
        }

        // Конструктор для редактирования
        public PatientEditForm(int id) : this()
        {
            patientId = id;
            Text = "Редактирование данных пациента";

            try
            {
                using (SQLiteConnection con = Database.GetConnection())
                {
                    con.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(
                        "SELECT FullName, BirthDate, Phone, Address FROM Patients WHERE Id = @id", con))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtName.Text = reader["FullName"] != DBNull.Value ? reader["FullName"].ToString() : "";
                                string bStr = reader["BirthDate"] != DBNull.Value ? reader["BirthDate"].ToString() : "";
                                if (!string.IsNullOrEmpty(bStr))
                                {
                                    DateTime d;
                                    string[] formats = { "yyyy-MM-dd", "dd.MM.yyyy", "yyyy/MM/dd", "d.M.yyyy" };
                                    if (DateTime.TryParseExact(bStr, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out d) ||
                                        DateTime.TryParse(bStr, out d))
                                    {
                                        dtpBirth.Value = d;
                                    }
                                }
                                txtPhone.Text = reader["Phone"] != DBNull.Value ? reader["Phone"].ToString() : "";
                                txtAddress.Text = reader["Address"] != DBNull.Value ? reader["Address"].ToString() : "";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось загрузить данные пациента: " + ex.Message, "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            string phone = txtPhone.Text.Trim();
            string address = txtAddress.Text.Trim();

            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Укажите ФИО пациента", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return;
            }

            if (dtpBirth.Value.Date >= DateTime.Today)
            {
                MessageBox.Show("Дата рождения указана неверно (не может быть в будущем)", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpBirth.Focus();
                return;
            }

            if (dtpBirth.Value.Year < 1900)
            {
                MessageBox.Show("Проверьте год рождения (указана некорректная дата)", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpBirth.Focus();
                return;
            }

            try
            {
                using (SQLiteConnection con = Database.GetConnection())
                {
                    con.Open();
                    using (SQLiteCommand cmd = con.CreateCommand())
                    {
                        if (patientId == 0)
                        {
                            cmd.CommandText =
                                "INSERT INTO Patients (FullName, BirthDate, Phone, Address) " +
                                "VALUES (@n, @b, @p, @a)";
                        }
                        else
                        {
                            cmd.CommandText =
                                "UPDATE Patients SET FullName = @n, BirthDate = @b, Phone = @p, " +
                                "Address = @a WHERE Id = @id";
                            cmd.Parameters.AddWithValue("@id", patientId);
                        }

                        cmd.Parameters.AddWithValue("@n", name);
                        cmd.Parameters.AddWithValue("@b", dtpBirth.Value.ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@p", phone);
                        cmd.Parameters.AddWithValue("@a", address);
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
