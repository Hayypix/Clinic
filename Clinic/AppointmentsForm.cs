using System;
using System.Data;
using System.Data.SQLite;
using System.Globalization;
using System.Windows.Forms;

namespace Clinic
{
    public partial class AppointmentsForm : Form
    {
        public AppointmentsForm()
        {
            InitializeComponent();

            cmbStatus.Items.AddRange(new object[] { "Все", "Запланирован", "Проведен", "Отменен" });
            cmbStatus.SelectedIndex = 0;

            cmbDateFilter.Items.AddRange(new object[] { "Все даты", "На сегодня", "Текущая неделя", "Текущий месяц" });
            cmbDateFilter.SelectedIndex = 0;

            FillAppointments();
        }

        public void FillAppointments()
        {
            try
            {
                DataTable table = new DataTable();
                string sql =
                    "SELECT a.Id, a.PatientId, a.DoctorId, a.Status, a.Price, " +
                    "IFNULL(a.Diagnosis, '') AS Diagnosis, " +
                    "a.Date AS DateRaw, a.Time, " +
                    "p.FullName AS PatientName, p.Phone AS PatientPhone, " +
                    "d.FullName AS DoctorName, d.Specialty, d.Room " +
                    "FROM Appointments a " +
                    "JOIN Patients p ON a.PatientId = p.Id " +
                    "JOIN Doctors d ON a.DoctorId = d.Id " +
                    "ORDER BY a.Date DESC, a.Time DESC";

                using (SQLiteConnection con = Database.GetConnection())
                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(sql, con))
                {
                    adapter.Fill(table);
                }

                // Добавляем красивую дату
                table.Columns.Add("DateText", typeof(string));
                foreach (DataRow row in table.Rows)
                {
                    string raw = row["DateRaw"] != DBNull.Value ? row["DateRaw"].ToString() : "";
                    DateTime d;
                    string[] formats = { "yyyy-MM-dd", "dd.MM.yyyy", "yyyy/MM/dd" };
                    if (DateTime.TryParseExact(raw, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out d) ||
                        DateTime.TryParse(raw, out d))
                    {
                        row["DateText"] = d.ToString("dd.MM.yyyy");
                    }
                    else
                    {
                        row["DateText"] = raw;
                    }
                }

                dgvAppointments.DataSource = table;

                // Скрытые поля
                if (dgvAppointments.Columns.Contains("Id")) dgvAppointments.Columns["Id"].Visible = false;
                if (dgvAppointments.Columns.Contains("PatientId")) dgvAppointments.Columns["PatientId"].Visible = false;
                if (dgvAppointments.Columns.Contains("DoctorId")) dgvAppointments.Columns["DoctorId"].Visible = false;
                if (dgvAppointments.Columns.Contains("DateRaw")) dgvAppointments.Columns["DateRaw"].Visible = false;
                if (dgvAppointments.Columns.Contains("PatientPhone")) dgvAppointments.Columns["PatientPhone"].Visible = false;
                if (dgvAppointments.Columns.Contains("Room")) dgvAppointments.Columns["Room"].Visible = false;

                // Видимые колонки
                if (dgvAppointments.Columns.Contains("PatientName"))
                {
                    dgvAppointments.Columns["PatientName"].HeaderText = "Пациент";
                    dgvAppointments.Columns["PatientName"].FillWeight = 22;
                }
                if (dgvAppointments.Columns.Contains("DoctorName"))
                {
                    dgvAppointments.Columns["DoctorName"].HeaderText = "Врач";
                    dgvAppointments.Columns["DoctorName"].FillWeight = 20;
                }
                if (dgvAppointments.Columns.Contains("Specialty"))
                {
                    dgvAppointments.Columns["Specialty"].HeaderText = "Специальность";
                    dgvAppointments.Columns["Specialty"].FillWeight = 16;
                }
                if (dgvAppointments.Columns.Contains("DateText"))
                {
                    dgvAppointments.Columns["DateText"].HeaderText = "Дата";
                    dgvAppointments.Columns["DateText"].FillWeight = 11;
                    dgvAppointments.Columns["DateText"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                if (dgvAppointments.Columns.Contains("Time"))
                {
                    dgvAppointments.Columns["Time"].HeaderText = "Время";
                    dgvAppointments.Columns["Time"].FillWeight = 9;
                    dgvAppointments.Columns["Time"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                if (dgvAppointments.Columns.Contains("Price"))
                {
                    dgvAppointments.Columns["Price"].HeaderText = "Стоимость, руб.";
                    dgvAppointments.Columns["Price"].FillWeight = 14;
                    dgvAppointments.Columns["Price"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvAppointments.Columns["Price"].DefaultCellStyle.Format = "N0";
                }
                if (dgvAppointments.Columns.Contains("Diagnosis"))
                {
                    dgvAppointments.Columns["Diagnosis"].HeaderText = "Диагноз / Заметки";
                    dgvAppointments.Columns["Diagnosis"].FillWeight = 22;
                }
                if (dgvAppointments.Columns.Contains("Status"))
                {
                    dgvAppointments.Columns["Status"].HeaderText = "Статус";
                    dgvAppointments.Columns["Status"].FillWeight = 14;
                    dgvAppointments.Columns["Status"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }

                ApplyFilter();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось загрузить записи: " + ex.Message, "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Записать на приём
        private void btnBook_Click(object sender, EventArgs e)
        {
            AppointmentForm f = new AppointmentForm();
            if (f.ShowDialog() == DialogResult.OK)
                FillAppointments();
        }

        // Провести приём
        private void btnComplete_Click(object sender, EventArgs e)
        {
            if (dgvAppointments.CurrentRow == null)
            {
                MessageBox.Show("Выберите приём из списка", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataRowView row = (DataRowView)dgvAppointments.CurrentRow.DataBoundItem;
            string status = row["Status"].ToString();

            if (status != "Запланирован")
            {
                if (MessageBox.Show("Этот приём уже имеет статус «" + status + "».\nОткрыть для просмотра диагноза и данных?",
                    "Информация", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    CompleteForm viewForm = new CompleteForm(Convert.ToInt32(row["Id"]), true);
                    viewForm.ShowDialog();
                }
                return;
            }

            int id = Convert.ToInt32(row["Id"]);
            CompleteForm f = new CompleteForm(id, false);
            if (f.ShowDialog() == DialogResult.OK)
                FillAppointments();
        }

        // Просмотр данных приёма
        private void btnView_Click(object sender, EventArgs e)
        {
            OpenSelectedAppointment();
        }

        private void dgvAppointments_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            OpenSelectedAppointment();
        }

        private void OpenSelectedAppointment()
        {
            if (dgvAppointments.CurrentRow == null)
            {
                MessageBox.Show("Выберите запись из списка", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataRowView row = (DataRowView)dgvAppointments.CurrentRow.DataBoundItem;
            int id = Convert.ToInt32(row["Id"]);
            string status = row["Status"].ToString();

            CompleteForm f = new CompleteForm(id, status != "Запланирован");
            if (f.ShowDialog() == DialogResult.OK)
                FillAppointments();
        }

        // Отменить запись
        private void btnCancelApp_Click(object sender, EventArgs e)
        {
            if (dgvAppointments.CurrentRow == null)
            {
                MessageBox.Show("Выберите запись из списка", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataRowView row = (DataRowView)dgvAppointments.CurrentRow.DataBoundItem;

            if (row["Status"].ToString() != "Запланирован")
            {
                MessageBox.Show("Отменить можно только запланированный приём", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Отменить запись пациента «" + row["PatientName"] + "» к врачу «" + row["DoctorName"] + "»?", "Подтверждение отмены",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    using (SQLiteConnection con = Database.GetConnection())
                    {
                        con.Open();
                        using (SQLiteCommand cmd = new SQLiteCommand(
                            "UPDATE Appointments SET Status = 'Отменен' WHERE Id = @id", con))
                        {
                            cmd.Parameters.AddWithValue("@id", Convert.ToInt32(row["Id"]));
                            cmd.ExecuteNonQuery();
                        }
                    }
                    FillAppointments();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при отмене: " + ex.Message, "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Удалить запись
        private void btnDeleteApp_Click(object sender, EventArgs e)
        {
            if (dgvAppointments.CurrentRow == null)
            {
                MessageBox.Show("Выберите запись из списка", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataRowView row = (DataRowView)dgvAppointments.CurrentRow.DataBoundItem;
            int id = Convert.ToInt32(row["Id"]);

            if (MessageBox.Show("Удалить выбранную запись на приём из базы данных?\n(Пациент: " + row["PatientName"] + ", Дата: " + row["DateText"] + " " + row["Time"] + ")",
                "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    using (SQLiteConnection con = Database.GetConnection())
                    {
                        con.Open();
                        using (SQLiteCommand cmd = new SQLiteCommand("DELETE FROM Appointments WHERE Id = @id", con))
                        {
                            cmd.Parameters.AddWithValue("@id", id);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    FillAppointments();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при удалении: " + ex.Message, "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Экспорт таблицы приёмов в Excel
        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            DataTable table = dgvAppointments.DataSource as DataTable;
            if (table == null || table.Rows.Count == 0)
            {
                MessageBox.Show("Нет данных для экспорта", "Информация",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Книга Excel (*.xlsx)|*.xlsx|CSV файл (*.csv)|*.csv";
                sfd.FileName = "Журнал_приёмов_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".xlsx";
                sfd.Title = "Экспорт журнала записей в Excel";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        DataTable exportTable = table.DefaultView.ToTable(false,
                            "PatientName", "DoctorName", "Specialty", "DateText", "Time", "Price", "Diagnosis", "Status");
                        exportTable.Columns["PatientName"].ColumnName = "Пациент";
                        exportTable.Columns["DoctorName"].ColumnName = "Врач";
                        exportTable.Columns["Specialty"].ColumnName = "Специальность";
                        exportTable.Columns["DateText"].ColumnName = "Дата";
                        exportTable.Columns["Time"].ColumnName = "Время";
                        exportTable.Columns["Price"].ColumnName = "Стоимость, руб.";
                        exportTable.Columns["Diagnosis"].ColumnName = "Диагноз";
                        exportTable.Columns["Status"].ColumnName = "Статус";

                        ExcelService.ExportTable(exportTable, sfd.FileName, "Журнал записей на приём");
                        MessageBox.Show("Журнал записей успешно сохранён в файл:\n" + sfd.FileName, "Экспорт завершён",
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

        // Фильтрация по тексту, статусу и периоду
        private void ApplyFilter()
        {
            DataTable table = dgvAppointments.DataSource as DataTable;
            if (table == null) return;

            string s = EscapeFilter(txtSearch.Text);
            string status = cmbStatus.SelectedItem != null ? cmbStatus.SelectedItem.ToString() : "Все";
            string dateFilter = cmbDateFilter.SelectedItem != null ? cmbDateFilter.SelectedItem.ToString() : "Все даты";

            System.Collections.Generic.List<string> parts = new System.Collections.Generic.List<string>();

            if (!string.IsNullOrEmpty(s))
            {
                parts.Add(string.Format("(PatientName LIKE '%{0}%' OR DoctorName LIKE '%{0}%' OR Specialty LIKE '%{0}%' OR Diagnosis LIKE '%{0}%')", s));
            }

            if (status != "Все" && !string.IsNullOrEmpty(status))
            {
                parts.Add(string.Format("Status = '{0}'", status));
            }

            if (dateFilter == "На сегодня")
            {
                string today = DateTime.Today.ToString("yyyy-MM-dd");
                parts.Add(string.Format("DateRaw = '{0}'", today));
            }
            else if (dateFilter == "Текущая неделя")
            {
                DateTime now = DateTime.Today;
                int diff = (7 + (now.DayOfWeek - DayOfWeek.Monday)) % 7;
                DateTime startOfWeek = now.AddDays(-diff);
                DateTime endOfWeek = startOfWeek.AddDays(6);
                parts.Add(string.Format("DateRaw >= '{0}' AND DateRaw <= '{1}'",
                    startOfWeek.ToString("yyyy-MM-dd"), endOfWeek.ToString("yyyy-MM-dd")));
            }
            else if (dateFilter == "Текущий месяц")
            {
                DateTime now = DateTime.Today;
                DateTime startOfMonth = new DateTime(now.Year, now.Month, 1);
                DateTime endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);
                parts.Add(string.Format("DateRaw >= '{0}' AND DateRaw <= '{1}'",
                    startOfMonth.ToString("yyyy-MM-dd"), endOfMonth.ToString("yyyy-MM-dd")));
            }

            string filter = string.Join(" AND ", parts.ToArray());

            try
            {
                table.DefaultView.RowFilter = filter;
            }
            catch
            {
                table.DefaultView.RowFilter = "";
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void cmbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void cmbDateFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }
    }
}
