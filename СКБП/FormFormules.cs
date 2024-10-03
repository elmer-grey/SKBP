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
using СКБП.Properties;
using static System.ComponentModel.Design.ObjectSelectorEditor;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Windows.Forms.DataFormats;
using static СКБП.SaveFileAs;
using Application = System.Windows.Forms.Application;

namespace СКБП
{
    public partial class FormFormules : Form
    {
        public FormFormules()
        {
            InitializeComponent();
        }

        public static int id;
        public static double SChR, level;
        public static string date3, date2, date1, date0, dateago, datefor;

        public void comboBoxFormule_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
        }

        public void FormFormules_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form FA = Application.OpenForms[0];
            FA.Show();
        }

        public void comboBoxFormules_SelectionChangeCommitted(object sender, EventArgs e)
        {
            switch (comboBoxFormules.SelectedIndex)
            {
                case 1:
                    ExportButton.Visible = true;
                    koefgroup0.Location = new Point(29, 54);
                    koefgroup0.Visible = true;
                    koefgroup1.Visible = false;
                    koefgroup2.Visible = false;
                    koefgroup3.Visible = false;
                    break;
                case 2:
                    ExportButton.Visible = true;
                    koefgroup1.Location = new Point(29, 54);
                    koefgroup1.Visible = true;
                    koefgroup0.Visible = false;
                    koefgroup2.Visible = false;
                    koefgroup3.Visible = false;
                    break;
                case 3:
                    ExportButton.Visible = true;
                    koefgroup2.Location = new Point(29, 54);
                    koefgroup2.Visible = true;
                    koefgroup0.Visible = false;
                    koefgroup1.Visible = false;
                    koefgroup3.Visible = false;
                    break;
                case 4:
                    ExportButton.Visible = true;
                    koefgroup3.Location = new Point(29, 54);
                    koefgroup3.Visible = true;
                    koefgroup1.Visible = false;
                    koefgroup2.Visible = false;
                    koefgroup0.Visible = false;
                    break;
                default:
                    ExportButton.Visible = false;
                    koefgroup0.Visible = false;
                    koefgroup1.Visible = false;
                    koefgroup2.Visible = false;
                    koefgroup3.Visible = false;
                    break;
            }
        }

        public void ExportData()
        {
            SaveFileAs sfa = new SaveFileAs();
            sfa.ShowDialog();

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Текстовый файл|*.txt";
            saveFileDialog1.Title = "Сохранить данные ...";
            saveFileDialog1.ShowDialog();

            if (saveFileDialog1.FileName != "")
            {
                FileStream fs = (FileStream)saveFileDialog1.OpenFile();
                fs.Close();
                DateTime dt = DateTime.Now;
                StreamWriter sw = new StreamWriter(fs.Name, true, Encoding.UTF8);
                Thread thread = new Thread(() => Clipboard.SetText(sw.NewLine));
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
                thread.Join();
                sw.WriteLine("Пользователь {0}. Время {1}\n", UserInfo.username, DateTime.Now);
                if (Data.Check0 == true)
                {
                    if (amountKoef0.Text == "")
                    {
                        var result = MessageBox.Show("Вы не заполнили формулу ''Коэффициент оборота по приему''! Продолжить?",
                            "Предупреждение", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                        if (result == DialogResult.OK)
                        {
                            if (YearAgo0.Checked == true)
                                sw.WriteLine($"Коэффициент оборота по приему\nСЧР равен: -" +
                                     $"\nНачало периода подсчёта: -\nКонец периода подсчёта: -\n" +
                                     $"Рассматриваемая должность: -\nКоличество: -\n" +
                        $"Результат равен: -\n" + "================\n");
                            else
                                sw.WriteLine($"Коэффициент оборота по приему\nСЧР равен: {SChRText0.Text}" +
                                 $"\nНачало периода подсчёта: {dateTimePicker0.Value.ToLongDateString()}\nКонец периода подсчёта: {dt}\n" +
                                 $"Рассматриваемая должность: {LevelComboBox0.Text}\nКоличество: -\n" +
                    $"Результат равен: -\n" + "================\n");
                        }
                    }
                    else
                    {
                        if (YearAgo0.Checked == true)
                            sw.WriteLine($"Коэффициент оборота по приему\nСЧР равен: {SChRText0.Text}" +
                                 $"\nНачало периода подсчёта: {YearAgoText0.Text}\nКонец периода подсчёта: {dateTimePicker0.Value.ToLongDateString()}\n" +
                                 $"Рассматриваемая должность: {LevelComboBox0.Text}\nКоличество: {amountKoef0.Text}\n" +
                $"Результат равен: {resultKoef0.Text}\n" + "================\n");
                        else
                            sw.WriteLine($"Коэффициент оборота по приему\nСЧР равен: {SChRText0.Text}" +
                             $"\nНачало периода подсчёта: {dateTimePicker0.Value.ToLongDateString()}\nКонец периода подсчёта: {YearForwardText0.Text}\n" +
                             $"Рассматриваемая должность: {LevelComboBox0.Text}\nКоличество: {amountKoef0.Text}\n" +
                $"Результат равен: {resultKoef0.Text}\n" + "================\n");
                    }
                }
                if (Data.Check1 == true)
                {
                    if (amountKoef1.Text == "")
                    {
                        var result = MessageBox.Show("Вы не заполнили формулу ''Коэффициент оборота по выбытию''! Продолжить?",
                            "Предупреждение", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                        if (result == DialogResult.OK)
                        {
                            if (YearAgo1.Checked == true)
                                sw.WriteLine($"Коэффициент оборота по выбытию\nСЧР равен: -" +
                                     $"\nНачало периода подсчёта: -\nКонец периода подсчёта: -\n" +
                                     $"Рассматриваемая должность: -\nКоличество: -\n" +
                        $"Результат равен: -\n" + "================\n");
                            else
                                sw.WriteLine($"Коэффициент оборота по выбытию\nСЧР равен: {SChRText1.Text}" +
                                 $"\nНачало периода подсчёта: {dateTimePicker1.Value.ToLongDateString()}\nКонец периода подсчёта: {dt}\n" +
                                 $"Рассматриваемая должность: {LevelComboBox1.Text}\nКоличество: -\n" +
                    $"Результат равен: -\n" + "================\n");
                        }
                    }
                    else
                    {
                        if (YearAgo1.Checked == true)
                            sw.WriteLine($"Коэффициент оборота по выбытию\nСЧР равен: {SChRText1.Text}" +
                                 $"\nНачало периода подсчёта: {YearAgoText1.Text}\nКонец периода подсчёта: {dateTimePicker1.Value.ToLongDateString()}\n" +
                                 $"Рассматриваемая должность: {LevelComboBox1.Text}\nКоличество: {amountKoef1.Text}\n" +
                $"Результат равен: {resultKoef1.Text}\n" + "================\n");
                        else
                            sw.WriteLine($"Коэффициент оборота по выбытию\nСЧР равен: {SChRText1.Text}" +
                             $"\nНачало периода подсчёта: {dateTimePicker1.Value.ToLongDateString()}\nКонец периода подсчёта: {YearForwardText1.Text}\n" +
                             $"Рассматриваемая должность: {LevelComboBox1.Text}\nКоличество: {amountKoef1.Text}\n" +
                $"Результат равен: {resultKoef1.Text}\n" + "================\n");
                    }
                }
                if (Data.Check2 == true)
                {
                    if (amountKoef2.Text == "")
                    {
                        var result = MessageBox.Show("Вы не заполнили формулу ''Коэффициент текучести кадров''! Продолжить?",
                            "Предупреждение", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                        if (result == DialogResult.OK)
                        {
                            if (YearAgo2.Checked == true)
                                sw.WriteLine($"Коэффициент текучести кадров\nСЧР равен: -" +
                                     $"\nНачало периода подсчёта: -\nКонец периода подсчёта: -\n" +
                                     $"Рассматриваемая должность: -\nКоличество: -\n" +
                        $"Результат равен: -\n" + "================\n");
                            else
                                sw.WriteLine($"Коэффициент текучести кадров\nСЧР равен: {SChRText2.Text}" +
                                 $"\nНачало периода подсчёта: {dateTimePicker2.Value.ToLongDateString()}\nКонец периода подсчёта: {dt}\n" +
                                 $"Рассматриваемая должность: {LevelComboBox2.Text}\nКоличество: -\n" +
                    $"Результат равен: -\n" + "================\n");
                        }
                    }
                    else
                    {
                        if (YearAgo2.Checked == true)
                            sw.WriteLine($"Коэффициент текучести кадров\nСЧР равен: {SChRText2.Text}" +
                                 $"\nНачало периода подсчёта: {YearAgoText2.Text}\nКонец периода подсчёта: {dateTimePicker2.Value.ToLongDateString()}\n" +
                                 $"Рассматриваемая должность: {LevelComboBox2.Text}\nКоличество: {amountKoef2.Text}\n" +
                $"Результат равен: {resultKoef2.Text}\n" + "================\n");
                        else
                            sw.WriteLine($"Коэффициент текучести кадров\nСЧР равен: {SChRText2.Text}" +
                             $"\nНачало периода подсчёта: {dateTimePicker2.Value.ToLongDateString()}\nКонец периода подсчёта: {YearForwardText2.Text}\n" +
                             $"Рассматриваемая должность: {LevelComboBox2.Text}\nКоличество: {amountKoef2.Text}\n" +
                $"Результат равен: {resultKoef2.Text}\n" + "================\n");
                    }
                }
                if (Data.Check3 == true)
                {
                    if (amountKoef3.Text == "")
                    {
                        var result = MessageBox.Show("Вы не заполнили формулу ''Коэффициент постоянства состава персонала предприятия''! Продолжить?",
                            "Предупреждение", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                        if (result == DialogResult.OK)
                        {
                            if (YearAgo3.Checked == true)
                                sw.WriteLine($"Коэффициент постоянства состава персонала предприятия\nСЧР равен: -" +
                                     $"\nНачало периода подсчёта: -\nКонец периода подсчёта: -\n" +
                                     $"Рассматриваемая должность: -\nКоличество: -\n" +
                        $"Результат равен: -\n" + "================\n");
                            else
                                sw.WriteLine($"Коэффициент постоянства состава персонала предприятия\nСЧР равен: {SChRText3.Text}" +
                                 $"\nНачало периода подсчёта: {dateTimePicker3.Value.ToLongDateString()}\nКонец периода подсчёта: {dt}\n" +
                                 $"Рассматриваемая должность: {LevelComboBox3.Text}\nКоличество: -\n" +
                    $"Результат равен: -\n" + "================\n");
                        }
                    }
                    else
                    {
                        if (YearAgo3.Checked == true)
                            sw.WriteLine($"Коэффициент постоянства состава персонала предприятия\nСЧР равен: {SChRText3.Text}" +
                                 $"\nНачало периода подсчёта: {YearAgoText3.Text}\nКонец периода подсчёта: {dateTimePicker3.Value.ToLongDateString()}\n" +
                                 $"Рассматриваемая должность: {LevelComboBox3.Text}\nКоличество: {amountKoef3.Text}\n" +
                $"Результат равен: {resultKoef3.Text}\n" + "================\n");
                        else
                            sw.WriteLine($"Коэффициент постоянства состава персонала предприятия\nСЧР равен: {SChRText3.Text}" +
                             $"\nНачало периода подсчёта: {dateTimePicker3.Value.ToLongDateString()}\nКонец периода подсчёта: {YearForwardText3.Text}\n" +
                             $"Рассматриваемая должность: {LevelComboBox3.Text}\nКоличество: {amountKoef3.Text}\n" +
                $"Результат равен: {resultKoef3.Text}\n" + "================\n");
                    }
                }
                sw.Close();
            }
            MessageBox.Show("Данные успешно сохранены в указанный файл!", "Уведомление",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        public void ExportButton_Click(object sender, EventArgs e)
        {
            ExportData();
        }

        public void FormFormules_Load(object sender, EventArgs e)
        {
            SChRText0.Text = "0";
            SChRText1.Text = "0";
            SChRText2.Text = "0";
            SChRText3.Text = "0";
            ExportButton.Visible = false;
            nameform.Text = $"Расчёты выполняет: {UserInfo.username}";
            date3 = dateTimePicker3.Value.ToString("yyyy-MM-dd");
            date2 = dateTimePicker2.Value.ToString("yyyy-MM-dd");
            date1 = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            date0 = dateTimePicker0.Value.ToString("yyyy-MM-dd");
            id = -1;
            Size = new Size(645, 450);
        }

        public void DateChange()
        {
            if (YearAgo0.Checked == true)
            {
                YearAgoText0.Visible = true;
                YearForwardText0.Visible = false;
                YearAgoText0.Text = dateTimePicker0.Value.AddYears(-1).ToLongDateString();
                dateago = dateTimePicker0.Value.AddYears(-1).ToString("yyyy-MM-dd");
                id = 0;
            }
            if (YearForward0.Checked == true)
            {
                YearAgoText0.Visible = false;
                YearForwardText0.Visible = true;
                YearForwardText0.Text = dateTimePicker0.Value.AddYears(1).ToLongDateString();
                datefor = dateTimePicker0.Value.AddYears(1).ToString("yyyy-MM-dd");
                id = 1;
            }
            if (YearAgo1.Checked == true)
            {
                YearAgoText1.Visible = true;
                YearForwardText1.Visible = false;
                YearAgoText1.Text = dateTimePicker1.Value.AddYears(-1).ToLongDateString();
                dateago = dateTimePicker1.Value.AddYears(-1).ToString("yyyy-MM-dd");
                id = 0;
            }
            if (YearForward1.Checked == true)
            {
                YearAgoText1.Visible = false;
                YearForwardText1.Visible = true;
                YearForwardText1.Text = dateTimePicker1.Value.AddYears(1).ToLongDateString();
                datefor = dateTimePicker1.Value.AddYears(1).ToString("yyyy-MM-dd");
                id = 1;
            }
            if (YearForward2.Checked == true)
            {
                YearAgoText2.Visible = false;
                YearForwardText2.Visible = true;
                YearForwardText2.Text = dateTimePicker2.Value.AddYears(1).ToLongDateString();
                datefor = dateTimePicker2.Value.AddYears(1).ToString("yyyy-MM-dd");
                id = 1;
            }
            if (YearAgo2.Checked == true)
            {
                YearAgoText2.Visible = true;
                YearForwardText2.Visible = false;
                YearAgoText2.Text = dateTimePicker2.Value.AddYears(-1).ToLongDateString();
                dateago = dateTimePicker2.Value.AddYears(-1).ToString("yyyy-MM-dd");
                id = 0;
            }
            if (YearAgo3.Checked == true)
            {
                YearAgoText3.Visible = true;
                YearForwardText3.Visible = false;
                YearAgoText3.Text = dateTimePicker3.Value.AddYears(-1).ToLongDateString();
                dateago = dateTimePicker3.Value.AddYears(-1).ToString("yyyy-MM-dd");
                id = 0;
            }
            if (YearForward3.Checked == true)
            {
                YearAgoText3.Visible = false;
                YearForwardText3.Visible = true;
                YearForwardText3.Text = dateTimePicker3.Value.AddYears(1).ToLongDateString();
                datefor = dateTimePicker3.Value.AddYears(1).ToString("yyyy-MM-dd");
                id = 1;
            }
        }

        public void koef3()
        {
            SqlDataReader dataReader = null;
            try
            {
                SqlConnection conn = new SqlConnection(Settings.Default.DBconn);
                {
                    int count = 0;
                    conn.Open();
                    SqlCommand sqlCommand;
                    if (id == 0)
                    {
                        sqlCommand = new SqlCommand($"SELECT COUNT(*)" +
                        $"FROM[calculator].[dbo].[worker]" +
                        $"WHERE startwork_worker < '{dateago}' AND(endwork_worker > '{date3}' OR endwork_worker = '1900-01-01')" +
                        $"AND level_worker = '{level}'", conn);
                        count = (int)sqlCommand.ExecuteScalar();
                    }
                    else if (id == 1)
                    {
                        sqlCommand = new SqlCommand($"SELECT COUNT(*)" +
                        $"FROM[calculator].[dbo].[worker]" +
                        $"WHERE startwork_worker < '{date3}' AND(endwork_worker > '{datefor}' OR endwork_worker = '1900-01-01')" +
                        $"AND level_worker = '{level}'", conn);
                        count = (int)sqlCommand.ExecuteScalar();
                    }

                    amountKoef3.Text = null;
                    amountKoef3.Text = count.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (dataReader != null && !dataReader.IsClosed)
                {
                    dataReader.Close();
                }
            }
        }

        public void koef2()
        {
            SqlDataReader dataReader = null;
            try
            {
                SqlConnection conn = new SqlConnection(Settings.Default.DBconn);
                {
                    int count = 0;
                    conn.Open();
                    SqlCommand sqlCommand;
                    if (id == 0)
                    {
                        sqlCommand = new SqlCommand($"SELECT COUNT(*)" +
                        $"FROM[calculator].[dbo].[worker]" +
                        $"WHERE (startwork_worker < '{dateago}' OR startwork_worker > '{dateago}') AND endwork_worker < '{date2}' " +
                        $"AND fire_worker = 'По собственному желанию' AND level_worker = '{level}'", conn);
                        count = (int)sqlCommand.ExecuteScalar();
                    }
                    else if (id == 1)
                    {
                        sqlCommand = new SqlCommand($"SELECT COUNT(*)" +
                        $"FROM[calculator].[dbo].[worker]" +
                        $"WHERE (startwork_worker < '{dateago}' OR startwork_worker > '{dateago}') AND endwork_worker < '{datefor}'" +
                        $"AND fire_worker = 'По собственному желанию' AND level_worker = '{level}'", conn);
                        count = (int)sqlCommand.ExecuteScalar();
                    }

                    amountKoef2.Text = null;
                    amountKoef2.Text = count.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (dataReader != null && !dataReader.IsClosed)
                {
                    dataReader.Close();
                }
            }
        }

        public void koef1()
        {
            SqlDataReader dataReader = null;
            try
            {
                SqlConnection conn = new SqlConnection(Settings.Default.DBconn);
                {
                    int count = 0;
                    conn.Open();
                    SqlCommand sqlCommand;
                    if (id == 0)
                    {
                        sqlCommand = new SqlCommand($"SELECT COUNT(*)" +
                        $"FROM[calculator].[dbo].[worker]" +
                        $"WHERE startwork_worker < '{dateago}' AND(endwork_worker > '{date1}' OR endwork_worker = '1900-01-01')" +
                        $"AND (fire_worker = 'Увольнение' OR fire_worker = 'Выход на пенсию') AND level_worker = '{level}'", conn);
                        count = (int)sqlCommand.ExecuteScalar();
                    }
                    else if (id == 1)
                    {
                        sqlCommand = new SqlCommand($"SELECT COUNT(*)" +
                        $"FROM[calculator].[dbo].[worker]" +
                        $"WHERE startwork_worker < '{date1}' AND(endwork_worker > '{datefor}' OR endwork_worker = '1900-01-01')" +
                        $"AND (fire_worker = 'Увольнение' OR fire_worker = 'Выход на пенсию') AND level_worker = '{level}'", conn);
                        count = (int)sqlCommand.ExecuteScalar();
                    }

                    amountKoef1.Text = null;
                    amountKoef1.Text = count.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (dataReader != null && !dataReader.IsClosed)
                {
                    dataReader.Close();
                }
            }
        }

        public void koef0()
        {
            SqlDataReader dataReader = null;
            try
            {
                SqlConnection conn = new SqlConnection(Settings.Default.DBconn);
                {
                    int count = 0;
                    conn.Open();
                    SqlCommand sqlCommand;
                    if (id == 0)
                    {
                        sqlCommand = new SqlCommand($"SELECT COUNT(*)" +
                        $"FROM[calculator].[dbo].[worker]" +
                        $"WHERE startwork_worker > '{dateago}' AND startwork_worker < '{date0}'" +
                        $"AND level_worker = '{level}'", conn);
                        count = (int)sqlCommand.ExecuteScalar();
                    }
                    else if (id == 1)
                    {
                        sqlCommand = new SqlCommand($"SELECT COUNT(*)" +
                        $"FROM[calculator].[dbo].[worker]" +
                        $"WHERE startwork_worker > '{date0}' AND startwork_worker < '{datefor}'" +
                        $"AND level_worker = '{level}'", conn);
                        count = (int)sqlCommand.ExecuteScalar();
                    }

                    amountKoef0.Text = null;
                    amountKoef0.Text = count.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (dataReader != null && !dataReader.IsClosed)
                {
                    dataReader.Close();
                }
            }
        }

        public void amountKoef3_TextChanged(object sender, EventArgs e)
        {
            /*int count = 0;
            string text = amountKoef3.Text;
            foreach (char amount in text)
            {
                if (!char.IsDigit(amount) && amount != ',')
                {
                    MessageBox.Show($"Введённый символ {amount} не является цифрой или запятой!", "Ошибка",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                    text = text.Remove(count, 1);
                }
                if (amount == ',' && count == 0)
                {
                    MessageBox.Show("Запятая не может быть первым символом!", "Ошибка",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                    text = text.Remove(count, 1);
                }
                count++;
            }
            amountKoef3.Text = text;*/
            if (amountKoef3.Text != "" && resultKoef3.Text != null)
            {
                switch (LevelComboBox3.SelectedIndex)
                {
                    case 0:
                        resultKoef3.Text = (Double.Parse(amountKoef3.Text) / SChR * 0.2).ToString();
                        break;
                    case 1:
                        resultKoef3.Text = (Double.Parse(amountKoef3.Text) / SChR * 0.8).ToString();
                        break;
                    case 2:
                        resultKoef3.Text = (Double.Parse(amountKoef3.Text) / SChR * 0.8).ToString();
                        break;
                    default:
                        resultKoef3.Text = "0";
                        amountKoef3.Text = "";
                        break;
                }
            }
            else
            {
                resultKoef3.Text = "0";
                amountKoef3.Text = "";
            }
        }

        public void amountKoef2_TextChanged(object sender, EventArgs e)
        {
            /*int count = 0;
            string text = amountKoef2.Text;
            foreach (char amount in text)
            {
                if (!char.IsDigit(amount) && amount != ',')
                {
                    MessageBox.Show($"Введённый символ {amount} не является цифрой или запятой!", "Ошибка",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                    text = text.Remove(count, 1);
                }
                if (amount == ',' && count == 0)
                {
                    MessageBox.Show("Запятая не может быть первым символом!", "Ошибка",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                    text = text.Remove(count, 1);
                }
                count++;
            }
            amountKoef2.Text = text;*/
            if (amountKoef2.Text != "" && resultKoef2.Text != null)
            {
                switch (LevelComboBox2.SelectedIndex)
                {
                    case 0:
                        resultKoef2.Text = (Double.Parse(amountKoef2.Text) / SChR * 0.2).ToString();
                        break;
                    case 1:
                        resultKoef2.Text = (Double.Parse(amountKoef2.Text) / SChR * 0.8).ToString();
                        break;
                    case 2:
                        resultKoef2.Text = (Double.Parse(amountKoef2.Text) / SChR * 0.8).ToString();
                        break;
                    default:
                        resultKoef2.Text = "0";
                        amountKoef2.Text = "";
                        break;
                }
            }
            else
            {
                resultKoef2.Text = "0";
                amountKoef2.Text = "";
            }
        }

        public void amountKoef1_TextChanged(object sender, EventArgs e)
        {
            /*int count = 0;
            string text = amountKoef1.Text;
            foreach (char amount in text)
            {
                if (!char.IsDigit(amount) && amount != ',')
                {
                    MessageBox.Show($"Введённый символ {amount} не является цифрой или запятой!", "Ошибка",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                    text = text.Remove(count, 1);
                }
                if (amount == ',' && count == 0)
                {
                    MessageBox.Show("Запятая не может быть первым символом!", "Ошибка",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                    text = text.Remove(count, 1);
                }
                count++;
            }
            amountKoef1.Text = text;*/
            if (amountKoef1.Text != "" && resultKoef1.Text != null)
            {
                switch (LevelComboBox1.SelectedIndex)
                {
                    case 0:
                        resultKoef1.Text = (Double.Parse(amountKoef1.Text) / SChR * 0.2).ToString();
                        break;
                    case 1:
                        resultKoef1.Text = (Double.Parse(amountKoef1.Text) / SChR * 0.8).ToString();
                        break;
                    case 2:
                        resultKoef1.Text = (Double.Parse(amountKoef1.Text) / SChR * 0.8).ToString();
                        break;
                    default:
                        resultKoef1.Text = "0";
                        amountKoef1.Text = "";
                        break;
                }
            }
            else
            {
                resultKoef1.Text = "0";
                amountKoef1.Text = "";
            }
        }

        public void amountKoef0_TextChanged(object sender, EventArgs e)
        {
            /*int count = 0;
            string text = amountKoef0.Text;
            foreach (char amount in text)
            {
                if (!char.IsDigit(amount) && amount != ',')
                {
                    MessageBox.Show($"Введённый символ {amount} не является цифрой или запятой!", "Ошибка",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                    text = text.Remove(count, 1);
                }
                if (amount == ',' && count == 0)
                {
                    MessageBox.Show("Запятая не может быть первым символом!", "Ошибка",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                    text = text.Remove(count, 1);
                }
                count++;
            }
            amountKoef0.Text = text;*/
            if (amountKoef0.Text != "" && resultKoef0.Text != null)
            {
                switch (LevelComboBox0.SelectedIndex)
                {
                    case 0:
                        resultKoef0.Text = (Double.Parse(amountKoef0.Text) / SChR * 0.2).ToString();
                        break;
                    case 1:
                        resultKoef0.Text = (Double.Parse(amountKoef0.Text) / SChR * 0.8).ToString();
                        break;
                    case 2:
                        resultKoef0.Text = (Double.Parse(amountKoef0.Text) / SChR * 0.8).ToString();
                        break;
                    default:
                        resultKoef0.Text = "0";
                        amountKoef0.Text = "";
                        break;
                }
            }
            else
            {
                resultKoef0.Text = "0";
                amountKoef0.Text = "";
            }
        }

        public void SChR_calculation()
        {
            SqlConnection conn = new SqlConnection(Settings.Default.DBconn);

            string query = "";
            switch (comboBoxFormules.SelectedIndex)
            {
                case 1:
                    if (id == 0)
                    {
                        query += $"DECLARE @StartDate DATE = '{dateago}';" +
                            $"DECLARE @EndDate DATE = '{date0}';";
                    }
                    else
                    {
                        query += $"DECLARE @StartDate DATE = '{date0}';" +
                            $"DECLARE @EndDate DATE = '{datefor}';";
                    }
                    break;
                case 2:
                    if (id == 0)
                    {
                        query += $"DECLARE @StartDate DATE = '{dateago}';" +
                            $"DECLARE @EndDate DATE = '{date1}';";
                    }
                    else
                    {
                        query += $"DECLARE @StartDate DATE = '{date1}';" +
                            $"DECLARE @EndDate DATE = '{datefor}';";
                    }
                    break;
                case 3:
                    if (id == 0)
                    {
                        query += $"DECLARE @StartDate DATE = '{dateago}';" +
                            $"DECLARE @EndDate DATE = '{date2}';";
                    }
                    else
                    {
                        query += $"DECLARE @StartDate DATE = '{date2}';" +
                            $"DECLARE @EndDate DATE = '{datefor}';";
                    }
                    break;
                case 4:
                    if (id == 0)
                    {
                        query += $"DECLARE @StartDate DATE = '{dateago}';" +
                            $"DECLARE @EndDate DATE = '{date3}';";
                    }
                    else
                    {
                        query += $"DECLARE @StartDate DATE = '{date3}';" +
                            $"DECLARE @EndDate DATE = '{datefor}';";
                    }
                    break;
                default:

                    break;
            }

            query += @"; WITH DateRangeCTE AS(SELECT @StartDate AS DateValue            
                UNION ALL            
                SELECT DATEADD(day, 1, DateValue)
                FROM DateRangeCTE            
                WHERE DateValue < @EndDate),
Employees AS(
    SELECT id_worker, position_worker, startwork_worker, CASE
                  WHEN endwork_worker = '1900-01-01' THEN @EndDate
               ELSE endwork_worker
               END AS ActualEndDate
    FROM [calculator].[dbo].[worker]";
            query += $"WHERE startwork_worker <= @EndDate AND level_worker = {level}";
            query += @"),
DailyCounts AS(
    SELECT
        DateValue,
        position_worker,
        COUNT(id_worker) AS DailyCount
    FROM
        DateRangeCTE
    JOIN
        Employees ON DateValue BETWEEN Employees.startwork_worker AND Employees.ActualEndDate
    GROUP BY
        DateValue,
        position_worker
)
SELECT
    SUM(DailyCount) AS TotalCount
FROM
    DailyCounts
OPTION(MAXRECURSION 0);";
            SqlCommand command = new SqlCommand(query, conn);
            try
            {
                conn.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    SChR = Math.Round(Convert.ToDouble(reader["TotalCount"]) / 365, 8);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            switch (comboBoxFormules.SelectedIndex)
            {
                case 1:
                    SChRText0.Text = SChR.ToString();
                    break;
                case 2:
                    SChRText1.Text = SChR.ToString();
                    break;
                case 3:
                    SChRText2.Text = SChR.ToString();
                    break;
                case 4:
                    SChRText3.Text = SChR.ToString();
                    break;
                default:
                    break;
            }
        }

        public void dateTimePicker3_ValueChanged(object sender, EventArgs e)
        {
            date3 = dateTimePicker3.Value.ToString("yyyy-MM-dd");
            DateChange();
            SChR_calculation();
            koef3();
        }

        public void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            date2 = dateTimePicker2.Value.ToString("yyyy-MM-dd");
            DateChange();
            SChR_calculation();
            koef2();
        }

        public void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            date1 = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            DateChange();
            SChR_calculation();
            koef1();
        }

        public void dateTimePicker0_ValueChanged(object sender, EventArgs e)
        {
            date0 = dateTimePicker0.Value.ToString("yyyy-MM-dd");
            DateChange();
            SChR_calculation();
            koef0();
        }

        public void YearAgo0_CheckedChanged(object sender, EventArgs e)
        {
            DateChange();
            SChR_calculation();
            koef0();
        }

        public void YearForward0_CheckedChanged(object sender, EventArgs e)
        {
            DateChange();
            SChR_calculation();
            koef0();
        }

        public void YearAgo1_CheckedChanged(object sender, EventArgs e)
        {
            DateChange();
            SChR_calculation();
            koef1();
        }

        public void YearForward1_CheckedChanged(object sender, EventArgs e)
        {
            DateChange();
            SChR_calculation();
            koef1();
        }

        public void YearAgo2_CheckedChanged(object sender, EventArgs e)
        {
            DateChange();
            SChR_calculation();
            koef2();
        }

        public void YearForward2_CheckedChanged(object sender, EventArgs e)
        {
            DateChange();
            SChR_calculation();
            koef2();
        }

        public void YearAgo3_CheckedChanged(object sender, EventArgs e)
        {
            DateChange();
            SChR_calculation();
            koef3();
        }

        public void YearForward3_CheckedChanged(object sender, EventArgs e)
        {
            DateChange();
            SChR_calculation();
            koef3();
        }

        public void LevelComboBox0_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (LevelComboBox0.SelectedIndex)
            {
                case 0:
                    level = 1;
                    break;
                case 1:
                    level = 2;
                    break;
                case 2:
                    level = 3;
                    break;
                default:
                    level = 0;
                    break;
            }
            SChR_calculation();
            koef0();
        }

        public void LevelComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (LevelComboBox1.SelectedIndex)
            {
                case 0:
                    level = 1;
                    break;
                case 1:
                    level = 2;
                    break;
                case 2:
                    level = 3;
                    break;
                default:
                    level = 0;
                    break;
            }
            SChR_calculation();
            koef1();
        }

        public void LevelComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (LevelComboBox2.SelectedIndex)
            {
                case 0:
                    level = 1;
                    break;
                case 1:
                    level = 2;
                    break;
                case 2:
                    level = 3;
                    break;
                default:
                    level = 0;
                    break;
            }
            SChR_calculation();
            koef2();
        }

        public void LevelComboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (LevelComboBox3.SelectedIndex)
            {
                case 0:
                    level = 1;
                    break;
                case 1:
                    level = 2;
                    break;
                case 2:
                    level = 3;
                    break;
                default:
                    level = 0;
                    break;
            }
            SChR_calculation();
            koef3();
        }

        public void resultKoef0_TextChanged(object sender, EventArgs e)
        {
            if (SChRText0.Text == "0" && amountKoef0.Text == "0")
                resultKoef0.Text = "0";
            resultKoef0.Text = Math.Round(Convert.ToDouble(resultKoef0.Text), 8).ToString();
        }
        public double result(int amount, double SCHR)
        {
            double res;
            res = Math.Round(amount / SCHR * 0.8, 8);
            return res;
        }
        public void resultKoef1_TextChanged(object sender, EventArgs e)
        {
            if (SChRText1.Text == "0" && amountKoef1.Text == "0")
                resultKoef1.Text = "0";
            resultKoef1.Text = Math.Round(Convert.ToDouble(resultKoef1.Text), 8).ToString();
        }

        public void resultKoef2_TextChanged(object sender, EventArgs e)
        {
            if (SChRText2.Text == "0" && amountKoef2.Text == "0")
                resultKoef2.Text = "0";
            resultKoef2.Text = Math.Round(Convert.ToDouble(resultKoef2.Text), 8).ToString();
        }

        public void resultKoef3_TextChanged(object sender, EventArgs e)
        {
            if (SChRText3.Text == "0" && amountKoef3.Text == "0")
                resultKoef3.Text = "0";
            resultKoef3.Text = Math.Round(Convert.ToDouble(resultKoef3.Text), 8).ToString();
        }

        private void YearAgo3_CheckedChanged_1(object sender, EventArgs e)
        {
            DateChange();
            SChR_calculation();
            koef3();
        }

        private void YearForward3_CheckedChanged_1(object sender, EventArgs e)
        {
            DateChange();
            SChR_calculation();
            koef3();
        }

        private void YearAgo0_CheckedChanged_1(object sender, EventArgs e)
        {
            DateChange();
            SChR_calculation();
            koef0();
        }

        private void YearForward0_CheckedChanged_1(object sender, EventArgs e)
        {
            DateChange();
            SChR_calculation();
            koef0();
        }

        private void YearAgo2_CheckedChanged_1(object sender, EventArgs e)
        {
            DateChange();
            SChR_calculation();
            koef2();
        }

        private void YearForward2_CheckedChanged_1(object sender, EventArgs e)
        {
            DateChange();
            SChR_calculation();
            koef2();
        }

        private void YearForward1_CheckedChanged_1(object sender, EventArgs e)
        {
            DateChange();
            SChR_calculation();
            koef1();
        }

        private void YearAgo1_CheckedChanged_1(object sender, EventArgs e)
        {
            DateChange();
            SChR_calculation();
            koef1();
        }

    }
}