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

        // вход в систему
        private void btnEnter_Click(object sender, EventArgs e)
        {
            if (txtLogin.Text.Trim() == "" || txtPass.Text == "")
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
                    SQLiteCommand cmd = new SQLiteCommand(
                        "SELECT COUNT(*) FROM Users WHERE Login = @l AND Password = @p", con);
                    cmd.Parameters.AddWithValue("@l", txtLogin.Text.Trim());
                    cmd.Parameters.AddWithValue("@p", txtPass.Text);

                    if (Convert.ToInt32(cmd.ExecuteScalar()) > 0)
                    {
                        MainForm f = new MainForm(txtLogin.Text.Trim());
                        Hide();
                        f.ShowDialog();
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("Неверный логин или пароль", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtPass.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message, "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // выход
        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        // показать/скрыть пароль
        private void chkShow_CheckedChanged(object sender, EventArgs e)
        {
            if (chkShow.Checked)
                txtPass.PasswordChar = '\0';
            else
                txtPass.PasswordChar = '•';
        }
    }
}
