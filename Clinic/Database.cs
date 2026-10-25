using System;
using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;

namespace Clinic
{
    // Работа с базой данных SQLite
    public static class Database
    {
        public static string DbFile()
        {
            return Path.Combine(Application.StartupPath, "clinic.db");
        }

        public static SQLiteConnection GetConnection()
        {
            return new SQLiteConnection("Data Source=" + DbFile() + ";Version=3;Foreign Keys=True;");
        }

        // Создание базы данных и таблиц при первом запуске
        public static void Init()
        {
            bool firstRun = !File.Exists(DbFile());

            using (SQLiteConnection con = GetConnection())
            {
                con.Open();
                using (SQLiteCommand pragma = new SQLiteCommand("PRAGMA foreign_keys = ON;", con))
                {
                    pragma.ExecuteNonQuery();
                }

                using (SQLiteCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = @"CREATE TABLE IF NOT EXISTS Users (
                                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                        Login TEXT NOT NULL,
                                        Password TEXT NOT NULL)";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = @"CREATE TABLE IF NOT EXISTS Doctors (
                                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                        FullName TEXT NOT NULL,
                                        Specialty TEXT NOT NULL,
                                        Phone TEXT,
                                        Room TEXT,
                                        Price REAL NOT NULL DEFAULT 0)";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = @"CREATE TABLE IF NOT EXISTS Patients (
                                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                        FullName TEXT NOT NULL,
                                        BirthDate TEXT,
                                        Phone TEXT,
                                        Address TEXT)";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = @"CREATE TABLE IF NOT EXISTS Appointments (
                                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                        PatientId INTEGER NOT NULL,
                                        DoctorId INTEGER NOT NULL,
                                        Date TEXT NOT NULL,
                                        Time TEXT NOT NULL,
                                        Price REAL,
                                        Diagnosis TEXT,
                                        Status TEXT NOT NULL DEFAULT 'Запланирован',
                                        FOREIGN KEY (PatientId) REFERENCES Patients(Id),
                                        FOREIGN KEY (DoctorId) REFERENCES Doctors(Id))";
                    cmd.ExecuteNonQuery();

                    // Индексы для ускорения поиска и объединения таблиц
                    cmd.CommandText = "CREATE INDEX IF NOT EXISTS idx_appointments_patient ON Appointments(PatientId);";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "CREATE INDEX IF NOT EXISTS idx_appointments_doctor ON Appointments(DoctorId);";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "CREATE INDEX IF NOT EXISTS idx_appointments_date ON Appointments(Date);";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "CREATE INDEX IF NOT EXISTS idx_appointments_status ON Appointments(Status);";
                    cmd.ExecuteNonQuery();

                    if (firstRun)
                    {
                        // Учетная запись по умолчанию
                        cmd.CommandText = "INSERT INTO Users (Login, Password) VALUES ('admin', '12345')";
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = @"INSERT INTO Doctors (FullName, Specialty, Phone, Room, Price) VALUES
                            ('Смирнова Елена Викторовна', 'Терапевт', '+7 (903) 111-22-33', '101', 1500),
                            ('Орлов Дмитрий Сергеевич', 'Хирург', '+7 (905) 222-33-44', '203', 2500),
                            ('Кузнецова Ольга Ивановна', 'Кардиолог', '+7 (906) 333-44-55', '105', 3000),
                            ('Волков Андрей Николаевич', 'Стоматолог', '+7 (909) 444-55-66', '108', 2000),
                            ('Соколова Наталья Петровна', 'Окулист', '+7 (910) 555-66-77', '110', 1800)";
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = @"INSERT INTO Patients (FullName, BirthDate, Phone, Address) VALUES
                            ('Гусев Павел Андреевич', '1990-04-12', '+7 (911) 123-45-67', 'г. Москва, ул. Гагарина, 12'),
                            ('Мельникова Ирина Олеговна', '1985-11-03', '+7 (912) 234-56-78', 'г. Москва, ул. Полевая, 7'),
                            ('Захаров Виктор Павлович', '1972-06-27', '+7 (913) 345-67-89', 'г. Москва, ул. Строителей, 30'),
                            ('Романова Дарья Игоревна', '2001-02-15', '+7 (914) 456-78-90', 'г. Москва, ул. Молодежная, 9')";
                        cmd.ExecuteNonQuery();

                        // Примеры записей с разными статусами
                        cmd.CommandText = @"INSERT INTO Appointments (PatientId, DoctorId, Date, Time, Price, Diagnosis, Status) VALUES
                            (1, 1, '2026-09-20', '10:00', 1500, 'ОРВИ, назначено лечение', 'Проведен'),
                            (2, 2, '2026-09-22', '14:00', 2500, NULL, 'Отменен'),
                            (3, 3, '2026-10-05', '12:30', 3000, NULL, 'Запланирован')";
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        // Статистика клиники для главного окна
        public struct ClinicStats
        {
            public int DoctorsCount;
            public int PatientsCount;
            public int TotalAppointments;
            public int PlannedAppointments;
            public int CompletedAppointments;
            public int TodayAppointments;
            public double TotalRevenue;
        }

        public static ClinicStats GetStats()
        {
            ClinicStats stats = new ClinicStats();
            try
            {
                using (SQLiteConnection con = GetConnection())
                {
                    con.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand("SELECT COUNT(*) FROM Doctors", con))
                        stats.DoctorsCount = Convert.ToInt32(cmd.ExecuteScalar());

                    using (SQLiteCommand cmd = new SQLiteCommand("SELECT COUNT(*) FROM Patients", con))
                        stats.PatientsCount = Convert.ToInt32(cmd.ExecuteScalar());

                    using (SQLiteCommand cmd = new SQLiteCommand("SELECT COUNT(*) FROM Appointments", con))
                        stats.TotalAppointments = Convert.ToInt32(cmd.ExecuteScalar());

                    using (SQLiteCommand cmd = new SQLiteCommand("SELECT COUNT(*) FROM Appointments WHERE Status = 'Запланирован'", con))
                        stats.PlannedAppointments = Convert.ToInt32(cmd.ExecuteScalar());

                    using (SQLiteCommand cmd = new SQLiteCommand("SELECT COUNT(*) FROM Appointments WHERE Status = 'Проведен'", con))
                        stats.CompletedAppointments = Convert.ToInt32(cmd.ExecuteScalar());

                    string todayIso = DateTime.Today.ToString("yyyy-MM-dd");
                    using (SQLiteCommand cmd = new SQLiteCommand("SELECT COUNT(*) FROM Appointments WHERE Date = @dt", con))
                    {
                        cmd.Parameters.AddWithValue("@dt", todayIso);
                        stats.TodayAppointments = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                    using (SQLiteCommand cmd = new SQLiteCommand("SELECT IFNULL(SUM(Price), 0) FROM Appointments WHERE Status = 'Проведен'", con))
                        stats.TotalRevenue = Convert.ToDouble(cmd.ExecuteScalar());
                }
            }
            catch
            {
            }
            return stats;
        }
    }
}
