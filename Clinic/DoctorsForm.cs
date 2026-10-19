using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

namespace Clinic
{
    public partial class DoctorsForm : Form
    {
        public DoctorsForm()
        {
            InitializeComponent();
            FillDoctors();
        }

        private void FillDoctors()
        {
            try
            {
                DataTable table = new DataTable();
                using (SQLiteConnection con = Database.GetConnection())
                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(
                    "SELECT Id, FullName, Specialty, Phone, Room, Price FROM Doctors ORDER BY FullName", con))
                {
                    adapter.Fill(table);
                }

                dgvDoctors.DataSource = table;

                // настройка колонок
                dgvDoctors.Columns["Id"].Visible = false;
                dgvDoctors.Columns["FullName"].HeaderText = "ФИО";
                dgvDoctors.Columns["Specialty"].HeaderText = "Специальность";
                dgvDoctors.Columns["Phone"].HeaderText = "Телефон";
                dgvDoctors.Columns["Room"].HeaderText = "Кабинет";
                dgvDoctors.Columns["Price"].HeaderText = "Цена приёма, руб.";

                dgvDoctors.Columns["Room"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvDoctors.Columns["Price"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvDoctors.Columns["Price"].DefaultCellStyle.Format = "0.##";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось загрузить врачей: " + ex.Message, "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvDoctors_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            btnEdit_Click(sender, e);
        }

        // добавить
        private void btnAdd_Click(object sender, EventArgs e)
        {
            DoctorEditForm f = new DoctorEditForm();
            if (f.ShowDialog() == DialogResult.OK)
                FillDoctors();
        }

        // изменить
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvDoctors.CurrentRow == null)
            {
                MessageBox.Show("Выберите врача", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = Convert.ToInt32(dgvDoctors.CurrentRow.Cells["Id"].Value);
            DoctorEditForm f = new DoctorEditForm(id);
            if (f.ShowDialog() == DialogResult.OK)
                FillDoctors();
        }

        // удалить
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvDoctors.CurrentRow == null)
            {
                MessageBox.Show("Выберите врача", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = Convert.ToInt32(dgvDoctors.CurrentRow.Cells["Id"].Value);

            try
            {
                using (SQLiteConnection con = Database.GetConnection())
                {
                    con.Open();

                    // врача, у которого есть записи, удалять нельзя
                    SQLiteCommand check = new SQLiteCommand(
                        "SELECT COUNT(*) FROM Appointments WHERE DoctorId = @id", con);
                    check.Parameters.AddWithValue("@id", id);
                    if (Convert.ToInt32(check.ExecuteScalar()) > 0)
                    {
                        MessageBox.Show("У этого врача есть записи на приём, удаление запрещено",
                            "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                if (MessageBox.Show("Удалить выбранного врача?", "Подтверждение",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    using (SQLiteConnection con = Database.GetConnection())
                    {
                        con.Open();
                        SQLiteCommand cmd = new SQLiteCommand("DELETE FROM Doctors WHERE Id = @id", con);
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                    }
                    FillDoctors();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при удалении: " + ex.Message, "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string EscapeFilter(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return "";
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (char c in text.Trim())
            {
                if (c == '\'')
                    sb.Append("''");
                else if (c == '[' || c == ']' || c == '*' || c == '%')
                    sb.Append("[").Append(c).Append("]");
                else
                    sb.Append(c);
            }
            return sb.ToString();
        }

        // фильтр по ФИО и специальности
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            DataTable table = dgvDoctors.DataSource as DataTable;
            if (table == null) return;

            string s = EscapeFilter(txtSearch.Text);
            try
            {
                if (string.IsNullOrEmpty(s))
                    table.DefaultView.RowFilter = "";
                else
                    table.DefaultView.RowFilter =
                        string.Format("FullName LIKE '%{0}%' OR Specialty LIKE '%{0}%'", s);
            }
            catch
            {
                table.DefaultView.RowFilter = "";
            }
        }
    }
}
