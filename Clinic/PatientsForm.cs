using System;
using System.Data;
using System.Data.SQLite;
using System.Globalization;
using System.Windows.Forms;

namespace Clinic
{
    public partial class PatientsForm : Form
    {
        public PatientsForm()
        {
            InitializeComponent();
            FillPatients();
        }

        public void FillPatients()
        {
            try
            {
                DataTable table = new DataTable();
                using (SQLiteConnection con = Database.GetConnection())
                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(
                    "SELECT Id, FullName, BirthDate, Phone, Address FROM Patients ORDER BY FullName", con))
                {
                    adapter.Fill(table);
                }

                // Добавляем вычисляемую колонку с отформатированной датой рождения
                table.Columns.Add("BirthDateFormatted", typeof(string));
                foreach (DataRow row in table.Rows)
                {
                    string bStr = row["BirthDate"] != DBNull.Value ? row["BirthDate"].ToString() : "";
                    if (!string.IsNullOrEmpty(bStr))
                    {
                        DateTime d;
                        string[] formats = { "yyyy-MM-dd", "dd.MM.yyyy", "yyyy/MM/dd", "d.M.yyyy" };
                        if (DateTime.TryParseExact(bStr, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out d) ||
                            DateTime.TryParse(bStr, out d))
                        {
                            row["BirthDateFormatted"] = d.ToString("dd.MM.yyyy");
                        }
                        else
                        {
                            row["BirthDateFormatted"] = bStr;
                        }
                    }
                    else
                    {
                        row["BirthDateFormatted"] = "—";
                    }
                }

                dgvPatients.DataSource = table;

                if (dgvPatients.Columns.Contains("Id"))
                    dgvPatients.Columns["Id"].Visible = false;
                if (dgvPatients.Columns.Contains("BirthDate"))
                    dgvPatients.Columns["BirthDate"].Visible = false;

                if (dgvPatients.Columns.Contains("FullName"))
                {
                    dgvPatients.Columns["FullName"].HeaderText = "ФИО пациента";
                    dgvPatients.Columns["FullName"].FillWeight = 32;
                }
                if (dgvPatients.Columns.Contains("BirthDateFormatted"))
                {
                    dgvPatients.Columns["BirthDateFormatted"].HeaderText = "Дата рождения";
                    dgvPatients.Columns["BirthDateFormatted"].FillWeight = 16;
                    dgvPatients.Columns["BirthDateFormatted"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                if (dgvPatients.Columns.Contains("Phone"))
                {
                    dgvPatients.Columns["Phone"].HeaderText = "Телефон";
                    dgvPatients.Columns["Phone"].FillWeight = 18;
                }
                if (dgvPatients.Columns.Contains("Address"))
                {
                    dgvPatients.Columns["Address"].HeaderText = "Адрес проживания";
                    dgvPatients.Columns["Address"].FillWeight = 34;
                }

                FilterPatients();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось загрузить пациентов: " + ex.Message, "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvPatients_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            btnEdit_Click(sender, e);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            PatientEditForm f = new PatientEditForm();
            if (f.ShowDialog() == DialogResult.OK)
                FillPatients();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvPatients.CurrentRow == null)
            {
                MessageBox.Show("Выберите пациента из списка", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = Convert.ToInt32(dgvPatients.CurrentRow.Cells["Id"].Value);
            PatientEditForm f = new PatientEditForm(id);
            if (f.ShowDialog() == DialogResult.OK)
                FillPatients();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvPatients.CurrentRow == null)
            {
                MessageBox.Show("Выберите пациента из списка", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = Convert.ToInt32(dgvPatients.CurrentRow.Cells["Id"].Value);
            string name = dgvPatients.CurrentRow.Cells["FullName"].Value.ToString();

            try
            {
                using (SQLiteConnection con = Database.GetConnection())
                {
                    con.Open();

                    using (SQLiteCommand check = new SQLiteCommand(
                        "SELECT COUNT(*) FROM Appointments WHERE PatientId = @id", con))
                    {
                        check.Parameters.AddWithValue("@id", id);
                        int count = Convert.ToInt32(check.ExecuteScalar());
                        if (count > 0)
                        {
                            MessageBox.Show("У этого пациента есть история записей на приём (" + count + " приёмов).\nУдаление пациента запрещено для сохранности медицинской карты.",
                                "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    if (MessageBox.Show("Удалить пациента «" + name + "» из базы данных?", "Подтверждение удаления",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        using (SQLiteCommand cmd = new SQLiteCommand("DELETE FROM Patients WHERE Id = @id", con))
                        {
                            cmd.Parameters.AddWithValue("@id", id);
                            cmd.ExecuteNonQuery();
                        }
                        FillPatients();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при удалении: " + ex.Message, "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Экспорт списка пациентов в Excel
        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            DataTable table = dgvPatients.DataSource as DataTable;
            if (table == null || table.Rows.Count == 0)
            {
                MessageBox.Show("Нет данных для экспорта", "Информация",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Книга Excel (*.xlsx)|*.xlsx|CSV файл (*.csv)|*.csv";
                sfd.FileName = "Список_пациентов_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".xlsx";
                sfd.Title = "Экспорт пациентов в Excel";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        DataTable exportTable = table.DefaultView.ToTable(false, "FullName", "BirthDateFormatted", "Phone", "Address");
                        exportTable.Columns["FullName"].ColumnName = "ФИО пациента";
                        exportTable.Columns["BirthDateFormatted"].ColumnName = "Дата рождения";
                        exportTable.Columns["Phone"].ColumnName = "Телефон";
                        exportTable.Columns["Address"].ColumnName = "Адрес";

                        ExcelService.ExportTable(exportTable, sfd.FileName, "Картотека пациентов клиники");
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

        private void FilterPatients()
        {
            DataTable table = dgvPatients.DataSource as DataTable;
            if (table == null) return;

            string s = EscapeFilter(txtSearch.Text);
            try
            {
                if (string.IsNullOrEmpty(s))
                    table.DefaultView.RowFilter = "";
                else
                    table.DefaultView.RowFilter =
                        string.Format("FullName LIKE '%{0}%' OR Phone LIKE '%{0}%' OR Address LIKE '%{0}%'", s);
            }
            catch
            {
                table.DefaultView.RowFilter = "";
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            FilterPatients();
        }
    }
}
