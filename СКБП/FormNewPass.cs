using Microsoft.VisualBasic.ApplicationServices;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Diagnostics.Metrics;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Data.SqlClient;
using СКБП.Properties;
using System.Data;

namespace СКБП
{
    public partial class FormNewPass : Form
    {
        public FormNewPass()
        {
            InitializeComponent();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(Settings.Default.DBconn);
            conn.Open();

            bool letter, digit, upper, punctuation;

            if (NewPassTextBox.Text == "" || NewPassTextBox.Text == " "
                || OldPassTextBox.Text == "" || OldPassTextBox.Text == " ")
            {
                MessageBox.Show("Одно или несколько полей заполнены неверно!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            /*else
            if (NameTextBox.Text != UserInfo.username)
            { MessageBox.Show("Такого пользователя не существует!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); }*/
            else
            if (OldPassTextBox.Text != UserInfo.password)
            {
                MessageBox.Show("Старый пароль введен неверно!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (OldPassTextBox.Text == NewPassTextBox.Text)
            {
                letter = false; digit = false; upper = false; punctuation = false;// separator = false;

                foreach (char pass in NewPassTextBox.Text)
                {
                    if (char.IsUpper(pass) && !upper)
                        upper = true;
                    if (char.IsDigit(pass) && !digit)
                        digit = true;
                    if (char.IsLetter(pass) && !letter)
                        letter = true;
                    if (char.IsPunctuation(pass) && !punctuation)
                        punctuation = true;
                    //if (char.IsSeparator(pass) && !separator)
                    //  separator = true;
                }

                if (!upper || !digit || !punctuation || !letter || NewPassTextBox.Text.Length < 8)
                    MessageBox.Show("Пароль не удовлетворяет условиям безопасности!\n - Специальные символы\n - Заглавные буквы\n" +
                        " - Буквы в нижнем регистре\n - Цифры\n - Минимальная длина 8 символов", "Ошибка",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    var result = MessageBox.Show("Пароли совпадают!", "Предупреждение", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                    if (result == DialogResult.OK)
                    {
                        string sqlUpdate = $"UPDATE [calculator].[dbo].[hr] " +
                        $"SET [pass_hr] = '{NewPassTextBox.Text}' " +
                            $"WHERE login_hr = '{UserInfo.username}' AND pass_hr = '{UserInfo.password}'";
                        SqlCommand sqlCom = conn.CreateCommand();
                        sqlCom.CommandText = sqlUpdate;
                        sqlCom.Parameters.AddWithValue("@pass_hr", NewPassTextBox.Text);
                        try
                        {
                            sqlCom.ExecuteNonQuery();
                        }
                        catch (Exception err)
                        {
                            MessageBox.Show("Ошибка выполнения запроса:\n" + err.Message,
                            "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        conn.Close();
                        UserInfo.password = NewPassTextBox.Text;
                        MessageBox.Show("Пароль успешно изменён!", "Уведомление",
                        MessageBoxButtons.OK, MessageBoxIcon.Information); Close();
                    }
                }
            }
            else
            {
                letter = false; digit = false; upper = false; punctuation = false;// separator = false;

                foreach (char pass in NewPassTextBox.Text)
                {
                    if (char.IsUpper(pass) && !upper)
                        upper = true;
                    if (char.IsDigit(pass) && !digit)
                        digit = true;
                    if (char.IsLetter(pass) && !letter)
                        letter = true;
                    if (char.IsPunctuation(pass) && !punctuation)
                        punctuation = true;
                    //if (char.IsSeparator(pass) && !separator)
                    //  separator = true;
                }

                if (!upper || !digit || !punctuation || !letter || NewPassTextBox.Text.Length < 8)
                    MessageBox.Show("Пароль не удовлетворяет условиям безопасности!\n - Специальные символы\n - Заглавные буквы\n" +
                        " - Буквы в нижнем регистре\n - Цифры\n - Минимальная длина 8 символов", "Ошибка",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    string sqlUpdate = $"UPDATE [calculator].[dbo].[hr] " +
                        $"SET [pass_hr] = '{NewPassTextBox.Text}' " +
                            $"WHERE login_hr = '{UserInfo.username}' AND pass_hr = '{UserInfo.password}'";
                    SqlCommand sqlCom = conn.CreateCommand();
                    sqlCom.CommandText = sqlUpdate;
                    sqlCom.Parameters.AddWithValue("@pass_hr", NewPassTextBox.Text);
                    try
                    {
                        sqlCom.ExecuteNonQuery();
                    }
                    catch (Exception err)
                    {
                        MessageBox.Show("Ошибка выполнения запроса:\n" + err.Message,
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    conn.Close();
                    UserInfo.password = NewPassTextBox.Text;
                    MessageBox.Show("Пароль успешно изменён!", "Уведомление",
                    MessageBoxButtons.OK, MessageBoxIcon.Information); Close();
                }
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FormNewPass_Load(object sender, EventArgs e)
        {
            /*if (UserInfo.username != "admin")
            {
                label2.Location = new Point(25, 34);
                label3.Location = new Point(25, 69);
                OldPassTextBox.Location = new Point(265, 31);
                NewPassTextBox.Location = new Point(265, 66);
                groupBox1.Location = new Point(25, 104); 
            }*/
        }

        private void FormNewPass_Paint(object sender, PaintEventArgs e)
        {
            //if (UserInfo.username != "admin")
            //   Size = new Size(501, 260);
        }

        private void button1_MouseDown(object sender, MouseEventArgs e)
        {
            NewPassTextBox.PasswordChar = '\0';
            button1.Image = Resources.Открыто;
        }

        private void button1_MouseUp(object sender, MouseEventArgs e)
        {
            NewPassTextBox.PasswordChar = '*';
            button1.Image = Resources.Скрыто;
        }
    }
}
