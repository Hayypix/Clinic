using System;
using System.Windows.Forms;

namespace Clinic
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // при первом запуске создается база данных
            try
            {
                Database.Init();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при создании базы данных: " + ex.Message,
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Application.Run(new LoginForm());
        }
    }
}
