using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using СКБП.Properties;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace СКБП
{
    public partial class FormReturnPass : Form
    {
        public FormReturnPass()
        {
            InitializeComponent();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            if (NameTextBox.Text == "")
            {
                MessageBox.Show("Вы ничего не ввели!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                SqlConnection conn = new SqlConnection(Settings.Default.DBconn);
                conn.Open();
                var sqlCommand = new SqlCommand($"SELECT COUNT(*)" +
                $"FROM[calculator].[dbo].[hr]" +
                            $"WHERE login_hr = '{NameTextBox.Text}'", conn);
                int count = (int)sqlCommand.ExecuteScalar();
                if (count == 0)
                {
                    MessageBox.Show("Такого пользователя не существует!", "Ошибка", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                    conn.Close();
                    return;
                }
                else
                {
                    var result = MessageBox.Show("Вы уверены, что хотите сбросить пароль? Это действие нельзя отменить!", "Предупреждение", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                    if (result == DialogResult.OK)
                    {
                        string sqlUpdate = $"UPDATE [calculator].[dbo].[hr] " +
                        $"SET [pass_hr] = 'KIBEVS1902' " +
                            $"WHERE login_hr = '{NameTextBox.Text}'";
                        SqlCommand sqlCom = conn.CreateCommand();
                        sqlCom.CommandText = sqlUpdate;
                        sqlCom.Parameters.AddWithValue("@pass_hr", "KIBEVS1902");
                        try
                        {
                            sqlCom.ExecuteNonQuery();

                            conn.Close();
                            MessageBox.Show("Пароль успешно изменён!", "Уведомление",
                            MessageBoxButtons.OK, MessageBoxIcon.Information); Close();
                        }
                        catch (Exception err)
                        {
                            MessageBox.Show("Ошибка выполнения запроса:\n" + err.Message,
                            "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            
        }
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
