using System;
using System.Data.SQLite;
using System.Globalization;
using System.Windows.Forms;

namespace Clinic
{
    public partial class DoctorEditForm : Form
    {
        private int doctorId;   // 0 - новый врач, иначе редактирование

        public DoctorEditForm()
        {
            InitializeComponent();
            doctorId = 0;

            cmbSpecialty.Items.AddRange(new object[]
            {
                "Терапевт", "Хирург", "Кардиолог", "Окулист",
                "Стоматолог", "Невролог", "Отоларинголог", "Педиатр",
                "Дерматолог", "Эндокринолог", "Уролог", "Гинеколог",
                "УЗИ-специалист", "Аллерголог", "Гастроэнтеролог"
            });
        }

        // Конструктор для редактирования существующего врача
        public DoctorEditForm(int id) : this()
        {
            doctorId = id;
            Text = "Редактирование данных врача";

            try
            {
                using (SQLiteConnection con = Database.GetConnection())
                {
                    con.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(
                        "SELECT FullName, Specialty, Phone, Room, Price FROM Doctors WHERE Id = @id", con))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtName.Text = reader["FullName"] != DBNull.Value ? reader["FullName"].ToString() : "";
                                cmbSpecialty.Text = reader["Specialty"] != DBNull.Value ? reader["Specialty"].ToString() : "";
                                txtPhone.Text = reader["Phone"] != DBNull.Value ? reader["Phone"].ToString() : "";
                                txtRoom.Text = reader["Room"] != DBNull.Value ? reader["Room"].ToString() : "";
                                if (reader["Price"] != DBNull.Value)
                                {
                                    double p = Convert.ToDouble(reader["Price"]);
                                    txtPrice.Text = p.ToString("0.##", CultureInfo.InvariantCulture);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось загрузить данные врача: " + ex.Message, "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Сохранение врача
        private void btnSave_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            string specialty = cmbSpecialty.Text.Trim();
            string phone = txtPhone.Text.Trim();
            string room = txtRoom.Text.Trim();

            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Укажите ФИО врача", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return;
            }

            if (string.IsNullOrEmpty(specialty))
            {
                MessageBox.Show("Укажите специальность врача", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbSpecialty.Focus();
                return;
            }

            double price;
            string priceStr = txtPrice.Text.Trim().Replace(" ", "").Replace("\u00A0", "").Replace(',', '.');
            if (!double.TryParse(priceStr, NumberStyles.Any, CultureInfo.InvariantCulture, out price) || price < 0 || price > 1000000)
            {
                MessageBox.Show("Укажите корректную стоимость приёма (от 0 руб.)", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPrice.Focus();
                return;
            }

            try
            {
                using (SQLiteConnection con = Database.GetConnection())
                {
                    con.Open();
                    using (SQLiteCommand cmd = con.CreateCommand())
                    {
                        if (doctorId == 0)
                        {
                            cmd.CommandText =
                                "INSERT INTO Doctors (FullName, Specialty, Phone, Room, Price) " +
                                "VALUES (@n, @s, @p, @r, @c)";
                        }
                        else
                        {
                            cmd.CommandText =
                                "UPDATE Doctors SET FullName = @n, Specialty = @s, Phone = @p, " +
                                "Room = @r, Price = @c WHERE Id = @id";
                            cmd.Parameters.AddWithValue("@id", doctorId);
                        }

                        cmd.Parameters.AddWithValue("@n", name);
                        cmd.Parameters.AddWithValue("@s", specialty);
                        cmd.Parameters.AddWithValue("@p", phone);
                        cmd.Parameters.AddWithValue("@r", room);
                        cmd.Parameters.AddWithValue("@c", price);
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
