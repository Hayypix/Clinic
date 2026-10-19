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

        // конструктор для редактирования существующего врача
        public DoctorEditForm(int id) : this()
        {
            doctorId = id;
            Text = "Изменение данных врача";

            try
            {
                using (SQLiteConnection con = Database.GetConnection())
                {
                    con.Open();
                    SQLiteCommand cmd = new SQLiteCommand(
                        "SELECT FullName, Specialty, Phone, Room, Price FROM Doctors WHERE Id = @id", con);
                    cmd.Parameters.AddWithValue("@id", id);

                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            txtName.Text = reader["FullName"].ToString();
                            cmbSpecialty.Text = reader["Specialty"].ToString();
                            txtPhone.Text = reader["Phone"].ToString();
                            txtRoom.Text = reader["Room"].ToString();
                            txtPrice.Text = Convert.ToDouble(reader["Price"]).ToString("0.##");
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

        // сохранить
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtName.Text.Trim() == "")
            {
                MessageBox.Show("Укажите ФИО врача", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cmbSpecialty.Text.Trim() == "")
            {
                MessageBox.Show("Укажите специальность", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            double price;
            string priceStr = txtPrice.Text.Trim().Replace(',', '.');
            if (!double.TryParse(priceStr, NumberStyles.Any, CultureInfo.InvariantCulture, out price) || price <= 0)
            {
                MessageBox.Show("Цена приёма указана неверно", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SQLiteConnection con = Database.GetConnection())
                {
                    con.Open();
                    SQLiteCommand cmd;

                    if (doctorId == 0)
                    {
                        cmd = new SQLiteCommand(
                            "INSERT INTO Doctors (FullName, Specialty, Phone, Room, Price) " +
                            "VALUES (@n, @s, @p, @r, @c)", con);
                    }
                    else
                    {
                        cmd = new SQLiteCommand(
                            "UPDATE Doctors SET FullName = @n, Specialty = @s, Phone = @p, " +
                            "Room = @r, Price = @c WHERE Id = @id", con);
                        cmd.Parameters.AddWithValue("@id", doctorId);
                    }

                    cmd.Parameters.AddWithValue("@n", txtName.Text.Trim());
                    cmd.Parameters.AddWithValue("@s", cmbSpecialty.Text.Trim());
                    cmd.Parameters.AddWithValue("@p", txtPhone.Text.Trim());
                    cmd.Parameters.AddWithValue("@r", txtRoom.Text.Trim());
                    cmd.Parameters.AddWithValue("@c", price);
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
