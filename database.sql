-- Скрипт создания базы данных "Частная клиника" (SQLite)
-- Этот же скрипт выполняется программой автоматически при первом запуске.

DROP TABLE IF EXISTS Appointments;
DROP TABLE IF EXISTS Patients;
DROP TABLE IF EXISTS Doctors;
DROP TABLE IF EXISTS Users;

-- пользователи системы
CREATE TABLE Users (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Login TEXT NOT NULL,
    Password TEXT NOT NULL
);

-- врачи
CREATE TABLE Doctors (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    FullName TEXT NOT NULL,        -- ФИО
    Specialty TEXT NOT NULL,       -- специальность
    Phone TEXT,                    -- телефон
    Room TEXT,                     -- кабинет
    Price REAL NOT NULL DEFAULT 0  -- цена приема, руб.
);

-- пациенты
CREATE TABLE Patients (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    FullName TEXT NOT NULL,        -- ФИО
    BirthDate TEXT,                -- дата рождения (гггг-мм-дд)
    Phone TEXT,                    -- телефон
    Address TEXT                   -- адрес
);

-- записи на прием
CREATE TABLE Appointments (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    PatientId INTEGER NOT NULL REFERENCES Patients(Id),  -- пациент
    DoctorId INTEGER NOT NULL REFERENCES Doctors(Id),    -- врач
    Date TEXT NOT NULL,                                  -- дата приема (гггг-мм-дд)
    Time TEXT NOT NULL,                                  -- время (чч:мм)
    Price REAL,                                          -- стоимость, руб.
    Diagnosis TEXT,                                      -- диагноз
    Status TEXT NOT NULL DEFAULT 'Запланирован'          -- Запланирован / Проведен / Отменен
);

-- индексы для ускорения поиска
CREATE INDEX IF NOT EXISTS idx_appointments_patient ON Appointments(PatientId);
CREATE INDEX IF NOT EXISTS idx_appointments_doctor ON Appointments(DoctorId);
CREATE INDEX IF NOT EXISTS idx_appointments_date ON Appointments(Date);
CREATE INDEX IF NOT EXISTS idx_appointments_status ON Appointments(Status);

INSERT INTO Users (Login, Password) VALUES ('admin', '12345');

INSERT INTO Doctors (FullName, Specialty, Phone, Room, Price) VALUES
('Смирнова Елена Викторовна', 'Терапевт', '+7 (903) 111-22-33', '101', 1500),
('Орлов Дмитрий Сергеевич', 'Хирург', '+7 (905) 222-33-44', '203', 2500),
('Кузнецова Ольга Ивановна', 'Кардиолог', '+7 (906) 333-44-55', '105', 3000),
('Волков Андрей Николаевич', 'Стоматолог', '+7 (909) 444-55-66', '108', 2000),
('Соколова Наталья Петровна', 'Окулист', '+7 (910) 555-66-77', '110', 1800);

INSERT INTO Patients (FullName, BirthDate, Phone, Address) VALUES
('Гусев Павел Андреевич', '1990-04-12', '+7 (911) 123-45-67', 'г. Москва, ул. Гагарина, 12'),
('Мельникова Ирина Олеговна', '1985-11-03', '+7 (912) 234-56-78', 'г. Москва, ул. Полевая, 7'),
('Захаров Виктор Павлович', '1972-06-27', '+7 (913) 345-67-89', 'г. Москва, ул. Строителей, 30'),
('Романова Дарья Игоревна', '2001-02-15', '+7 (914) 456-78-90', 'г. Москва, ул. Молодежная, 9');

INSERT INTO Appointments (PatientId, DoctorId, Date, Time, Price, Diagnosis, Status) VALUES
(1, 1, '2026-09-20', '10:00', 1500, 'ОРВИ, назначено лечение', 'Проведен'),
(2, 2, '2026-09-22', '14:00', 2500, NULL, 'Отменен'),
(3, 3, '2026-10-05', '12:30', 3000, NULL, 'Запланирован');
