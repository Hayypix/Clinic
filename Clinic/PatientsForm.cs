using System;
using System.Data;
using System.Data.SQLite;
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

        private void FillPatients()
        {
            try
            {
                DataTable table = new DataTable();
                using (SQLiteConnection con = Database.GetConnection())
                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(
                    "SELECT Id, FullName, strftime('%d.%m.%Y', BirthDate) AS BirthDate, Phone, Address " +
                    "FROM Patients ORDER BY FullName", con))
                {
                    adapter.Fill(table);
                }

                dgvPatients.DataSource = table;

                dgvPatients.Columns["Id"].Visible = false;
                dgvPatients.Columns["FullName"].HeaderText = "ФИО";
                dgvPatients.Columns["BirthDate"].HeaderText = "Дата рождения";
                dgvPatients.Columns["Phone"].HeaderText = "Телефон";
                dgvPatients.Columns["Address"].HeaderText = "Адрес";

                dgvPatients.Columns["BirthDate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
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
                MessageBox.Show("Выберите пациента", "Внимание",
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
                MessageBox.Show("Выберите пациента", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = Convert.ToInt32(dgvPatients.CurrentRow.Cells["Id"].Value);

            try
            {
                using (SQLiteConnection con = Database.GetConnection())
                {
                    con.Open();

                    SQLiteCommand check = new SQLiteCommand(
                        "SELECT COUNT(*) FROM Appointments WHERE PatientId = @id", con);
                    check.Parameters.AddWithValue("@id", id);
                    if (Convert.ToInt32(check.ExecuteScalar()) > 0)
                    {
                        MessageBox.Show("У этого пациента есть записи на приём, удаление запрещено",
                            "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                if (MessageBox.Show("Удалить выбранного пациента?", "Подтверждение",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    using (SQLiteConnection con = Database.GetConnection())
                    {
                        con.Open();
                        SQLiteCommand cmd = new SQLiteCommand("DELETE FROM Patients WHERE Id = @id", con);
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                    }
                    FillPatients();
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

        private void txtSearch_TextChanged(object sender, EventArgs e)
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
                        string.Format("FullName LIKE '%{0}%' OR Phone LIKE '%{0}%'", s);
            }
            catch
            {
                table.DefaultView.RowFilter = "";
            }
        }
    }
}
