using System;
using System.Data.SQLite;
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

        // конструктор для редактирования
        public PatientEditForm(int id) : this()
        {
            patientId = id;
            Text = "Изменение данных пациента";

            try
            {
                using (SQLiteConnection con = Database.GetConnection())
                {
                    con.Open();
                    SQLiteCommand cmd = new SQLiteCommand(
                        "SELECT FullName, BirthDate, Phone, Address FROM Patients WHERE Id = @id", con);
                    cmd.Parameters.AddWithValue("@id", id);

                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            txtName.Text = reader["FullName"].ToString();
                            DateTime d;
                            if (DateTime.TryParse(reader["BirthDate"].ToString(), out d))
                                dtpBirth.Value = d;
                            txtPhone.Text = reader["Phone"].ToString();
                            txtAddress.Text = reader["Address"].ToString();
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
            if (txtName.Text.Trim() == "")
            {
                MessageBox.Show("Укажите ФИО пациента", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dtpBirth.Value.Date >= DateTime.Today)
            {
                MessageBox.Show("Дата рождения указана неверно", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SQLiteConnection con = Database.GetConnection())
                {
                    con.Open();
                    SQLiteCommand cmd;

                    if (patientId == 0)
                    {
                        cmd = new SQLiteCommand(
                            "INSERT INTO Patients (FullName, BirthDate, Phone, Address) " +
                            "VALUES (@n, @b, @p, @a)", con);
                    }
                    else
                    {
                        cmd = new SQLiteCommand(
                            "UPDATE Patients SET FullName = @n, BirthDate = @b, Phone = @p, " +
                            "Address = @a WHERE Id = @id", con);
                        cmd.Parameters.AddWithValue("@id", patientId);
                    }

                    cmd.Parameters.AddWithValue("@n", txtName.Text.Trim());
                    cmd.Parameters.AddWithValue("@b", dtpBirth.Value.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@p", txtPhone.Text.Trim());
                    cmd.Parameters.AddWithValue("@a", txtAddress.Text.Trim());
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
