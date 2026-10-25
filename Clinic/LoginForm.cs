using System;
using System.Data.SQLite;
using System.Windows.Forms;

namespace Clinic
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        // Вход в систему
        private void btnEnter_Click(object sender, EventArgs e)
        {
            string login = txtLogin.Text.Trim();
            string pass = txtPass.Text;

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(pass))
            {
                MessageBox.Show("Заполните логин и пароль", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SQLiteConnection con = Database.GetConnection())
                {
                    con.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(
                        "SELECT COUNT(*) FROM Users WHERE Login = @l AND Password = @p", con))
                    {
                        cmd.Parameters.AddWithValue("@l", login);
                        cmd.Parameters.AddWithValue("@p", pass);

                        if (Convert.ToInt32(cmd.ExecuteScalar()) > 0)
                        {
                            MainForm f = new MainForm(login);
                            Hide();
                            f.ShowDialog();
                            Close();
                        }
                        else
                        {
                            MessageBox.Show("Неверный логин или пароль", "Ошибка",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtPass.Clear();
                            txtPass.Focus();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка подключения: " + ex.Message, "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Выход
        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        // Показать / скрыть пароль
        private void chkShow_CheckedChanged(object sender, EventArgs e)
        {
            if (chkShow.Checked)
                txtPass.PasswordChar = '\0';
            else
                txtPass.PasswordChar = '•';
        }
    }
}
