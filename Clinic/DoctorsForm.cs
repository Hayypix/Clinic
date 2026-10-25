using System;
using System.Data;
using System.Data.SQLite;
using System.Globalization;
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

        public void FillDoctors()
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

                // Настройка колонок
                if (dgvDoctors.Columns.Contains("Id"))
                    dgvDoctors.Columns["Id"].Visible = false;

                if (dgvDoctors.Columns.Contains("FullName"))
                {
                    dgvDoctors.Columns["FullName"].HeaderText = "ФИО врача";
                    dgvDoctors.Columns["FullName"].FillWeight = 32;
                }
                if (dgvDoctors.Columns.Contains("Specialty"))
                {
                    dgvDoctors.Columns["Specialty"].HeaderText = "Специальность";
                    dgvDoctors.Columns["Specialty"].FillWeight = 22;
                }
                if (dgvDoctors.Columns.Contains("Phone"))
                {
                    dgvDoctors.Columns["Phone"].HeaderText = "Телефон";
                    dgvDoctors.Columns["Phone"].FillWeight = 18;
                }
                if (dgvDoctors.Columns.Contains("Room"))
                {
                    dgvDoctors.Columns["Room"].HeaderText = "Кабинет";
                    dgvDoctors.Columns["Room"].FillWeight = 12;
                    dgvDoctors.Columns["Room"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                if (dgvDoctors.Columns.Contains("Price"))
                {
                    dgvDoctors.Columns["Price"].HeaderText = "Цена приёма, руб.";
                    dgvDoctors.Columns["Price"].FillWeight = 16;
                    dgvDoctors.Columns["Price"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvDoctors.Columns["Price"].DefaultCellStyle.Format = "N0";
                }

                FilterDoctors();
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

        // Добавить
        private void btnAdd_Click(object sender, EventArgs e)
        {
            DoctorEditForm f = new DoctorEditForm();
            if (f.ShowDialog() == DialogResult.OK)
                FillDoctors();
        }

        // Изменить
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvDoctors.CurrentRow == null)
            {
                MessageBox.Show("Выберите врача из списка", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = Convert.ToInt32(dgvDoctors.CurrentRow.Cells["Id"].Value);
            DoctorEditForm f = new DoctorEditForm(id);
            if (f.ShowDialog() == DialogResult.OK)
                FillDoctors();
        }

        // Удалить
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvDoctors.CurrentRow == null)
            {
                MessageBox.Show("Выберите врача из списка", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = Convert.ToInt32(dgvDoctors.CurrentRow.Cells["Id"].Value);
            string name = dgvDoctors.CurrentRow.Cells["FullName"].Value.ToString();

            try
            {
                using (SQLiteConnection con = Database.GetConnection())
                {
                    con.Open();

                    // Врача, у которого есть записи на приём, удалять нельзя
                    using (SQLiteCommand check = new SQLiteCommand(
                        "SELECT COUNT(*) FROM Appointments WHERE DoctorId = @id", con))
                    {
                        check.Parameters.AddWithValue("@id", id);
                        int count = Convert.ToInt32(check.ExecuteScalar());
                        if (count > 0)
                        {
                            MessageBox.Show("У этого врача есть приёмы в базе (" + count + " зап.), удаление невозможно.\nСначала отмените или переназначьте приёмы.",
                                "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    if (MessageBox.Show("Удалить врача «" + name + "» из базы данных?", "Подтверждение удаления",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        using (SQLiteCommand cmd = new SQLiteCommand("DELETE FROM Doctors WHERE Id = @id", con))
                        {
                            cmd.Parameters.AddWithValue("@id", id);
                            cmd.ExecuteNonQuery();
                        }
                        FillDoctors();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при удалении: " + ex.Message, "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Экспорт списка врачей в Excel
        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            DataTable table = dgvDoctors.DataSource as DataTable;
            if (table == null || table.Rows.Count == 0)
            {
                MessageBox.Show("Нет данных для экспорта", "Информация",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Книга Excel (*.xlsx)|*.xlsx|CSV файл (*.csv)|*.csv";
                sfd.FileName = "Список_врачей_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".xlsx";
                sfd.Title = "Экспорт врачей в Excel";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        DataTable exportTable = table.DefaultView.ToTable(false, "FullName", "Specialty", "Phone", "Room", "Price");
                        exportTable.Columns["FullName"].ColumnName = "ФИО врача";
                        exportTable.Columns["Specialty"].ColumnName = "Специальность";
                        exportTable.Columns["Phone"].ColumnName = "Телефон";
                        exportTable.Columns["Room"].ColumnName = "Кабинет";
                        exportTable.Columns["Price"].ColumnName = "Стоимость приёма, руб.";

                        ExcelService.ExportTable(exportTable, sfd.FileName, "Штат врачей клиники");
                        MessageBox.Show("Данные успешно сохранены в файл:\n" + sfd.FileName, "Экспорт завершён",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка экспорта: " + ex.Message, "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
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

        private void FilterDoctors()
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
                        string.Format("FullName LIKE '%{0}%' OR Specialty LIKE '%{0}%' OR Phone LIKE '%{0}%'", s);
            }
            catch
            {
                table.DefaultView.RowFilter = "";
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            FilterDoctors();
        }
    }
}
