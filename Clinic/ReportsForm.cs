using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Globalization;
using System.Windows.Forms;

namespace Clinic
{
    public partial class ReportsForm : Form
    {
        private DataTable currentReportTable;

        public ReportsForm()
        {
            InitializeComponent();
            InitFilters();
            GenerateReport();
        }

        private void InitFilters()
        {
            // Типы отчётов
            cmbReportType.Items.Clear();
            cmbReportType.Items.AddRange(new object[]
            {
                "Финансовая сводка по врачам",
                "Журнал приёмов за период",
                "Загрузка и продуктивность врачей",
                "Посещаемость пациентов",
                "Текущие запланированные приёмы"
            });
            cmbReportType.SelectedIndex = 0;

            // Предустановленные периоды
            cmbPreset.Items.Clear();
            cmbPreset.Items.AddRange(new object[]
            {
                "Текущий месяц",
                "Последние 30 дней",
                "Сегодня",
                "Текущая неделя",
                "Текущий год",
                "За всё время"
            });
            cmbPreset.SelectedIndex = 0;

            // Статусы
            cmbStatusFilter.Items.Clear();
            cmbStatusFilter.Items.AddRange(new object[] { "Все статусы", "Проведен", "Запланирован", "Отменен" });
            cmbStatusFilter.SelectedIndex = 0;

            // Загрузка врачей для фильтра
            LoadDoctorsFilter();
        }

        private void LoadDoctorsFilter()
        {
            try
            {
                cmbDoctorFilter.Items.Clear();
                cmbDoctorFilter.Items.Add("Все врачи");

                using (SQLiteConnection con = Database.GetConnection())
                {
                    con.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand("SELECT Id, FullName, Specialty FROM Doctors ORDER BY FullName", con))
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cmbDoctorFilter.Items.Add(reader["FullName"].ToString() + " (" + reader["Specialty"].ToString() + ")");
                        }
                    }
                }
                cmbDoctorFilter.SelectedIndex = 0;
            }
            catch
            {
                cmbDoctorFilter.Items.Add("Все врачи");
                cmbDoctorFilter.SelectedIndex = 0;
            }
        }

        private void cmbPreset_SelectedIndexChanged(object sender, EventArgs e)
        {
            DateTime now = DateTime.Today;
            switch (cmbPreset.SelectedIndex)
            {
                case 0: // Текущий месяц
                    dtpFrom.Value = new DateTime(now.Year, now.Month, 1);
                    dtpTo.Value = new DateTime(now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month));
                    break;
                case 1: // Последние 30 дней
                    dtpFrom.Value = now.AddDays(-30);
                    dtpTo.Value = now;
                    break;
                case 2: // Сегодня
                    dtpFrom.Value = now;
                    dtpTo.Value = now;
                    break;
                case 3: // Текущая неделя
                    int diff = (7 + (now.DayOfWeek - DayOfWeek.Monday)) % 7;
                    dtpFrom.Value = now.AddDays(-diff);
                    dtpTo.Value = dtpFrom.Value.AddDays(6);
                    break;
                case 4: // Текущий год
                    dtpFrom.Value = new DateTime(now.Year, 1, 1);
                    dtpTo.Value = new DateTime(now.Year, 12, 31);
                    break;
                case 5: // За всё время
                    dtpFrom.Value = new DateTime(2020, 1, 1);
                    dtpTo.Value = now.AddYears(1);
                    break;
            }
        }

        private void cmbReportType_SelectedIndexChanged(object sender, EventArgs e)
        {
            GenerateReport();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            GenerateReport();
        }

        // Формирование выбранного отчёта
        public void GenerateReport()
        {
            string fromStr = dtpFrom.Value.ToString("yyyy-MM-dd");
            string toStr = dtpTo.Value.ToString("yyyy-MM-dd");
            string selectedDoctor = cmbDoctorFilter.SelectedItem != null ? cmbDoctorFilter.SelectedItem.ToString() : "Все врачи";
            string selectedStatus = cmbStatusFilter.SelectedItem != null ? cmbStatusFilter.SelectedItem.ToString() : "Все статусы";

            try
            {
                DataTable table = new DataTable();
                int totalCount = 0;
                int completedCount = 0;
                double totalRevenue = 0;

                using (SQLiteConnection con = Database.GetConnection())
                {
                    con.Open();

                    switch (cmbReportType.SelectedIndex)
                    {
                        case 0: // Финансовая сводка по врачам
                            table = BuildDoctorsFinanceReport(con, fromStr, toStr, selectedDoctor);
                            break;

                        case 1: // Журнал приёмов за период
                            table = BuildAppointmentsJournalReport(con, fromStr, toStr, selectedDoctor, selectedStatus);
                            break;

                        case 2: // Загрузка и продуктивность врачей
                            table = BuildDoctorWorkloadReport(con, fromStr, toStr, selectedDoctor);
                            break;

                        case 3: // Посещаемость пациентов
                            table = BuildPatientsAnalyticsReport(con, fromStr, toStr);
                            break;

                        case 4: // Текущие запланированные приёмы
                            table = BuildUpcomingAppointmentsReport(con, selectedDoctor);
                            break;

                        default:
                            table = BuildDoctorsFinanceReport(con, fromStr, toStr, selectedDoctor);
                            break;
                    }

                    // Подсчёт показателей сводки
                    using (SQLiteCommand cmd = con.CreateCommand())
                    {
                        string countSql = "SELECT COUNT(*), " +
                                          "IFNULL(SUM(CASE WHEN Status = 'Проведен' THEN 1 ELSE 0 END), 0), " +
                                          "IFNULL(SUM(CASE WHEN Status = 'Проведен' THEN Price ELSE 0 END), 0) " +
                                          "FROM Appointments WHERE Date >= @from AND Date <= @to";

                        if (selectedDoctor != "Все врачи")
                        {
                            string docName = selectedDoctor.Split('(')[0].Trim();
                            countSql += " AND DoctorId IN (SELECT Id FROM Doctors WHERE FullName = @docName)";
                            cmd.Parameters.AddWithValue("@docName", docName);
                        }

                        if (selectedStatus != "Все статусы")
                        {
                            countSql += " AND Status = @stat";
                            cmd.Parameters.AddWithValue("@stat", selectedStatus);
                        }

                        cmd.CommandText = countSql;
                        cmd.Parameters.AddWithValue("@from", fromStr);
                        cmd.Parameters.AddWithValue("@to", toStr);

                        using (SQLiteDataReader r = cmd.ExecuteReader())
                        {
                            if (r.Read())
                            {
                                totalCount = Convert.ToInt32(r[0]);
                                completedCount = Convert.ToInt32(r[1]);
                                totalRevenue = Convert.ToDouble(r[2]);
                            }
                        }
                    }
                }

                currentReportTable = table;
                dgvReport.DataSource = table;

                // Настройка выравнивания колонок
                FormatReportGrid();

                // Обновление карточек показателей
                lblCardTotal.Text = "Всего приёмов: " + totalCount;
                lblCardCompleted.Text = "Проведено: " + completedCount;
                lblCardRevenue.Text = "Выручка: " + totalRevenue.ToString("N0", CultureInfo.CurrentCulture) + " руб.";

                double avgCheck = completedCount > 0 ? (totalRevenue / completedCount) : 0;
                lblCardAvgCheck.Text = "Средний чек: " + avgCheck.ToString("N0", CultureInfo.CurrentCulture) + " руб.";

                lblStatusInfo.Text = string.Format("Строк в отчёте: {0}  |  Период: {1} - {2}  |  Сформировано: {3}",
                    table.Rows.Count, dtpFrom.Value.ToString("dd.MM.yyyy"), dtpTo.Value.ToString("dd.MM.yyyy"), DateTime.Now.ToString("HH:mm:ss"));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при формировании отчёта: " + ex.Message, "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 1. Финансовая сводка по врачам
        private DataTable BuildDoctorsFinanceReport(SQLiteConnection con, string from, string to, string doctorFilter)
        {
            DataTable dt = new DataTable();
            string sql = @"
                SELECT 
                    d.FullName AS 'ФИО врача',
                    d.Specialty AS 'Специальность',
                    d.Room AS 'Кабинет',
                    COUNT(a.Id) AS 'Всего записей',
                    SUM(CASE WHEN a.Status = 'Проведен' THEN 1 ELSE 0 END) AS 'Проведено',
                    SUM(CASE WHEN a.Status = 'Отменен' THEN 1 ELSE 0 END) AS 'Отменено',
                    IFNULL(SUM(CASE WHEN a.Status = 'Проведен' THEN a.Price ELSE 0 END), 0) AS 'Выручка, руб.',
                    CASE WHEN SUM(CASE WHEN a.Status = 'Проведен' THEN 1 ELSE 0 END) > 0 
                         THEN ROUND(IFNULL(SUM(CASE WHEN a.Status = 'Проведен' THEN a.Price ELSE 0 END), 0) / SUM(CASE WHEN a.Status = 'Проведен' THEN 1 ELSE 0 END), 0)
                         ELSE 0 END AS 'Средний чек, руб.'
                FROM Doctors d
                LEFT JOIN Appointments a ON d.Id = a.DoctorId AND a.Date >= @from AND a.Date <= @to";

            if (doctorFilter != "Все врачи")
            {
                string name = doctorFilter.Split('(')[0].Trim();
                sql += " WHERE d.FullName = @docName";
            }

            sql += " GROUP BY d.Id, d.FullName, d.Specialty, d.Room ORDER BY [Выручка, руб.] DESC";

            using (SQLiteDataAdapter da = new SQLiteDataAdapter(sql, con))
            {
                da.SelectCommand.Parameters.AddWithValue("@from", from);
                da.SelectCommand.Parameters.AddWithValue("@to", to);
                if (doctorFilter != "Все врачи")
                {
                    da.SelectCommand.Parameters.AddWithValue("@docName", doctorFilter.Split('(')[0].Trim());
                }
                da.Fill(dt);
            }

            return dt;
        }

        // 2. Журнал приёмов за период
        private DataTable BuildAppointmentsJournalReport(SQLiteConnection con, string from, string to, string doctorFilter, string statusFilter)
        {
            DataTable dt = new DataTable();
            string sql = @"
                SELECT 
                    strftime('%d.%m.%Y', a.Date) AS 'Дата',
                    a.Time AS 'Время',
                    p.FullName AS 'Пациент',
                    IFNULL(p.Phone, '') AS 'Телефон пациента',
                    d.FullName AS 'Врач',
                    d.Specialty AS 'Специальность',
                    d.Room AS 'Каб.',
                    a.Price AS 'Стоимость, руб.',
                    IFNULL(a.Diagnosis, '') AS 'Диагноз / Заметки',
                    a.Status AS 'Статус'
                FROM Appointments a
                JOIN Patients p ON a.PatientId = p.Id
                JOIN Doctors d ON a.DoctorId = d.Id
                WHERE a.Date >= @from AND a.Date <= @to";

            if (doctorFilter != "Все врачи")
            {
                sql += " AND d.FullName = @docName";
            }
            if (statusFilter != "Все статусы")
            {
                sql += " AND a.Status = @stat";
            }

            sql += " ORDER BY a.Date DESC, a.Time DESC";

            using (SQLiteDataAdapter da = new SQLiteDataAdapter(sql, con))
            {
                da.SelectCommand.Parameters.AddWithValue("@from", from);
                da.SelectCommand.Parameters.AddWithValue("@to", to);
                if (doctorFilter != "Все врачи")
                    da.SelectCommand.Parameters.AddWithValue("@docName", doctorFilter.Split('(')[0].Trim());
                if (statusFilter != "Все статусы")
                    da.SelectCommand.Parameters.AddWithValue("@stat", statusFilter);

                da.Fill(dt);
            }

            return dt;
        }

        // 3. Загрузка и продуктивность врачей
        private DataTable BuildDoctorWorkloadReport(SQLiteConnection con, string from, string to, string doctorFilter)
        {
            DataTable dt = new DataTable();
            string sql = @"
                SELECT 
                    d.FullName AS 'ФИО врача',
                    d.Specialty AS 'Специальность',
                    d.Price AS 'Базовый тариф, руб.',
                    COUNT(a.Id) AS 'Всего приёмов',
                    SUM(CASE WHEN a.Status = 'Запланирован' THEN 1 ELSE 0 END) AS 'Ожидается',
                    SUM(CASE WHEN a.Status = 'Проведен' THEN 1 ELSE 0 END) AS 'Завершено успешно',
                    SUM(CASE WHEN a.Status = 'Отменен' THEN 1 ELSE 0 END) AS 'Отменено пациентом',
                    IFNULL(SUM(CASE WHEN a.Status = 'Проведен' THEN a.Price ELSE 0 END), 0) AS 'Итого доход, руб.'
                FROM Doctors d
                LEFT JOIN Appointments a ON d.Id = a.DoctorId AND a.Date >= @from AND a.Date <= @to";

            if (doctorFilter != "Все врачи")
                sql += " WHERE d.FullName = @docName";

            sql += " GROUP BY d.Id, d.FullName, d.Specialty, d.Price ORDER BY [Завершено успешно] DESC";

            using (SQLiteDataAdapter da = new SQLiteDataAdapter(sql, con))
            {
                da.SelectCommand.Parameters.AddWithValue("@from", from);
                da.SelectCommand.Parameters.AddWithValue("@to", to);
                if (doctorFilter != "Все врачи")
                    da.SelectCommand.Parameters.AddWithValue("@docName", doctorFilter.Split('(')[0].Trim());

                da.Fill(dt);
            }

            return dt;
        }

        // 4. Посещаемость пациентов
        private DataTable BuildPatientsAnalyticsReport(SQLiteConnection con, string from, string to)
        {
            DataTable dt = new DataTable();
            string sql = @"
                SELECT 
                    p.FullName AS 'ФИО пациента',
                    strftime('%d.%m.%Y', p.BirthDate) AS 'Дата рождения',
                    IFNULL(p.Phone, '') AS 'Телефон',
                    COUNT(a.Id) AS 'Всего визитов',
                    SUM(CASE WHEN a.Status = 'Проведен' THEN 1 ELSE 0 END) AS 'Проведено приёмов',
                    IFNULL(SUM(CASE WHEN a.Status = 'Проведен' THEN a.Price ELSE 0 END), 0) AS 'Оплачено услуг, руб.',
                    MAX(strftime('%d.%m.%Y', a.Date)) AS 'Последний приём'
                FROM Patients p
                LEFT JOIN Appointments a ON p.Id = a.PatientId AND a.Date >= @from AND a.Date <= @to
                GROUP BY p.Id, p.FullName, p.BirthDate, p.Phone
                ORDER BY [Оплачено услуг, руб.] DESC, [Всего визитов] DESC";

            using (SQLiteDataAdapter da = new SQLiteDataAdapter(sql, con))
            {
                da.SelectCommand.Parameters.AddWithValue("@from", from);
                da.SelectCommand.Parameters.AddWithValue("@to", to);
                da.Fill(dt);
            }

            return dt;
        }

        // 5. Текущие запланированные приёмы
        private DataTable BuildUpcomingAppointmentsReport(SQLiteConnection con, string doctorFilter)
        {
            DataTable dt = new DataTable();
            string sql = @"
                SELECT 
                    strftime('%d.%m.%Y', a.Date) AS 'Дата приёма',
                    a.Time AS 'Время',
                    p.FullName AS 'Пациент',
                    IFNULL(p.Phone, '') AS 'Телефон пациента',
                    d.FullName AS 'Врач',
                    d.Specialty AS 'Специальность',
                    d.Room AS 'Кабинет',
                    a.Price AS 'Стоимость, руб.'
                FROM Appointments a
                JOIN Patients p ON a.PatientId = p.Id
                JOIN Doctors d ON a.DoctorId = d.Id
                WHERE a.Status = 'Запланирован'";

            if (doctorFilter != "Все врачи")
                sql += " AND d.FullName = @docName";

            sql += " ORDER BY a.Date ASC, a.Time ASC";

            using (SQLiteDataAdapter da = new SQLiteDataAdapter(sql, con))
            {
                if (doctorFilter != "Все врачи")
                    da.SelectCommand.Parameters.AddWithValue("@docName", doctorFilter.Split('(')[0].Trim());

                da.Fill(dt);
            }

            return dt;
        }

        // Настройка отображения сетки отчета
        private void FormatReportGrid()
        {
            foreach (DataGridViewColumn col in dgvReport.Columns)
            {
                string header = col.HeaderText.ToLower();
                if (header.Contains("дата") || header.Contains("время") || header.Contains("каб") || header.Contains("статус"))
                {
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                else if (header.Contains("руб.") || header.Contains("стоимость") || header.Contains("выручка") || header.Contains("чек") || header.Contains("доход"))
                {
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    col.DefaultCellStyle.Format = "N0";
                }
                else if (header.Contains("всего") || header.Contains("проведено") || header.Contains("отменено") || header.Contains("ожидается"))
                {
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
            }
        }

        // Экспорт текущего отчёта в Excel
        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            if (currentReportTable == null || currentReportTable.Rows.Count == 0)
            {
                MessageBox.Show("Сначала сформируйте отчёт для экспорта", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                string reportName = cmbReportType.SelectedItem.ToString().Replace(" ", "_");
                sfd.Filter = "Книга Excel (*.xlsx)|*.xlsx|CSV файл (*.csv)|*.csv";
                sfd.FileName = reportName + "_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".xlsx";
                sfd.Title = "Сохранение отчёта клиники в Excel";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        Dictionary<string, string> summary = new Dictionary<string, string>();
                        summary["Тип отчёта:"] = cmbReportType.SelectedItem.ToString();
                        summary["Период формирования:"] = dtpFrom.Value.ToString("dd.MM.yyyy") + " - " + dtpTo.Value.ToString("dd.MM.yyyy");
                        summary["Всего записей:"] = lblCardTotal.Text.Replace("Всего приёмов: ", "");
                        summary["Проведено приёмов:"] = lblCardCompleted.Text.Replace("Проведено: ", "");
                        summary["Общая выручка:"] = lblCardRevenue.Text.Replace("Выручка: ", "");
                        summary["Средний чек:"] = lblCardAvgCheck.Text.Replace("Средний чек: ", "");
                        summary["Дата выгрузки:"] = DateTime.Now.ToString("dd.MM.yyyy HH:mm");

                        ExcelService.ExportTable(currentReportTable, sfd.FileName, cmbReportType.SelectedItem.ToString(), summary);
                        MessageBox.Show("Отчёт успешно выгружен в Excel:\n" + sfd.FileName, "Экспорт завершён",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка при экспорте в Excel: " + ex.Message, "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        // Загрузка / Добавление отчёта из Excel
        private void btnImportExcel_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Файлы Excel и CSV (*.xlsx;*.csv)|*.xlsx;*.csv|Таблицы Excel (*.xlsx)|*.xlsx|CSV файлы (*.csv)|*.csv";
                ofd.Title = "Выберите файл Excel или CSV для импорта";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        DataTable importedTable = ExcelService.ImportTable(ofd.FileName);
                        if (importedTable == null || importedTable.Rows.Count == 0)
                        {
                            MessageBox.Show("Файл не содержит строк с данными или пуст", "Внимание",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        // Показываем в сетке
                        currentReportTable = importedTable;
                        dgvReport.DataSource = importedTable;
                        FormatReportGrid();

                        lblStatusInfo.Text = string.Format("Импортировано строк: {0} из файла «{1}»",
                            importedTable.Rows.Count, System.IO.Path.GetFileName(ofd.FileName));

                        // Проверяем, содержит ли файл данные пациентов или врачей для добавления в базу
                        CheckAndOfferDataImport(importedTable);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка при чтении файла Excel: " + ex.Message, "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        // Умное добавление записей из импортированного файла в SQLite
        private void CheckAndOfferDataImport(DataTable table)
        {
            bool hasPatientCols = false;
            bool hasDoctorCols = false;

            string nameCol = null;
            string phoneCol = null;
            string specialtyCol = null;
            string roomCol = null;
            string priceCol = null;
            string birthCol = null;
            string addressCol = null;

            foreach (DataColumn c in table.Columns)
            {
                string lower = c.ColumnName.ToLower();
                if (lower.Contains("фио") || lower.Contains("пациент") || lower.Contains("врач") || lower.Contains("имя") || lower.Contains("name"))
                    nameCol = c.ColumnName;
                if (lower.Contains("телефон") || lower.Contains("phone"))
                    phoneCol = c.ColumnName;
                if (lower.Contains("специальность") || lower.Contains("specialty") || lower.Contains("должность"))
                    specialtyCol = c.ColumnName;
                if (lower.Contains("кабинет") || lower.Contains("room"))
                    roomCol = c.ColumnName;
                if (lower.Contains("цена") || lower.Contains("тариф") || lower.Contains("price") || lower.Contains("стоимость"))
                    priceCol = c.ColumnName;
                if (lower.Contains("рождения") || lower.Contains("birth"))
                    birthCol = c.ColumnName;
                if (lower.Contains("адрес") || lower.Contains("address"))
                    addressCol = c.ColumnName;
            }

            if (nameCol != null && specialtyCol != null)
                hasDoctorCols = true;
            if (nameCol != null && (birthCol != null || addressCol != null))
                hasPatientCols = true;

            if (hasDoctorCols)
            {
                DialogResult dr = MessageBox.Show(
                    string.Format("В файле обнаружены данные врачей ({0} записей).\nЖелаете добавить новых врачей в базу данных клиники?", table.Rows.Count),
                    "Импорт в базу данных", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dr == DialogResult.Yes)
                {
                    int added = 0;
                    using (SQLiteConnection con = Database.GetConnection())
                    {
                        con.Open();
                        foreach (DataRow row in table.Rows)
                        {
                            string docName = row[nameCol] != null ? row[nameCol].ToString().Trim() : "";
                            string specialty = specialtyCol != null && row[specialtyCol] != null ? row[specialtyCol].ToString().Trim() : "Терапевт";
                            string phone = phoneCol != null && row[phoneCol] != null ? row[phoneCol].ToString().Trim() : "";
                            string room = roomCol != null && row[roomCol] != null ? row[roomCol].ToString().Trim() : "100";
                            double pr = 1500;
                            if (priceCol != null && row[priceCol] != null)
                            {
                                double.TryParse(row[priceCol].ToString().Replace(" ", "").Replace(',', '.'),
                                    NumberStyles.Any, CultureInfo.InvariantCulture, out pr);
                            }

                            if (string.IsNullOrEmpty(docName)) continue;

                            // Проверка дубликата
                            using (SQLiteCommand check = new SQLiteCommand("SELECT COUNT(*) FROM Doctors WHERE FullName = @n", con))
                            {
                                check.Parameters.AddWithValue("@n", docName);
                                if (Convert.ToInt32(check.ExecuteScalar()) == 0)
                                {
                                    using (SQLiteCommand insert = new SQLiteCommand(
                                        "INSERT INTO Doctors (FullName, Specialty, Phone, Room, Price) VALUES (@n, @s, @p, @r, @pr)", con))
                                    {
                                        insert.Parameters.AddWithValue("@n", docName);
                                        insert.Parameters.AddWithValue("@s", specialty);
                                        insert.Parameters.AddWithValue("@p", phone);
                                        insert.Parameters.AddWithValue("@r", room);
                                        insert.Parameters.AddWithValue("@pr", pr);
                                        insert.ExecuteNonQuery();
                                        added++;
                                    }
                                }
                            }
                        }
                    }
                    MessageBox.Show("Импорт завершён! Добавлено новых врачей: " + added, "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDoctorsFilter();
                }
            }
            else if (hasPatientCols)
            {
                DialogResult dr = MessageBox.Show(
                    string.Format("В файле обнаружены данные пациентов ({0} записей).\nЖелаете добавить новых пациентов в картотеку клиники?", table.Rows.Count),
                    "Импорт в базу данных", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dr == DialogResult.Yes)
                {
                    int added = 0;
                    using (SQLiteConnection con = Database.GetConnection())
                    {
                        con.Open();
                        foreach (DataRow row in table.Rows)
                        {
                            string patName = row[nameCol] != null ? row[nameCol].ToString().Trim() : "";
                            string phone = phoneCol != null && row[phoneCol] != null ? row[phoneCol].ToString().Trim() : "";
                            string addr = addressCol != null && row[addressCol] != null ? row[addressCol].ToString().Trim() : "";
                            string birth = "1990-01-01";
                            if (birthCol != null && row[birthCol] != null)
                            {
                                DateTime d;
                                string rawBirth = row[birthCol].ToString().Trim();
                                if (DateTime.TryParse(rawBirth, out d))
                                    birth = d.ToString("yyyy-MM-dd");
                            }

                            if (string.IsNullOrEmpty(patName)) continue;

                            // Проверка дубликата
                            using (SQLiteCommand check = new SQLiteCommand(
                                "SELECT COUNT(*) FROM Patients WHERE FullName = @n AND Phone = @p", con))
                            {
                                check.Parameters.AddWithValue("@n", patName);
                                check.Parameters.AddWithValue("@p", phone);
                                if (Convert.ToInt32(check.ExecuteScalar()) == 0)
                                {
                                    using (SQLiteCommand insert = new SQLiteCommand(
                                        "INSERT INTO Patients (FullName, BirthDate, Phone, Address) VALUES (@n, @b, @p, @a)", con))
                                    {
                                        insert.Parameters.AddWithValue("@n", patName);
                                        insert.Parameters.AddWithValue("@b", birth);
                                        insert.Parameters.AddWithValue("@p", phone);
                                        insert.Parameters.AddWithValue("@a", addr);
                                        insert.ExecuteNonQuery();
                                        added++;
                                    }
                                }
                            }
                        }
                    }
                    MessageBox.Show("Импорт завершён! Добавлено новых пациентов: " + added, "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }
}
