using System;
using System.Data;
using System.Data.SQLite;
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

            FillAppointments();
        }

        private void FillAppointments()
        {
            try
            {
                DataTable table = new DataTable();
                string sql =
                    "SELECT a.Id, a.PatientId, a.DoctorId, a.Status, a.Price, " +
                    "IFNULL(a.Diagnosis, '') AS Diagnosis, " +
                    "a.Date AS DateRaw, a.Time, " +
                    "p.FullName AS PatientName, d.FullName AS DoctorName, d.Specialty, " +
                    "strftime('%d.%m.%Y', a.Date) AS DateText " +
                    "FROM Appointments a, Patients p, Doctors d " +
                    "WHERE a.PatientId = p.Id AND a.DoctorId = d.Id " +
                    "ORDER BY a.Date DESC, a.Time DESC";

                using (SQLiteConnection con = Database.GetConnection())
                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(sql, con))
                {
                    adapter.Fill(table);
                }

                dgvAppointments.DataSource = table;

                // скрытые поля
                dgvAppointments.Columns["Id"].Visible = false;
                dgvAppointments.Columns["PatientId"].Visible = false;
                dgvAppointments.Columns["DoctorId"].Visible = false;
                dgvAppointments.Columns["DateRaw"].Visible = false;

                // видимые колонки
                dgvAppointments.Columns["PatientName"].HeaderText = "Пациент";
                dgvAppointments.Columns["DoctorName"].HeaderText = "Врач";
                dgvAppointments.Columns["Specialty"].HeaderText = "Специальность";
                dgvAppointments.Columns["DateText"].HeaderText = "Дата";
                dgvAppointments.Columns["Time"].HeaderText = "Время";
                dgvAppointments.Columns["Price"].HeaderText = "Стоимость, руб.";
                dgvAppointments.Columns["Diagnosis"].HeaderText = "Диагноз";
                dgvAppointments.Columns["Status"].HeaderText = "Статус";

                // форматирование и выравнивание
                dgvAppointments.Columns["DateText"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvAppointments.Columns["Time"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvAppointments.Columns["Price"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvAppointments.Columns["Price"].DefaultCellStyle.Format = "0.##";
                dgvAppointments.Columns["Status"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                ApplyFilter();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось загрузить записи: " + ex.Message, "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // записать на приём
        private void btnBook_Click(object sender, EventArgs e)
        {
            AppointmentForm f = new AppointmentForm();
            if (f.ShowDialog() == DialogResult.OK)
                FillAppointments();
        }

        // провести приём
        private void btnComplete_Click(object sender, EventArgs e)
        {
            if (dgvAppointments.CurrentRow == null)
            {
                MessageBox.Show("Выберите запись в списке", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataRowView row = (DataRowView)dgvAppointments.CurrentRow.DataBoundItem;
            string status = row["Status"].ToString();

            if (status != "Запланирован")
            {
                if (MessageBox.Show("Этот приём уже имеет статус '" + status + "'.\nОткрыть для просмотра данных?",
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

        // просмотр данных приёма
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
                MessageBox.Show("Выберите запись в списке", "Внимание",
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

        // отменить запись
        private void btnCancelApp_Click(object sender, EventArgs e)
        {
            if (dgvAppointments.CurrentRow == null)
            {
                MessageBox.Show("Выберите запись в списке", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataRowView row = (DataRowView)dgvAppointments.CurrentRow.DataBoundItem;

            if (row["Status"].ToString() != "Запланирован")
            {
                MessageBox.Show("Можно отменить только запланированный приём", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Отменить запись этого пациента?", "Подтверждение",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    using (SQLiteConnection con = Database.GetConnection())
                    {
                        con.Open();
                        SQLiteCommand cmd = new SQLiteCommand(
                            "UPDATE Appointments SET Status = 'Отменен' WHERE Id = @id", con);
                        cmd.Parameters.AddWithValue("@id", Convert.ToInt32(row["Id"]));
                        cmd.ExecuteNonQuery();
                    }
                    FillAppointments();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message, "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        // фильтрация по тексту и статусу
        private void ApplyFilter()
        {
            DataTable table = dgvAppointments.DataSource as DataTable;
            if (table == null) return;

            string s = EscapeFilter(txtSearch.Text);
            string status = cmbStatus.SelectedItem != null ? cmbStatus.SelectedItem.ToString() : "Все";

            string filter = "";
            if (!string.IsNullOrEmpty(s))
            {
                filter = string.Format("(PatientName LIKE '%{0}%' OR DoctorName LIKE '%{0}%' OR Specialty LIKE '%{0}%' OR Diagnosis LIKE '%{0}%')", s);
            }

            if (status != "Все" && !string.IsNullOrEmpty(status))
            {
                if (filter.Length > 0) filter += " AND ";
                filter += string.Format("Status = '{0}'", status);
            }

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
    }
}
