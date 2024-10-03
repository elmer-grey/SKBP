using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using СКБП.Properties;
using static System.Net.Mime.MediaTypeNames;

namespace СКБП
{
    public partial class FormAuthorization : Form
    {
        public FormAuthorization()
        {
            InitializeComponent();
            //UserInfo.username = "user";
            //UserInfo.password = "user";
        }

        private void SignButton_Click(object sender, EventArgs e)
        {

            SqlConnection conn = new SqlConnection(Settings.Default.DBconn);
            try
            {
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    SqlCommand sqlCommand = new SqlCommand($"SELECT [id_hr],[login_hr],[pass_hr]" +
                $"FROM [calculator].[dbo].[hr]" +
                            $"WHERE login_hr = '{NameTextBox.Text}' AND pass_hr = '{PassTextBox.Text}'", conn);

                    SqlDataReader dataReader = sqlCommand.ExecuteReader();
                    if (dataReader.Read())
                    {
                        UserInfo.username = Convert.ToString(dataReader["login_hr"]).TrimEnd();
                        UserInfo.password = Convert.ToString(dataReader["pass_hr"]).TrimEnd();
                    }
                    string default_password = "KIBEVS1902";
                    if (NameTextBox.Text == "" || PassTextBox.Text == "")
                    {
                        MessageBox.Show("Вы не ввели логин или пароль!\nПожалуйста, заполните поля!", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (NameTextBox.Text == UserInfo.username && PassTextBox.Text == UserInfo.password)
                    {
                        if (UserInfo.password == default_password)
                        {
                            conn.Close();
                            MessageBox.Show("Это Ваш первый вход в систему!\nПожалуйста, поменяйте пароль!", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            FormNewPass np = new FormNewPass();
                            np.ShowDialog();
                        }
                        else
                        {
                            MessageBox.Show("Вход произошёл успешно!");
                            Hide();
                            if (!checkBox.Checked)
                            {
                                NameTextBox.Text = "";
                                PassTextBox.Text = "";
                            }
                            FormFormules FormForm = new FormFormules();
                            FormForm.Show();
                        }
                    }
                    else
                    {
                        if (NameTextBox.Text == "admin" && PassTextBox.Text == "admin")
                        {
                            MessageBox.Show("Вход произошёл успешно!");
                            Hide();
                            if (!checkBox.Checked)
                            {
                                NameTextBox.Text = "";
                                PassTextBox.Text = "";
                            }
                            FormFormules FormForm = new FormFormules();
                            FormForm.Show();
                        }
                        else
                            MessageBox.Show("Вы ввели неверный логин или пароль!", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Нет подключения");
            }
        }

        private void ChangeLabel_Click(object sender, EventArgs e)
        {
            UserInfo.username = "admin";
            NameTextBox.Text = "";
            PassTextBox.Text = "";
            FormReturnPass np = new FormReturnPass();
            np.ShowDialog();
            UserInfo.username = "user";
        }

        private void FormAuthorization_FormClosed(object sender, FormClosedEventArgs e)
        {
            Settings.Default.FormPos = this.Location;
            Settings.Default.Save();
        }

        private void FormAuthorization_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = MessageBox.Show("Вы хотите закрыть программу?", "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes;
        }

        private void FormAuthorization_Load(object sender, EventArgs e)
        {
            this.Location = Settings.Default.FormPos;
            ChangeLabel.Visible = false;
            button1.Image = Resources.Скрыто;
        }

        private void PassTextBox_TextChanged(object sender, EventArgs e)
        {
            if (NameTextBox.Text == "admin" && PassTextBox.Text == "admin")
                ChangeLabel.Visible = true;
            else ChangeLabel.Visible = false;
        }

        private void NameTextBox_TextChanged(object sender, EventArgs e)
        {
            if (NameTextBox.Text == "admin" && PassTextBox.Text == "admin")
                ChangeLabel.Visible = true;
            else ChangeLabel.Visible = false;
        }

        private void button1_MouseDown(object sender, MouseEventArgs e)
        {
            PassTextBox.PasswordChar = '\0';
            button1.Image = Resources.Открыто;
        }

        private void button1_MouseUp(object sender, MouseEventArgs e)
        {
            PassTextBox.PasswordChar = '*';
            button1.Image = Resources.Скрыто;
        }

        private void PassTextBox_Enter(object sender, EventArgs e)
        {
            PassTextBox.SelectAll();
        }
    }
}

