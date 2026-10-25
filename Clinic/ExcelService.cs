using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Xml.Linq;

namespace Clinic
{
    // Сервис экспорта и импорта данных в формате Excel (.xlsx и .csv)
    // Работает автономно через OpenXML (System.IO.Compression) без сторонних библиотек
    public static class ExcelService
    {
        // Экспорт таблицы DataTable в формат Excel (.xlsx)
        public static void ExportTable(DataTable table, string filePath, string sheetTitle, Dictionary<string, string> summaryInfo = null)
        {
            if (table == null)
                throw new ArgumentNullException("table");

            if (File.Exists(filePath))
                File.Delete(filePath);

            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            using (ZipArchive zip = new ZipArchive(fs, ZipArchiveMode.Create))
            {
                // 1. [Content_Types].xml
                AddZipEntry(zip, "[Content_Types].xml",
                    "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>\r\n" +
                    "<Types xmlns=\"http://schemas.openxmlformats.org/package/2006/content-types\">\r\n" +
                    "  <Default Extension=\"rels\" ContentType=\"application/vnd.openxmlformats-package.relationships+xml\"/>\r\n" +
                    "  <Default Extension=\"xml\" ContentType=\"application/xml\"/>\r\n" +
                    "  <Override PartName=\"/xl/workbook.xml\" ContentType=\"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml\"/>\r\n" +
                    "  <Override PartName=\"/xl/worksheets/sheet1.xml\" ContentType=\"application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml\"/>\r\n" +
                    "  <Override PartName=\"/xl/styles.xml\" ContentType=\"application/vnd.openxmlformats-officedocument.spreadsheetml.styles+xml\"/>\r\n" +
                    "</Types>");

                // 2. _rels/.rels
                AddZipEntry(zip, "_rels/.rels",
                    "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>\r\n" +
                    "<Relationships xmlns=\"http://schemas.openxmlformats.org/package/2006/relationships\">\r\n" +
                    "  <Relationship Id=\"rId1\" Type=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument\" Target=\"xl/workbook.xml\"/>\r\n" +
                    "</Relationships>");

                // 3. xl/_rels/workbook.xml.rels
                AddZipEntry(zip, "xl/_rels/workbook.xml.rels",
                    "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>\r\n" +
                    "<Relationships xmlns=\"http://schemas.openxmlformats.org/package/2006/relationships\">\r\n" +
                    "  <Relationship Id=\"rId1\" Type=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet\" Target=\"worksheets/sheet1.xml\"/>\r\n" +
                    "  <Relationship Id=\"rId2\" Type=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships/styles\" Target=\"styles.xml\"/>\r\n" +
                    "</Relationships>");

                // 4. xl/workbook.xml
                AddZipEntry(zip, "xl/workbook.xml",
                    "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>\r\n" +
                    "<workbook xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\" xmlns:r=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships\">\r\n" +
                    "  <sheets>\r\n" +
                    "    <sheet name=\"Отчет\" sheetId=\"1\" r:id=\"rId1\"/>\r\n" +
                    "  </sheets>\r\n" +
                    "</workbook>");

                // 5. xl/styles.xml (Медицинский стиль: бирюзовый акцент #0E7490 / #16707A)
                AddZipEntry(zip, "xl/styles.xml", BuildStylesXml());

                // 6. xl/worksheets/sheet1.xml
                string sheetXml = BuildWorksheetXml(table, sheetTitle, summaryInfo);
                AddZipEntry(zip, "xl/worksheets/sheet1.xml", sheetXml);
            }
        }

        // Формирование стилей Excel
        private static string BuildStylesXml()
        {
            return "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>\r\n" +
                   "<styleSheet xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\">\r\n" +
                   "  <numFmts count=\"2\">\r\n" +
                   "    <numFmt numFmtId=\"164\" formatCode=\"#,##0.00\\ _р_у_б_.\"/>\r\n" +
                   "    <numFmt numFmtId=\"165\" formatCode=\"dd.mm.yyyy\"/>\r\n" +
                   "  </numFmts>\r\n" +
                   "  <fonts count=\"4\">\r\n" +
                   "    <font><sz val=\"10\"/><name val=\"Calibri\"/><color rgb=\"FF1F2937\"/></font>\r\n" +
                   "    <font><b/><sz val=\"14\"/><name val=\"Calibri\"/><color rgb=\"FF0F766E\"/></font>\r\n" +
                   "    <font><b/><sz val=\"10.5\"/><name val=\"Calibri\"/><color rgb=\"FFFFFFFF\"/></font>\r\n" +
                   "    <font><b/><sz val=\"10\"/><name val=\"Calibri\"/><color rgb=\"FF111827\"/></font>\r\n" +
                   "  </fonts>\r\n" +
                   "  <fills count=\"5\">\r\n" +
                   "    <fill><patternFill patternType=\"none\"/></fill>\r\n" +
                   "    <fill><patternFill patternType=\"gray125\"/></fill>\r\n" +
                   "    <fill><patternFill patternType=\"solid\"><fgColor rgb=\"FF0E7490\"/></patternFill></fill>\r\n" + // 2 - Бирюзовый заголовок
                   "    <fill><patternFill patternType=\"solid\"><fgColor rgb=\"FFF0FDFA\"/></patternFill></fill>\r\n" + // 3 - Чередующаяся светлая строка
                   "    <fill><patternFill patternType=\"solid\"><fgColor rgb=\"FFE2E8F0\"/></patternFill></fill>\r\n" + // 4 - Карточки сводки
                   "  </fills>\r\n" +
                   "  <borders count=\"2\">\r\n" +
                   "    <border><left/><right/><top/><bottom/></border>\r\n" +
                   "    <border>\r\n" +
                   "      <left style=\"thin\"><color rgb=\"FFCBD5E1\"/></left>\r\n" +
                   "      <right style=\"thin\"><color rgb=\"FFCBD5E1\"/></right>\r\n" +
                   "      <top style=\"thin\"><color rgb=\"FFCBD5E1\"/></top>\r\n" +
                   "      <bottom style=\"thin\"><color rgb=\"FFCBD5E1\"/></bottom>\r\n" +
                   "    </border>\r\n" +
                   "  </borders>\r\n" +
                   "  <cellStyleXfs count=\"1\">\r\n" +
                   "    <xf numFmtId=\"0\" fontId=\"0\" fillId=\"0\" borderId=\"0\"/>\r\n" +
                   "  </cellStyleXfs>\r\n" +
                   "  <cellXfs count=\"8\">\r\n" +
                   "    <xf numFmtId=\"0\" fontId=\"0\" fillId=\"0\" borderId=\"0\" xfId=\"0\"/>\r\n" + // 0: Обычный
                   "    <xf numFmtId=\"0\" fontId=\"1\" fillId=\"0\" borderId=\"0\" xfId=\"0\"/>\r\n" + // 1: Заголовок отчета (крупный)
                   "    <xf numFmtId=\"0\" fontId=\"2\" fillId=\"2\" borderId=\"1\" xfId=\"0\" applyFont=\"1\" applyFill=\"1\" applyBorder=\"1\" applyAlignment=\"1\"><alignment horizontal=\"center\" vertical=\"center\"/></xf>\r\n" + // 2: Шапка таблицы
                   "    <xf numFmtId=\"0\" fontId=\"0\" fillId=\"0\" borderId=\"1\" xfId=\"0\" applyBorder=\"1\" applyAlignment=\"1\"><alignment vertical=\"center\"/></xf>\r\n" + // 3: Ячейка с рамкой слева
                   "    <xf numFmtId=\"0\" fontId=\"0\" fillId=\"0\" borderId=\"1\" xfId=\"0\" applyBorder=\"1\" applyAlignment=\"1\"><alignment horizontal=\"center\" vertical=\"center\"/></xf>\r\n" + // 4: Ячейка по центру
                   "    <xf numFmtId=\"164\" fontId=\"0\" fillId=\"0\" borderId=\"1\" xfId=\"0\" applyNumberFormat=\"1\" applyBorder=\"1\" applyAlignment=\"1\"><alignment horizontal=\"right\" vertical=\"center\"/></xf>\r\n" + // 5: Валюта / Число
                   "    <xf numFmtId=\"0\" fontId=\"3\" fillId=\"4\" borderId=\"1\" xfId=\"0\" applyFont=\"1\" applyFill=\"1\" applyBorder=\"1\" applyAlignment=\"1\"><alignment vertical=\"center\"/></xf>\r\n" + // 6: Сводная карточка
                   "    <xf numFmtId=\"0\" fontId=\"0\" fillId=\"3\" borderId=\"1\" xfId=\"0\" applyFill=\"1\" applyBorder=\"1\" applyAlignment=\"1\"><alignment vertical=\"center\"/></xf>\r\n" + // 7: Чередующаяся подсветка
                   "  </cellXfs>\r\n" +
                   "</styleSheet>";
        }

        // Построение содержимого листа Excel
        private static string BuildWorksheetXml(DataTable table, string sheetTitle, Dictionary<string, string> summaryInfo)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
            sb.AppendLine("<worksheet xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\">");

            // Колонки и их ширина
            int visibleColsCount = 0;
            List<DataColumn> cols = new List<DataColumn>();
            foreach (DataColumn c in table.Columns)
            {
                if (!c.ColumnName.EndsWith("Raw", StringComparison.OrdinalIgnoreCase) &&
                    !c.ColumnName.Equals("Id", StringComparison.OrdinalIgnoreCase) &&
                    !c.ColumnName.Equals("PatientId", StringComparison.OrdinalIgnoreCase) &&
                    !c.ColumnName.Equals("DoctorId", StringComparison.OrdinalIgnoreCase))
                {
                    cols.Add(c);
                    visibleColsCount++;
                }
            }

            if (cols.Count == 0)
            {
                foreach (DataColumn c in table.Columns)
                    cols.Add(c);
                visibleColsCount = cols.Count;
            }

            sb.AppendLine("  <cols>");
            for (int i = 0; i < cols.Count; i++)
            {
                double width = Math.Max(cols[i].ColumnName.Length * 1.6, 15);
                // Если колонка диагноза, адреса или ФИО - делаем шире
                string name = cols[i].ColumnName.ToLower();
                if (name.Contains("фио") || name.Contains("name") || name.Contains("пациент") || name.Contains("врач"))
                    width = 28;
                else if (name.Contains("диагноз") || name.Contains("diagnosis") || name.Contains("адрес") || name.Contains("address"))
                    width = 34;
                else if (name.Contains("телефон") || name.Contains("phone"))
                    width = 19;
                else if (name.Contains("специальность") || name.Contains("specialty"))
                    width = 20;

                sb.AppendFormat("    <col min=\"{0}\" max=\"{0}\" width=\"{1}\" customWidth=\"1\"/>\r\n", i + 1, width.ToString("0.0", CultureInfo.InvariantCulture));
            }
            sb.AppendLine("  </cols>");

            sb.AppendLine("  <sheetData>");

            int rowIndex = 1;

            // Название отчета
            sb.AppendFormat("    <row r=\"{0}\" ht=\"26\" customHeight=\"1\">\r\n", rowIndex);
            sb.AppendFormat("      <c r=\"A{0}\" s=\"1\" t=\"inlineStr\"><is><t>{1}</t></is></c>\r\n", rowIndex, EscapeXml(sheetTitle));
            sb.AppendLine("    </row>");
            rowIndex++;

            // Сводные данные (если переданы)
            if (summaryInfo != null && summaryInfo.Count > 0)
            {
                foreach (KeyValuePair<string, string> kvp in summaryInfo)
                {
                    sb.AppendFormat("    <row r=\"{0}\">\r\n", rowIndex);
                    sb.AppendFormat("      <c r=\"A{0}\" s=\"6\" t=\"inlineStr\"><is><t>{1}</t></is></c>\r\n", rowIndex, EscapeXml(kvp.Key));
                    sb.AppendFormat("      <c r=\"B{0}\" s=\"3\" t=\"inlineStr\"><is><t>{1}</t></is></c>\r\n", rowIndex, EscapeXml(kvp.Value));
                    sb.AppendLine("    </row>");
                    rowIndex++;
                }
                // Пустая строка-разделитель
                rowIndex++;
            }

            // Шапка таблицы
            sb.AppendFormat("    <row r=\"{0}\" ht=\"24\" customHeight=\"1\">\r\n", rowIndex);
            for (int col = 0; col < cols.Count; col++)
            {
                string cellRef = GetColumnLetter(col + 1) + rowIndex;
                sb.AppendFormat("      <c r=\"{0}\" s=\"2\" t=\"inlineStr\"><is><t>{1}</t></is></c>\r\n",
                    cellRef, EscapeXml(cols[col].ColumnName));
            }
            sb.AppendLine("    </row>");
            rowIndex++;

            // Строки с данными
            for (int r = 0; r < table.Rows.Count; r++)
            {
                DataRow row = table.Rows[r];
                sb.AppendFormat("    <row r=\"{0}\">\r\n", rowIndex);

                for (int col = 0; col < cols.Count; col++)
                {
                    string cellRef = GetColumnLetter(col + 1) + rowIndex;
                    object val = row[cols[col]];

                    if (val == null || val == DBNull.Value || string.IsNullOrEmpty(val.ToString()))
                    {
                        sb.AppendFormat("      <c r=\"{0}\" s=\"3\"/>\r\n", cellRef);
                        continue;
                    }

                    string text = val.ToString();

                    // Проверяем числовые значения (цены, выручка)
                    double numVal;
                    if (double.TryParse(text.Replace(" руб.", "").Replace(" ", "").Replace(',', '.'),
                        NumberStyles.Any, CultureInfo.InvariantCulture, out numVal) &&
                        (cols[col].ColumnName.ToLower().Contains("цена") ||
                         cols[col].ColumnName.ToLower().Contains("стоимость") ||
                         cols[col].ColumnName.ToLower().Contains("выручка") ||
                         cols[col].ColumnName.ToLower().Contains("сумма") ||
                         cols[col].ColumnName.ToLower().Contains("price")))
                    {
                        sb.AppendFormat("      <c r=\"{0}\" s=\"5\"><v>{1}</v></c>\r\n",
                            cellRef, numVal.ToString(CultureInfo.InvariantCulture));
                    }
                    else if (cols[col].ColumnName.ToLower().Contains("дата") ||
                             cols[col].ColumnName.ToLower().Contains("время") ||
                             cols[col].ColumnName.ToLower().Contains("статус") ||
                             cols[col].ColumnName.ToLower().Contains("кабинет"))
                    {
                        // Центрированный текст
                        sb.AppendFormat("      <c r=\"{0}\" s=\"4\" t=\"inlineStr\"><is><t>{1}</t></is></c>\r\n",
                            cellRef, EscapeXml(text));
                    }
                    else
                    {
                        // Обычный текст
                        sb.AppendFormat("      <c r=\"{0}\" s=\"3\" t=\"inlineStr\"><is><t>{1}</t></is></c>\r\n",
                            cellRef, EscapeXml(text));
                    }
                }

                sb.AppendLine("    </row>");
                rowIndex++;
            }

            sb.AppendLine("  </sheetData>");
            sb.AppendLine("</worksheet>");

            return sb.ToString();
        }

        // Импорт таблицы из Excel (.xlsx или .csv)
        public static DataTable ImportTable(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("Файл не найден", filePath);

            string ext = Path.GetExtension(filePath).ToLower();
            if (ext == ".csv" || ext == ".txt")
                return ImportCsv(filePath);

            DataTable dt = new DataTable();

            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (ZipArchive zip = new ZipArchive(fs, ZipArchiveMode.Read))
            {
                // Загружаем общие строки (SharedStrings), если они есть
                List<string> sharedStrings = new List<string>();
                ZipArchiveEntry sstEntry = zip.GetEntry("xl/sharedStrings.xml");
                if (sstEntry != null)
                {
                    using (Stream stream = sstEntry.Open())
                    {
                        XDocument xdoc = XDocument.Load(stream);
                        XNamespace ns = "http://schemas.openxmlformats.org/spreadsheetml/2006/main";
                        foreach (XElement si in xdoc.Descendants(ns + "si"))
                        {
                            StringBuilder text = new StringBuilder();
                            foreach (XElement t in si.Descendants(ns + "t"))
                                text.Append(t.Value);
                            sharedStrings.Add(text.ToString());
                        }
                    }
                }

                // Загружаем первый лист sheet1.xml
                ZipArchiveEntry sheetEntry = zip.GetEntry("xl/worksheets/sheet1.xml");
                if (sheetEntry == null)
                    throw new InvalidOperationException("В файле Excel не найден лист с данными.");

                using (Stream stream = sheetEntry.Open())
                {
                    XDocument xdoc = XDocument.Load(stream);
                    XNamespace ns = "http://schemas.openxmlformats.org/spreadsheetml/2006/main";

                    var rows = xdoc.Descendants(ns + "row");
                    List<List<string>> rawRows = new List<List<string>>();

                    foreach (XElement rowElem in rows)
                    {
                        SortedDictionary<int, string> rowValues = new SortedDictionary<int, string>();
                        foreach (XElement cell in rowElem.Elements(ns + "c"))
                        {
                            string cellRef = (string)cell.Attribute("r");
                            int colIndex = GetColumnIndexFromRef(cellRef);
                            string cellType = (string)cell.Attribute("t");

                            string val = "";
                            if (cellType == "s")
                            {
                                XElement v = cell.Element(ns + "v");
                                if (v != null)
                                {
                                    int sstId;
                                    if (int.TryParse(v.Value, out sstId) && sstId >= 0 && sstId < sharedStrings.Count)
                                        val = sharedStrings[sstId];
                                }
                            }
                            else if (cellType == "inlineStr")
                            {
                                XElement isElem = cell.Element(ns + "is");
                                if (isElem != null)
                                {
                                    XElement t = isElem.Element(ns + "t");
                                    if (t != null) val = t.Value;
                                }
                            }
                            else
                            {
                                XElement v = cell.Element(ns + "v");
                                if (v != null) val = v.Value;
                            }

                            rowValues[colIndex] = val;
                        }

                        if (rowValues.Count > 0)
                        {
                            int maxCol = 0;
                            foreach (int c in rowValues.Keys)
                                if (c > maxCol) maxCol = c;

                            List<string> rowList = new List<string>();
                            for (int i = 1; i <= maxCol; i++)
                            {
                                rowList.Add(rowValues.ContainsKey(i) ? rowValues[i] : "");
                            }
                            rawRows.Add(rowList);
                        }
                    }

                    if (rawRows.Count == 0)
                        return dt;

                    // Поиск строки с шапкой таблицы (строка с максимальным числом непустых колонок)
                    int headerRowIdx = 0;
                    int maxCols = 0;
                    for (int i = 0; i < Math.Min(rawRows.Count, 15); i++)
                    {
                        int nonEmpties = 0;
                        foreach (string item in rawRows[i])
                            if (!string.IsNullOrWhiteSpace(item)) nonEmpties++;

                        if (nonEmpties > maxCols)
                        {
                            maxCols = nonEmpties;
                            headerRowIdx = i;
                        }
                    }

                    // Создаем колонки
                    List<string> headers = rawRows[headerRowIdx];
                    int colCounter = 1;
                    foreach (string h in headers)
                    {
                        string colName = string.IsNullOrWhiteSpace(h) ? ("Колонка " + colCounter) : h.Trim();
                        // Устранение дубликатов имен колонок
                        string uniqueName = colName;
                        int suffix = 1;
                        while (dt.Columns.Contains(uniqueName))
                        {
                            uniqueName = colName + "_" + suffix++;
                        }
                        dt.Columns.Add(uniqueName, typeof(string));
                        colCounter++;
                    }

                    // Заполняем строки
                    for (int i = headerRowIdx + 1; i < rawRows.Count; i++)
                    {
                        List<string> r = rawRows[i];
                        bool hasData = false;
                        DataRow dr = dt.NewRow();
                        for (int col = 0; col < dt.Columns.Count; col++)
                        {
                            if (col < r.Count && !string.IsNullOrWhiteSpace(r[col]))
                            {
                                dr[col] = r[col].Trim();
                                hasData = true;
                            }
                        }
                        if (hasData)
                            dt.Rows.Add(dr);
                    }
                }
            }

            return dt;
        }

        // Импорт текстового CSV
        public static DataTable ImportCsv(string filePath)
        {
            DataTable dt = new DataTable();
            string[] lines = File.ReadAllLines(filePath, Encoding.GetEncoding(1251));
            if (lines.Length == 0)
                lines = File.ReadAllLines(filePath, Encoding.UTF8);

            if (lines.Length == 0) return dt;

            // Определяем разделитель (; или ,)
            char sep = lines[0].Contains(";") ? ';' : ',';

            int headerRowIdx = 0;
            int maxCols = 0;
            for (int i = 0; i < Math.Min(lines.Length, 15); i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i])) continue;
                string[] parts = lines[i].Split(sep);
                int nonEmpties = 0;
                foreach (string p in parts)
                    if (!string.IsNullOrWhiteSpace(p)) nonEmpties++;
                if (nonEmpties > maxCols)
                {
                    maxCols = nonEmpties;
                    headerRowIdx = i;
                }
            }

            string[] headers = lines[headerRowIdx].Split(sep);
            int colCounter = 1;
            foreach (string h in headers)
            {
                string name = h.Trim('\"', ' ', '\t');
                if (string.IsNullOrEmpty(name)) name = "Колонка " + colCounter;
                string uniqueName = name;
                int suffix = 1;
                while (dt.Columns.Contains(uniqueName))
                    uniqueName = name + "_" + suffix++;
                dt.Columns.Add(uniqueName, typeof(string));
                colCounter++;
            }

            for (int i = headerRowIdx + 1; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i])) continue;
                string[] parts = lines[i].Split(sep);
                DataRow dr = dt.NewRow();
                bool hasData = false;
                for (int col = 0; col < dt.Columns.Count && col < parts.Length; col++)
                {
                    string v = parts[col].Trim('\"', ' ', '\t');
                    if (!string.IsNullOrEmpty(v))
                    {
                        dr[col] = v;
                        hasData = true;
                    }
                }
                if (hasData)
                    dt.Rows.Add(dr);
            }

            return dt;
        }

        // Вспомогательный метод для записи файлов в ZIP-архив
        private static void AddZipEntry(ZipArchive zip, string entryName, string content)
        {
            ZipArchiveEntry entry = zip.CreateEntry(entryName);
            using (Stream s = entry.Open())
            using (StreamWriter sw = new StreamWriter(s, Encoding.UTF8))
            {
                sw.Write(content);
            }
        }

        // Преобразование номера колонки (1, 2, ...) в букву ('A', 'B', ...)
        public static string GetColumnLetter(int columnNumber)
        {
            string col = "";
            while (columnNumber > 0)
            {
                int mod = (columnNumber - 1) % 26;
                col = (char)('A' + mod) + col;
                columnNumber = (columnNumber - mod) / 26;
            }
            return col;
        }

        // Извлечение номера колонки из адреса ячейки (например, 'B12' -> 2)
        private static int GetColumnIndexFromRef(string cellRef)
        {
            int col = 0;
            for (int i = 0; i < cellRef.Length; i++)
            {
                char c = cellRef[i];
                if (char.IsLetter(c))
                {
                    col = col * 26 + (char.ToUpper(c) - 'A' + 1);
                }
                else break;
            }
            return col;
        }

        // Экранирование XML-спецсимволов
        private static string EscapeXml(string text)
        {
            if (string.IsNullOrEmpty(text)) return "";
            return text.Replace("&", "&amp;")
                       .Replace("<", "&lt;")
                       .Replace(">", "&gt;")
                       .Replace("\"", "&quot;")
                       .Replace("'", "&apos;");
        }
    }
}
